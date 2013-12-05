namespace BloombergConsoleTester
{
    public class RequestType
    {
        public string Name { get; private set; }

        public static RequestType HistoricalDataRequest = new RequestType(@"HistoricalDataRequest");
        public static RequestType ReferenceDataRequest = new RequestType(@"ReferenceDataRequest");

        RequestType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(RequestType requestType)
        {
            return requestType.Name;
        }
    }
}