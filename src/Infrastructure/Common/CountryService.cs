using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Newtonsoft.Json;

namespace Infrastructure.Common
{
    public class CountryService : ICountryService
    {
        private const string url = "https://freeapi.dnslytics.net/v1/ip2asn/";

        public async Task<string> GetCountryCode(string ip)
        {
#if DEBUG
            if (IsLocalIpAddress(ip))
                return "BG";
#endif
            string response;

            using (var http = new HttpClient())
                response = await http.GetStringAsync(url + ip);

            return JsonConvert.DeserializeObject<CountryJson>(response).Country;
        }

#if DEBUG
        private bool IsLocalIpAddress(string ip)
        {
            try
            {
                var hostIPs = Dns.GetHostAddresses(ip);
                var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (var hostIP in hostIPs)
                {
                    if (IPAddress.IsLoopback(hostIP))
                        return true;

                    foreach (var localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP))
                            return true;
                    }
                }
            }
            catch { }

            return false;
        }
#endif
    }
}
