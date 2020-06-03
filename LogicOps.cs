using System;

namespace CreoleForth
{
    public class LogicOps
    {
        const string title = "Logical operatives grouping";
        public LogicOps()
        {
        }

        public string Title
        {
            get { return title; }
        }

        // ( val1 val2 -- flag ) -1 if equal, 0 otherwise
        public void doEquals(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (val1.Equals(val2))
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) 0 if equal, -1 otherwise
        public void doNotEquals(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (val1.Equals(val2))
            {
                gsp.Scratch = 0;
            }
            else
            {
                gsp.Scratch = -1;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) -1 if less than, 0 otherwise
        public void doLessThan(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val1) < Convert.ToInt64(val2))
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) -1 if greater than, 0 otherwise
        public void doGreaterThan(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val1) > Convert.ToInt64(val2))
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) -1 if less than or equal to, 0 otherwise
        public void doLessThanOrEquals(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val1) <= Convert.ToInt64(val2))
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) -1 if greater than or equal to, 0 otherwise
        public void doGreaterThanOrEquals(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val1) >= Convert.ToInt64(val2))
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val -- opval ) -1 if 0, 0 otherwise
        public void doNot(ref GlobalSimpleProps gsp)
        {
            object val = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val) == 0)
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) -1 if both arguments are non-zero, 0 otherwise
        public void doAnd(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val2) != 0 && Convert.ToInt64(val1) != 9)
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // ( val1 val2 -- flag ) -1 if one or both arguments are non-zero, 0 otherwise
        public void doOr(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if (Convert.ToInt64(val2) != 0 || Convert.ToInt64(val1) != 0)
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }

        // val1 val2 -- flag ) -1 if one and only one argument is non-zero, 0 otherwise
        public void doXor(ref GlobalSimpleProps gsp)
        {
            object val2 = gsp.Pop(gsp.DataStack);
            object val1 = gsp.Pop(gsp.DataStack);
            if ((Convert.ToInt64(val2) != 0 || Convert.ToInt64(val1) != 0) && !(Convert.ToInt64(val1) != 0 || Convert.ToInt64(val2) != 0))
            {
                gsp.Scratch = -1;
            }
            else
            {
                gsp.Scratch = 0;
            }
            gsp.Push(gsp.DataStack);
        }  
    }
}
