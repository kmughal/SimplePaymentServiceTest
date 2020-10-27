using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SimplePaymentServiceTests.Infrastructure;
using SimplePaymentServiceTests.Services;
using System;
using System.IO;
using System.Threading.Tasks;

WebHost.CreateDefaultBuilder()
    .Configure(app =>
    {
        app.UseRouting();
        app.UseEndpoints(e =>
        {
            var payService = (IPaymentService)IocProvider.ServiceProvider.GetService(typeof(IPaymentService));

            e.MapGet("/", c => c.Response.WriteAsync($"Welcome to new way of building web api end points!"));

            e.MapGet("/pay", async (c) =>
            {
                var result = payService.MakePayment(null);
                await c.Response.WriteAsync($"Pay service result : {result.Success}");
            });

            e.MapGet("/weatherData", async (c) =>
           {
               var data = await File.ReadAllTextAsync(@"mock\weather-data.json");
               await c.Response.WriteAsync(data);

           });
            e.MapGet("/currentdate", CurrentDate);
        });


    })
        .Build().Run();


Task CurrentDate(HttpContext c)
{
    c.Response.WriteAsync($"Todays :{DateTime.Now.ToShortDateString()}");
    return null;
}


