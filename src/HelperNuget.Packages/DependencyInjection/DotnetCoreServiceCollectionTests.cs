using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Utitlity.Nuget.Packages.DependencyInjection
{
    /* Nuget Package Dependency: Microsoft.Extensions.DependencyInjection */
    /// <summary>
    /// It is needed when Web api invoke .NET core standard libray projects.
    /// Dependency will be registered from Web context and accesseble from Service projects (Domain Service, Repository etc.)
    /// </summary>
    public class DotnetCoreServiceCollectionTests
    {
        [Fact]
        public void ShouldGetObject()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<EmployeeService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var employeeService = serviceProvider.GetService(typeof(EmployeeService));

            Assert.NotNull(employeeService);
        }
    }

    /// <summary>
    /// Testing purpose only
    /// </summary>
    public class EmployeeService
    {
        public string Id { get; set; }
    }
}
