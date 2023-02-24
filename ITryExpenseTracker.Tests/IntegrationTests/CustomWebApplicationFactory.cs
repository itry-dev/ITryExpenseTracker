using ITryExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace ITryExpenseTracker.Tests.IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<DataContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            // Create open Connection so EF won't automatically close it.
            //services.AddSingleton<DbConnection>(container =>
            //{
            //    var connection = new MySqlConnection("DataSource=:memory:");
            //    connection.Open();

            //    return connection;
            //});

            services.AddDbContext<DataContext>((container, options) =>
            {
                //var connection = container.GetRequiredService<DbConnection>();
                //var serverVersion = new MySqlServerVersion(ITryExpenseTracker.Core.Constants.MySqlServer.GetVersion());
                options.UseInMemoryDatabase("itryexpensetracker");
                //.UseMySql("DataSource=:memory:", serverVersion, opt => opt.EnableRetryOnFailure(1));
            });

            
        });

        builder.UseEnvironment("Development");
    }
}
