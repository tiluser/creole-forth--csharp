using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreoleForth
{
    public delegate void Codefield(ref GlobalSimpleProps gsp);

    public class CreoleWord
    {
        private string namefield;
        private string fqnamefield;
        private Codefield codefield;
        private string codefieldstr;
        private string vocabulary;
        private string compileactionfield;
        private string helpfield;
        private int prevrowlocfield;
        private int rowlocfield;
        private int linkfield;
        private int indexfield;
        public List<int> ParamField;
        public List<object> DataField;
        private int paramfieldstart;

        public CreoleWord(string namefield, Codefield codefield, string codefieldstr, string vocabulary, string compileactionfield,
                          string helpfield, int prevrowlocfield, int rowlocfield, int linkfield, int indexfield, List<int> paramfield,
                          List<object> datafield)
        {
            this.namefield = namefield;
            this.codefield = codefield;
            this.codefieldstr = codefieldstr;
            this.vocabulary = vocabulary;
            this.fqnamefield = namefield + "." + vocabulary;
            this.compileactionfield = compileactionfield;
            this.helpfield = helpfield;
            this.prevrowlocfield = prevrowlocfield;
            this.rowlocfield = rowlocfield;
            this.linkfield = linkfield;
            this.indexfield = indexfield;
            this.ParamField = paramfield;
            this.DataField = datafield;
            this.paramfieldstart = 0;
        }

        public string NameField
        {
            get { return namefield;  }
        }

        public string FQNameField
        {
            get { return fqnamefield; }
        }

        public Codefield CodeField
        {
            get { return codefield; }
            set { codefield = value;  }
        }

        public string CodeFieldStr
        {
            get { return codefieldstr; }
            set { codefieldstr = value; }
        }

        public string Vocabulary
        {
            get { return vocabulary; }
            set { vocabulary = value;  }
        }

        public string CompileActionField
        {
            get { return compileactionfield; }
            set { compileactionfield = value; }
        }

        public string HelpField
        {
            get { return helpfield; }
            set { helpfield = value; }
        }

        public int PrevRowLocField
        {
            get { return prevrowlocfield;  }
        }

        public int RowLocField
        {
            get { return rowlocfield; }
        }

        public int LinkField
        {
            get { return linkfield; }
        }

        public int IndexField
        {
            get { return indexfield; }
        }

        public int ParamFieldStart
        {
            get { return paramfieldstart; }
            set { paramfieldstart = value; }
        }
    }
}
