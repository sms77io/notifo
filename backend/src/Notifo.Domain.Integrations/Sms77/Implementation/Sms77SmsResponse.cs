// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Text.Json.Serialization;

namespace Notifo.Domain.Integrations.Sms77.Implementation
{
    public sealed class Sms77SmsResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("recipients")]
        public Sms77SmsResponseRecipients Recipients { get; set; }
    }

    public sealed class Sms77SmsResponseRecipients
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("totalSentCount")]
        public int TotalSentCount { get; set; }

        [JsonPropertyName("totalDeliveredCount")]
        public int TotalDeliveredCount { get; set; }

        [JsonPropertyName("totalDeliveryFailedCount")]
        public int TotalDeliveryFailedCount { get; set; }

        [JsonPropertyName("items")]
        public Sms77SmsResponseRecipient[] Items { get; set; }
    }

    public sealed class Sms77SmsResponseRecipient
    {
        [JsonPropertyName("recipient")]
        public long Recipient { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Sms77Status Status { get; set; }
    }
}
