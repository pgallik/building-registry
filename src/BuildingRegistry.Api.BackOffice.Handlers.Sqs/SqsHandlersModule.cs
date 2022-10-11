﻿namespace BuildingRegistry.Api.BackOffice.Handlers.Sqs
{
    using System;
    using Amazon;
    using Autofac;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple;

    public sealed class SqsHandlersModule : Module
    {
        private readonly string _queueUrl;

        public SqsHandlersModule(string queueUrl)
        {
            _queueUrl = queueUrl ?? throw new ArgumentNullException(nameof(queueUrl));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(_ => new SqsOptions(RegionEndpoint.EUWest1, EventsJsonSerializerSettingsProvider.CreateSerializerSettings()))
                .SingleInstance();

            builder.Register(c => new SqsQueue(c.Resolve<SqsOptions>(), _queueUrl))
                .As<ISqsQueue>()
                .AsSelf()
                .SingleInstance();
        }
    }
}