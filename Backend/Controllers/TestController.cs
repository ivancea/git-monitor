using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GitMonitor.Controllers
{
    /// <summary>
    /// Test controller.
    /// </summary>
    [ApiController]
    [Route(ApiConstants.ApiPath + "test")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Test controller endpoint.
        /// </summary>
        /// <returns>Test string.</returns>
        [HttpGet("")]
        public string Get()
        {
            return "Test";
        }
    }
}
