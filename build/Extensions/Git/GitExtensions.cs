using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

namespace Build.Extensions.Git;

public static class GitExtensions
{
  const string BinaryName = "git";
  const string ArgumentSeparator = " ";

  public static async Task<bool> TryGitClean(this ICakeContext context)
  {
    var args = new[] { "clean", "-fxd" };
    context.Log.Information($"git {string.Join(" ", args)}");

    var result = await Cli.Wrap(BinaryName)
      .WithArguments(args, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}
