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

        public void GetCenters(Action<Exception, List<Center>> callback)
        {
            var request = new RestRequest("/api/centers/", Method.GET);
            _client.ExecuteAsync<List<Center>>(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(rslt.ErrorException, rslt.Data);
            });
        }

        public void AddCenter(Center center, Action<Exception, Center> callback, string photo = "")
        {
            var request = new RestRequest("/api/centers/", Method.POST);
            request.AlwaysMultipartFormData = true;
            
            if (photo != "")
            {
                request.AddFile("Photo", photo);
            }

            request.AddParameter("CenterName", center.CenterName);
            request.AddParameter("Address", center.Address);
            request.AddParameter("Limit", center.Limit);
         
            _client.ExecuteAsync<Center>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(rslt.ErrorException, rslt.Data);
            });
        }

        public void DeleteCenter(int id, Action<Exception> callback)
        {
            var request = new RestRequest("/api/centers/" + id + "/", Method.DELETE);
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

        public void AddPerson(Person person, Action<Exception, Person> callback, string photo = "")
        {
            var request = new RestRequest("/api/people/", Method.POST);
            request.AlwaysMultipartFormData = true;
            
            if (photo != "")
            {
                request.AddFile("Photo", photo);
            }
            
            request.AddParameter("Birthday", person.Birthday);
            request.AddParameter("FirstName", person.FirstName);
            request.AddParameter("MiddleName", person.MiddleName);
            request.AddParameter("LastName", person.LastName);
            request.AddParameter("BloodType", person.BloodType);
            request.AddParameter("Address", person.Address);
            request.AddParameter("Sickness", person.Sickness);
            request.AddParameter("Contact", person.Contact);
            request.AddParameter("Members", person.Members);

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
