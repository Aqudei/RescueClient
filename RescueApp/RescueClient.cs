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

        public void AddPerson(Person person, Action<Exception, Person> callback, string photo = "")
        {
            var request = new RestRequest("/api/people/", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.DateFormat = @"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz";
            if (photo != "")
            {
                request.AddFile("Photo", photo);
            }

            if (person.Birthday.HasValue)
            {
                request.AddParameter("Birthday", person.Birthday.Value.ToShortDateString());
            }

            request.AddParameter("FirstName", person.FirstName);
            request.AddParameter("MiddleName", person.MiddleName);
            request.AddParameter("LastName", person.LastName);
            request.AddParameter("BloodType", person.BloodType);
            request.AddParameter("Address", person.Address);

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

        public void DeletePerson(int id, Action<Exception> callback)
        {
            var request = new RestRequest("/api/people/" + id + "/", Method.DELETE);
            _client.ExecuteAsync(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    callback(null);
                }
                else
                {
                    callback(new Exception("Error on Delete"));
                }
            });
        }
    }
}
