﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace Notifo.Domain.Integrations.Sms77.Implementation
{
    public sealed class Sms77SmsStatus
    {
        public string Id { get; set; }

        public string? Reference { get; set; }

        public string Recipient { get; set; }

        public int StatusErrorCode { get; set; }

        public Sms77Status Status { get; set; }
    }
}
