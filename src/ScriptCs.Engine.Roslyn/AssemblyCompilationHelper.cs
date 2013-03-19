using System;
using System.IO;
using System.Reflection;
using Roslyn.Compilers.Common;
using Roslyn.Scripting;

namespace ScriptCs.Engine.Roslyn
{
    public class AssemblyCompilationHelper : ICompilationHelper
    {
        private FileStream _exeStream;

        private bool _disposed;

        public AssemblyCompilationHelper(string dllPath)
        {
            _exeStream = new FileStream(dllPath, FileMode.CreateNew);
        }

        public Assembly LoadAssembly()
        {
            if (_exeStream != null)
            {
                return Assembly.LoadFile(_exeStream.Name);
            }

            throw new InvalidOperationException("Cannot load assembly. Stream is null.");
        }

        public CommonEmitResult EmitSubmissionCompilation(Submission<object> submission)
        {
            return submission.Compilation.Emit(_exeStream);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _exeStream.Dispose();
                }

                _disposed = true;
            }
        }

        ~AssemblyCompilationHelper()
        {
            Dispose(false);
        }
    }
}