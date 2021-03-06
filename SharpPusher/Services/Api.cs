﻿using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharpPusher.Services
{
    public abstract class Api
    {
        /// <summary>
        /// Name of the Api service. Used for showing the name in GUI.
        /// </summary>
        public abstract string ApiName { get; }


        /// <summary>
        /// Broadcasts a signed raw transactions in hex format.
        /// </summary>
        /// <param name="txHex">Signed raw transaction in hex format</param>
        /// <returns>Result of broadcasting.</returns>
        public abstract Task<Response<string>> PushTx(string txHex);


        /// <summary>
        /// Broadcasts a signed raw transactions in hex format.
        /// </summary>
        /// <param name="txHex">Signed raw transaction in hex format</param>
        /// <param name="jKey">The JSON key used for making the HttpContent in JSON format.</param>
        /// <param name="url">Api url to use.</param>
        /// <returns>Result of broadcasting.</returns>
        protected static async Task<Response<string>> PushTx(string txHex, string jKey, string url)
        {
            Response<string> resp = new Response<string>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    JObject tx = new JObject() 
                    {
                        {jKey, txHex}
                    };

                    HttpResponseMessage httpResp = await client.PostAsync(url, new StringContent(tx.ToString()));
                    resp.Result = await httpResp.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    string errMsg = (ex.InnerException == null) ? ex.Message : ex.Message + " " + ex.InnerException;
                    resp.Errors.Add(errMsg);
                }
            }

            return resp;
        }

    }
}
