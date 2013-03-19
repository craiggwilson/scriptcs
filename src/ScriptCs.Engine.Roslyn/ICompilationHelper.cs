using System;
using System.Reflection;
using Roslyn.Compilers.Common;
using Roslyn.Scripting;

namespace ScriptCs.Engine.Roslyn
{
    public interface ICompilationHelper : IDisposable
    {
        Assembly LoadAssembly();

        CommonEmitResult EmitSubmissionCompilation(Submission<object> submission);
    }
}
