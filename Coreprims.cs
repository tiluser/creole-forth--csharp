using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CreoleForth
{
    public class Coreprims
    {

        public void doNop(ref GlobalSimpleProps gsp)
        {
            // do-nothing primitive that's very useful.
        }

        // ( n1 n2 -- sum ) Adds two numbers on the stack
        public void doPlus(ref GlobalSimpleProps gsp)
        {
            object arg1, arg2, sum;
            arg2 = gsp.Pop(gsp.DataStack);
            arg1 = gsp.Pop(gsp.DataStack);
            sum = Convert.ToDouble(arg1) + Convert.ToDouble(arg2);
            gsp.Scratch = sum;
            gsp.Push(gsp.DataStack);
        }

        // ( n1 n2 -- diff ) Subtracts two numbers on the stack
        public void doMinus(ref GlobalSimpleProps gsp)
        {
            object arg1, arg2, diff;
            arg2 = gsp.Pop(gsp.DataStack);
            arg1 = gsp.Pop(gsp.DataStack);
            diff = Convert.ToDouble(arg1) - Convert.ToDouble(arg2);
            gsp.Scratch = diff;
            gsp.Push(gsp.DataStack);
        }

        // ( n1 n2 -- product ) Multiplies two numbers on the stack
        public void doMultiply(ref GlobalSimpleProps gsp)
        {
            object arg1, arg2, product;
            arg2 = gsp.Pop(gsp.DataStack);
            arg1 = gsp.Pop(gsp.DataStack);
            product = Convert.ToDouble(arg1) * Convert.ToDouble(arg2);
            gsp.Scratch = product;
            gsp.Push(gsp.DataStack);
        }

        // ( n1 n2 -- quotient ) Divides two numbers on the stack
        public void doDivide(ref GlobalSimpleProps gsp)
        {
            object arg1, arg2, quotient;
            arg2 = gsp.Pop(gsp.DataStack);
            arg1 = gsp.Pop(gsp.DataStack);
            quotient = Convert.ToDouble(arg1) / Convert.ToDouble(arg2);
            gsp.Scratch = quotient;
            gsp.Push(gsp.DataStack);
        }

        // ( n1 n2 -- remainder ) Returns remainder of division operation
        public void doMod(ref GlobalSimpleProps gsp)
        {
            object arg1, arg2, remainder;
            arg2 = gsp.Pop(gsp.DataStack);
            arg1 = gsp.Pop(gsp.DataStack);
            remainder = (int) arg1 % (int) arg2;
            gsp.Scratch = remainder;
            gsp.Push(gsp.DataStack);
        }

        // ( val -- val val ) Duplicates the argument on top of the stack
        public void doDup(ref GlobalSimpleProps gsp)
        {
            object arg;
            arg = gsp.Pop(gsp.DataStack);
            gsp.Push(gsp.DataStack);
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- val2 val1 ) Swaps the positions of the top two stack arguments
        public void doSwap(ref GlobalSimpleProps gsp)
        {
            object arg1, arg2;
            arg2 = gsp.Pop(gsp.DataStack);
            arg1 = gsp.Pop(gsp.DataStack);
            gsp.Scratch = arg2;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = arg1;
            gsp.Push(gsp.DataStack);
        }
        
        // ( val1 val2 val3 -- val2 val3 val1 ) Moves the third stack argument to the top
        public void doRot(ref GlobalSimpleProps gsp)
        {
            object val3 = gsp.Pop(gsp.DataStack);
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);

            gsp.Scratch = val2;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val3;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val1;
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 val3 -- val3 val1 val2 ) Moves the top stack argument to the top
        public void doMinusRot(ref GlobalSimpleProps gsp)
        {
            object val3 = gsp.Pop(gsp.DataStack);
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);

            gsp.Scratch = val3;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val1;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val2;
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- val2 ) Removes second stack argument
        public void doNip(ref GlobalSimpleProps gsp)
        {
            object val1 = gsp.Pop(gsp.DataStack);
            gsp.Pop(gsp.DataStack);
            gsp.Scratch = val1;
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- val2 val1 val2 ) Copies top stack argument under second argument
        public void doTuck(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            gsp.Scratch = val2;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val1;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val2;
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- val1 val2 val1 ) Copies second stack argument to the top of the stack
        public void doOver(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            gsp.Scratch = val1;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val2;
            gsp.Push(gsp.DataStack);
            gsp.Scratch = val1;
            gsp.Push(gsp.DataStack);
        }

        // ( val -- ) Drops the argument at the top of the stack
        public void doDrop(ref GlobalSimpleProps gsp)
        {
            gsp.Pop(gsp.DataStack);
        }

        // ( val -- ) Prints the argument at the top of the stack
        public void doDot(ref GlobalSimpleProps gsp)
        {
            gsp.Scratch = gsp.Pop(gsp.DataStack);
            Console.WriteLine(gsp.Scratch);
        }

        // ( -- n ) Returns the stack depth
        public void doDepth(ref GlobalSimpleProps gsp)
        {
            gsp.Scratch = gsp.DataStack.Count;
            gsp.Push(gsp.DataStack);
        }

        // ( -- ) prints out Hello World
        public void doHello(ref GlobalSimpleProps gsp)
        {
            // The quintessential "Hello world" instruction
            //System.Diagnostics.Debug.Write("Hello world");
            Console.WriteLine("Hello world");
        }

        // ( -- ) pops up a Tulip message
        public void doTulip(ref GlobalSimpleProps gsp)
        {
            MessageBox.Show("Tulip");
        }

        // ( msg -- ) pops up an alert box saying the message
        public void doMsgBox(ref GlobalSimpleProps gsp)
        {
            string message = (string) gsp.Pop(gsp.DataStack);
            MessageBox.Show(message);
        }

        // ( -- ) lists the dictionary definitions
        public void doVList(ref GlobalSimpleProps gsp)
        {
            CreoleWord cw;
            List<string> definitionTable = new List<string>();
            //        definitionTable.Add("Index    Name    Vocabulary    Code Field    Param Field    Data Field    Help Field");
            //       definitionTable.Add("-----    ----    ----------    ----------    -----------    ----------    ----------");
            definitionTable.Add("Index    Name    Vocabulary    Param Field    Data Field    Help Field");
            definitionTable.Add("-----    ----    ----------    -----------    ----------    ----------");
            for (int i =0; i <= gsp.Cfb.Address.Count - 1; i++)
            {
                cw = gsp.Cfb.Address[i];
                string pfStr = "";
                for (int j = 0; i < cw.ParamField.Count; j++)
                {
                    pfStr += cw.ParamField[j].ToString() + "x";
                //    System.Console.WriteLine("pfStr");
                }


                
                if (cw != null)
                {
                    definitionTable.Add(cw.IndexField.ToString() + " " +
                        cw.NameField    + " " +
                        cw.Vocabulary   + " " +
                        //       cw.CodeFieldStr + " " +
                        cw.ParamField.Count.ToString() + " " + pfStr + " " +  
                  //      cw.DataField.ToString() + " "  + 
                        cw.HelpField
                        );
                }
;            }
            foreach (string dftVal in definitionTable)
            {
                Console.WriteLine(dftVal);
            }
        }

        // ( -- ) Print's today's date
        public void doToday(ref GlobalSimpleProps gsp)
        {
            string today = DateTime.Now.Date.ToString();
            Console.WriteLine(today);
        }

        // ( -- datetime ) Puts the current datetime on the stack
        public void doNow(ref GlobalSimpleProps gsp)
        {
            DateTime now = DateTime.Now;
            gsp.Scratch = now;
            gsp.Push(gsp.DataStack);
        }
    }
}