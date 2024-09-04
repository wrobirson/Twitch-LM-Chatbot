using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;
using Nuke.Common.Tooling;

class Build : NukeBuild
{
    [Solution] readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = Configuration.Release;

    AbsolutePath ServerProject => RootDirectory / "TwitchLMChatBot.Server";
    AbsolutePath ClientProject => RootDirectory / "TwitchLMChatBot.Client";
    AbsolutePath ClientBuildOutput => ClientProject / "dist";
    AbsolutePath PusblishDirectory => RootDirectory / "publish";

    AbsolutePath OutputWebRoot => PusblishDirectory / "wwwroot";

    AbsolutePath DevelopConfig = RootDirectory / "publish" / "appsettings.Development.json";

    [Parameter("Runtime identifier for self-contained deployment (e.g., net8.0)")]
    readonly string FrameworkIdentifier = "net8.0";  // 

    [Parameter("Runtime identifier for self-contained deployment (e.g., win-x64)")]
    readonly string RuntimeIdentifier = "win-x86";  // 

    Target Compile => _ => _
        .Executes(() =>
        {
            PusblishDirectory.DeleteDirectory();
            PusblishDirectory.CreateOrCleanDirectory();

            DotNetRestore(s => s
             .SetProjectFile(Solution));

            DotNetPublish(s => s
                .SetProject(ServerProject)
                .SetConfiguration(Configuration)
                .SetSelfContained(true)
                .SetFramework(FrameworkIdentifier)
                .SetRuntime(RuntimeIdentifier)                // Enable self-contained deployment
                .EnablePublishSingleFile()            // Enable single file output
                //.EnablePublishTrimmed()
                .SetOutput(PusblishDirectory));

            NpmInstall(s => s
               .SetProcessWorkingDirectory(ClientProject));

            NpmRun(s => s
                .SetCommand("build")
                .SetProcessWorkingDirectory(ClientProject));

            CopyDirectoryRecursively(ClientBuildOutput, 
                OutputWebRoot, DirectoryExistsPolicy.Merge);

            DevelopConfig.DeleteFile();
        });

    public static int Main() => Execute<Build>(x => x.Compile);
}
