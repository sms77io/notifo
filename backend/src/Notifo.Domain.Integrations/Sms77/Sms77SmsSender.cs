// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Globalization;
using Microsoft.AspNetCore.Http;
using Notifo.Domain.Apps;
using Notifo.Domain.Channels.Sms;
using Notifo.Domain.Integrations.Resources;
using Notifo.Domain.Integrations.Sms77.Implementation;
using Notifo.Infrastructure;

namespace Notifo.Domain.Integrations.Sms77
{
    public sealed class Sms77SmsSender : ISmsSender
    {
        private readonly Sms77Client sms77Client;
        private readonly ISmsCallback smsCallback;
        private readonly ISmsUrl smsUrl;
        private readonly string integrationId;

        public Sms77SmsSender(
            Sms77Client sms77Client,
            ISmsCallback smsCallback,
            ISmsUrl smsUrl,
            string integrationId)
        {
            this.sms77Client = sms77Client;
            this.smsCallback = smsCallback;
            this.smsUrl = smsUrl;
            this.integrationId = integrationId;
        }

        public async Task HandleCallbackAsync(App app, HttpContext httpContext)
        {
            var status = await sms77Client.ParseStatusAsync(httpContext);

            if (status.Reference != null)
            {
                var result = default(SmsResult);

                switch (status.Status)
                {
                    case Sms77Status.Delivered:
                        result = SmsResult.Delivered;
                        break;
                    case Sms77Status.Delivery_Failed:
                        result = SmsResult.Failed;
                        break;
                    case Sms77Status.Sent:
                        result = SmsResult.Sent;
                        break;
                }

                if (result != SmsResult.Unknown)
                {
                    await smsCallback.HandleCallbackAsync(status.Recipient, status.Reference, result, httpContext.RequestAborted);
                }
            }
        }

        public async Task<SmsResult> SendAsync(App app, string to, string body, string token,
            CancellationToken ct = default)
        {
            try
            {
                var callbackUrl = smsUrl.SmsWebhookUrl(app.Id, integrationId);

                var sms = new Sms77SmsMessage(to, body, token, callbackUrl);

                var response = await sms77Client.SendSmsAsync(sms, ct);

                if (response.Recipients.TotalSentCount != 1)
                {
                    var errorMessage = string.Format(CultureInfo.CurrentCulture, Texts.Sms77_ErrorUnknown, to);

                    throw new DomainException(errorMessage);
                }

                return SmsResult.Sent;
            }
            catch (ArgumentException ex)
            {
                var errorMessage = string.Format(CultureInfo.CurrentCulture, Texts.Sms77_Error, to, ex.Message);

                throw new DomainException(errorMessage);
            }
        }
    }
}
