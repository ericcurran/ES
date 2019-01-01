using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BusinessLogic.Tests
{
    internal class XunitLogger : ILogger
    {
        private ITestOutputHelper _testOutputHelper;
        private string _categoryName;

        public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();
            public void Dispose()
            { }
        }
    }
}