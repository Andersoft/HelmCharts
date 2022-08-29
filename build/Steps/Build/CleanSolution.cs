using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Git;
using Cake.Frosting;
using Cake.Git;

namespace Build.Steps.Build;

[TaskName("Clean Solution")]
public sealed class CleanSolution : AsyncFrostingTask<BuildContext>
{
  public override async Task RunAsync(BuildContext context)
  {
    if(await context.TryGitClean() is false) 
    {
      throw new Exception("Failed to clean solution");
    };
  }
}