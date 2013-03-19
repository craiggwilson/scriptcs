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

        private string _fileName;

        public AssemblyCompilationHelper(string dllPath)
        {
            _exeStream = new FileStream(dllPath, FileMode.OpenOrCreate);
        }

        public Assembly LoadAssembly()
        {
            return Assembly.LoadFile(_fileName);
        }

        public CommonEmitResult EmitSubmissionCompilation(Submission<object> submission)
        {
            var result = submission.Compilation.Emit(_exeStream);

            if (result.Success)
            {
                _fileName = _exeStream.Name;
            }

            _exeStream.Dispose();
            return result;
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