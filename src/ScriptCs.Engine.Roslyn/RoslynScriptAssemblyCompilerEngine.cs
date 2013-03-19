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
            const string DllExtension = ".dll";
            var extension = Path.GetExtension(this.FileName);
            var dllName = string.IsNullOrEmpty(extension) ? 
                string.Concat(this.FileName, DllExtension) :
                this.FileName.Replace(extension, ".dll");
            var outputPath = Path.Combine(this.BaseDirectory, dllName);
            return new AssemblyCompilationHelper(outputPath);
        }
    }
}