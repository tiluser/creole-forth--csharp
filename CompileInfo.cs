using System;

namespace CreoleForth
{
    public class CompileInfo
    {
        string fqName;
        object address;
        string compileAction;

        public CompileInfo(string fqName, object address, string compileAction)
        {
            this.fqName = fqName;
            this.address = address;
            this.compileAction = compileAction;
        }

        public string FQName
        {
            get { return fqName; }
            set { fqName = value; }
        }

        public object Address
        {
            get { return address; }
            set { address = value; }
        }

        public string CompileAction
        {
            get { return compileAction; }
            set { compileAction = value; }
        }

    }
}

