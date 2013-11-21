using System;

//using BEmu; //un-comment this line to use the Bloomberg API Emulator
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BenchmarkPlus.CommonDomain.Extensions;
using BenchmarkPlus.CommonDomain.ValueObjects;
using Bloomberglp.Blpapi; //un-comment this line to use the actual Bloomberg API

namespace BloombergConsoleTester
{ //un-comment this line to use the Bloomberg API Emulator
    //using Bloomberglp.Blpapi; //un-comment this line to use the actual Bloomberg API

    public static class HistoricalDataRequest
    {
        const string serviceName = "//blp/refdata";
        const string periodicitySelection = "MONTHLY";
        const string currencyCode = "USD";

        public static void RunExample(string security)
        {

            var session = new Session(new SessionOptions { ServerHost = "localhost", ServerPort = 8194 });

            if (session.Start() && session.OpenService(serviceName))
            {
                var service = session.GetService(serviceName);
                if (service == null)
                {
                    Console.WriteLine("Service is null");
                }
                else
                {
                    var priceRequest = service.CreateRequest(RequestType.HistoricalDataRequest);

                    priceRequest.Append("securities", security);

                    priceRequest.Append("fields", "PX_LAST");
                    priceRequest.Set("startDate", new YearMonth(2012, 1).AsDateTime().ToBBDateTimeString());
                    priceRequest.Set("endDate", new YearMonth(2013, 11).AsDateTime().ToBBDateTimeString());
                    priceRequest.Set("periodicitySelection", periodicitySelection);
                    priceRequest.Set("currency", currencyCode);
                    //priceRequest.Set("maxDataPoints", 12);

                    session.SendRequest(priceRequest, null);
                    
                    var collection = new BBIndexReturnCollection();
                    session.ForEachEvent(@event => ForEachFieldData(@event, fdi =>
                    {
                        var date = fdi.GetElementAsDate("date").ToSystemDateTime();
                        var lastPrice = fdi.GetElementAsFloat64("PX_LAST");

                        collection.Add(date, Convert.ToDecimal(lastPrice));
                    }));

                    foreach (var index in collection)
                    {
                        if (index._previous == null) continue;

                        // =BDP("SPX INDEX", "CUST_TRR_RETURN_HOLDING_PER", "CUST_TRR_START_DT,CUST_TRR_END_DT,CUST_TRR_CRNCY", "20121031,20121130,USD")
                        var start = index._previous.DateTime;
                        var end = index.DateTime;

                        var returnRequest = service.CreateRequest(RequestType.ReferenceDataRequest);
                        returnRequest.Append("securities", security);
                        returnRequest.Append("fields", "CUST_TRR_RETURN_HOLDING_PER");
                        returnRequest.Append("fields", "CUST_TRR_START_DT");
                        returnRequest.Append("fields", "CUST_TRR_END_DT");
                        returnRequest.Append("fields", "CUST_TRR_CRNCY");
                        returnRequest.AddOverride("CUST_TRR_START_DT", start.ToBBDateTimeString());
                        returnRequest.AddOverride("CUST_TRR_END_DT", end.ToBBDateTimeString());
                        returnRequest.AddOverride("CUST_TRR_CRNCY", currencyCode);

                        session.SendRequest(returnRequest, null);

                        //session.ForEachEvent(PrintEventMessages);
                        session.ForEachEvent(@event => ForEachFieldData(@event, fdi =>
                        {
                            if (fdi.HasElement("CUST_TRR_RETURN_HOLDING_PER")
                                && fdi.HasElement("CUST_TRR_START_DT")
                                && fdi.HasElement("CUST_TRR_END_DT")
                                && fdi.HasElement("CUST_TRR_CRNCY"))
                            {
                                var sd = fdi.GetElementAsDatetime("CUST_TRR_START_DT");
                                var ed = fdi.GetElementAsDatetime("CUST_TRR_END_DT");
                                var crncy = fdi.GetElementAsString("CUST_TRR_CRNCY");
                                var rtn = fdi.GetElementAsFloat64("CUST_TRR_RETURN_HOLDING_PER");
                                Console.WriteLine("{0}\t{1}\t\t{2}\t{3}\t{4}\t{5}", index.DateTime.ToShortDateString(), index.Value, Percent.CreateFromPercentageNotation(Convert.ToDecimal(rtn)).ToString("0.00"), sd, ed, crncy);
                                index.Return = Percent.CreateFromPercentageNotation(Convert.ToDecimal(rtn));
                            }
                        }));
                    }

                    //foreach (var index in collection)
                    //{
                    //    Console.WriteLine("{0:yyyy-MM-dd}: VALUE = {1}, RETURN = {2}", index.DateTime, index.Value, index.Return.IfNotNull(x => x.ToString(@"0.0000")));
                    //}
                }
            }
            else
            {
                Console.WriteLine("Cannot connect to server.  Check that the server host is \"localhost\" or \"127.0.0.1\" and that the server port is 8194.");
            }
        }
    
        static void PrintEventMessages(Event @event)
        {
            var messages = @event.GetMessages().ToList();
            Console.WriteLine("EventType: {0}", @event.Type);
            Console.WriteLine("Message Count: {0}", messages.Count);
            for (int i = 0; i < messages.Count; i++)
            {
                var message = messages[i];
                Console.WriteLine(string.Empty);
                Console.WriteLine(@"Message {0}", i);
                Console.WriteLine(@"-----");
                Console.WriteLine(message.ToString());
                Console.WriteLine(string.Empty);
            }
        }

        static void ForEachFieldData(Event @event, Action<Element> processFieldDataItem)
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
                        processFieldDataItem(fieldData);
                    }
                }
                else if (message.MessageType.ToString() == "ReferenceDataResponse")
                {
                    var securityDatas = message["securityData"];
                    for (int idx = 0; idx < securityDatas.NumValues; idx++)
                    {
                        var securityData = securityDatas.GetValueAsElement(idx);
                        var fieldData = securityData["fieldData"];
                        processFieldDataItem(fieldData);
                    }
                }
            }
        }

        static void AddToCollection(Event @event, BBIndexReturnCollection collection)
        {
            ForEachFieldData(@event, elmValues =>
            {
                var date = elmValues.GetElementAsDate("date").ToSystemDateTime();
                var lastPrice = elmValues.GetElementAsFloat64("PX_LAST");

                collection.Add(date, Convert.ToDecimal(lastPrice));
            });
        }
    }
}
