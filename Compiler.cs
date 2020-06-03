using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CreoleForth
{
    public class Compiler
    {
        const string title = "Compiler grouping";

        public string Title
        {
            get { return title; }
        }

        // ( n -- ) Compiles value off the TOS into the next parameter field cell
        public void doComma(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            gsp.Scratch = gsp.Pop(gsp.DataStack);
            int token = (int)gsp.Scratch;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            newCreoleWord.ParamField.Add(token);
            newCreoleWord.ParamFieldStart++;
            gsp.ParamFieldPtr = newCreoleWord.ParamField.Count + 1;
        }

        // Executes at time zero of colon compilation, when CompileInfo triplets are placed in the PAD area.
        // Example : comment handling - the pointer is moved past the comments
        // ( -- ) Single-line comment handling
        public void doSingleLineCmts(ref GlobalSimpleProps gsp)
        {
            while (gsp.ParsedInput[gsp.OuterPtr] != "__#EOL#__")
            {
                gsp.OuterPtr += 1;
            }
        }

        // ( -- ) Multiline comment handling
        public void doParenCmts(ref GlobalSimpleProps gsp)
        {
            while (!gsp.ParsedInput[gsp.OuterPtr].Contains(")"))
            {
                gsp.OuterPtr += 1;
            }
        }

        // ( -- list ) List compiler
        public void compileList(ref GlobalSimpleProps gsp)
        {
            List<string> compiledList = new List<string>();
            gsp.OuterPtr += 1;
            Regex rx = new Regex(@"\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            bool isFound = rx.IsMatch(gsp.ParsedInput[gsp.OuterPtr]);
            while (!isFound)
            {
                compiledList.Add(gsp.ParsedInput[gsp.OuterPtr] + " ");
                gsp.OuterPtr++;
                isFound = rx.IsMatch(gsp.ParsedInput[gsp.OuterPtr]);
            }
            string joinedList = compiledList.ToString().Trim();
            gsp.Scratch = joinedList;
            gsp.Push(gsp.DataStack);
        }

        // ( address -- ) Executes the word corresponding to the address on the stack
        public void doExecute(ref GlobalSimpleProps gsp)
        {
            int address = (int)gsp.Pop(gsp.DataStack);
            gsp.InnerPtr = address;
            CreoleWord cw = gsp.Cfb.Address[address];
            cw.CodeField(ref gsp);
        }

        // ( -- location ) Returns address of the next available dictionary location
        public void doHere(ref GlobalSimpleProps gsp)
        {
            int hereLoc = gsp.Cfb.Address.Count;
            gsp.Scratch = hereLoc;
            gsp.Push(gsp.DataStack);
        }

        // Used internally by doCreate - is not compiled into the dictionary
        public void doMyAddress(ref GlobalSimpleProps gsp)
        {
            CreoleWord cw = gsp.Cfb.Address[gsp.InnerPtr];
            gsp.Scratch = cw.IndexField;
            gsp.Push(gsp.DataStack);
        }

        // CREATE <name>. Adds a named entry into the dictionary
        public void doCreate(ref GlobalSimpleProps gsp)
        {
            int hereLoc = gsp.Cfb.Address.Count;
            string name = gsp.ParsedInput[gsp.OuterPtr + 1];
            List<int> paramList = new List<int>();
            List<object> data = new List<object>();
            string help = "TODO: ";
            CreoleWord cw = new CreoleWord(name, gsp.Cfb.Modules.Compiler.doMyAddress, "Compiler.doMyAddress", gsp.CurrentVocab, "COMPINPF", help, hereLoc - 1, hereLoc, hereLoc - 1, hereLoc, paramList, data);
            string fqName = name + "." + gsp.CurrentVocab;
            gsp.Cfb.Dict[fqName] = cw;
            gsp.Cfb.Address.Add(gsp.Cfb.Dict[fqName]);
            gsp.OuterPtr += 2;
        }

        private GlobalSimpleProps gspDeepCopy(GlobalSimpleProps gsp)
        {
            GlobalSimpleProps gspComp = new GlobalSimpleProps();

            gspComp.DataStack = new List<object>();
            gspComp.ReturnStack = new List<object>();
            gspComp.VocabStack = new List<string>();
            gspComp.PrefilterStack = new List<string>();
            gspComp.PostfilterStack = new List<string>();
            gspComp.PADarea = new List<CompileInfo>();

            gspComp.ParsedInput = new List<string>();
            gspComp.LoopCurrIndexes.Clear();
            foreach (int lci in gsp.LoopCurrIndexes)
            {
                gspComp.LoopCurrIndexes.Add(lci);
            }

            gspComp.InputArea = gsp.InputArea;
            gspComp.OutputArea = gsp.OutputArea;
            gspComp.OuterPtr = gsp.OuterPtr;
            gspComp.InnerPtr = gsp.InnerPtr;
            gspComp.ParamFieldPtr = gsp.ParamFieldPtr;
            gspComp.LoopLabelPtr = gsp.LoopLabelPtr;
            gspComp.CurrentVocab = gsp.CurrentVocab;
            gspComp.HelpCommentField = gsp.HelpCommentField;
            gspComp.SoundField = gsp.SoundField;
            gspComp.CompiledList = gsp.CompiledList;
            gspComp.CurrCreoleWord = gsp.CurrCreoleWord;
            gspComp.Pause = gsp.Pause;
            gspComp.MinArgsSwitch = gsp.MinArgsSwitch;

            return gspComp;
        }

        // ( -- ) Starts compilation of a colon definition
        public void compileColon(ref GlobalSimpleProps gsp)
        {
            int hereLoc = gsp.Cfb.Address.Count;
            string name = gsp.ParsedInput[gsp.OuterPtr + 1];
            List<int> paramList = new List<int>();
            List<object> data = new List<object>();
            string help = "TODO: ";
            string rawWord = "";
            int searchVocabPtr = 0;
            bool isFound = false;
            string compAction = "";
            CompileInfo compInfo;
            bool isSemiPresent = false;
            int colonIndex = -1;
            Codefield codefield;
            CreoleWord cw;

            // I have to do a deep copy here
            GlobalSimpleProps gspComp = gspDeepCopy(gsp);

            // Elementary syntax check - if a colon isn't followed by a matching semicolon, you get an error message and the stacks and input are cleared.
            for (int i = 0; i < gsp.ParsedInput.Count; i++)
            {
                if (gsp.ParsedInput[i] == ":")
                {
                    colonIndex = i;
                }
                if (gsp.ParsedInput[i] == ";" && i > colonIndex)
                {
                    isSemiPresent = true;
                }
            }

            if (isSemiPresent == false)
            {
                Debug.Print("Error: colon def must have matching semicolon");
                gsp.cleanFields();
                return;
            }

            // Compilation is started when the IMMEDIATE vocabulary is pushed onto the vocabulary stack. No need for the usual Forth STATE flag.
            gsp.VocabStack.Add(gsp.BFC.ImmediateVocab);
            cw = new CreoleWord(name, gsp.Cfb.Modules.Interpreter.doColon, "Interpreter.doColon", gsp.CurrentVocab, "COMPINPF", help, hereLoc - 1, hereLoc, hereLoc - 1, hereLoc, paramList, data);
            // The smudge flag avoids accidental recursion. But it's easy enough to get around if you want to. 
            string fqNameSmudged = name + "." + gsp.CurrentVocab + "." + gsp.BFC.SmudgeFlag;
            string fqName = name + "." + gsp.CurrentVocab;
            string fqWord = "";

            gsp.Cfb.Dict[fqNameSmudged] = cw;
            gsp.Cfb.Address.Add(gsp.Cfb.Dict[fqNameSmudged]);
            gsp.OuterPtr += 2;

            // Parameter field contents are set up in the PAD area. Each word is looked up one at a time in the dictionary, and its name, address, and
            // compilation action are placed in the CompileInfo triplet.
            while (gsp.OuterPtr < gsp.ParsedInput.Count && gsp.VocabStack[gsp.VocabStack.Count - 1] == gsp.BFC.ImmediateVocab && gsp.ParsedInput[gsp.OuterPtr] != ";")
            {
                rawWord = gsp.ParsedInput[gsp.OuterPtr];
                searchVocabPtr = gsp.VocabStack.Count - 1;
                isFound = false;
                while (searchVocabPtr >= 0)
                {
                    fqWord = rawWord.ToUpper() + "." + gsp.VocabStack[searchVocabPtr];
                    if (gsp.Cfb.Dict.ContainsKey(fqWord))
                    {
                        cw = gsp.Cfb.Dict[fqWord];
                        compAction = cw.CompileActionField;
                        if (compAction != gsp.BFC.ExecZeroAction)
                        {
                            compInfo = new CompileInfo(fqWord, cw.IndexField, compAction);
                            gsp.PADarea.Add(compInfo);
                        }
                        else
                        {
                            // This is stuff where the outer ptr is manipulated such as comments
                            codefield = cw.CodeField;
                            codefield(ref gsp);
                        }
                        isFound = true;
                        break;
                    }
                    else
                    {
                        searchVocabPtr--;
                    }
                }

                // If no dictionary entry is found, it's tagged as a literal.
                if (isFound == false)
                {
                    compInfo = new CompileInfo(rawWord, rawWord, gsp.BFC.CompLitAction);
                    gsp.PADarea.Add(compInfo);
                }
                gsp.OuterPtr++;

                // 1. Builds the definition in the parameter field from the PAD area. Very simple; the address of each word appears before its associated
                //    compilation action. Most of the time, it will be COMPINPF, which will simply compile the word into the parameter field (it's actually
                //    , (comma) with a different name for readability purposes).
                //    Compiling words such as CompileIf will execute since that's the compilation action they're tagged with.
                // 2. Attaches it to the smudged definition.
                // 3. "Unsmudges" the new definition by copying it to its proper fully-qualified property and places it in the Address array.
                // 4. Deletes the smudged definition.
                // 5. Pops the IMMEDIATE vocabulary off the vocabulary stack and halts compilation

                int i = 0;
                gspComp.VocabStack = gsp.VocabStack;
                gspComp.Cfb.Address.Add(gsp.Cfb.Dict[fqNameSmudged]);

                // Putting the args and compilation actions together then executing them seems to cause a problem with compiling words.
                // Getting around this by putting one arg on the stack, one in the input area, then executing.

                while (i < gsp.PADarea.Count)
                {
                    compInfo = gsp.PADarea[i];
                    gspComp.DataStack.Add(compInfo.Address);
                    gspComp.InputArea = compInfo.CompileAction;
                    gspComp.Cfb.Modules.Interpreter.doParseInput(ref gspComp);
                    gspComp.Cfb.Modules.Interpreter.doOuter(ref gspComp);
                    i++;
                }

                gspComp.InputArea = ";";
                gspComp.Cfb.Modules.Interpreter.doParseInput(ref gspComp);
                gspComp.Cfb.Modules.Interpreter.doOuter(ref gspComp);

                cw = gspComp.Cfb.Address[hereLoc];
                gsp.Cfb.Dict[fqName] = cw;
                gsp.Cfb.Dict.Remove(fqNameSmudged);
            }
        }

        // ( -- ) Terminates compilation of a colon definition
        public void doSemi(ref GlobalSimpleProps gsp)
        {
            gsp.PADarea.Clear();
            gsp.Pop(val: gsp.VocabStack);
        }

        // ( -- ) Compiles doLit and a literal into the dictionary
        public void compileLiteral(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string runTimeAction = "doLiteral." + gsp.BFC.ImmediateVocab;
            int doLitAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            object litVal = gsp.Pop(gsp.DataStack);
            newCreoleWord.ParamField.Add(doLitAddr);
            newCreoleWord.DataField.Add(litVal);
            newCreoleWord.ParamField.Add(newCreoleWord.DataField.Count - 1);
            gsp.ParamFieldPtr = newCreoleWord.ParamField.Count - 1;
        }

        // ( -- lit ) Run-time code that pushes a literal onto the stack
        public void doLiteral(ref GlobalSimpleProps gsp)
        {
            CreoleWord currWord = gsp.Cfb.Address[gsp.InnerPtr];
            List<int> paramField = currWord.ParamField;
            List<object> dataField = currWord.DataField;
            ReturnLoc rLoc = (ReturnLoc)gsp.Pop(gsp.ReturnStack);
            int litDataFieldAddr = paramField[gsp.ParamFieldPtr];
            gsp.DataStack.Add(dataField[litDataFieldAddr]);
            rLoc.ParamFieldAddr++;
            gsp.ParamFieldPtr = rLoc.ParamFieldAddr;
            gsp.ReturnStack.Add(rLoc);
        }

        // ( addr -- val ) Fetches the value in the param field  at addr
        public void doFetch(ref GlobalSimpleProps gsp)
        {
            int address = (int)gsp.Pop(gsp.DataStack);
            List<object> dataField = gsp.Cfb.Address[address].DataField;

            object storedVal = null;
            if (dataField.Count > 0)
            {
                storedVal = gsp.Cfb.Address[address].DataField[0];
            }
            gsp.DataStack.Add(storedVal);
        }
        // (val addr --) Stores the value in the param field at addr
        public void doStore(ref GlobalSimpleProps gsp)
        {
            int address = (int)gsp.Pop(gsp.DataStack);
            object valToStore = gsp.Pop(gsp.DataStack);
            gsp.Cfb.Address[address].DataField.Add(valToStore);
        }

        // (  -- ) Sets the current (compilation) vocabulary to the context vocabulary (the one on top of the vocabulary stack)
        public void doSetCurrentToContext(ref GlobalSimpleProps gsp)
        {
            string currentVocab = gsp.VocabStack[gsp.VocabStack.Count - 1];
            gsp.CurrentVocab = currentVocab;
            Debug.Print("Current vocab is now " + gsp.CurrentVocab);
        }

        // ( -- ) Flags a word as immediate (so it executes instead of compiling inside a colon definition)
        public void doImmediate(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string fqName = newCreoleWord.FQNameField;
            newCreoleWord.CompileActionField = "EXECUTE";
            newCreoleWord.Vocabulary = "IMMEDIATE";
        }

        // ( -- location ) Compile-time code for IF
        public void compileIf(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string runTimeAction = "0BRANCH." + gsp.BFC.ImmediateVocab;
            int zeroBranchAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            newCreoleWord.ParamField.Add(zeroBranchAddr);
            newCreoleWord.ParamField.Add(-1);
            gsp.ParamFieldPtr = newCreoleWord.ParamField.Count - 1;
            gsp.DataStack.Add(gsp.ParamFieldPtr);
        }

        // ( -- location ) Compile-time code for ELSE
        public void compileElse(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string jumpAddrLookup = "JUMP." + gsp.BFC.ImmediateVocab;
            int jumpAddr = gsp.Cfb.Dict[jumpAddrLookup].IndexField;
            string elseAddrLookup = "doElse." + gsp.BFC.ImmediateVocab;
            int elseAddr = gsp.Cfb.Dict[elseAddrLookup].IndexField;
            newCreoleWord.ParamField.Add(jumpAddr);
            newCreoleWord.ParamField.Add(-1);
            int jumpAddrPFLoc = newCreoleWord.ParamField.Count - 1;
            newCreoleWord.ParamField.Add(elseAddr);
            int zeroBrAddrPFLoc = (int)gsp.Pop(gsp.DataStack);
            newCreoleWord.ParamField[zeroBrAddrPFLoc] = newCreoleWord.ParamField.Count - 1;
            gsp.DataStack.Add(jumpAddrPFLoc);
            gsp.ParamFieldPtr = newCreoleWord.ParamField.Count - 1;
        }

        //  ( -- location ) Compile-time code for THEN
        public void compileThen(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            int branchPFLoc = (int) gsp.Pop(gsp.DataStack);
            string runTimeAction = "doThen." + gsp.BFC.ImmediateVocab;
            int thenAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            newCreoleWord.ParamField.Add(thenAddr);
            newCreoleWord.ParamField[branchPFLoc] = newCreoleWord.ParamField.Count - 1;
        }

        // ( flag -- ) Run-time code for IF
        public void do0Branch(ref GlobalSimpleProps gsp)
        {
            CreoleWord currWord = gsp.Cfb.Address[gsp.InnerPtr];

            List<int> paramField = currWord.ParamField;
            ReturnLoc rLoc = (ReturnLoc)gsp.Pop(gsp.ReturnStack);
            int jumpAddr = paramField[rLoc.ParamFieldAddr];
            int branchFlag = (int)gsp.Pop(gsp.DataStack);
            if (branchFlag == 0)
            {
                gsp.ParamFieldPtr = jumpAddr;
            }
            else
            {
                gsp.ParamFieldPtr++;
            }
            rLoc.ParamFieldAddr = gsp.ParamFieldPtr;
            gsp.ReturnStack.Add(rLoc);
        }

        // ( -- beginLoc ) Compile-time code for BEGIN
        public void compileBegin(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string runTimeAction = "doBegin." + gsp.BFC.ImmediateVocab;
            int beginAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            newCreoleWord.ParamField.Add(beginAddr);
            int beginLoc = newCreoleWord.ParamField.Count - 1;
            gsp.DataStack.Add(beginLoc);
        }

        // ( beginLoc -- ) Compile-time code for UNTIL
        public void compileUntil(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            int beginLoc = (int)gsp.Pop(gsp.DataStack);
            string runTimeAction = "0BRANCH." + gsp.BFC.ImmediateVocab;
            int zeroBranchAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            newCreoleWord.ParamField.Add(zeroBranchAddr);
            newCreoleWord.ParamField.Add(beginLoc);
        }

        // ( -- beginLoc ) Compile-time code for DO
        public void compileDo(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string doStartDoAddrLookup = "doStartDo." + gsp.BFC.ImmediateVocab;
            int doStartDoAddr = gsp.Cfb.Dict[doStartDoAddrLookup].IndexField;
            newCreoleWord.ParamField.Add(doStartDoAddr);
            string doAddrLookup = "doDo." + gsp.BFC.ImmediateVocab;
            int doAddr = gsp.Cfb.Dict[doAddrLookup].IndexField;
            newCreoleWord.ParamField.Add(doAddr);
            int doLoc = newCreoleWord.ParamField.Count - 1;
            gsp.DataStack.Add(doLoc);
        }

        // ( -- beginLoc ) Compile-time code for LOOP
        public void compileLoop(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string runTimeAction = "doLoop." + gsp.BFC.ImmediateVocab;
            int loopAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            int doLoc = newCreoleWord.ParamField.Count - 1;
            newCreoleWord.ParamField.Add(loopAddr);
            newCreoleWord.ParamField.Add(doLoc);
        }

        // ( -- beginLoc ) Compile-time code for +LOOP
        public void compilePlusLoop(ref GlobalSimpleProps gsp)
        {
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord newCreoleWord = gsp.Cfb.Address[newRow];
            string runTimeAction = "doPlusLoop." + gsp.BFC.ImmediateVocab;
            int loopAddr = gsp.Cfb.Dict[runTimeAction].IndexField;
            int doLoc = newCreoleWord.ParamField.Count - 1;
            newCreoleWord.ParamField.Add(loopAddr);
            newCreoleWord.ParamField.Add(doLoc);
        }

        // ( start end -- ) Starts off the Do by getting the start and end
        public void doStartDo(ref GlobalSimpleProps gsp)
        {
            ReturnLoc rLoc = (ReturnLoc)gsp.Pop(gsp.ReturnStack);
            int startIndex = (int)gsp.Pop(gsp.DataStack);
            int loopEnd = (int)gsp.Pop(gsp.DataStack);
            LoopInfo li = new LoopInfo(gsp.LoopLabels[gsp.LoopLabelPtr], startIndex, loopEnd);
            for (int i = 0; i < 3; i++)
            {
                gsp.LoopCurrIndexes.Add(0);
            }
            gsp.LoopLabelPtr++;
            gsp.ReturnStack.Add(li);
            gsp.ReturnStack.Add(rLoc);
        }

        // ( inc -- ) Loops back to doDo until the start >= the end and increments with inc
        public void doPlusLoop(ref GlobalSimpleProps gsp)
        {
            int incVal = (int)gsp.Pop(gsp.DataStack);
            CreoleWord currWord = gsp.Cfb.Address[gsp.InnerPtr];
            List<int> paramField = currWord.ParamField;
            ReturnLoc rLoc = (ReturnLoc)gsp.Pop(gsp.ReturnStack);
            LoopInfo li = (LoopInfo)gsp.Pop(gsp.ReturnStack);
            int jumpAddr = paramField[rLoc.ParamFieldAddr];
            int loopLimit = li.Limit;
            string loopLabel = li.Label;
            int currIndex = li.Index;
            if (incVal == 0)
            {
                loopLimit += incVal;
            }
            else
            {
                loopLimit -= incVal;
            }

            if ((incVal > 0) && (currIndex >= loopLimit) || (incVal < 0) && (currIndex <= loopLimit))
            {
                gsp.ParamFieldPtr++;
                rLoc.ParamFieldAddr = gsp.ParamFieldPtr;
                gsp.LoopLabelPtr--;
            }
            else 
            {
                gsp.ParamFieldPtr = jumpAddr;
                currIndex = currIndex + incVal;
                li.Index = currIndex;
                rLoc.ParamFieldAddr = gsp.ParamFieldPtr;
                gsp.ReturnStack.Add(li);
            }

            if (loopLabel == "I")
            {
                gsp.LoopCurrIndexes[0] = currIndex;
            }
            else if (loopLabel == "J")
            {
                gsp.LoopCurrIndexes[1] = currIndex; ;
            }
            else if (loopLabel == "K")
            {
                gsp.LoopCurrIndexes[2] = currIndex; ;
            }
            else
            {
                Debug.Print("Error: Invalid loop label");
            }
            gsp.ReturnStack.Add(rLoc);
        }

        // doLoop is treated as a special case of doPlusLoop
        // # ( -- ) Loops back to doDo until the start equals the end
        public void doLoop(ref GlobalSimpleProps gsp)
        {
            gsp.DataStack.Add(1);
            string runTimeAction = "dPlusLoop." + gsp.BFC.ImmediateVocab;
            CreoleWord cw = gsp.Cfb.Dict[runTimeAction];
            Codefield codeField = cw.CodeField;
            codeField(ref gsp);
        }

        // ( -- index ) Returns the index of I
        public void doIndexI(ref GlobalSimpleProps gsp)
        {
            gsp.DataStack.Add(gsp.LoopCurrIndexes[0]);
        }

        // ( -- index ) Returns the index of J
        public void doIndexJ(ref GlobalSimpleProps gsp)
        {
            gsp.DataStack.Add(gsp.LoopCurrIndexes[1]);
        }

        // ( -- index ) Returns the index of K
        public void doIndexK(ref GlobalSimpleProps gsp)
        {
            gsp.DataStack.Add(gsp.LoopCurrIndexes[2]);
        }

        // ( -- ) Jumps unconditionally to the parameter field location next to it and is compiled by ELSE 
        public void doJump(ref GlobalSimpleProps gsp)
        {
            CreoleWord currWord = gsp.Cfb.Address[gsp.InnerPtr];
            List<int> paramField = currWord.ParamField;
            int jumpAddr = paramField[gsp.ParamFieldPtr + 1];
            ReturnLoc rLoc = (ReturnLoc)gsp.Pop(gsp.ReturnStack);
            gsp.ParamFieldPtr = jumpAddr;
            rLoc.ParamFieldAddr = gsp.ParamFieldPtr;
            gsp.ReturnStack.Add(rLoc);
        }

        // Don't have to put it before compile-time definition, but you have to in Python so doing it here too.
        // ( address -- ) Run-time code for DOES>. 
        public void doDoes(ref GlobalSimpleProps gsp)
        {
            CreoleWord currWord = gsp.Cfb.Address[gsp.InnerPtr];
            string codeFieldStr = currWord.CodeFieldStr;

            // DOES> has to react differently depending on whether it's inside a colon definition or not
            if (codeFieldStr == "doDoes")
            {
                int execToken = currWord.IndexField;
                Debug.Print("Direct execution of doDoes");
                gsp.ParamFieldPtr = currWord.ParamFieldStart;
                gsp.DataStack.Add(execToken);
                gsp.Cfb.Modules.Interpreter.doColon(ref gsp);
            }
            else
            {
                int execToken = currWord.ParamField[gsp.ParamFieldPtr - 1];
                Debug.Print(gsp.ParamFieldPtr.ToString());
                Debug.Print("Execution token is " + execToken.ToString());
                gsp.DataStack.Add(execToken);
                gsp.Cfb.Modules.Compiler.doExecute(ref gsp);
            }
        }

        //  DOES> <list of runtime actions>. When defining word is created, copies code following it into the child definition
        public void compileDoes(ref GlobalSimpleProps gsp)
        {
            ReturnLoc rLoc = (ReturnLoc)gsp.Pop(gsp.ReturnStack);
            int parentRow = rLoc.DictAddr;
            int newRow = gsp.Cfb.Address.Count - 1;
            CreoleWord parentCreoleWord = gsp.Cfb.Address[parentRow];
            CreoleWord childCreoleWord = gsp.Cfb.Address[newRow];
            string fqNameField = childCreoleWord.FQNameField;
            int doesAddr = gsp.Cfb.Dict["DOES>.FORTH"].IndexField;
            int i = 0;
            childCreoleWord.CodeField = gsp.Cfb.Modules.Compiler.doDoes;
            childCreoleWord.CodeFieldStr = "doDoes";
            // Find the location of the does address in the parent definition
            int startCopyPoint = -1;
            while (i < parentCreoleWord.ParamField.Count)
            {
                if (parentCreoleWord.ParamField[i] == doesAddr)
                {
                    startCopyPoint = i + 1;
                    break;
                }
                else
                {
                    i++;
                }
            }

            // Need the definition's address do doDoes can get it easily either when it's being
            // called from the interpreter from from within a compiled definition
            childCreoleWord.ParamField.Add(newRow);
            childCreoleWord.ParamFieldStart = childCreoleWord.ParamField.Count;
            i = 0;
            while (startCopyPoint < parentCreoleWord.ParamField.Count)
            {
                childCreoleWord.ParamField.Add(parentCreoleWord.ParamField[startCopyPoint]);
                startCopyPoint++;
                i++;
            }
            rLoc.ParamFieldAddr += i;
            gsp.ReturnStack.Add(rLoc);
            gsp.Cfb.Address[newRow] = childCreoleWord;
            gsp.Cfb.Dict[fqNameField] = childCreoleWord;
        }
    }
}
