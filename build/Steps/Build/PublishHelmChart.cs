using System;
using System.IO;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Helm;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Publish Helm Chart")]
[IsDependentOn(typeof(PackageHelmChart))]
public sealed class PublishHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    var options = new HelmPublishOptions
    {
      PackageFolder = Path.GetFullPath(Path.Combine(context.SolutionPath, "artifacts/helm/")),
      RepositoryAddress = context.HelmRepositoryAddress
    };

    if (await context.TryPublishHelmChartAsync(options) is false)
    {
      throw new Exception("Failed to package helm chart");
    }
  }
}