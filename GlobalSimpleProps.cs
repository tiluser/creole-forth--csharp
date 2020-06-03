using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CreoleForth
{
    public class GlobalSimpleProps
    {
        private List<string> looplabels;
        private List<int> loopcurrindexes;

        private int  looplabelptr;
        private int  outerptr;
        private int  innerptr;
        private int  paramfieldptr;
        private string inputarea;
        private string outputarea;
        private string currentvocab;
        private string helpcommentfield;
        private string soundfield;
        private string compiledlist;
        private bool minargsswitch;
        private bool pause;
        private object scratch;
    
        private BasicForthConstants bfc;
        private CreoleForthBundle cfb;
        // private boolean pause;
        // private boolean oncontinue;
 
        public GlobalSimpleProps()
        {
            bfc = new BasicForthConstants();
            looplabels = new List<string>(); 
            LoopLabels.Add("I");
            LoopLabels.Add("J");
            LoopLabels.Add("K");
            looplabelptr = 0;
            loopcurrindexes = new List<int>(0);
            LoopCurrIndexes.Add(0);
            LoopCurrIndexes.Add(0);
            LoopCurrIndexes.Add(0);
            
            outerptr = 0;
            innerptr = 0;
            paramfieldptr = 0;
            inputarea = "";
            outputarea = "";
            currentvocab = "";
            helpcommentfield = "";
            soundfield = "";
            compiledlist = "";
            minargsswitch = false;
            bool pause = false;
        }

        public List<object> DataStack
        {
            get; set;
        }

        public List<object> ReturnStack
        {
            get; set;
        }

        public List<string> VocabStack
        {
            get; set;
        }

        public List<string> PrefilterStack
        {
            get; set;
        }
        public List<string> PostfilterStack
        {
            get; set;
        }
        public List<CompileInfo> PADarea
        {
            get; set;
        }

        public List<string> ParsedInput
        {
            get; set;
        }

        public List<string> CompiledList
        {
            get; set;
        }

        public List<string> LoopLabels
        {
            get { return looplabels; }
        }
        public List<int> LoopCurrIndexes
        {
            get { return loopcurrindexes; }
            set { loopcurrindexes = value; }
        }

        public int LoopLabelPtr
        {
            get { return looplabelptr;  }
            set { looplabelptr = value; }
        }

        public int OuterPtr 
        {
            get { return outerptr; }
            set { outerptr = value;    }
        }

        public int InnerPtr 
        {
            get { return innerptr; }
            set { innerptr = value;    }
        }

        public int ParamFieldPtr 
        {
            get { return paramfieldptr; }
            set { paramfieldptr = value;    }
        }

        public string InputArea
        {
            get { return inputarea;  }
            set { inputarea = value; }
        }

        public string OutputArea
        {
            get { return outputarea;  }
            set { outputarea = value; }
        }

        public string CurrentVocab
        {
            get { return currentvocab;  }
            set { currentvocab = value; }
        }

        public string HelpCommentField
        {
            get { return helpcommentfield;  }
            set { helpcommentfield = value; }
        }

        public string SoundField
        {
            get { return soundfield;  }
            set { soundfield = value; }
        }

        public bool MinArgsSwitch
        {
            get { return minargsswitch;  }
            set { minargsswitch = value; }
        }

        public bool Pause
        {
            get { return pause; }
            set { pause = value; }
        }

        public object Scratch
        {
            get { return scratch;  }
            set { scratch = value; }
        }

        public CreoleWord CurrCreoleWord
        {
            get; set;
        }

        public BasicForthConstants BFC
        {
            get { return bfc;  }
        }

        public CreoleForthBundle Cfb
        {
            get; set;
        }

        public object Pop(List<object> val)
        {
            object retVal = null;

            if (val.Count > 0) //prevent IndexOutOfRangeException for empty list
            {
                retVal = val[val.Count - 1];
                val.RemoveAt(val.Count - 1);
            }
            else
            {
                Debug.WriteLine("Stack underflow");
            }
            scratch = retVal;
            return retVal;
        }

        public string Pop(List<string> val)
        {
            string retVal = "";

            if (val.Count > 0) //prevent IndexOutOfRangeException for empty list
            {
                retVal = val[val.Count - 1];
                val.RemoveAt(val.Count - 1);
            }
            else
            {
                Debug.WriteLine("Stack underflow");
            }
            scratch = retVal;
            return retVal;
        }

        public void Push(List<object> val)
        {
            val.Add(scratch);
        }

        //  Clean; up all the inputs
        public void cleanFields()
        {
            DataStack.Clear();
            ReturnStack.Clear();
            PADarea.Clear();
            ParsedInput.Clear();
            LoopCurrIndexes[0] = 0;
            LoopCurrIndexes[1] = 0;
            LoopCurrIndexes[2] = 0;
            looplabelptr = 0;
            outerptr = 0;
            innerptr = 0;
            paramfieldptr = 0;
            inputarea = "";
            outputarea = "";
            helpcommentfield = "";
            pause = false;
        }
    }
}