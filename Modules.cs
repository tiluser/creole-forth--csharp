using System;

namespace CreoleForth
{
    public class Modules
    {
        private Coreprims coreprims;
        private Interpreter interpreter;
        private Compiler compiler;
        private LogicOps logicops;
        private AppSpec appspec;

        public Modules(Coreprims coreprims, Interpreter interpreter, Compiler compiler, LogicOps logicops, AppSpec appspec)
        {
            this.coreprims = coreprims;
            this.interpreter = interpreter;
            this.compiler = compiler;
            this.logicops = logicops;
            this.appspec = appspec;
        }

        public Coreprims Coreprims
        {
            get { return coreprims; }
            set { coreprims = value; }
        }

        public Interpreter Interpreter
        {
            get { return interpreter; }
            set { interpreter = value; }
        }

        public Compiler Compiler
        {
            get { return compiler; }
            set { compiler = value; }
        }

        public LogicOps LogicOps
        {
            get { return logicops; }
            set { logicops = value; }
        }

        public AppSpec AppSpec
        {
            get { return appspec; }
            set { appspec = value; }
        }
    }
}


