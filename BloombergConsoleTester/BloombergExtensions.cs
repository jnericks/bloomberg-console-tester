using System;
using Bloomberglp.Blpapi;

namespace BloombergConsoleTester
{
    public static class BloombergExtensions
    {
        public static void AddOverride(this Request request, string fieldId, string fieldValue)
        {
            var overrides = request.GetElement(@"overrides");
            var @override = overrides.AppendElement();
            @override.SetElement(@"fieldId", fieldId);
            @override.SetElement(@"value", fieldValue);
        }

        public static string ToBBDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(@"yyyyMMdd");
        }

        public static void ForEachEvent(this Session session, Action<Event> process)
        {
            var continueToLoop = true;
            while (continueToLoop)
            {
                var @event = session.NextEvent();
                switch (@event.Type)
                {
                    case Event.EventType.PARTIAL_RESPONSE:
                        process(@event);
                        break;
                    case Event.EventType.RESPONSE: // final event
                        process(@event);
                        continueToLoop = false;
                        break;
                }
            }
        }
    }
}