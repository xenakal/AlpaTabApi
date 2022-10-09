using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AlpaTabApi.Tests;

public static class HttpClientHelper
{
    public static HttpClient CreateClient()
    {
        var application = new WebApplicationFactory<Program>();
        return application.CreateClient();
    }
}
