using System.IO;
using log4net;

namespace ScriptCs.Engine.Roslyn
{
    public class RoslynScriptAssemblyCompilerEngine : RoslynScriptCompilerEngine
    {
        public RoslynScriptAssemblyCompilerEngine(IScriptHostFactory scriptHostFactory, ILog logger)
            : base(scriptHostFactory, logger)
        {
        }

        protected override ICompilationHelper CreateCompilationHelper()
        {
            var outputPath = Path.Combine(this.BaseDirectory, "output.dll");
            return new AssemblyCompilationHelper(outputPath);
        }
    }
}