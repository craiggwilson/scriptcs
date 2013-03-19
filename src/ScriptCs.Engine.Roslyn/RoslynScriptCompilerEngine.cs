using System;
using System.Linq;
using System.Reflection;
using log4net;
using Roslyn.Scripting;
using ScriptCs.Exceptions;

namespace ScriptCs.Engine.Roslyn
{
    public abstract class RoslynScriptCompilerEngine : RoslynScriptEngine
    {
        private const string CompiledScriptClass = "Submission#0";
        private const string CompiledScriptMethod = "<Factory>";
        private readonly ILog _logger;

        public RoslynScriptCompilerEngine(IScriptHostFactory scriptHostFactory, ILog logger)
            : base(scriptHostFactory, logger)
        {
            this._logger = logger;
        }

        protected abstract ICompilationHelper CreateCompilationHelper();

        protected override void Execute(string code, Session session)
        {
            _logger.Debug("Compiling submission");
            var submission = session.CompileSubmission<object>(code);

            using (ICompilationHelper compilationHelper = this.CreateCompilationHelper())
            {
                var result = compilationHelper.EmitSubmissionCompilation(submission);
                bool compileSuccess = result.Success;

                if (result.Success)
                {
                    _logger.Debug("Compilation was successful.");
                }
                else
                {
                    var errors = string.Join(Environment.NewLine, result.Diagnostics.Select(x => x.ToString()));
                    _logger.ErrorFormat("Error occurred when compiling: {0})", errors);
                }

                if (compileSuccess)
                {
                    var assembly = compilationHelper.LoadAssembly();
                    var type = assembly.GetType(CompiledScriptClass);
                    var method = type.GetMethod(CompiledScriptMethod, BindingFlags.Static | BindingFlags.Public);

                    try
                    {
                        method.Invoke(null, new[] { session });
                    }
                    catch (Exception e)
                    {
                        _logger.Error("An error occurred when executing the scripts.");
                        var message = string.Format(
                            "Exception Message: {0} {1}Stack Trace:{2}",
                            e.InnerException.Message,
                            Environment.NewLine,
                            e.InnerException.StackTrace);
                        throw new ScriptExecutionException(message);
                    }
                }
            }
        }
    }
}