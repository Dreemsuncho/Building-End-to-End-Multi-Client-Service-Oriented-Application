using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using NUnit.Framework;

namespace CarRental.Web.Tests
{
    public abstract class ApiControllerTestClass
    {
        protected HttpRequestMessage _request;

        [SetUp]
        public void initialize()
        {
            this._request = this._GetRequest();
        }

        private HttpRequestMessage _GetRequest()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.Properties["MS_HttpConfiguration"] = config;

            return request;
        }

        protected T _GetResponseData<T>(HttpResponseMessage result)
        {
            var content = result.Content as ObjectContent<T>;

            if (content != null)
            {
                T data = (T)(content.Value);
                return data;
            }
            else
            {
                return default(T);
            }
        }

    }
}
