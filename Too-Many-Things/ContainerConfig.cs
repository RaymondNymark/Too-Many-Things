using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using EntityFrameworkCore.DbContextScope;
using Microsoft.Extensions.Logging;
using Too_Many_Things.Services;
using Too_Many_Things.Models;

namespace Too_Many_Things
{
    public static class ContainerConfig
    {
        // Simple IOC container configure method.
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ChecklistService>().As<IChecklistService>();
            //builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>();
            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
            return builder.Build();
        }
    }
}
