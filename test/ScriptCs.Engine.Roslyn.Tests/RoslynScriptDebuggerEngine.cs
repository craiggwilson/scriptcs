﻿using System;
using System.Linq;
using Moq;
using ScriptCs.Contracts;
using ScriptCs.Engine.Roslyn;
using Should;
using Xunit;

namespace ScriptCs.Tests
{
    using ScriptCs.Exceptions;

    public class RoslynScriptDebuggerEngineTests
    {
        private static RoslynScriptDebuggerEngine CreateScriptEngine(
            Mock<IScriptHostFactory> scriptHostFactory = null)
        {
            scriptHostFactory = scriptHostFactory ?? new Mock<IScriptHostFactory>();

            return new RoslynScriptDebuggerEngine(scriptHostFactory.Object);
        }

        public class TheExecuteMethod
        {
            [Fact]
            public void ShouldThrowExceptionThrownByScriptWhenErrorOccurs()
            {
                var code = string.Format(
                    "{0}{1}{2}", 
                    "using System;",
                    Environment.NewLine,
                    @"throw new InvalidOperationException(""InvalidOperationExceptionMessage."");");

                var scriptEngine = CreateScriptEngine();

                var exception = Assert.Throws<ScriptExecutionException>(
                    () =>
                    scriptEngine.Execute(
                        code, Enumerable.Empty<string>(), new ScriptPackSession(Enumerable.Empty<IScriptPack>())));
                
                exception.Message.ShouldContain("line 2");
                exception.Message.ShouldContain("Exception Message: InvalidOperationExceptionMessage.");
            }
        }
    }
}
