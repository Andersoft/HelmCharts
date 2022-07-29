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
            HelmRepositoryName = context.Arguments.GetArgument("helm_repository_name");
            Version = context.Arguments.GetArgument("chart_version");
            HelmRepositoryAddress = context.Arguments.GetArgument("helm_repository_address");
        }

        public string SolutionPath { get; internal set; }

        public string HelmRepositoryName { get; set; }

        public string Version { get; set; }

        public string HelmRepositoryAddress { get; set; }
    }
}
