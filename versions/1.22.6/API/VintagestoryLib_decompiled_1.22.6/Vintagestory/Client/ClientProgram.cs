using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CommandLine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.ClientNative;
using Vintagestory.Common;
using Vintagestory.Server;
using Vintagestory.Server.Network;

namespace Vintagestory.Client;

public class ClientProgram
{
	private CrashReporter crashreporter;

	private DummyNetwork dummyNetwork;

	private DummyNetwork dummyNetworkUdp;

	private StartServerArgs startServerargs;

	private static Logger logger;

	public ClientPlatformWindows platform;

	private static string[] rawArgs;

	private static ClientProgramArgs progArgs;

	private static readonly PosixSignalRegistration[] Signals = new PosixSignalRegistration[2];

	public static ScreenManager screenManager;

	public static void Main(string[] rawArgs)
	{
		AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.AssemblyResolve;
		ClientProgram.rawArgs = rawArgs;
		new ClientProgram(rawArgs);
	}

	public ClientProgram(string[] rawArgs)
	{
		ClientProgram clientProgram = this;
		AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
		progArgs = new ClientProgramArgs();
		ParserResult<ClientProgramArgs> progArgsRaw = new Parser(delegate(ParserSettings config)
		{
			config.HelpWriter = null;
			config.IgnoreUnknownArguments = true;
			config.AutoHelp = false;
			config.AutoVersion = false;
		}).ParseArguments<ClientProgramArgs>(rawArgs);
		progArgs = progArgsRaw.Value;
		if (progArgs.DataPath != null && progArgs.DataPath.Length > 0)
		{
			GamePaths.DataPath = progArgs.DataPath;
		}
		if (progArgs.LogPath != null && progArgs.LogPath.Length > 0)
		{
			GamePaths.CustomLogPath = progArgs.LogPath;
		}
		if (RuntimeEnv.OS == OS.Windows && (progArgs.PrintVersion || progArgs.PrintHelp))
		{
			WindowsConsole.Attach();
		}
		if (progArgs.PrintVersion)
		{
			Console.WriteLine("1.22.6");
			return;
		}
		if (progArgs.PrintHelp)
		{
			Console.WriteLine(progArgs.GetUsage(progArgsRaw));
			return;
		}
		if (progArgs.InstallModId != null)
		{
			progArgs.InstallModId = progArgs.InstallModId.Replace("vintagestorymodinstall://", "");
		}
		UriHandler handler = UriHandler.Instance;
		if (handler.TryConnectClientPipe())
		{
			if (progArgs.ConnectServerAddress != null)
			{
				handler.SendConnect(progArgs.ConnectServerAddress);
				handler.Dispose();
				return;
			}
			if (progArgs.InstallModId != null)
			{
				handler.SendModInstall(progArgs.InstallModId);
				handler.Dispose();
				return;
			}
		}
		else
		{
			handler.StartPipeServer();
		}
		dummyNetwork = new DummyNetwork();
		dummyNetworkUdp = new DummyNetwork();
		dummyNetwork.Start();
		dummyNetworkUdp.Start();
		crashreporter = new CrashReporter(EnumAppSide.Client);
		try
		{
			crashreporter.Start(delegate
			{
				clientProgram.Start(progArgs, rawArgs);
			});
		}
		finally
		{
			handler.Dispose();
		}
	}

	private void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Exception exceptionObject = (Exception)e.ExceptionObject;
		if (crashreporter == null)
		{
			platform.XPlatInterface.ShowMessageBox("Fatal Error", exceptionObject.Message);
		}
		else if (!crashreporter.isCrashing)
		{
			crashreporter.Crash(exceptionObject);
		}
	}

	private unsafe void Start(ClientProgramArgs args, string[] rawArgs)
	{
		string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		if (!Debugger.IsAttached)
		{
			Environment.CurrentDirectory = appPath;
		}
		GamePaths.EnsurePathsExist();
		logger = new ClientLogger();
		logger.TraceLog = args.TraceLog;
		ClientPlatformWindows platform = new ClientPlatformWindows(logger);
		platform.ShaderUniforms.SepiaLevel = ClientSettings.SepiaLevel;
		platform.ShaderUniforms.ExtraContrastLevel = ClientSettings.ExtraContrastLevel;
		CrashReporter.SetLogger((Logger)platform.Logger);
		platform.LogAndTestHardwareInfosStage1();
		screenManager = new ScreenManager(platform);
		GuiStyle.DecorativeFontName = ClientSettings.DecorativeFontName;
		GuiStyle.StandardFontName = ClientSettings.DefaultFontName;
		if (!Directory.Exists(GamePaths.AssetsPath))
		{
			platform.XPlatInterface.ShowMessageBox("Vintagestory Warning", "Critical files are missing from your game installation. Please try re-installing.");
			return;
		}
		Lang.PreLoad(ScreenManager.Platform.Logger, GamePaths.AssetsPath, ClientSettings.Language);
		if (RuntimeEnv.OS == OS.Windows && !ClientSettings.SkipNvidiaProfileCheck && NvidiaGPUFix64.SOP_SetProfile("Vintagestory", GetExecutableName()) == 1)
		{
			platform.XPlatInterface.ShowMessageBox("Vintagestory Nvidia Profile", Lang.Get("Your game is now configured to use your dedicated NVIDIA Graphics card. This requires a restart so please start the game again."));
			return;
		}
		if (!CleanInstallCheck.IsCleanInstall())
		{
			platform.XPlatInterface.ShowMessageBox("Vintagestory Warning", Lang.Get("launchfailure-notcleaninstall"));
			return;
		}
		if (RuntimeEnv.OS == OS.Windows && !ClientSettings.MultipleInstances)
		{
			new Mutex(initiallyOwned: true, "Vintagestory", out var createdNew);
			if (!createdNew)
			{
				platform.XPlatInterface.ShowMessageBox(Lang.Get("Multiple Instances"), Lang.Get("game-alreadyrunning"));
				return;
			}
		}
		Signals[0] = PosixSignalRegistration.Create(PosixSignal.SIGTERM, OnExit);
		Signals[1] = PosixSignalRegistration.Create(PosixSignal.SIGINT, OnExit);
		platform.SetServerExitInterface(platform.ServerExitState);
		platform.crashreporter = crashreporter;
		platform.singlePlayerServerDummyNetwork = new DummyNetwork[2];
		platform.singlePlayerServerDummyNetwork[0] = dummyNetwork;
		platform.singlePlayerServerDummyNetwork[1] = dummyNetworkUdp;
		this.platform = platform;
		platform.OnStartSinglePlayerServer = delegate(StartServerArgs serverargs)
		{
			startServerargs = serverargs;
			Thread thread = new Thread(ServerThreadStart);
			thread.Name = "SingleplayerServer";
			thread.Priority = ThreadPriority.BelowNormal;
			thread.IsBackground = true;
			thread.Start();
		};
		WindowState windowState = ClientSettings.GameWindowMode switch
		{
			3 => WindowState.Fullscreen, 
			2 => WindowState.Maximized, 
			1 => WindowState.Fullscreen, 
			_ => WindowState.Normal, 
		};
		ScreenManager.Platform.Logger.Debug("Creating game window with window mode " + windowState);
		Size2i screenSize = ScreenManager.Platform.ScreenSize;
		if (ClientSettings.IsNewSettingsFile)
		{
			int width = 1280;
			int height = 850;
			float guiscale = 1f;
			if (screenSize.Width - 20 < width || screenSize.Height - 20 < height)
			{
				guiscale = 0.875f;
				width = Math.Min(screenSize.Width - 20, width);
				height = Math.Min(screenSize.Height - 20, height);
			}
			if (height < 680)
			{
				guiscale = 0.75f;
			}
			if (screenSize.Width > 2500)
			{
				guiscale = 1.25f;
			}
			if (screenSize.Width > 3000)
			{
				guiscale = 1.5f;
				width = 2000;
			}
			if (screenSize.Width > 5000)
			{
				guiscale = 2f;
			}
			if (screenSize.Height > 1300)
			{
				screenSize.Height = 1200;
			}
			ClientSettings.ScreenWidth = width;
			ClientSettings.ScreenHeight = height;
			ClientSettings.GUIScale = guiscale;
		}
		if (ClientSettings.ScreenWidth < 10)
		{
			ClientSettings.ScreenWidth = 10;
		}
		if (ClientSettings.ScreenHeight < 10)
		{
			ClientSettings.ScreenHeight = 10;
		}
		string[] array = ClientSettings.GlContextVersion.Split('.');
		int openGlMajor = array[0].ToInt(4);
		int openGlMinor = array[1].ToInt(3);
		GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
		NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
		{
			Title = "Vintage Story",
			APIVersion = new Version(openGlMajor, openGlMinor),
			ClientSize = new Vector2i(ClientSettings.ScreenWidth, ClientSettings.ScreenHeight),
			Flags = ContextFlags.Default,
			Vsync = ((ClientSettings.VsyncMode != 0) ? VSyncMode.On : VSyncMode.Off),
			WindowState = windowState,
			WindowBorder = (WindowBorder)ClientSettings.WindowBorder
		};
		if (RuntimeEnv.OS == OS.Mac)
		{
			nativeWindowSettings.Flags = ContextFlags.ForwardCompatible;
		}
		GLFW.SetErrorCallback(GlfwErrorCallback);
		GameWindowNative gamewindow = AttemptToOpenWindow(gameWindowSettings, nativeWindowSettings, openGlMajor, openGlMinor, 3);
		if (windowState == WindowState.Normal && !RuntimeEnv.IsWaylandSession)
		{
			try
			{
				gamewindow.CenterWindow();
			}
			catch (Exception e)
			{
				ScreenManager.Platform.Logger.Warning("Error trying to center the window");
				ScreenManager.Platform.Logger.Warning(e);
			}
		}
		platform.StartAudio();
		platform.LogAndTestHardwareInfosStage2();
		platform.window = gamewindow;
		platform.XPlatInterface.Window = gamewindow;
		platform.SetDirectMouseMode(ClientSettings.DirectMouseMode);
		platform.WindowSize.Width = gamewindow.ClientSize.X;
		platform.WindowSize.Height = gamewindow.ClientSize.Y;
		if (ClientSettings.GameWindowMode == 3)
		{
			platform.SetWindowAttribute(WindowAttribute.AutoIconify, value: false);
		}
		screenManager.Start(args, rawArgs);
		platform.Start();
		Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
		try
		{
			gamewindow.Run();
		}
		finally
		{
			if (RuntimeEnv.OS == OS.Windows)
			{
				GLFW.IconifyWindow(gamewindow.WindowPtr);
			}
			Thread.CurrentThread.Priority = ThreadPriority.Normal;
			ScreenManager.Platform.Logger.Debug("After gamewindow.Run()");
			platform.DisposeFrameBuffers(platform.FrameBuffers);
			platform.StopAudio();
			gamewindow.Dispose();
		}
	}

	private GameWindowNative AttemptToOpenWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, int openGlMajor, int openGlMinor, int tries)
	{
		nativeWindowSettings.APIVersion = new Version(openGlMajor, openGlMinor);
		GameWindowNative gamewindow;
		try
		{
			gamewindow = new GameWindowNative(gameWindowSettings, nativeWindowSettings);
		}
		catch (Exception ex)
		{
			bool throwException = false;
			if (tries <= 1)
			{
				throwException = true;
			}
			else if (ex is GLFWException ge)
			{
				int j = ge.Message.IndexOf(openGlMinor + ", got version ");
				if (j > 0)
				{
					openGlMajor = ex.Message[j + 15] - 48;
					openGlMinor = ex.Message[j + 17] - 48;
					Thread.Sleep(100);
					return AttemptToOpenWindow(gameWindowSettings, nativeWindowSettings, openGlMajor, openGlMinor, tries - 1);
				}
				j = ge.Message.IndexOf("OpenGL version " + openGlMajor + "." + openGlMinor);
				if (j < 0)
				{
					throwException = true;
				}
			}
			if (openGlMajor < 3 || (openGlMajor == 3 && openGlMinor <= 3))
			{
				throwException = true;
			}
			if (throwException)
			{
				throw new Exception("** Unable to start OpenGL graphics. Try: (1) restart the computer (2) update the graphics driver (3) more advice at wiki.vintagestory.at/Troubleshooting_Guide#OpenGL_crash **", ex);
			}
			if (openGlMajor > 4)
			{
				openGlMajor = 4;
				openGlMinor = 3;
			}
			else if (openGlMajor == 4 && openGlMinor > 3)
			{
				openGlMinor = 3;
			}
			else
			{
				openGlMajor = 3;
				openGlMinor = 3;
			}
			Thread.Sleep(100);
			return AttemptToOpenWindow(gameWindowSettings, nativeWindowSettings, openGlMajor, openGlMinor, tries - 1);
		}
		if (openGlMajor < 4 || (openGlMajor == 4 && openGlMinor < 3))
		{
			ClientSettings.AllowSSBOs = false;
		}
		return gamewindow;
	}

	private void GlfwErrorCallback(ErrorCode error, string description)
	{
		if (error == ErrorCode.FormatUnavailable)
		{
			platform.Logger.Debug("GLFW FormatUnavailable: " + description);
			return;
		}
		platform.Logger.Error($"GLFW Exception: ErrorCode:{error} {description}");
	}

	private void OnExit(PosixSignalContext ctx)
	{
		ctx.Cancel = true;
		UriHandler.Instance.Dispose();
		screenManager.GamePlatform.WindowExit("SIGTERM or SIGINT received", EnumExitMode.HardExit);
	}

	public void ServerThreadStart()
	{
		ServerMain server = null;
		ServerProgramArgs serverArgs = new Parser(delegate(ParserSettings config)
		{
			config.IgnoreUnknownArguments = true;
			config.AutoHelp = false;
			config.AutoVersion = false;
		}).ParseArguments<ServerProgramArgs>(rawArgs).Value;
		dummyNetwork.Clear();
		platform.Logger.Notification("Server args parsed");
		try
		{
			server = new ServerMain(startServerargs, rawArgs, serverArgs, isDedicatedServer: false);
			platform.Logger.Notification("Server main instantiated");
			server.exitState = platform.ServerExitState;
			DummyTcpNetServer netServer = new DummyTcpNetServer();
			netServer.SetNetwork(dummyNetwork);
			server.MainSockets[0] = netServer;
			DummyUdpNetServer udpNetServer = new DummyUdpNetServer();
			udpNetServer.SetNetwork(dummyNetworkUdp);
			server.UdpSockets[0] = udpNetServer;
			platform.IsServerRunning = true;
			platform.SetGamePausedState(paused: false);
			server.PreLaunch();
			server.Launch();
			platform.Logger.Notification("Server launched");
			bool wasPaused = false;
			do
			{
				if (!wasPaused && platform.IsGamePaused)
				{
					server.Suspend(newSuspendState: true);
					wasPaused = true;
				}
				if (wasPaused && !platform.IsGamePaused)
				{
					server.Suspend(newSuspendState: false);
					wasPaused = false;
				}
				server.Process();
				if (!platform.singlePlayerServerLoaded)
				{
					platform.Logger.VerboseDebug("--- Server started ---");
				}
				platform.singlePlayerServerLoaded = true;
			}
			while (platform.ServerExitState == null || platform.ServerExitState.Mode == EnumExitMode.None);
			server.Stop("Exit request by client", platform.ServerExitState.Mode);
			platform.IsServerRunning = false;
			platform.singlePlayerServerLoaded = false;
			server.Dispose();
		}
		catch (Exception ex)
		{
			platform.Logger.Fatal(ex);
			if (server != null)
			{
				server.Stop("Exception thrown by server during startup or process", EnumExitMode.SoftExit);
				platform.IsServerRunning = false;
				platform.singlePlayerServerLoaded = false;
				try
				{
					server.Dispose();
				}
				catch (Exception)
				{
				}
			}
			if (ex is RestartGameException)
			{
				screenManager.OfferRestart(ex.Message);
			}
		}
		dummyNetwork.Clear();
	}

	private static string GetExecutableName()
	{
		string fileName = Process.GetCurrentProcess().MainModule.FileName;
		int num = fileName.LastIndexOf('\\');
		return fileName.Substring(num + 1, fileName.Length - num - 1);
	}
}
