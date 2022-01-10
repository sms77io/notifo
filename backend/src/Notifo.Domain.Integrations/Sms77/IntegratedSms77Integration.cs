// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.Extensions.DependencyInjection;
using Notifo.Domain.Channels;
using Notifo.Domain.Channels.Sms;
using Notifo.Domain.Integrations.Resources;
using Notifo.Domain.Integrations.Sms77.Implementation;

namespace Notifo.Domain.Integrations.Sms77
{
    public sealed class IntegratedSms77Integration : IIntegration
    {
        public IntegrationDefinition Definition { get; } =
            new IntegrationDefinition(
                "Sms77Integrated",
                Texts.Sms77Integrated_Name,
                "./integrations/sms77.svg",
                new List<IntegrationProperty>(),
                new List<UserProperty>(),
                new HashSet<string>
                {
                    Providers.Sms
                })
            {
                Description = Texts.Sms77Integrated_Description
            };

        public bool CanCreate(Type serviceType, string id, ConfiguredIntegration configured)
        {
            return serviceType == typeof(ISmsSender);
        }

        public object? Create(Type serviceType, string id, ConfiguredIntegration configured, IServiceProvider serviceProvider)
        {
            if (CanCreate(serviceType, id, configured))
            {
                return new Sms77SmsSender(
                    serviceProvider.GetRequiredService<Sms77Client>(),
                    serviceProvider.GetRequiredService<ISmsCallback>(),
                    serviceProvider.GetRequiredService<ISmsUrl>(),
                    id);
            }

            return null;
        }
    }
}
