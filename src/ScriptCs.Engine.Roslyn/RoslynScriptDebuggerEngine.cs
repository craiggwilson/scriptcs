using log4net;

namespace ScriptCs.Engine.Roslyn
{
    public class RoslynScriptDebuggerEngine : RoslynScriptCompilerEngine
    {
        public RoslynScriptDebuggerEngine(IScriptHostFactory scriptHostFactory, ILog logger)
            : base(scriptHostFactory, logger)
        {
        }

        protected override ICompilationHelper CreateCompilationHelper()
        {
            return new DebugCompilationHelper();
        }
    }
}