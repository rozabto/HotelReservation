using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Application.Common.Interfaces;

namespace Infrastructure.Common
{
    public class CheckoutService : ICheckoutService
    {
        private static readonly char[] HEXADECIMAL = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private const string MESSAGE_DIGEST = "SHA256";
        private const string ENCODING = "UTF-8";

        public string GenerateCheckout(Dictionary<string, string> parameters, string marchantKey)
        {
            const string pppUrl = "https://ppp-test.safecharge.com/ppp/purchase.do?";

            var checksum = CalculateChecksum(parameters, marchantKey);
            parameters.Add("checksum", checksum);
            var encoded_params = EncodeParameters(parameters);

            return pppUrl + encoded_params;
        }

        public string CalculateChecksum(Dictionary<string, string> _params, string merchantKey)
        {
            var allVals = new StringBuilder();
            allVals.Append(merchantKey);
            foreach (var param in _params)
                allVals.Append(param.Value);

            return GetHash(allVals.ToString());
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
