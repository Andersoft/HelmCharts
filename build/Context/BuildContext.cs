using System.Collections.Generic;
using Cake.Core;
using Cake.Frosting;

namespace Build.Context
{
    public class BuildContext : FrostingContext
    {
        public BuildContext(ICakeContext context)
        : base(context)
        {
            SolutionPath = context.Arguments.GetArgument("solution_path");
            HelmRepositoryUsername = context.Arguments.GetArgument("helm_repository_username");
            HelmRepositoryPassword = context.Arguments.GetArgument("helm_repository_password");
            Version = context.Arguments.GetArgument("chart_version");
            HelmRepositoryAddress = context.Arguments.GetArgument("helm_repository_address");
        }

        public string SolutionPath { get; internal set; }

        public string HelmRepositoryUsername { get; set; }
        public string HelmRepositoryPassword { get; set; }

        public string Version { get; set; }

        public string HelmRepositoryAddress { get; set; }
    }
}
