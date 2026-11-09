using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Vintagestory.API.Util;

namespace Vintagestory.API.Config;

/// <summary>
/// Information about the runningtime environment
/// </summary>
public static class RuntimeEnv
{
	/// <summary>
	/// If TEXTURE_DEBUG_DISPOSE is set, the initial value set here will be overridden
	/// </summary>
	public static bool DebugTextureDispose;

	/// <summary>
	/// If VAO_DEBUG_DISPOSE is set, the initial value set here will be overridden
	/// </summary>
	public static bool DebugVAODispose;

	/// <summary>
	/// Debug sound memory leaks. No ENV var
	/// </summary>
	public static bool DebugSoundDispose;

	/// <summary>
	/// If true, will print the stack trace on some of the blockaccessor if something attempts to get or set blocks outside of its available chunks
	/// </summary>
	public static bool DebugOutOfRangeBlockAccess;

	/// <summary>
	/// If true, will print allocation trace whenever a new task was enqueued to the thread pool
	/// </summary>
	public static bool DebugThreadPool;

	public static int MainThreadId;

	public static int ServerMainThreadId;

	public static float GUIScale;

	/// <summary>
	/// The current operating system
	/// </summary>
	public static readonly OS OS;

	/// <summary>
	/// The Env variable which contains the OS specific search paths for libarires
	/// </summary>
	public static readonly string EnvSearchPathName;

	/// <summary>
	/// This will only be set to true on Linux when the session is a wayland session
	/// This is needed so we can skip adding a app Icon or setting the cursor postion since those are not supported on wayland
	/// </summary>
	public static readonly bool IsWaylandSession;

	/// <summary>
	/// Whether we are in a dev environment or not
	/// </summary>
	public static readonly bool IsDevEnvironment;

	static RuntimeEnv()
	{
		DebugTextureDispose = false;
		DebugVAODispose = false;
		DebugSoundDispose = false;
		DebugOutOfRangeBlockAccess = false;
		DebugThreadPool = false;
		IsDevEnvironment = !Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets"));
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			OS = OS.Windows;
			EnvSearchPathName = "PATH";
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			OS = OS.Linux;
			EnvSearchPathName = "LD_LIBRARY_PATH";
			string sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE");
			string useWayland = Environment.GetEnvironmentVariable("OPENTK_4_USE_WAYLAND");
			if (sessionType == "wayland" && useWayland == null)
			{
				Environment.SetEnvironmentVariable("OPENTK_4_USE_WAYLAND", "0");
				IsWaylandSession = false;
			}
			else
			{
				IsWaylandSession = sessionType == "wayland" && useWayland != "0";
			}
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			OS = OS.Mac;
			EnvSearchPathName = "DYLD_FRAMEWORK_PATH";
		}
	}

	public static string GetLocalIpAddress()
	{
		try
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				if (networkInterface.OperationalStatus != OperationalStatus.Up)
				{
					continue;
				}
				IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
				if (iPProperties.GatewayAddresses.Count == 0)
				{
					continue;
				}
				foreach (UnicastIPAddressInformation unicastAddress in iPProperties.UnicastAddresses)
				{
					if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(unicastAddress.Address))
					{
						return unicastAddress.Address.ToString();
					}
				}
			}
			return "Unknown ip";
		}
		catch (Exception)
		{
			try
			{
				return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault((IPAddress ip) => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
			}
			catch (Exception)
			{
				return "Unknown ip";
			}
		}
	}

	public static string GetOsString()
	{
		switch (OS)
		{
		case OS.Windows:
			return $"Windows {Environment.OSVersion.Version}";
		case OS.Mac:
			return $"Mac {Environment.OSVersion.Version}";
		case OS.Linux:
			try
			{
				if (File.Exists("/etc/os-release"))
				{
					string distro = File.ReadAllLines("/etc/os-release").FirstOrDefault((string line) => line.StartsWithOrdinal("PRETTY_NAME="))?.Split('=').ElementAt(1).Trim('"');
					return $"Linux ({distro}) [Kernel {Environment.OSVersion.Version}]";
				}
			}
			catch (Exception)
			{
			}
			return $"Linux (Unknown) [Kernel {Environment.OSVersion.Version}]";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
