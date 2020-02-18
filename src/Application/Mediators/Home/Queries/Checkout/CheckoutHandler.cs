using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Home.Queries.Checkout
{
    public class CheckoutHandler : IRequestHandler<CheckoutQuery, CheckoutResponse>
    {
        private readonly IConfiguration _configuration;

        private static readonly char[] HEXADECIMAL = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private const string MESSAGE_DIGEST = "SHA256";
        private const string ENCODING = "UTF-8";

        public CheckoutHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<CheckoutResponse> Handle(CheckoutQuery request, CancellationToken cancellationToken)
        {
            const string pppUrl = "https://ppp-test.safecharge.com/ppp/purchase.do?";

            var _params = ConstructParametersMap();
            var _paramsForChecksum = ConstructParametersMapForChecksum();
            var checksum = CalculateChecksum(_paramsForChecksum, _configuration.GetValue<string>("Key:SafeCharge"));
            _params.Add("checksum", checksum);
            var encoded_params = EncodeParameters(_params);

            return Task.FromResult(
                new CheckoutResponse
                {
                    Url = pppUrl + encoded_params
                });
        }

        private string EncodeParameters(Dictionary<string, string> _params)
        {
            var postData = new StringBuilder();
            foreach (var param in _params)
            {
                if (postData.Length != 0)
                    postData.Append("&");

                postData.Append(HttpUtility.UrlEncode(param.Key, Encoding.GetEncoding(ENCODING)));
                postData.Append("=");
                postData.Append(HttpUtility.UrlEncode(param.Value, Encoding.GetEncoding(ENCODING)));
            }

            return postData.ToString();
        }

        private Dictionary<string, string> ConstructParametersMap() =>
            new Dictionary<string, string>
            {
                { "currency", "EUR" },
                { "item_name_1", "5Test Product" },
                { "item_number_1", "1" },
                { "item_quantity_1", "1" },
                { "item_amount_1", "50.00" },
                { "numberofitems", "1" },
                { "encoding", "utf-8" },
                { "merchant_id", "4778151621448449994" },
                { "merchant_site_id", "183073" },
                { "time_stamp", "2019-07-08.09:55:50" },
                { "version", "4.0.0" },
                { "user_token_id", "niki4616@gmail.com" },
                { "user_token", "auto" },
                { "total_amount", "50.00" },
                { "notify_url", "https://localhost:5001" },
                { "success", "https://localhost:5001" },
                { "back", "https://localhost:5001" }
            };

        private Dictionary<string, string> ConstructParametersMapForChecksum() =>
            new Dictionary<string, string>
            {
                { "currency", "EUR" },
                { "item_name_1", "5Test Product" },
                { "item_number_1", "1" },
                { "item_quantity_1", "1" },
                { "item_amount_1", "50.00" },
                { "numberofitems", "1" },
                { "encoding", "utf-8" },
                { "merchant_id", "4778151621448449994" },
                { "merchant_site_id", "183073" },
                { "time_stamp", "2019-07-08.09:55:50" },
                { "version", "4.0.0" },
                { "user_token_id", "niki4616@gmail.com" },
                { "user_token", "auto" },
                { "total_amount", "50.00" },
                { "notify_url", "https://localhost:5001" },
                { "success", "https://localhost:5001" },
                { "back", "https://localhost:5001" }
            };

        private string CalculateChecksum(Dictionary<string, string> _params, string merchantKey)
        {
            var allVals = new StringBuilder();
            allVals.Append(merchantKey);
            foreach (var param in _params)
                allVals.Append(param.Value);

            return GetHash(allVals.ToString());
        }

        private string GetHash(string text)
        {
            byte[] bytes;
            using (var md = HashAlgorithm.Create(MESSAGE_DIGEST))
            {
                if (md == null)
                {
                    Console.Error.WriteLine("SHA256 implementation not found.");
                    return null;
                }

                byte[] encoded;
                try
                {
                    encoded = Encoding.GetEncoding(ENCODING).GetBytes(text);
                }
                catch (EncoderFallbackException ex)
                {
                    Console.Error.WriteLine("Cannot encode text into bytes: " + ex.Message);
                    return null;
                }

                bytes = md.ComputeHash(encoded);
            }

            var sb = new StringBuilder(2 * bytes.Length);
            for (var i = 0; i < bytes.Length; i++)
            {
                var low = bytes[i] & 0x0f;
                var high = (bytes[i] & 0xf0) >> 4;
                sb.Append(HEXADECIMAL[high]);
                sb.Append(HEXADECIMAL[low]);
            }
            return sb.ToString();
        }
    }
}
