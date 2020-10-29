using System;
using System.Collections.Generic;
using System.Linq;
using GitMonitor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GitMonitor.Controllers
{
    /// <summary>
    /// Repository operations controller.
    /// </summary>
    [ApiController]
    [Route(ApiConstants.ApiPath + "repository")]
    public class RepositoryController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryController"/> class.
        /// </summary>
        /// <param name="yamlConfigurationService">The YAML configuration service.</param>
        /// <param name="gitService">The git service.</param>
        public RepositoryController(YamlConfigurationService yamlConfigurationService, GitService gitService)
        {
            YamlConfigurationService = yamlConfigurationService;
            GitService = gitService;
        }

        private YamlConfigurationService YamlConfigurationService { get; }

        private GitService GitService { get; }

        [HttpGet("/diff")]
        public ActionResult<string> GetDiff(string repositoryName, string commitHash)
        {
            var repositoryDescriptor = YamlConfigurationService.Repositories?.Find(r => r.Name == repositoryName);

            if (repositoryDescriptor == null)
            {
                return NotFound($"Repository '{repositoryName}' not found");
            }

            string? diff = GitService.GetDiff(repositoryDescriptor, commitHash);

            if (diff == null)
            {
                return NotFound($"Commit '{commitHash}' not found");
            }

            return diff;
        }
    }
}
