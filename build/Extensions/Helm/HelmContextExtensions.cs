using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

namespace Build.Extensions.Helm;

public static class HelmContextExtensions
{
  const string BinaryName = "helm";
  const string ArgumentSeparator = " ";

  public static async Task<bool> TryAuthenticateHelmRepository(
    this ICakeContext context,
    HelmRepositoryOptions options)
  {
    var args = new[] { "registry", "login", options.RepositoryAddress, $"--username {options.RepositoryUsername}", $"--password {options.RepositoryPassword}" };
    context.Log.Information($"helm {string.Join(" ", args)}");

    var result = await Cli.Wrap(BinaryName)
      .WithArguments(args, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }

  public static async Task<bool> TryPublishHelmChartAsync(
    this ICakeContext context,
    HelmPublishOptions options)
  {
    var packages = Directory.EnumerateFiles(options.PackageFolder);

    foreach (var package in packages)
    {
      var args = new string[] {"push", package, $"oci://{options.RepositoryAddress}/helm"};
      context.Log.Information($"helm {string.Join(" ", args)}");

      var result = await Cli.Wrap(BinaryName)
        .WithArguments(args, false)
        .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
        .ExecuteBufferedAsync();

      if (result.ExitCode!=0)
      {
        return false;
      }
    }

    return true;
  }
  

  public static async Task<bool> TryPackageHelmChartAsync(
    this ICakeContext context,
    HelmPackageOptions options)
  {
    string arguments = string.IsNullOrWhiteSpace(options.AppVersion)
      ? string.Empty
      : $"--app-version {options.AppVersion}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.DependencyUpdate)
      ? string.Empty
      : $"--dependency-update {options.DependencyUpdate}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Destination)
      ? string.Empty
      : $"--destination {options.Destination}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Key)
      ? string.Empty
      : $"--key  {options.Key}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Keyring)
      ? string.Empty
      : $"--keyring  {options.Keyring}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.PassphraseFile)
      ? string.Empty
      : $"--passphrase-file {options.PassphraseFile}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Sign)
      ? string.Empty
      : $"--sign {options.Sign}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Version)
      ? string.Empty
      : $"--version {options.Version}{ArgumentSeparator}";

    context.Log.Information($"helm package {options.ChartPath} {arguments}");

    var result = await Cli.Wrap(BinaryName)
      .WithWorkingDirectory(options.ChartPath)
      .WithArguments(new[] { "package", options.ChartPath, arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}