using System;
using System.Collections.Generic;
using System.Text;

namespace CreoleForth
{
    public class Interpreter
    {
        const string title = "Interpreter grouping";

        public Interpreter()
        {
        }

        public string Title
        {
            get { return title; }
        }

        // ( -- ) Empties the vocabulary stack, then puts ONLY on it
        public void doOnly(ref GlobalSimpleProps gsp)
        {
            gsp.VocabStack.Clear();
            gsp.VocabStack.Add("ONLY");
        }

        // ( -- ) Puts FORTH on the vocabulary stack
        public void doForth(ref GlobalSimpleProps gsp)
        {
            gsp.VocabStack.Add("FORTH");
        }

        // Puts APPSPEC on the vocabulary stack
        public void doAppSpec(ref GlobalSimpleProps gsp)
        {
            gsp.VocabStack.Add("APPSPEC");
        }

        // Splits the input into individual words
        public void doParseInput(ref GlobalSimpleProps gsp)
        {
            string[] parsedInputArray;
            parsedInputArray = gsp.InputArea.Split(' ');
            List<string> parsedInputList = new List<string>();
            parsedInputList.AddRange(parsedInputArray);
            gsp.ParsedInput = parsedInputList;
        }

        // Looks up the word based on its list index and executes whatever is in its code field
        public void doInner(ref GlobalSimpleProps gsp)
        {
            try
            {
                CreoleWord cw = gsp.Cfb.Address[gsp.InnerPtr];
                Codefield cf = cw.CodeField;
                cf(ref gsp);
            }
            catch (System.IndexOutOfRangeException e)
            {
                Console.WriteLine("Error: Stack underflow " + e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        // Searches vocabularies from top to bottom for work. If found, execute. If not, it gets pushed onto the stack.
        public void doOuter(ref GlobalSimpleProps gsp)
        {

            string rawWord = "";
            string fqWord = "";
            bool isFound = false;
            gsp.OuterPtr = 0;
            gsp.InnerPtr = 0;
            gsp.ParamFieldPtr = 0;

            while (gsp.OuterPtr < gsp.ParsedInput.Count)
            {
                if (gsp.Pause == false)
                {
                    rawWord = gsp.ParsedInput[gsp.OuterPtr];
                    int searchVocabPtr = gsp.VocabStack.Count - 1;
                    while (searchVocabPtr >= 0)
                    {
                        fqWord = rawWord.ToUpper() + "." + gsp.VocabStack[searchVocabPtr];
                        if (gsp.Cfb.Dict.ContainsKey(fqWord))
                        {
                            CreoleWord cw = gsp.Cfb.Dict[fqWord];
                            gsp.InnerPtr = cw.IndexField;
                            doInner(ref gsp);
                            isFound = true;
                            break;
                        }
                        else
                        {
                            searchVocabPtr -= 1;
                        }
                    }
                }
                if (isFound == false)
                {
                    gsp.DataStack.Add(rawWord);
                }
                gsp.OuterPtr += 1;
                isFound = false;
            }
             gsp.PADarea.Clear();
        }

        // Run time code for colon definitions
        public void doColon(ref GlobalSimpleProps gsp)
        {
            CreoleWord currWord = gsp.Cfb.Address[gsp.InnerPtr];
            List<int> paramField = currWord.ParamField;
            
            while (gsp.ParamFieldPtr < paramField.Count)
            {
                int addrInPF = paramField[gsp.ParamFieldPtr];
                Codefield codeField = gsp.Cfb.Address[addrInPF].CodeField;
                gsp.ParamFieldPtr++;
                ReturnLoc rLoc = new ReturnLoc(gsp.InnerPtr, gsp.ParamFieldPtr);
                gsp.Scratch = rLoc;
                gsp.Push(gsp.ReturnStack);
                codeField(ref gsp);
                rLoc = (ReturnLoc) gsp.Pop(gsp.ReturnStack);
                gsp.InnerPtr = rLoc.DictAddr;
                gsp.ParamFieldPtr = rLoc.ParamFieldAddr;
            }
        }
    }
}


