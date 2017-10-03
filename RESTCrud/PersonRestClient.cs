using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTCrud
{
    public class PersonRestClient : CrudRestClientBase<UploadPersonModel, DownloadPersonModel>
    {
        public PersonRestClient(string baseUrl) : base(baseUrl, "people")
        { }
    }
}
