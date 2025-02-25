﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Notifo.Domain.UserNotifications;

namespace Notifo.Domain.Channels.Messaging
{
    public sealed class MessagingJob
    {
        public BaseUserNotification Notification { get; set; }

        public string? TemplateName { get; }

        public Dictionary<string, string> Targets { get; set; } = new Dictionary<string, string>();

        public bool IsImmediate { get; set; }

        public string ScheduleKey
        {
            get => Notification.Id.ToString();
        }

        public MessagingJob()
        {
        }

        public MessagingJob(BaseUserNotification notification, string? templateName)
        {
            Notification = notification;

            TemplateName = templateName;
        }
    }
}
