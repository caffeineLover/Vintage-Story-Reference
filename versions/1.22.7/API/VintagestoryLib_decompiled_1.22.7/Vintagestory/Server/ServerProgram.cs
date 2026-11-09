using System;
using System.Runtime.InteropServices;
using CommandLine;
using VSPlatform;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.ClientNative;
using Vintagestory.Common;
using Vintagestory.Common.Convert;

namespace Vintagestory.Server;

public class ServerProgram
{
	private static ServerMain server;

	private static string[] args;

	private static ServerProgramArgs progArgs;

	private static readonly PosixSignalRegistration[] Signals = new PosixSignalRegistration[3];

	public static void Main(string[] args)
	{
		AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.AssemblyResolve;
		if (RuntimeEnv.OS == OS.Windows)
		{
			ConsoleWindowUtil.QuickEditMode(Enable: false);
		}
		ServerProgram.args = args;
		new ServerProgram();
	}

	private static void OnExit(PosixSignalContext ctx)
	{
		ctx.Cancel = true;
		ServerMain.Logger.Notification("Server termination event " + ctx.Signal.ToString() + " received. Shutting down server");
		if (server != null && server.RunPhase != EnumServerRunPhase.Standby)
		{
			server.EnqueueMainThreadTask(delegate
			{
				EnumExitMode exitMode = ctx.Signal switch
				{
					PosixSignal.SIGTERM => EnumExitMode.SoftExit, 
					PosixSignal.SIGINT => EnumExitMode.SoftExit, 
					PosixSignal.SIGQUIT => EnumExitMode.HardExit, 
					_ => EnumExitMode.SoftExit, 
				};
				server.Stop("External close event " + ctx.Signal.ToString() + " received", exitMode);
			});
			ServerMain.Logger.Notification("Server: Exit() called");
		}
	}

	public ServerProgram()
	{
		progArgs = new ServerProgramArgs();
		ParserResult<ServerProgramArgs> progArgsRaw = new Parser(delegate(ParserSettings config)
		{
			config.HelpWriter = null;
			config.AutoHelp = false;
			config.AutoVersion = false;
		}).ParseArguments<ServerProgramArgs>(args);
		progArgs = progArgsRaw.Value;
		if (progArgs.DataPath != null && progArgs.DataPath.Length > 0)
		{
			GamePaths.DataPath = progArgs.DataPath;
		}
		if (progArgs.LogPath != null && progArgs.LogPath.Length > 0)
		{
			GamePaths.CustomLogPath = progArgs.LogPath;
		}
		if (progArgs.PrintVersion)
		{
			Console.WriteLine();
			Console.Write("1.22.7");
		}
		else if (progArgs.PrintHelp)
		{
			Console.WriteLine();
			Console.Write(progArgs.GetUsage(progArgsRaw));
		}
		else
		{
			GamePaths.EnsurePathsExist();
			CrashReporter.EnableGlobalExceptionHandling(blnIsConsole: true);
			new CrashReporter(EnumAppSide.Server).Start(Main);
		}
	}

	private void Main()
	{
		ServerMain.xPlatInterface = XPlatformInterfaces.GetInterface();
		Signals[0] = PosixSignalRegistration.Create(PosixSignal.SIGTERM, OnExit);
		Signals[1] = PosixSignalRegistration.Create(PosixSignal.SIGINT, OnExit);
		Signals[2] = PosixSignalRegistration.Create(PosixSignal.SIGQUIT, OnExit);
		ServerMain.Logger = new ServerLogger(progArgs);
		TyronThreadPool.Inst.Logger = ServerMain.Logger;
		AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
		Lang.PreLoad(ServerMain.Logger, GamePaths.AssetsPath, ServerSettings.Language);
		if (!CleanInstallCheck.IsCleanInstall())
		{
			ServerMain.Logger.Error("Your Server installation still contains old files from a previous game version, which may break things. Please fully delete the /assets folder and then do a full reinstallation. Shutting down server.");
			Environment.Exit(0);
			return;
		}
		server = new ServerMain(null, args, progArgs);
		if (progArgs.GenConfigAndExit || progArgs.SetConfigAndExit != null)
		{
			Environment.Exit(server.ExitCode);
			return;
		}
		ServerMain.Logger.Notification("C# Framework: " + FrameworkInfos());
		ServerMain.Logger.Notification("Zstd Version: " + ZstdNative.Version);
		ServerMain.Logger.Notification("Operating System: " + RuntimeEnv.GetOsString());
		ServerMain.Logger.Notification("CPU Cores: {0}", Environment.ProcessorCount);
		ServerMain.Logger.Notification("CPU: {0}", ServerMain.xPlatInterface.GetCpuInfo());
		ServerMain.Logger.Notification("Available RAM: {0} MB", ServerMain.xPlatInterface.GetRamCapacity() / 1024);
		server.exitState = new GameExitState();
		server.Standalone = true;
		server.PreLaunch();
		if (progArgs.Standby)
		{
			server.StandbyLaunch();
		}
		else
		{
			server.Launch();
		}
		do
		{
			server.Process();
		}
		while (server.exitState == null || !server.exitState.Exit);
		server.Stop("Stop through standalone server exit request", server.exitState.Mode);
		server.Dispose();
		Environment.Exit(server.ExitCode);
	}

	private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs ue)
	{
		if (ue.ExceptionObject is Exception e)
		{
			ServerMain.Logger.Fatal("Unhandled exception: ");
			ServerMain.Logger.Fatal(e);
		}
		else
		{
			ServerMain.Logger.Fatal("Unhandled exception: {0}", ue.ExceptionObject);
		}
	}

	public static string FrameworkInfos()
	{
		return ".net " + Environment.Version;
	}
}
