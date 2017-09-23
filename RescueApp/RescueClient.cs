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
        public void AddPerson(Person person, Action<Exception, Person> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/people/", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(person);
            _client.ExecuteAsync<Person>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false)
                {
                    var reqUpload = new RestRequest("/api/people/" + rslt.Data.Id + "/upload/", Method.PATCH);
                    reqUpload.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<Person>(reqUpload, rsltUpload =>
                    {
                        if (rsltUpload.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            rsltUpload.Data.Photo = NormalizeUri(rsltUpload.Data.Photo);
                            callback(null, rsltUpload.Data);
                        }
                        else
                        {
                            callback(new Exception(), null);
                        }
                    });
                }

                callback(null, rslt.Data);
            });
        }
        public void UpdatePerson(PersonMinusPhoto person,
            Action<Exception, Person> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/people/" + person.Id + "/", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(person);
            _client.ExecuteAsync<Person>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (string.IsNullOrEmpty(choosenPhoto) == false
                        && IsRemoteUri(choosenPhoto) == false)
                    {
                        var uploadRequest = new RestRequest("/api/people/" + person.Id + "/upload/", Method.PATCH);
                        uploadRequest.AddFile("Photo", choosenPhoto);

                        _client.ExecuteAsync<Person>(uploadRequest, r =>
                        {
                            if (r.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                r.Data.Photo = NormalizeUri(r.Data.Photo);
                                callback(null, r.Data);
                            }
                        });
                    }
                    callback(null, rslt.Data);
                }
                else
                {
                    callback(new Exception("Error on Delete"), null);
                }
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

        public void GetHouseholds(Action<Exception, List<Household>> callback)
        {
            var request = new RestRequest("/api/households/", Method.GET);
            _client.ExecuteAsync<List<Household>>(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(rslt.ErrorException, rslt.Data);
            });
        }
        public void DeleteHousehold(int id, Action<Exception> callback)
        {
            var request = new RestRequest("/api/households/" + id + "/", Method.DELETE);
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
        public void AddHousehold(Household household, Action<Exception,
            Household> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/households/", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(household);
            _client.ExecuteAsync<Household>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false)
                {
                    var uploadRequest = new RestRequest("/api/household/" + rslt.Data.Id + "/upload/", Method.PATCH);
                    uploadRequest.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<Household>(uploadRequest, rsltUpload =>
                    {
                        if (rsltUpload.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            rsltUpload.Data.Photo = NormalizeUri(rsltUpload.Data.Photo);
                            callback(null, rsltUpload.Data);
                        }
                        else
                        {
                            callback(new Exception(), null);
                        }
                    });
                }

                callback(null, rslt.Data);
            });
        }
        public void UpdateHousehold(Household household, Action<Exception,
            Household> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/households/" + household.Id + "/", Method.PATCH);

            request.AddParameter("Address", household.Address);
            request.AddParameter("EconomicStatus", household.EconomicStatus);
            request.AddParameter("HouseNumber", household.HouseNumber);
            request.AddParameter("IsOwned", household.IsOwned);

            _client.ExecuteAsync<Household>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false
                    && IsRemoteUri(choosenPhoto) == false)
                {
                    var uploadRequest = new RestRequest("/api/household/" + rslt.Data.Id + "/upload/", Method.PATCH);
                    uploadRequest.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<Household>(uploadRequest, rsltUpload =>
                    {
                        if (rsltUpload.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            rsltUpload.Data.Photo = NormalizeUri(rsltUpload.Data.Photo);
                            callback(null, rsltUpload.Data);
                        }
                        else
                        {
                            callback(new Exception(), null);
                        }
                    });
                }

                callback(null, rslt.Data);
            });
        }

        private Boolean IsRemoteUri(String uri)
        {
            try
            {
                var uriObj = new Uri(uri);
                return !uriObj.IsFile;
            }
            catch
            {
                return false;
            }
        }
        private string NormalizeUri(string uri)
        {
            if (uri.StartsWith("/"))
            {
                return _client.BaseUrl.ToString() + uri.TrimStart("/".ToCharArray());
            }
            return uri;
        }
    }
}
