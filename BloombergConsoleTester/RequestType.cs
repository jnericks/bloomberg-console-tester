namespace BloombergConsoleTester
{
    public class RequestType
    {
        public string Type { get; private set; }

        public static RequestType HistoricalDataRequest = new RequestType(@"HistoricalDataRequest");
        public static RequestType ReferenceDataRequest = new RequestType(@"ReferenceDataRequest");

        RequestType(string type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type;
        }

        public static implicit operator string(RequestType requestType)
        {
            return requestType.Type;
        }
    }
}