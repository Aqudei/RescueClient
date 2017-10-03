using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RESTCrud
{
    public abstract class CrudRestClientBase<ToServer, FromServer> : IRestCrudClient<ToServer, FromServer> where FromServer : new()
    {
        protected readonly string baseUrl;
        protected readonly string resouceBase;
        protected RestClient client;

        public CrudRestClientBase(string baseUrl, string resouceBase)
        {
            this.baseUrl = baseUrl;
            this.resouceBase = resouceBase;

            client = new RestSharp.RestClient(baseUrl);
        }

        public void Add(ToServer item, Action<Exception, FromServer> callback)
        {
            var req = new RestSharp.RestRequest(resouceBase + "/", Method.POST);
            req.RequestFormat = DataFormat.Json;
            client.ExecuteAsync<FromServer>(req, res =>
            {
                if (res.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    callback(null, res.Data);
                    return;
                }
                else
                {
                    var msg = string.Format("Failed to add: {0}\n{1}\n{2}", item.GetType().Name,
                        res.ErrorMessage, res.Content.Trim("[]".ToCharArray()));

                    callback(new Exception(msg), res.Data);
                    return;
                }
            });
        }

        public void List(Action<Exception, IEnumerable<FromServer>> callback)
        {
            var req = new RestSharp.RestRequest(resouceBase + "/", Method.GET);

            client.ExecuteAsync<List<FromServer>>(req, res =>
            {
                if (res.StatusCode != System.Net.HttpStatusCode.BadRequest && res.ErrorException != null)
                {
                    callback(null, res.Data);
                    return;
                }
                else
                {
                    var msg = string.Format("Failed to obtain listing.\n{0}\n{1}",
                       res.ErrorMessage, res.Content.Trim("[]".ToCharArray()));

                    callback(new Exception(msg), null);
                    return;
                }
            });

        }
    }
}
