using System;
using System.IO;
using System.Net;

namespace Core.External
{
    // WOLFRAM API KEY           : 3A3XVA-U94H7LH2JK
    // HOME PAGE FOR API         : https://products.wolframalpha.com/api/
    // DOCS FOR API WE ARE USING : https://products.wolframalpha.com/short-answers-api/documentation/
    //read the links provided above if you want to understand what this code does.

    public static class Wolfram
    {
        private const string URL = "http://api.wolframalpha.com/v1/result";
        private const string KEY = "3A3XVA-U94H7LH2JK";

        /// <summary>
        /// queries wolfram api to get result for a query
        /// </summary>
        /// <param name="query">the query for which a reult will be reutrned</param>
        /// <returns>result of the query from wolfram api</returns>
        public static string GetSolution(string query)
        {
            Console.WriteLine($"[DBG]     : calling wolfram for response to : {query}");
            // set request url and parameters
            WebRequest req = WebRequest.Create($"{URL}?appid={KEY}&i={query}%3f");
            req.Credentials = CredentialCache.DefaultCredentials;
            // send request
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); 
            Console.WriteLine($"[WLF] STT : {resp.StatusDescription}");
            // default return value
            string result = "err"; 
            // read response data
            using (Stream dat = resp.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dat))
                {
                    // set read data to a var
                    result = (reader.ReadToEnd()); 
                }
            }
            return result; // return response data
        }

        /// <summary>
        /// test your connection to api
        /// </summary>
        /// <returns>connection status</returns>
        public static bool Connected()
        {
            try
            {
                // try to get solution to 1=1 (should be true)
                GetSolution("1=1");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
