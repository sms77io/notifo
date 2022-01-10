// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Notifo.Domain.Integrations.Sms77.Implementation;

namespace Notifo.Domain.Integrations.Sms77
{
    public sealed class Sms77ClientPool : CachePool<Sms77Client>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public Sms77ClientPool(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
            : base(memoryCache)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public Sms77Client GetServer(string accessKey, long phoneNumber, Dictionary<string, string>? phoneNumbers)
        {
            var cacheKey = $"Sms77SmsSender_{accessKey}_{phoneNumber}";

            var found = GetOrCreate(cacheKey, () =>
            {
                var options = Options.Create(new Sms77Options
                {
                    AccessKey = accessKey,
                    PhoneNumber = phoneNumber.ToString(CultureInfo.InvariantCulture),
                    PhoneNumbers = phoneNumbers
                });

                var sender = new Sms77Client(httpClientFactory, options);

                return sender;
            });

            return found;
        }
    }
}
