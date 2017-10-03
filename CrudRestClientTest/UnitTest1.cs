using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CrudRestClientTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RESTCrud.PersonRestClient client = new RESTCrud.PersonRestClient("http://127.0.0.1:8000/api/");
            client.List((ex, people) =>
            {
                foreach (var p in people)
                {
                    Console.WriteLine(p.FirstName);
                }
            });
        }
    }
}
