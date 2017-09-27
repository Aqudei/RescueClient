﻿using System;
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

        public void GetPeople(Action<Exception, List<DownloadPersonModel>> callback)
        {
            var request = new RestRequest("/api/people/", Method.GET);
            _client.ExecuteAsync<List<DownloadPersonModel>>(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(rslt.ErrorException, rslt.Data);
            });
        }
        public void AddPerson(DownloadPersonModel person, Action<Exception, DownloadPersonModel> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/people/", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(person);
            _client.ExecuteAsync<DownloadPersonModel>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created || rslt.ErrorException != null)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false)
                {
                    var reqUpload = new RestRequest("/api/people/" + rslt.Data.Id + "/upload/", Method.PATCH);
                    reqUpload.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<DownloadPersonModel>(reqUpload, rsltUpload =>
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

        public void ToggleMembership(DownloadPersonModel person, DownloadHouseholdModel household,
            Action<Exception, DownloadHouseholdModel> callback)
        {
            var request = new RestRequest("/api/people/" + person.Id + "/toggle_membership/", Method.PATCH);
            request.AddParameter("household_id", household.Id);
            _client.ExecuteAsync<DownloadHouseholdModel>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, rslt.Data);
                }
                else
                {
                    callback(new Exception("Cannot Toggle Membership"), null);
                }
            });
        }

        public void UpdatePerson(UploadPersonModel person,
            Action<Exception, DownloadPersonModel> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/people/" + person.Id + "/", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(person);
            _client.ExecuteAsync<DownloadPersonModel>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (string.IsNullOrEmpty(choosenPhoto) == false
                        && Uploadable(choosenPhoto))
                    {
                        var uploadPhotoRequest = new RestRequest("/api/people/" + person.Id + "/upload/", Method.PATCH);
                        uploadPhotoRequest.AddFile("Photo", choosenPhoto);

                        _client.ExecuteAsync<DownloadPersonModel>(uploadPhotoRequest, r =>
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
        public void AddCenter(Center center, Action<Exception, Center> callback)
        {
            var request = new RestRequest("/api/centers/", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(center);

            _client.ExecuteAsync<Center>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(center.Photo) == false && Uploadable(center.Photo))
                    {
                        var uploadRequest = new RestRequest("/api/centers/" + rslt.Data.Id + "/upload/", Method.PATCH);
                        uploadRequest.AddFile("Photo", center.Photo);
                        _client.ExecuteAsync<Center>(uploadRequest, _rslt =>
                        {
                            if (_rslt.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                _rslt.Data.Photo = NormalizeUri(_rslt.Data.Photo);
                                callback(null, _rslt.Data);
                            }
                            else
                            {
                                callback(null, rslt.Data);
                            }
                            return;
                        });
                    }
                }

                callback(null, rslt.Data);
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

        public void GetHouseholds(Action<Exception, List<DownloadHouseholdModel>> callback)
        {
            var request = new RestRequest("/api/households/", Method.GET);
            _client.ExecuteAsync<List<DownloadHouseholdModel>>(request, (rslt) =>
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
        public void AddHousehold(UploadHouseholdModel household, Action<Exception,
            DownloadHouseholdModel> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/households/", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(household);
            _client.ExecuteAsync<DownloadHouseholdModel>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false && Uploadable(choosenPhoto))
                {
                    var uploadRequest = new RestRequest("/api/households/" + rslt.Data.Id + "/upload/", Method.PATCH);
                    uploadRequest.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<DownloadHouseholdModel>(uploadRequest, rsltUpload =>
                    {
                        if (rsltUpload.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            rsltUpload.Data.Photo = NormalizeUri(rsltUpload.Data.Photo);
                            callback(null, rsltUpload.Data);
                            return;
                        }
                        else
                        {
                            callback(null, rslt.Data);
                            return;
                        }
                    });
                }

                callback(null, rslt.Data);
            });
        }
        public void UpdateHousehold(UploadHouseholdModel household, Action<Exception,
            DownloadHouseholdModel> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/households/" + household.Id + "/", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(household);
            _client.ExecuteAsync<DownloadHouseholdModel>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false
                    && Uploadable(choosenPhoto))
                {
                    var uploadRequest = new RestRequest("/api/household/" + rslt.Data.Id + "/upload/", Method.PATCH);
                    uploadRequest.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<DownloadHouseholdModel>(uploadRequest, rsltUpload =>
                    {
                        if (rsltUpload.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            rsltUpload.Data.Photo = NormalizeUri(rsltUpload.Data.Photo);
                            callback(null, rsltUpload.Data);
                            return;
                        }
                        else
                        {
                            callback(null, rslt.Data);
                            return;
                        }
                    });
                }

                callback(null, rslt.Data);
            });
        }

        public void AddIncident(Incident incident, Action<Exception, Incident> callback)
        {
            var request = new RestRequest("/api/incidents/", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(incident);
            _client.ExecuteAsync<Incident>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.Created || rslt.ErrorException != null)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(null, rslt.Data);
            });
        }
        public void ToggleIncidentStatus(Action<Exception, List<Incident>> callback)
        {
            var request = new RestRequest("/api/incidents/", Method.PATCH);
        }

        public void GetStats(Action<Exception, Statistics> callback)
        {
            var request = new RestRequest("/api/statistics/", Method.GET);
            _client.ExecuteAsync<Statistics>(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription), null);
                    return;
                }

                callback(null, rslt.Data);
            });
        }

        private Boolean Uploadable(String uri)
        {
            try
            {
                var uriObj = new Uri(uri);
                return uriObj.IsFile;
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
