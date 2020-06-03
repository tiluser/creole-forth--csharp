using System;

namespace CreoleForth
{
    public class BasicForthConstants
    {
        const string smudgeflag = "SMUDGED";
        const string immediatevocab = "IMMEDIATE";
        const string prefiltervocab = "PREFILTER";
        const string postfiltervocab = "POSTFILTER";
        const string execzeroaction = "EXEC0";
        const string complitaction = "COMPLIT";

        public string SmudgeFlag
        {
            get { return smudgeflag; }
        }

        public string ImmediateVocab
        {
            get { return immediatevocab; }
        }

        public string PrefilterVocab
        {
            get { return prefiltervocab; }
        }

        public string PostfilterVocab
        {
            get { return postfiltervocab; }
        }

        public string ExecZeroAction
        {
            get { return execzeroaction; }
        }

        public string CompLitAction
        {
            get { return complitaction; }
        }
    }


}

