using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace BusinessLogic.Tests
{
    public class XunitLoggerProvider:ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XunitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XunitLogger(_testOutputHelper, categoryName);

        }

        public void Dispose()
        {
           
        }
    }
}
