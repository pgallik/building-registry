namespace BuildingRegistry.Api.BackOffice.Infrastructure.Modules
{
    using System.Reflection;
    using Autofac;
    using Handlers.Building;
    using Handlers.Sqs.Handlers.Building;
    using MediatR;
    using Module = Autofac.Module;

    public class MediatRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var ctx = context.Resolve<IComponentContext>();
                return type => ctx.Resolve(type);
            });

            builder.RegisterAssemblyTypes(typeof(PlanBuildingHandler).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(PlanBuildingSqsHandler).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }
    }
}
