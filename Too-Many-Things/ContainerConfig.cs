using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Too_Many_Things.Core;
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

            //builder.RegisterType<TooManyThingsContext>().
            return builder.Build();
        }
    }
}
