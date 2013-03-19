using System;
using System.IO;
using System.Reflection;
using Roslyn.Compilers.Common;
using Roslyn.Scripting;

namespace ScriptCs.Engine.Roslyn
{
    public class DebugCompilationHelper : ICompilationHelper
    {
        private MemoryStream _exeStream;
        private MemoryStream _pdbStream;

        private byte[] _exeBytes;
        private byte[] _pdbBytes;

        private bool _disposed;

        public DebugCompilationHelper()
        {
            _exeStream = new MemoryStream();
            _pdbStream = new MemoryStream();
        }

        public Assembly LoadAssembly()
        {
            return AppDomain.CurrentDomain.Load(_exeBytes, _pdbBytes);
        }

        public CommonEmitResult EmitSubmissionCompilation(Submission<object> submission)
        {
            var result = submission.Compilation.Emit(_exeStream, pdbStream: _pdbStream);

            if (result.Success)
            {
                _pdbBytes = _pdbStream.ToArray();
                _exeBytes = _exeStream.ToArray();
            }
            
            _exeStream.Dispose();
            _pdbStream.Dispose();
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
                    _pdbStream.Dispose();
                }

                _disposed = true;
            }
        }

        ~DebugCompilationHelper()
        {
            Dispose(false);
        }
    }
}