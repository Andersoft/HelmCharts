using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Helm;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Add Helm repository")]
public sealed class AddRemoteRepository : AsyncFrostingTask<BuildContext>
{
  public override async Task RunAsync(BuildContext context)
  {
    var options = new HelmRepositoryOptions()
    {
      RepositoryAddress= context.HelmRepositoryAddress,
      RepositoryUsername = context.HelmRepositoryUsername,
      RepositoryPassword = context.HelmRepositoryPassword
    };

    if (await context.TryAuthenticateHelmRepository(options) is false)
    {
      throw new Exception("Failed to add helm repository");
    }
  }
}