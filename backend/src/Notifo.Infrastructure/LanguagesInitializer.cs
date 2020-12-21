﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Notifo.Infrastructure.Initialization;

namespace Notifo.Infrastructure
{
    public sealed class LanguagesInitializer : IInitializable
    {
        private readonly LanguagesOptions options;

        public LanguagesInitializer(IOptions<LanguagesOptions> options)
        {
            Guard.NotNull(options, nameof(options));

            this.options = options.Value;
        }

        public Task InitializeAsync(CancellationToken ct = default)
        {
            foreach (var (key, value) in options)
            {
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                {
                    Language.AddLanguage(key, value);
                }
            }

            return Task.CompletedTask;
        }
    }
}
