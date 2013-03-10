﻿using System;
using Autofac;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using System.Collections.Generic;

namespace PancakeProwler.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private NLog.Logger _log;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            CreateLogger();
            CreateContainer();
        }

        private void CreateLogger()
        {
            _log = NLog.LogManager.GetLogger("PancakeProwler");
        }

        private void CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_log);
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(PancakeProwler.Data.SQL.Repositories.RecipeRepository).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).Where(x => x.Name.EndsWith("Controller")).AsSelf().PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}