using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreoleForth;

namespace CreoleForthConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Coreprims cp = new Coreprims();
            Interpreter interpreter = new Interpreter();
            Compiler compiler = new Compiler();
            LogicOps logicops = new LogicOps();
            AppSpec appspec = new AppSpec();
            Modules modules = new Modules(cp, interpreter, compiler, logicops, appspec);
            List<CompileInfo> PADarea = new List<CompileInfo>();
            CreoleForthBundle cfb1 = new CreoleForthBundle();
            cfb1.Modules = modules;

            GlobalSimpleProps gsp = new GlobalSimpleProps();
            List<string> vs = new List<string>();
            gsp.PADarea = PADarea;

            vs.Add("ONLY");
            vs.Add("FORTH");
            vs.Add("APPSPEC");

            gsp.VocabStack = vs;
            gsp.CurrentVocab = "FORTH";

            // The onlies
            cfb1.buildPrimitive("ONLY", cfb1.Modules.Interpreter.doOnly, "Interpreter.doOnly", "ONLY", "EXECUTE", "( -- ) Empties the vocabulary stack, then puts ONLY on it");
            cfb1.buildPrimitive("FORTH", cfb1.Modules.Interpreter.doForth, "Interpreter.doForth", "ONLY", "EXECUTE", "( -- ) Puts FORTH on the vocabulary stack");
            cfb1.buildPrimitive("APPSPEC", cfb1.Modules.Interpreter.doAppSpec, "Interpreter.doAppSpec", "ONLY", "EXECUTE", "( -- ) Puts APPSPEC on the vocabulary stack");
            cfb1.buildPrimitive("NOP", cfb1.Modules.Coreprims.doNop, "Coreprims.doNop", "ONLY", "COMPINPF", "( -- ) Do-nothing primitive which is surprisingly useful");
            cfb1.buildPrimitive("__#EOL#__", cfb1.Modules.Coreprims.doNop, "Coreprims.doNop", "ONLY", "COMPINPF", "( -- ) EOL Marker");

            // hello world, dialogs, and help
            cfb1.buildPrimitive("HELLO", cfb1.Modules.Coreprims.doHello, "Coreprims.doHello", "FORTH", "COMPINPF", "( -- ) prints out Hello World");

            cfb1.buildPrimitive("TULIP", cfb1.Modules.Coreprims.doTulip, "Coreprims.doTulip", "FORTH", "COMPINPF", "( -- ) pops up a Tulip message");
            cfb1.buildPrimitive("MSGBOX", cfb1.Modules.Coreprims.doMsgBox, "Coreprims.doMsgBox", "FORTH", "COMPINPF", "( msg -- ) pops up an alert box saying the message");
            cfb1.buildPrimitive("VLIST", cfb1.Modules.Coreprims.doVList, "Coreprims.doVList", "FORTH", "COMPINPF", "( -- ) lists the dictionary definitions");

 
            // Basic math
            cfb1.buildPrimitive("+", cfb1.Modules.Coreprims.doPlus, "Coreprims.doPlus", "FORTH", "COMPINPF", "( n1 n2 -- sum ) Adds two numbers on the stack");
            cfb1.buildPrimitive("-", cfb1.Modules.Coreprims.doMinus, "Coreprims.doMinus", "FORTH", "COMPINPF", "( n1 n2 -- difference ) Subtracts two numbers on the stack");
            cfb1.buildPrimitive("*", cfb1.Modules.Coreprims.doMultiply, "Coreprims.doMultiply", "FORTH", "COMPINPF", "( n1 n2 -- product ) Multiplies two numbers on the stack");
            cfb1.buildPrimitive("/", cfb1.Modules.Coreprims.doDivide, "Coreprims.doDivide", "FORTH", "COMPINPF", "( n1 n2 -- quotient ) Divides two numbers on the stack");
            cfb1.buildPrimitive("%", cfb1.Modules.Coreprims.doMod, "Coreprims.doMod", "FORTH", "COMPINPF", "( n1 n2 -- remainder ) Returns remainder of division operation");

            // Date/time handling
            cfb1.buildPrimitive("TODAY", cfb1.Modules.Coreprims.doToday, "Coreprims.doToday", "FORTH", "COMPINPF", "( -- ) Print's today's date");
            cfb1.buildPrimitive("NOW", cfb1.Modules.Coreprims.doNow, "Coreprims.doNow", "FORTH", "COMPINPF", "( -- datetime ) Puts the current datetime on the stack");

            // Stack manipulation
            cfb1.buildPrimitive("DUP", cfb1.Modules.Coreprims.doDup, "Coreprims.doDup", "FORTH", "COMPINPF", "( val --  val val ) Duplicates the argument on top of the stack");
            cfb1.buildPrimitive("SWAP", cfb1.Modules.Coreprims.doSwap, "Coreprims.doSwap", "FORTH", "COMPINPF", "( val1 val2 -- val2 val1 ) Swaps the positions of the top two stack arguments");
            cfb1.buildPrimitive("ROT", cfb1.Modules.Coreprims.doRot, "Coreprims.doRot", "FORTH", "COMPINPF", "( val1 val2 val3 -- val2 val3 val1 ) Moves the third stack argument to the top");
            cfb1.buildPrimitive("-ROT", cfb1.Modules.Coreprims.doMinusRot, "Coreprims.doMinusRot", "FORTH", "COMPINPF", "( val1 val2 val3 -- val3 val1 val2 ) Moves the top stack argument to the third position");
            cfb1.buildPrimitive("NIP", cfb1.Modules.Coreprims.doNip, "Coreprims.doNip", "FORTH", "COMPINPF", "( val1 val2 -- val2 ) Removes second stack argument");
            cfb1.buildPrimitive("TUCK", cfb1.Modules.Coreprims.doTuck, "Coreprims.doTuck", "FORTH", "COMPINPF", "( val1 val2 -- val2 val1 val2 ) Copies top stack argument under second argument");
            cfb1.buildPrimitive("OVER", cfb1.Modules.Coreprims.doOver, "Coreprims.doOver", "FORTH", "COMPINPF", "( val1 val2 -- val1 val2 val1 ) Copies second stack argument to the top of the stack");
            cfb1.buildPrimitive("DROP", cfb1.Modules.Coreprims.doDrop, "Coreprims.doDrop", "FORTH", "COMPINPF", "( val -- ) Drops the argument at the top of the stack");
            cfb1.buildPrimitive(".", cfb1.Modules.Coreprims.doDot, "Coreprims.doDot", "FORTH", "COMPINPF", "( val -- ) Prints the argument at the top of the stack");
            cfb1.buildPrimitive("DEPTH", cfb1.Modules.Coreprims.doDepth, "Coreprims.doDepth", "FORTH", "COMPINPF", "( -- n ) Returns the stack depth");

            // Logical operatives
            cfb1.buildPrimitive("=", cfb1.Modules.LogicOps.doEquals, "LogicOps.doEquals", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if equal, 0 otherwise");
            cfb1.buildPrimitive("<>", cfb1.Modules.LogicOps.doNotEquals, "LogicOps.doNotEquals", "FORTH", "COMPINPF", "( val1 val2 -- flag ) 0 if equal, -1 otherwise");
            cfb1.buildPrimitive("<", cfb1.Modules.LogicOps.doLessThan, "LogicOps.doLessThan", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if less than, 0 otherwise");
            cfb1.buildPrimitive(">", cfb1.Modules.LogicOps.doGreaterThan, "LogicOps.doGreaterThan", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if greater than, 0 otherwise");
            cfb1.buildPrimitive("<=", cfb1.Modules.LogicOps.doLessThanOrEquals, "LogicOps.doLessThanOrEquals", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if less than or equal to, 0 otherwise");
            cfb1.buildPrimitive(">=", cfb1.Modules.LogicOps.doGreaterThanOrEquals, "LogicOps.doGreaterThanOrEquals", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if greater than or equal to, 0 otherwise");

            cfb1.buildPrimitive("NOT", cfb1.Modules.LogicOps.doNot, "LogicOps.doNot", "FORTH", "COMPINPF", "( val -- opval ) -1 if 0, 0 otherwise");
            cfb1.buildPrimitive("AND", cfb1.Modules.LogicOps.doAnd, "LogicOps.doAnd", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if both arguments are non-zero, 0 otherwise");
            cfb1.buildPrimitive("OR", cfb1.Modules.LogicOps.doOr, "LogicOps.doOr", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if one or both arguments are non-zero, 0 otherwise");
            cfb1.buildPrimitive("XOR", cfb1.Modules.LogicOps.doXor, "LogicOps.doXor", "FORTH", "COMPINPF", "( val1 val2 -- flag ) -1 if one and only one argument is non-zero, 0 otherwise");

            // Compiler definitions
            cfb1.buildPrimitive(",", cfb1.Modules.Compiler.doComma, "Compiler.doComma", "FORTH", "COMPINPF", "( n --) Compiles value off the TOS into the next parameter field cell");
            cfb1.buildPrimitive("COMPINPF", cfb1.Modules.Compiler.doComma, "Compiler.doComma", "IMMEDIATE", "COMPINPF", "( n --) Does the same thing as , (comma) - given a different name for ease of reading");
            cfb1.buildPrimitive("EXECUTE", cfb1.Modules.Compiler.doExecute, "Compiler.doExecute", "FORTH", "COMPINPF", "( address --) Executes the word corresponding to the address on the stack");
            cfb1.buildPrimitive(":", cfb1.Modules.Compiler.compileColon, "Compiler.compileColon", "FORTH", "COMPINPF", "( -- ) Starts compilation of a colon definition");
            cfb1.buildPrimitive(";", cfb1.Modules.Compiler.doSemi, "Compiler.doSemi", "IMMEDIATE", "EXECUTE", "( -- ) Terminates compilation of a colon definition");
            cfb1.buildPrimitive("COMPLIT", cfb1.Modules.Compiler.compileLiteral, "Compiler.compileLiteral", "IMMEDIATE", "EXECUTE", "( -- ) Compiles doLit and a literal into the dictionary");
            cfb1.buildPrimitive("doLiteral", cfb1.Modules.Compiler.doLiteral, "Compiler.doLiteral", "IMMEDIATE", "NOP", "( -- lit ) Run-time code that pushes a literal onto the stack");
            cfb1.buildPrimitive("HERE", cfb1.Modules.Compiler.doHere, "Compiler.doHere", "FORTH", "COMPINPF", "( -- location ) Returns address of the next available dictionary location");
            cfb1.buildPrimitive("CREATE", cfb1.Modules.Compiler.doCreate, "Compiler.doCreate", "FORTH", "COMPINPF", "CREATE <name>. Adds a named entry into the dictionary");
            cfb1.buildPrimitive("doDoes", cfb1.Modules.Compiler.doDoes, "Compiler.doDoes", "IMMEDIATE", "COMPINPF", "( address -- ) Run-time code for DOES>");
            cfb1.buildPrimitive("DOES>", cfb1.Modules.Compiler.compileDoes, "Compiler.compileDoes", "FORTH", "COMPINPF",
                                "DOES> <list of runtime actions>. When defining word is created, copies code following it into the child definition");
            cfb1.buildPrimitive("@", cfb1.Modules.Compiler.doFetch, "Compiler.doFetch", "FORTH", "COMPINPF", "( addr -- val ) Fetches the value in the param field  at addr");
            cfb1.buildPrimitive("!", cfb1.Modules.Compiler.doStore, "Compiler.doStore", "FORTH", "COMPINPF", "( val addr --) Stores the value in the param field  at addr");
            cfb1.buildPrimitive("DEFINITIONS", cfb1.Modules.Compiler.doSetCurrentToContext, "Compiler.doSetCurrentToContext", "FORTH",
            "COMPINPF", "(  -- ). Sets the current (compilation) vocabulary to the context vocabulary (the one on top of the vocabulary stack)");
            cfb1.buildPrimitive("IMMEDIATE", cfb1.Modules.Compiler.doImmediate, "Compiler.doImmediate", "FORTH", "COMPINPF",
                                "( -- ) Flags a word as immediate (so it executes instead of compiling inside a colon definition)");

            // Branching compiler definitions
            cfb1.buildPrimitive("IF", cfb1.Modules.Compiler.compileIf, "Compiler.compileIf", "IMMEDIATE", "EXECUTE", "( -- location ) Compile-time code for IF");
            cfb1.buildPrimitive("ELSE", cfb1.Modules.Compiler.compileElse, "Compiler.compileElse", "IMMEDIATE", "EXECUTE", "( -- location ) Compile-time code for ELSE");
            cfb1.buildPrimitive("THEN", cfb1.Modules.Compiler.compileThen, "Compiler.compileThen", "IMMEDIATE", "EXECUTE", "( -- location ) Compile-time code for THEN");
            cfb1.buildPrimitive("0BRANCH", cfb1.Modules.Compiler.do0Branch, "Compiler.do0Branch", "IMMEDIATE", "NOP", "( flag -- ) Run-time code for IF");
            cfb1.buildPrimitive("JUMP", cfb1.Modules.Compiler.doJump, "Compiler.doJump", "IMMEDIATE", "NOP","( -- ) Jumps unconditionally to the parameter field location next to it and is compiled by ELSE");
            cfb1.buildPrimitive("doElse", cfb1.Modules.Coreprims.doNop, "CorePrims.doNOP", "IMMEDIATE", "NOP", "( -- ) Run-time code for ELSE");
            cfb1.buildPrimitive("doThen", cfb1.Modules.Coreprims.doNop, "CorePrims.doNOP", "IMMEDIATE", "NOP", "( -- ) Run-time code for THEN");
            cfb1.buildPrimitive("BEGIN", cfb1.Modules.Compiler.compileBegin, "Compiler.CompileBegin", "IMMEDIATE", "EXECUTE", "( -- beginLoc ) Compile-time code for BEGIN");
            cfb1.buildPrimitive("UNTIL", cfb1.Modules.Compiler.compileUntil, "Compiler.CompileUntil", "IMMEDIATE", "EXECUTE", "( beginLoc -- ) Compile-time code for UNTIL");
            cfb1.buildPrimitive("doBegin", cfb1.Modules.Coreprims.doNop, "CorePrims.doNOP", "IMMEDIATE", "NOP", "( -- ) Run-time code for BEGIN");
            cfb1.buildPrimitive("DO", cfb1.Modules.Compiler.compileDo, "Compiler.compileDo", "IMMEDIATE", "EXECUTE", "( -- beginLoc ) Compile-time code for DO");
            cfb1.buildPrimitive("LOOP", cfb1.Modules.Compiler.compileLoop, "Compiler.compileLoop", "IMMEDIATE", "EXECUTE", "( -- beginLoc ) Compile-time code for LOOP");
            cfb1.buildPrimitive("+LOOP", cfb1.Modules.Compiler.compilePlusLoop, "Compiler.compilePlusLoop", "IMMEDIATE", "EXECUTE", "( -- beginLoc ) Compile-time code for +LOOP");
            cfb1.buildPrimitive("doStartDo", cfb1.Modules.Compiler.doStartDo, "Compiler.doStartDo", "IMMEDIATE", "COMPINPF", "( start end -- ) Starts off the Do by getting the start and end");
            cfb1.buildPrimitive("doDo", cfb1.Modules.Coreprims.doNop, "CorePrims.doNOP", "IMMEDIATE", "COMPINPF", "( -- ) Marker for DoLoop to return to");
            cfb1.buildPrimitive("doLoop", cfb1.Modules.Compiler.doLoop, "Compiler.doLoop", "IMMEDIATE", "COMPINPF", "( -- ) Loops back to doDo until the start equals the end");
            cfb1.buildPrimitive("doPlusLoop", cfb1.Modules.Compiler.doPlusLoop, "Compiler.doPlusLoop", "IMMEDIATE", "COMPINPF", "( inc -- ) Loops back to doDo until the start >= the end and increments with inc");
            cfb1.buildPrimitive("I", cfb1.Modules.Compiler.doIndexI, "Compiler.doIndexI", "FORTH", "COMPINPF", "( -- index ) Returns the index of I");
            cfb1.buildPrimitive("J", cfb1.Modules.Compiler.doIndexJ, "Compiler.doIndexJ", "FORTH", "COMPINPF", "( -- index ) Returns the index of J");
            cfb1.buildPrimitive("K", cfb1.Modules.Compiler.doIndexK, "Compiler.doIndexK", "FORTH", "COMPINPF", "( -- index ) Returns the index of K");

            // Commenting and list compiler
            cfb1.buildPrimitive("//", cfb1.Modules.Compiler.doSingleLineCmts, "Compiler.doSingleLineCmts", "FORTH", gsp.BFC.ExecZeroAction, "( -- ) Single-line comment handling");
            cfb1.buildPrimitive("(", cfb1.Modules.Compiler.doParenCmts, "Compiler.doParenCmts", "FORTH", gsp.BFC.ExecZeroAction, "( -- ) Multiline comment handling");
            cfb1.buildPrimitive("{", cfb1.Modules.Compiler.compileList, "Compiler.compileList", "FORTH", gsp.BFC.ExecZeroAction, "( -- list ) List compiler");

            gsp.Cfb = cfb1;
            // High-level definitions
            
            cfb1.buildHighLevel(ref gsp, ": x  NOP ;", "Sample high-level definition built with the colon compiler"); 
            /*
            cfb1.buildHighLevel(gsp, ": HT IF HELLO ELSE TULIP THEN ;", "My first high-level definition");
            cfb1.buildHighLevel(gsp, ": HT2 NOP NOP NOP ;", "My second high-level definition");
            cfb1.buildHighLevel(gsp, ": TLIT 3 4 5 ;", "Testing literals");
            cfb1.buildHighLevel(gsp, ": TESTBU BEGIN 1 + DUP 10 TULIP > UNTIL . ;", "Testing BEGIN UNTIL");
            cfb1.buildHighLevel(gsp, ": TDL DO HELLO LOOP ;", "Testing DO LOOP");
            cfb1.buildHighLevel(gsp, ": CONSTANT CREATE , DOES> @ ;", "The quintessential defining word");
            */
            cfb1.buildHighLevel(ref gsp, ": H3 HELLO HELLO HELLO ;", "3 hellos");
            CreoleWord cw = cfb1.Address[cfb1.Address.Count - 1];
            cw.ParamField.Add(5);
            cw.ParamField.Add(5);
            cw.ParamField.Add(5);
            cw.HelpField = "3 hellos modified";
            cfb1.Address[cfb1.Address.Count - 1] = cw;


/*
            gsp.DataStack = ds;
            gsp.Scratch = 3;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = 4;
            gsp.Push(gsp.DataStack);
            cp.doPlus(gsp);
            gsp.Pop(gsp.DataStack);
            System.Console.WriteLine(gsp.Scratch);
            cp.doHello(gsp);
            */
//     gsp.InputArea = "CREATE NEWDEF";
//    cfb1.Modules.Interpreter.doParseInput(ref gsp);
//    cfb1.Modules.Interpreter.doOuter(ref gsp);
            gsp.InputArea = "VLIST";
            cfb1.Modules.Interpreter.doParseInput(ref gsp);
            cfb1.Modules.Interpreter.doOuter(ref gsp);
        }
    }
}
