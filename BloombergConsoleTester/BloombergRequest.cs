using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkPlus.CommonDomain.ValueObjects;
using BloombergConsoleTester.Core.Models;
using Bloomberglp.Blpapi;

namespace BloombergConsoleTester
{
    public interface IBloomberg
    {
        IEnumerable<TotalReturns> GetTotalReturnsFor(IEnumerable<Index> indexes);
    }

    public class Bloomberg : IBloomberg
    {
        ICreateBloombergSession _sessionFactory;

        public Bloomberg(ICreateBloombergSession sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<TotalReturns> GetTotalReturnsFor(IEnumerable<Index> indexes)
        {
            return indexes.Select(GetTotalReturnsFor);
        }

        TotalReturns GetTotalReturnsFor(Index security)
        {
            var result = new TotalReturns(security);

            using (var session = _sessionFactory.GetSession())
            {
                var service = session.CreateService();

                var priceRequest = service.CreateRequest(RequestType.HistoricalDataRequest);

                priceRequest.Append("securities", security.BloombergName);

                priceRequest.Append("fields", "PX_LAST");
                priceRequest.Set("startDate", new YearMonth(2012, 1).AsDateTime().ToBBDateTimeString());
                priceRequest.Set("endDate", new YearMonth(2013, 12).AsDateTime().ToBBDateTimeString());
                priceRequest.Set("periodicitySelection", "MONTHLY");
                priceRequest.Set("currency", security.CurrencyCode);

                session.ProcessRequest(priceRequest, @event => @event.ForEachFieldData(fdi =>
                {
                    var date = fdi.GetElementAsDate("date").ToSystemDateTime();
                    var lastPrice = fdi.GetElementAsFloat64("PX_LAST");

                    result.Add(date, Convert.ToDecimal(lastPrice));
                }));

                if (security.UseUnderlyingPriceForReturn)
                { // Calculate Return manually
                    foreach (var index in result)
                    {
                        var lastValue = index.GetLastValue();
                        if (index.Value == null || lastValue == null || lastValue == 0m) continue;
                        index.Return = Percent.CreateFromDecimalNotation((index.Value.Value / lastValue) - 1);
                    }
                }
                else
                { // Retrieve Return from Bloomberg
                    foreach (var index in result)
                    {
                        if (index._previous == null) continue;

                        // =BDP("SPX INDEX", "CUST_TRR_RETURN_HOLDING_PER", "CUST_TRR_START_DT,CUST_TRR_END_DT,CUST_TRR_CRNCY", "20121031,20121130,USD")
                        var start = index._previous.DateTime;
                        var end = index.DateTime;

                        var returnRequest = service.CreateRequest(RequestType.ReferenceDataRequest);
                        returnRequest.Append("securities", security.BloombergName);
                        returnRequest.Append("fields", "CUST_TRR_RETURN_HOLDING_PER");
                        returnRequest.AddOverride("CUST_TRR_START_DT", start.ToBBDateTimeString());
                        returnRequest.AddOverride("CUST_TRR_END_DT", end.ToBBDateTimeString());
                        returnRequest.AddOverride("CUST_TRR_CRNCY", security.CurrencyCode);
                        
                        session.ProcessRequest(returnRequest, @event => @event.ForEachFieldData(fdi =>
                        {
                            if (fdi.HasElement("CUST_TRR_RETURN_HOLDING_PER"))
                            {
                                var rtn = fdi.GetElementAsFloat64("CUST_TRR_RETURN_HOLDING_PER");
                                index.Return = Percent.CreateFromPercentageNotation(Convert.ToDecimal(rtn));
                            }
                        }));
                    }
                }
            }

            return result;
        }
    }

    public interface IBloombergSession : IDisposable
    {
        Service CreateService();
        void ProcessRequest(Request request, Action<Event> process);
    }

    public class BloombergSession : IBloombergSession
    {
        Session _session;

        public BloombergSession()
        {
            _session = new Session(new SessionOptions { ServerHost = "localhost", ServerPort = 8194 });
        }

        public Service CreateService()
        {
            const string serviceName = "//blp/refdata";
            if (_session.Start() && _session.OpenService(serviceName))
                return _session.GetService(serviceName);
            else
                throw new Exception("Cannot connect to Bloomberg server. Check that the server host is \"localhost\" or \"127.0.0.1\" and that the server port is 8194.");
        }

        public void ProcessRequest(Request request, Action<Event> process)
        {
            _session.SendRequest(request, null);

            var continueToLoop = true;
            while (continueToLoop)
            {
                var @event = _session.NextEvent();
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

        public void Dispose()
        {
            _session.Dispose();
        }
    }

    public interface ICreateBloombergSession
    {
        IBloombergSession GetSession();
    }

    public class BloombergSessionFactory : ICreateBloombergSession
    {
        public IBloombergSession GetSession()
        {
            return new BloombergSession();
        }
    }
}