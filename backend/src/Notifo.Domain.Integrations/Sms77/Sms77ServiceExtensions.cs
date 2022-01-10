// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.Extensions.Configuration;
using Notifo.Domain.Integrations;
using Notifo.Domain.Integrations.Sms77;
using Notifo.Domain.Integrations.Sms77.Implementation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Sms77ServiceExtensions
    {
        public static void IntegrateSms77(this IServiceCollection services, IConfiguration config)
        {
            const string key = "sms:sms77";

            var options = config.GetSection(key).Get<Sms77Options>();

            if (options.IsValid())
            {
                services.ConfigureAndValidate<Sms77Options>(config, key);

                services.AddSingletonAs<Sms77Client>()
                    .AsSelf();

                services.AddSingletonAs<IntegratedSms77Integration>()
                    .As<IIntegration>();
            }

            services.AddSingletonAs<Sms77Integration>()
                .As<IIntegration>();

            services.AddSingletonAs<Sms77ClientPool>()
                .AsSelf();
        }
    }
}
