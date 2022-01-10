﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Notifo.Infrastructure;
using PhoneNumbers;

#pragma warning disable CA1822 // Mark members as static

namespace Notifo.Domain.Integrations.Sms77.Implementation
{
    public sealed class Sms77Client
    {
        private static readonly char[] TrimChars = { ' ', '+', '0' };
        private readonly Sms77Options options;
        private readonly IHttpClientFactory httpClientFactory;

        public Sms77Client(IHttpClientFactory httpClientFactory, IOptions<Sms77Options> options)
        {
            this.httpClientFactory = httpClientFactory;

            this.options = options.Value;
        }

        public async Task<Sms77SmsResponse> SendSmsAsync(Sms77SmsMessage message,
            CancellationToken ct)
        {
            Guard.NotNull(message, nameof(message));
            Guard.NotNullOrEmpty(message.Body, nameof(message.Body));
            Guard.NotNullOrEmpty(message.To, nameof(message.To));

            var (to, body, reference, reportUrl) = message;

            if (body.Length > 140)
            {
                throw new ArgumentException("Text must not have more than 140 characters.", nameof(message));
            }

            to = PhoneNumberUtil.Normalize(to).TrimStart(TrimChars);

            if (!long.TryParse(to, NumberStyles.Integer, CultureInfo.InvariantCulture, out var recipient))
            {
                throw new ArgumentException("Not a valid phone number.", nameof(message));
            }

            using (var client = httpClientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("AccessKey", options.AccessKey);

                var request = new
                {
                    originator = GetOriginator(to),
                    body,
                    reportUrl,
                    reference,
                    recipients = new[]
                    {
                        recipient
                    }
                };

                var response = await client.PostAsJsonAsync("https://gateway.sms77.io/api/sms", request, ct);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Sms77SmsResponse>((JsonSerializerOptions?)null, ct);

                    return result!;
                }

                throw await HandleErrorAsync(response, ct);
            }
        }

        public Task<Sms77SmsStatus> ParseStatusAsync(HttpContext httpContext)
        {
            var result = new Sms77SmsStatus();

            var query = httpContext.Request.Query;

            if (query.TryGetValue("id", out var id))
            {
                result.Id = id;
            }

            if (query.TryGetValue("recipient", out var recipient))
            {
                result.Recipient = recipient;
            }

            if (query.TryGetValue("reference", out var reference))
            {
                result.Reference = reference;
            }

            if (query.TryGetValue("status", out var statusString) && Enum.TryParse<Sms77Status>(statusString, true, out var status))
            {
                result.Status = status;
            }

            if (query.TryGetValue("statusErrorCode", out var codeString) && int.TryParse(codeString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var code))
            {
                result.StatusErrorCode = code;
            }

            return Task.FromResult(result);
        }

        private static async Task<Exception> HandleErrorAsync(HttpResponseMessage response,
            CancellationToken ct)
        {
            var errors = await response.Content.ReadFromJsonAsync<Sms77Errors>((JsonSerializerOptions?)null, ct);
            var error = errors?.Errors?.FirstOrDefault();

            if (error != null)
            {
                var message = $"Sms77 request failed: Code={error.Code}, Description={error.Description}";

                return new HttpIntegrationException<Sms77Error>(message, (int)response.StatusCode, error);
            }
            else
            {
                var message = "Sms77 request failed with unknown error.";

                return new HttpIntegrationException<Sms77Error>(message, (int)response.StatusCode);
            }
        }

        private string GetOriginator(string phoneNumber)
        {
            if (options.PhoneNumbers?.Count > 0 && phoneNumber.Length > 2)
            {
                var countryCode = phoneNumber[..2];

                if (options.PhoneNumbers.TryGetValue(countryCode, out var originator))
                {
                    return originator;
                }
            }

            return options.PhoneNumber;
        }
    }
}