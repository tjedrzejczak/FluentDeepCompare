using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDeepCompare
{
    public class ComparsionResult
    {
        public bool AreEqual { get; protected set; }
        public string Message { get; protected set; }
        public Exception Exception { get; protected set; }
        public List<ComparsionResult> ComponentResults { get; } = new List<ComparsionResult>();

        public ComparsionResult(bool areEqual, string message, Exception exception = null)
        {
            AreEqual = areEqual;
            Message = message;
            Exception = exception;
        }

        public ComparsionResult(string message, IEnumerable<ComparsionResult> componentResults, Exception exception = null)
            : this(AreAllComponentsEqual(componentResults), message, exception)
        {
            if (componentResults != null)
                ComponentResults.AddRange(componentResults);
        }

        private static bool AreAllComponentsEqual(IEnumerable<ComparsionResult> componentResults)
            => (componentResults is null) || componentResults.All(x => x.AreEqual);

        public string AggregateAllMessages()
            => String.Join(Environment.NewLine, GetAllMessages());

        internal IEnumerable<string> GetAllMessages(bool debug = false)
        {
            if (!String.IsNullOrWhiteSpace(Message))
                yield return Message;

            if (Exception != null)
                yield return Exception.Message;

            if (Exception != null && debug)
                yield return Exception.StackTrace;

            if (ComponentResults != null)
                foreach (string msg in ComponentResults.SelectMany(x => x.GetAllMessages()))
                    yield return msg;
        }
    }
}