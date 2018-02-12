using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using GamR.Backend.Core.Aggregates;
using Events = GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using GamR.Backend.Core.Framework.Impl;
using GamR.Backend.Web.ApiModels;
using GamR.Backend.Web.Views;
using GamR.Backend.Web.Views.ViewManagers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Owin;
using Module = Autofac.Module;

namespace GamR.Backend.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public IConfigurationRoot Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseOwin(x => x.UseNancy());
        }
    }

    public class BootStrapper : AutofacNancyBootstrapper
    {
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var eventBus = new InMemoryBus();
            var jsonEventStore = new JsonEventStore(new InMemoryEventStore(eventBus), "store.json");
            existingContainer.Update(x => x.RegisterInstance<IEventStore>(jsonEventStore));
            existingContainer.Update(x => x.RegisterInstance(eventBus).As<IEventSubscriber>());
            existingContainer.Update(x => x.RegisterInstance(eventBus).As<IEventPublisher>());
            existingContainer.Update(x => x.RegisterGeneric(typeof(Repository<>)));
            existingContainer.Update(x => x.RegisterType<CsvEventLoader>());
            
            
            var matchesView = new MatchesListViewManager();
            var gameMatchesViewManager = new MatchGamesViewManger();
            var playersViewManager = new PlayersViewManager();
            
            Task.WaitAll(gameMatchesViewManager.SubscribeAll(eventBus), 
                         matchesView.SubscribeAll(eventBus),
                         playersViewManager.SubscribeAll(eventBus));

            existingContainer.Update(x => x.RegisterInstance(matchesView).SingleInstance());
            existingContainer.Update(x => x.RegisterInstance(gameMatchesViewManager).SingleInstance());
            existingContainer.Update(x => x.RegisterInstance(playersViewManager).SingleInstance());
            
            base.ConfigureApplicationContainer(existingContainer);
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,DELETE,PUT,OPTIONS");
                ctx.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers");
            });
            base.RequestStartup(container, pipelines, context);
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var csvEventLoader = container.Resolve<CsvEventLoader>();
            csvEventLoader.LoadEvents("whist_20170705.csv").Wait();
            csvEventLoader.LoadEvents("whist_20170926.csv").Wait();

            base.ApplicationStartup(container, pipelines);
        }
    }
}
