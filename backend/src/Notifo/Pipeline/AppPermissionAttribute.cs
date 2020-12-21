﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.Authorization;

namespace Notifo.Pipeline
{
    public sealed class AppPermissionAttribute : AuthorizeAttribute
    {
        public string[] RequiredAppRoles { get; }

        public AppPermissionAttribute(params string[] appRoles)
        {
            RequiredAppRoles = appRoles;
        }
    }
}
