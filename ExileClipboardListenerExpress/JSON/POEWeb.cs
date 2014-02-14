using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;

namespace ExileClipboardListener.JSON
{
    public static class POEWeb
    {
        private const string LoginUrl = @"https://www.pathofexile.com/login";
        private const string CharacterUrl = @"http://www.pathofexile.com/character-window/get-characters";
        private const string StashUrl = @"http://www.pathofexile.com/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}";
        //private const string InventoryUrl = @"http://www.pathofexile.com/character-window/get-items?character={0}";
        private const string HashRegEx = "name=\\\"hash\\\" value=\\\"(?<hash>[a-zA-Z0-9]{1,})\\\"";
        private static CookieContainer _credentialCookies;

        public static bool Authenticate()
        {
            _credentialCookies = new CookieContainer();
            String username = Properties.Settings.Default.Username;
            String password = JSON.StringCipher.Decrypt(Properties.Settings.Default.Password);
            var request = (HttpWebRequest)RequestThrottle.Instance.Create(LoginUrl);
            request.CookieContainer = _credentialCookies;
            request.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 1.1.4322)";
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            HttpWebRequest getHash = request;
            var hashResponse = (HttpWebResponse)getHash.GetResponse();
            string loginResponse = Encoding.Default.GetString(GetMemoryStreamFromResponse(hashResponse).ToArray());
            string hashValue = Regex.Match(loginResponse, HashRegEx).Groups["hash"].Value;
            var request2 = (HttpWebRequest)RequestThrottle.Instance.Create(LoginUrl);
            request2.CookieContainer = _credentialCookies;
            request2.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 1.1.4322)";
            request2.Method = "POST";
            request2.ContentType = "application/x-www-form-urlencoded";
            request2.AllowAutoRedirect = false;
            var data = new StringBuilder();
            data.Append("login_email=" + Uri.EscapeDataString(username));
            data.Append("&login_password=" + Uri.EscapeDataString(password));
            data.Append("&hash=" + hashValue);
            byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());
            request2.ContentLength = byteData.Length;
            Stream postStream = request2.GetRequestStream();
            postStream.Write(byteData, 0, byteData.Length);
            var response = (HttpWebResponse)request2.GetResponse();

            //If we didn't get a redirect then something went very wrong
            return response.StatusCode == HttpStatusCode.Found;
        }

        public static List<Character> GetCharacters()
        {
            //Deserialise a request to get a list of characters
            var deserialiser = new DataContractJsonSerializer(typeof(List<DataContracts.JSONCharacter>));
            var request = (HttpWebRequest)RequestThrottle.Instance.Create(CharacterUrl);
            request.CookieContainer = _credentialCookies;
            request.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 1.1.4322)";
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            var response = (HttpWebResponse)request.GetResponse();
            var stream = GetMemoryStreamFromResponse(response);
            var characters = (List<DataContracts.JSONCharacter>)deserialiser.ReadObject(stream);
            var charList = characters.Select(c => new Character(c)).ToList();
            return charList;
        }

        public static DataContracts.JSONStash GetStash(string league, string tab)
        {
            //Desearilise a stash just to get items
            var desearialiseStash = new DataContractJsonSerializer(typeof(DataContracts.JSONStash));
            var requestStash = (HttpWebRequest)RequestThrottle.Instance.Create(String.Format(StashUrl, league, tab));
            requestStash.CookieContainer = _credentialCookies;
            requestStash.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 1.1.4322)";
            requestStash.Method = "GET";
            requestStash.ContentType = "application/x-www-form-urlencoded";
            var responseStash = (HttpWebResponse)requestStash.GetResponse();
            var streamStash = GetMemoryStreamFromResponse(responseStash);
            var proxy = (DataContracts.JSONStash)desearialiseStash.ReadObject(streamStash);
            //MessageBox.Show(proxy.Items.Count + " Items found!");
            return proxy;
        }

        private static MemoryStream GetMemoryStreamFromResponse(WebResponse response)
        {
            var reader = new StreamReader(response.GetResponseStream());
            byte[] buffer = reader.ReadAllBytes();
            RequestThrottle.Instance.Complete();
            return new MemoryStream(buffer);
        }
    }
}
