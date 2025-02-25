﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

namespace Notifo.Domain.Integrations.Sms77.Implementation
{
    public sealed record Sms77SmsMessage(
        string To,
        string Body,
        string? Reference = null,
        string? ReportUrl = null);
}
