namespace Application.Common.Models
{
    public class IpJson
    {
        public CountryNamesJson Country_Names { get; set; }
    }

    public class CountryJson
    {
        public IpJson Ip { get; set; }
    }

    public class CountryNamesJson
    {
        public string En { get; set; }
    }
}
