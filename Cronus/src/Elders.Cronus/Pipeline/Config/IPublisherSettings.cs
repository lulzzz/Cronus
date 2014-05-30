using System;
using System.Linq;
using System.Reflection;
using Elders.Cronus.DomainModelling;
using Elders.Cronus.EventSourcing;
using Elders.Cronus.Pipeline.Transport;
using Elders.Protoreg;

namespace Elders.Cronus.Pipeline.Config
{
    public interface IPipelinePublisherSettings<TContract> : IHaveTransport<IPipelineTransport>, ISettingsBuilder<IPublisher<TContract>>, IHavePipelineSettings<TContract>, IHaveSerializer
        where TContract : IMessage
    {

    }

    public static class PublisherSettingsExtensions
    {
        public static void CopySerializerTo(this IHaveSerializer self, IHaveSerializer destination)
        {
            destination.Serializer = self.Serializer;
        }

        static ProtoregSerializer serializer = null;

        public static T UseContractsFromAssemblies<T>(this T self, Assembly[] assembliesContainingContracts, Type[] contracts = null)
            where T : IHaveSerializer, ICanConfigureSerializer
        {
            if (serializer != null)
                throw new InvalidOperationException("Protoreg serializer is already initialized.");

            var protoreg = new ProtoRegistration();
            protoreg.RegisterAssembly(typeof(EventBatchWraper));
            protoreg.RegisterAssembly(typeof(IMessage));

            foreach (var ass in assembliesContainingContracts)
            {
                protoreg.RegisterAssembly(ass);
            }

            if (contracts != null)
                contracts.ToList().ForEach(c => protoreg.RegisterCommonType(c));

            serializer = new ProtoregSerializer(protoreg);
            serializer.Build();
            self.Serializer = serializer;
            return self;
        }
    }
}