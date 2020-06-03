using System;

namespace CreoleForth
{
    public class ReturnLoc
    {
        int dictaddr;
        int paramfieldaddr;
        public ReturnLoc(int dictaddr, int paramfieldaddr)
        {
            this.dictaddr = dictaddr;
            this.paramfieldaddr = paramfieldaddr;
        }

        public int DictAddr
        {
            get { return dictaddr; }
            set { dictaddr = value;  }
        }

        public int ParamFieldAddr
        {
            get { return paramfieldaddr; }
            set { paramfieldaddr = value; }
        }
    }
}
