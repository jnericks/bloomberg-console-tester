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

        public static void ForEachFieldData(this Event @event, Action<Element> process)
        {
            foreach (var message in @event.GetMessages())
            {
                if (message.MessageType.ToString() == "HistoricalDataResponse")
                {
                    var securityData = message["securityData"];
                    var fieldDatas = securityData["fieldData"];
                    for (var idx = 0; idx < fieldDatas.NumValues; idx++)
                    {
                        var fieldData = fieldDatas.GetValueAsElement(idx);
                        process(fieldData);
                    }
                }
                else if (message.MessageType.ToString() == "ReferenceDataResponse")
                {
                    var securityDatas = message["securityData"];
                    for (int idx = 0; idx < securityDatas.NumValues; idx++)
                    {
                        var securityData = securityDatas.GetValueAsElement(idx);
                        var fieldData = securityData["fieldData"];
                        process(fieldData);
                    }
                }
            }
        }
    }
}