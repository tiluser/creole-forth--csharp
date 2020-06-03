using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreoleForth
{
    public class CreoleForthBundle
    {
        private Modules modules;
        public List<CreoleWord> Address;
        public Dictionary<string, CreoleWord> Dict;

        public CreoleForthBundle()
        {
            Address = new List<CreoleWord>();
            Dict = new Dictionary<string, CreoleWord>();
        }

        public void buildPrimitive(string name, Codefield codefield, string codeFieldStr, string vocab, string compAction, string help)
        {

            List<int> paramList = new List<int>();
            List<object> dataList = new List<object>();
            string fqName = name + "." + vocab;
            CreoleWord cw = new CreoleWord(name, codefield, codeFieldStr, vocab, compAction, help, Address.Count() - 1, Address.Count(),
                                           Address.Count() - 1, Address.Count(), paramList, dataList);
            Dict[fqName] = cw;
            Address.Add(cw);
        }

        public void buildHighLevel(ref GlobalSimpleProps gsp, string code, string help)
        {
            gsp.InputArea = code;
            gsp.PADarea.Clear();
            Modules.Interpreter.doParseInput(ref gsp);
            Modules.Interpreter.doOuter(ref gsp);
            Address[Address.Count - 1].HelpField = help;
            CreoleWord newDef = Address[Address.Count - 1];
            Dict[newDef.FQNameField] = newDef;
        }

        public Modules Modules
        {
            get { return modules; }
            set { modules = value;  }
        }
    }
}
