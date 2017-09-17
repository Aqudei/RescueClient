using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Extensions;
using RescueApp.Models;
using System.Diagnostics;

namespace RescueApp
{
    public class RescueClient
    {
        private readonly RestClient _client;

        public RescueClient()
        {
            _client = new RestClient();
            _client.BaseUrl = new Uri("http://localhost:8000");
        }

        public void GetPeople(Action<Exception, List<Person>> callback)
        {
            var request = new RestRequest("/api/people/", Method.GET);
            _client.ExecuteAsync<List<Person>>(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(rslt.ErrorException, rslt.Data);
            });
        }

        public void AddPerson(Person person, Action<Exception, Person> callback)
        {
            var request = new RestRequest("/api/people/", Method.POST);
            request.RequestFormat = DataFormat.Json;
          
            request.AddBody(person);
            _client.ExecuteAsync<Person>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(rslt.ErrorException, rslt.Data);
            });
        }
    }
}
