﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifo.Areas.Api.Controllers.Logs.Dtos;
using Notifo.Domain;
using Notifo.Domain.Log;
using Notifo.Pipeline;
using NSwag.Annotations;

namespace Notifo.Areas.Api.Controllers.Logs
{
    [OpenApiTag("Logs")]
    public class LogsController : BaseController
    {
        private readonly ILogStore logStore;

        public LogsController(ILogStore logStore)
        {
            this.logStore = logStore;
        }

        /// <summary>
        /// Query log entries.
        /// </summary>
        /// <param name="appId">The app where the log entries belongs to.</param>
        /// <param name="q">The query object.</param>
        /// <returns>
        /// 200 => Log entries returned.
        /// </returns>
        [HttpGet("api/apps/{appId}/logs/")]
        [AppPermission(Roles.Admin)]
        [Produces(typeof(ListResponseDto<LogEntryDto>))]
        public async Task<IActionResult> GetLogs(string appId, [FromQuery] QueryDto q)
        {
            var medias = await logStore.QueryAsync(appId, q.ToQuery<LogQuery>(), HttpContext.RequestAborted);

            var response = new ListResponseDto<LogEntryDto>();

            response.Items.AddRange(medias.Select(LogEntryDto.FromDomainObject));
            response.Total = medias.Total;

            return Ok(response);
        }
    }
}
