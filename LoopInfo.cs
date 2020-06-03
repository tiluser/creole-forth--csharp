using System;

namespace CreoleForth
{
    public class LoopInfo
    {
        string label;
        int index;
        int limit;

        public LoopInfo(string label, int index, int limit)
        {
            this.label = label;
            this.index = index;
            this.limit = limit;
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

    }
}

