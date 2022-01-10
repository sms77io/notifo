// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Text.Json.Serialization;

namespace Notifo.Domain.Integrations.Sms77.Implementation
{
    public sealed class Sms77Errors
    {
        [JsonPropertyName("errors")]
        public Sms77Error[] Errors { get; set; }
    }
}
