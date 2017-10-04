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


        //PEOPLE ENDPOINGS
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
                    callback(new Exception(rslt.ErrorMessage + "\n" + rslt.StatusDescription
                        + "\n" + rslt.Content.Trim("[]".ToCharArray())), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false)
                {
                    var reqUpload = new RestRequest("/api/people/" + rslt.Data.id + "/upload/", Method.PATCH);
                    reqUpload.AddFile("Photo", choosenPhoto);
                    _client.ExecuteAsync<DownloadPersonModel>(reqUpload, rsltUpload =>
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
        public void UpdatePerson(UploadPersonModel person,
            Action<Exception, DownloadPersonModel> callback, string choosenPhoto = "")
        {
            var request = new RestRequest("/api/people/" + person.id + "/", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(person);
            _client.ExecuteAsync<DownloadPersonModel>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (string.IsNullOrEmpty(choosenPhoto) == false
                        && Uploadable(choosenPhoto))
                    {
                        var uploadPhotoRequest = new RestRequest("/api/people/" + person.id + "/upload/", Method.PATCH);
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

        public void CheckIn(int id, Action<Exception, MonitoringSummary> callback)
        {
            var request = new RestRequest("/api/people/" + id + "/check_in/", Method.POST);
            _client.ExecuteAsync<MonitoringSummary>(request, (res) =>
            {
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, res.Data);
                    return;
                }
                else
                {
                    var msg = string.Format("{0}\n{1}\n{2}",
                       res.ErrorMessage, res.Content, res.StatusDescription);
                    callback(new Exception(msg), null);
                    return;
                }

            });
        }

        //EVACUATION CENTER ENDPOINTS
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
                    callback(new Exception("" + rslt.StatusDescription
                         + "\n" + rslt.Content.Trim("[]".ToCharArray())), null);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(center.Photo) == false && Uploadable(center.Photo))
                    {
                        var uploadRequest = new RestRequest("/api/centers/" + rslt.Data.id + "/upload/", Method.PATCH);
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
        public void UpdateCenter(Center center, Action<Exception, Center> callback)
        {
            var request = new RestRequest("/api/centers/" + center.id + "/", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(center);

            _client.ExecuteAsync<Center>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription +
                        "\n" + rslt.Content + "\n" + rslt.ErrorMessage), null);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(center.Photo) == false && Uploadable(center.Photo))
                    {
                        var uploadRequest = new RestRequest("/api/centers/" + rslt.Data.id + "/upload/", Method.PATCH);
                        uploadRequest.AddFile("Photo", center.Photo);
                        _client.ExecuteAsync<Center>(uploadRequest, _rslt =>
                        {
                            if (_rslt.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                _rslt.Data.Photo = NormalizeUri(_rslt.Data.Photo);
                                callback(null, _rslt.Data);
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
                }
            });
        }

        //HOUSEHOLD ENPOINTS
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
                    callback(new Exception("" + rslt.StatusDescription + "\n"
                        + rslt.Content.Trim("[]".ToCharArray())), null);
                    return;
                }

                if (string.IsNullOrEmpty(choosenPhoto) == false && Uploadable(choosenPhoto))
                {
                    var uploadRequest = new RestRequest("/api/households/" + rslt.Data.id + "/upload/", Method.PATCH);
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
            var request = new RestRequest("/api/households/" + household.id + "/", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(household);
            _client.ExecuteAsync<DownloadHouseholdModel>(request, rslt =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription
                        + "\n" + rslt.Content + "\n" + rslt.ErrorMessage), null);

                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(choosenPhoto) == false
                        && Uploadable(choosenPhoto))
                    {
                        var uploadRequest = new RestRequest("/api/household/" + rslt.Data.id + "/upload/", Method.PATCH);
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
        public void ToggleIncidentStatus(Incident incident, Action<Exception, List<Incident>> callback)
        {
            var request = new RestRequest("/api/incidents/" + incident.id + "/toggle/", Method.PATCH);
            _client.ExecuteAsync<List<Incident>>(request, (response) =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, response.Data);
                    return;
                }
                else
                {
                    callback(new Exception(string.Format("{0}\n{1}",
                        response.Content, response.ErrorMessage)), null);

                    return;
                }
            });
        }
        public void GetIncidents(Action<Exception, List<Incident>> callback)
        {
            var request = new RestRequest("/api/incidents/", Method.GET);
            _client.ExecuteAsync<List<Incident>>(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(new Exception("" + rslt.StatusDescription
                        + "\n" + rslt.Content + "\n" + rslt.ErrorMessage), null);
                    return;
                }

                callback(null, rslt.Data);
            });
        }
        public void DeleteIncident(Incident incident, Action<Exception> callback)
        {
            var request = new RestRequest("/api/incidents/" + incident.id + "/", Method.DELETE);
            _client.ExecuteAsync(request, (rslt) =>
            {
                if (rslt.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    callback(new Exception("" + rslt.StatusDescription
                        + "\n" + rslt.Content + "\n" + rslt.ErrorMessage));
                    return;
                }
                else
                    callback(null);
            });
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
        public void ToggleHouseholdMembership(DownloadPersonModel person, DownloadHouseholdModel household,
           Action<Exception, DownloadHouseholdModel> callback)
        {
            var request = new RestRequest("/api/people/" + person.id + "/toggle_membership/", Method.PATCH);
            request.AddParameter("household_id", household.id);
            _client.ExecuteAsync<DownloadHouseholdModel>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, rslt.Data);
                }
                else
                {
                    callback(new Exception("Cannot Toggle Membership\n+"
                        + rslt.Content?.Trim("[]".ToCharArray())), null);
                }
            });
        }

        public void ToggleEvacuationMembership(DownloadPersonModel person, Center center,
           Action<Exception, Center> callback)
        {
            var request = new RestRequest("/api/people/" + person.id + "/toggle_evacuation_membership/", Method.PATCH);
            request.AddParameter("center_id", center.id);
            _client.ExecuteAsync<Center>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, rslt.Data);
                }
                else
                {
                    callback(new Exception("Cannot Toggle Center Membership\n+"
                        + rslt.Content?.Trim("[]".ToCharArray()) + "\n" + rslt.ErrorMessage), null);
                }
            });
        }


        public void GetMonitoring(Action<Exception, List<MonitoringSummary>> callback)
        {
            var request = new RestRequest("/api/monitoring/", Method.GET);

            _client.ExecuteAsync<List<MonitoringSummary>>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, rslt.Data);
                }
                else
                {
                    callback(new Exception(rslt.Content + "\n" + rslt.ErrorMessage), null);
                }
            });
        }

        public void GetMonitoringDetail(int pk, Action<Exception, MonitoringInfo> callback)
        {
            var request = new RestRequest("/api/monitoring/" + pk + "/", Method.GET);

            _client.ExecuteAsync<MonitoringInfo>(request, rslt =>
            {
                if (rslt.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    callback(null, rslt.Data);
                }
                else
                {
                    callback(new Exception(rslt.Content + "\n" + rslt.ErrorMessage), null);
                }
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
