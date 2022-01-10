// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace Notifo.Domain.Integrations.Sms77.Implementation
{
    public enum Sms77Status
    {
        Answered,
        Buffered,
        Calling,
        Delivered,
        Delivery_Failed,
        Expired,
        Failed,
        Scheduled,
        Sent
    }
}
