using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using Cairo;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using VSPlatform;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.ClientNative;
using Vintagestory.Common;
using Vintagestory.Common.Convert;

namespace Vintagestory.Client.NoObf;

public sealed class ClientPlatformWindows : ClientPlatformAbstract
{
	public class GLBuffer
	{
		public int BufferId;
	}

	private AudioOpenAl audio;

	public GameExitState gameexit;

	public bool SupportsThickLines;

	private int cpuCoreCount = 2;

	private AssetManager assetManager;

	private Stopwatch frameStopWatch;

	internal Stopwatch uptimeStopWatch = new Stopwatch();

	private Logger logger;

	private int doResize;

	private static DebugProc _debugProcCallback;

	private static GCHandle _debugProcCallbackHandle;

	public GameWindowNative window;

	private Screenshot screenshot = new Screenshot();

	public Action<StartServerArgs> OnStartSinglePlayerServer;

	public GameExitState ServerExitState = new GameExitState();

	public bool singlePlayerServerLoaded;

	public DummyNetwork[] singlePlayerServerDummyNetwork;

	private Size2i windowsize = new Size2i();

	private Size2i screensize = new Size2i();

	private List<OnFocusChanged> focusChangedDelegates = new List<OnFocusChanged>();

	private ActionBoolReturn windowClosedHandler;

	private NewFrameHandler frameHandler;

	public CrashReporter crashreporter;

	private OnCrashHandler onCrashHandler;

	public List<KeyEventHandler> keyEventHandlers = new List<KeyEventHandler>();

	public List<MouseEventHandler> mouseEventHandlers = new List<MouseEventHandler>();

	public Action<string> fileDropEventHandler;

	private bool debugDrawCalls;

	private List<string> drawCallStacks = new List<string>();

	private List<FrameBufferRef> frameBuffers;

	private MeshRef screenQuad;

	private bool serverRunning;

	private bool gamepause;

	private bool OffscreenBuffer = true;

	private bool RenderBloom;

	private bool RenderGodRays;

	private bool RenderFXAA;

	private bool RenderSSAO;

	private bool SetupSSAO;

	private int ShadowMapQuality;

	private float ssaaLevel;

	public GLBuffer[] PixelPackBuffer;

	public int sampleCount = 32;

	public int CurrentPixelPackBufferNum;

	private Random rand = new Random();

	private float[] ssaoKernel = new float[192];

	private FrameBufferRef curFb;

	private float[] clearColor = new float[4] { 0f, 0f, 0f, 1f };

	private bool glDebugMode;

	private bool supportsGlDebugMode;

	private bool supportsPersistentMapping;

	public bool ENABLE_MIPMAPS = true;

	public bool ENABLE_ANISOTROPICFILTERING;

	public bool ENABLE_TRANSPARENCY = true;

	public bool allowPStorage;

	[ThreadStatic]
	private static FaceData[] facedataBuffer;

	[ThreadStatic]
	private static int[] customIntsPruned;

	private const float minUV = -1.5E-05f;

	private const float maxUV = 1.000015f;

	private Vector2 previousMousePosition;

	private CursorState previousCursorState;

	private float mouseX;

	private float mouseY;

	private Dictionary<string, MouseCursor> preLoadedCursors = new Dictionary<string, MouseCursor>();

	private bool ignoreNormalMouseMoveEvent;

	private int ignoreGrabbedMouseMoveEvents;

	private float prevWheelValue;

	private long lastKeyUpMs;

	private int lastKeyUpKey;

	private ShaderProgramMinimalGui minimalGuiShaderProgram;

	public override IList<string> AvailableAudioDevices => audio.Devices;

	public override string CurrentAudioDevice
	{
		get
		{
			return audio.CurrentDevice;
		}
		set
		{
			LoadedSoundNative.ChangeOutputDevice(delegate
			{
				audio.RecreateContext(logger);
			});
		}
	}

	public override float MasterSoundLevel
	{
		get
		{
			return audio.MasterSoundLevel;
		}
		set
		{
			audio.MasterSoundLevel = value;
		}
	}

	public override AssetManager AssetManager => assetManager;

	public override ILogger Logger => logger;

	public override long EllapsedMs => uptimeStopWatch.ElapsedMilliseconds;

	public override Size2i WindowSize => windowsize;

	public override Size2i ScreenSize => screensize;

	public override EnumWindowBorder WindowBorder
	{
		get
		{
			return (EnumWindowBorder)window.WindowBorder;
		}
		set
		{
			window.WindowBorder = (WindowBorder)value;
		}
	}

	public override int CpuCoreCount => cpuCoreCount;

	public override bool IsFocused => window.IsFocused;

	public override bool DebugDrawCalls
	{
		get
		{
			return debugDrawCalls;
		}
		set
		{
			debugDrawCalls = value;
			if (!value)
			{
				logger.Notification("Call stacks:");
				int i = 0;
				foreach (string val in drawCallStacks)
				{
					logger.Notification("{0}: {1}", i++, val.Substring(0, 600));
				}
			}
			drawCallStacks.Clear();
		}
	}

	public override List<FrameBufferRef> FrameBuffers => frameBuffers;

	public override bool IsServerRunning
	{
		get
		{
			return serverRunning;
		}
		set
		{
			serverRunning = value;
		}
	}

	public override bool IsGamePaused => gamepause;

	public override FrameBufferRef CurrentFrameBuffer
	{
		get
		{
			return curFb;
		}
		set
		{
			curFb = value;
			if (value == null)
			{
				GL.BindFramebuffer((FramebufferTarget)36160, 0);
				return;
			}
			GL.BindFramebuffer((FramebufferTarget)36160, value.FboId);
			GL.Viewport(0, 0, value.Width, value.Height);
		}
	}

	private FrameBufferRef CurrentFrameBufferKeepVw
	{
		get
		{
			return curFb;
		}
		set
		{
			curFb = value;
			GL.BindFramebuffer((FramebufferTarget)36160, value?.FboId ?? 0);
		}
	}

	public override bool GlErrorChecking { get; set; }

	public override bool GlDebugMode
	{
		get
		{
			return glDebugMode;
		}
		set
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			if (value)
			{
				if (!supportsGlDebugMode)
				{
					throw new NotSupportedException("Your graphics card does not seem to support gl debug mode (neither GL_ARB_debug_output nor GL_KHR_debug was found)");
				}
				_debugProcCallback = new DebugProc(DebugCallback);
				_debugProcCallbackHandle = GCHandle.Alloc(_debugProcCallback);
				GL.DebugMessageCallback(_debugProcCallback, (IntPtr)IntPtr.Zero);
				GL.Enable((EnableCap)37600);
				GL.Enable((EnableCap)33346);
			}
			else
			{
				GL.Disable((EnableCap)37600);
				GL.Disable((EnableCap)33346);
			}
			glDebugMode = value;
		}
	}

	public override bool GlScissorFlagEnabled => GL.IsEnabled((EnableCap)3089);

	public override string CurrentMouseCursor { get; protected set; }

	public override bool MouseGrabbed
	{
		get
		{
			return window.CursorState == CursorState.Grabbed;
		}
		set
		{
			CursorState newState = (value ? CursorState.Grabbed : CursorState.Normal);
			if (newState != window.CursorState && !RuntimeEnv.IsWaylandSession)
			{
				Vector2 newPos = new Vector2((float)window.ClientSize.X / 2f, (float)window.ClientSize.Y / 2f);
				SetMousePosition(newPos.X, newPos.Y);
				window.MousePosition = newPos;
			}
			window.CursorState = newState;
		}
	}

	public override DefaultShaderUniforms ShaderUniforms { get; set; } = new DefaultShaderUniforms();

	public override ShaderProgramMinimalGui MinimalGuiShader => minimalGuiShaderProgram;

	public void StartAudio()
	{
		if (audio == null)
		{
			audio = new AudioOpenAl(logger);
		}
	}

	public override void AddAudioSettingsWatchers()
	{
		ClientSettings.Inst.AddWatcher("audioDevice", delegate(string newDevice)
		{
			CurrentAudioDevice = newDevice;
		});
		ClientSettings.Inst.AddWatcher<bool>("useHRTFaudio", delegate
		{
			LoadedSoundNative.ChangeOutputDevice(delegate
			{
				audio.RecreateContext(logger);
			});
		});
	}

	public void StopAudio()
	{
		ScreenManager.IntroMusic?.Dispose();
		if (audio != null)
		{
			audio.Dispose();
			audio = null;
		}
	}

	public override AudioData CreateAudioData(IAsset asset)
	{
		StartAudio();
		AudioMetaData sampleFromArray = audio.GetSampleFromArray(asset);
		sampleFromArray.Loaded = 2;
		return sampleFromArray;
	}

	public override ILoadedSound CreateAudio(SoundParams sound, AudioData data)
	{
		if ((data as AudioMetaData).Asset == null)
		{
			return null;
		}
		if (data.Loaded < 2)
		{
			if (data.Loaded == 0)
			{
				logger.VerboseDebug("Loading sound file, game may stutter " + (data as AudioMetaData)?.Asset.Location);
				data.Load();
			}
			else
			{
				logger.VerboseDebug("Attempt to use still-loading sound file, sound may error or not play " + (data as AudioMetaData)?.Asset.Location);
			}
		}
		return new LoadedSoundNative(sound, (AudioMetaData)data);
	}

	public override ILoadedSound CreateAudio(SoundParams sound, AudioData data, ClientMain game)
	{
		if ((data as AudioMetaData)?.Asset == null)
		{
			return null;
		}
		if (data.Loaded == 0)
		{
			logger.VerboseDebug("Loading sound file, game may stutter " + (data as AudioMetaData)?.Asset.Location);
			data.Load();
		}
		return new LoadedSoundNative(sound, (AudioMetaData)data, game);
	}

	public override void UpdateAudioListener(float posX, float posY, float posZ, float orientX, float orientY, float orientZ)
	{
		StartAudio();
		audio.UpdateListener(new Vector3(posX, posY, posZ), new Vector3(orientX, orientY, orientZ));
	}

	public ClientPlatformWindows(Logger logger)
	{
		if (logger == null)
		{
			this.logger = new NullLogger();
		}
		else
		{
			base.XPlatInterface = XPlatformInterfaces.GetInterface();
			screensize = base.XPlatInterface.GetScreenSize();
			this.logger = logger;
		}
		TyronThreadPool.Inst.Logger = this.logger;
		uptimeStopWatch.Start();
		frameStopWatch = new Stopwatch();
		frameStopWatch.Start();
	}

	private void window_RenderFrame(FrameEventArgs e)
	{
		if (doResize != 0 && Environment.TickCount >= doResize)
		{
			Window_Resize();
		}
		ScreenManager.FrameProfiler.Begin(null);
		if (ClientSettings.VsyncMode != 1 && base.MaxFps > 10f && base.MaxFps < 241f)
		{
			int freeTime = (int)(1000f / base.MaxFps - 1000f * (float)frameStopWatch.ElapsedTicks / (float)Stopwatch.Frequency);
			if (freeTime > 0)
			{
				Thread.Sleep(freeTime);
			}
		}
		float dt = (float)frameStopWatch.ElapsedTicks / (float)Stopwatch.Frequency;
		frameStopWatch.Restart();
		ScreenManager.FrameProfiler.Mark("sleep");
		UpdateMousePosition();
		RenderBloom = ClientSettings.Bloom && base.DoPostProcessingEffects;
		RenderGodRays = ClientSettings.GodRayQuality > 0 && base.DoPostProcessingEffects;
		RenderFXAA = ClientSettings.FXAA && base.DoPostProcessingEffects;
		RenderSSAO = ClientSettings.SSAOQuality > 0 && base.DoPostProcessingEffects;
		SetupSSAO = ClientSettings.SSAOQuality > 0;
		ShadowMapQuality = ClientSettings.ShadowMapQuality;
		ShaderProgramBase.shadowmapQuality = ShadowMapQuality;
		frameHandler.OnNewFrame(dt);
		window.SwapBuffers();
		ScreenManager.FrameProfiler.End();
	}

	public string GetGraphicsCardRenderer()
	{
		return GL.GetString((StringName)7937);
	}

	public void LogAndTestHardwareInfosStage1()
	{
		logger.Notification("Process path: {0}", GamePaths.RemoveUsername(Environment.ProcessPath));
		logger.Notification("Operating System: " + RuntimeEnv.GetOsString());
		logger.Notification("CPU Cores: {0}", Environment.ProcessorCount);
		logger.Notification("CPU: {0}", base.XPlatInterface.GetCpuInfo());
		logger.Notification("Available RAM: {0} MB", base.XPlatInterface.GetRamCapacity() / 1024);
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			LogFrameworkVersions();
		}
	}

	private void LogFrameworkVersions()
	{
		logger.Notification("C# Framework: " + GetFrameworkInfos());
		logger.Notification("Cairo Graphics Version: " + CairoAPI.VersionString);
	}

	public void LogAndTestHardwareInfosStage2()
	{
		logger.Notification("Graphics Card Vendor: " + GL.GetString((StringName)7936));
		logger.Notification("Graphics Card Version: " + GL.GetString((StringName)7938));
		logger.Notification("Graphics Card Renderer: " + GL.GetString((StringName)7937));
		logger.Notification("Graphics Card ShadingLanguageVersion: " + GL.GetString((StringName)35724));
		logger.Notification("GL.MaxVertexUniformComponents: " + GL.GetInteger((GetPName)35658));
		logger.Notification("GL.MaxUniformBlockSize: " + GL.GetInteger((GetPName)35376));
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			LogFrameworkVersions();
		}
		logger.Notification("OpenAL Version: " + AL.Get(ALGetString.Version));
		string path = System.IO.Path.Combine(GamePaths.Binaries, "Lib/OpenTK.dll");
		if (File.Exists(path))
		{
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(path);
			logger.Notification("OpenTK Version: " + fvi.FileVersion + " (" + fvi.Comments + ")");
		}
		logger.Notification("Zstd Version: " + ZstdNative.Version);
		CheckGlError("loghwinfo");
		if (RuntimeEnv.OS != OS.Mac && ClientSettings.TestGlExtensions)
		{
			HashSet<string> required = new HashSet<string>(new string[4] { "GL_ARB_framebuffer_object", "GL_ARB_vertex_array_object", "GL_ARB_draw_instanced", "GL_ARB_explicit_attrib_location" });
			int count = GL.GetInteger((GetPName)33309);
			for (int i = 0; i < count; i++)
			{
				string extension = GL.GetString((StringNameIndexed)7939, i);
				supportsGlDebugMode |= extension == "GL_ARB_debug_output" || extension == "GL_KHR_debug";
				supportsPersistentMapping |= extension == "GL_ARB_buffer_storage";
				if (required.Contains(extension))
				{
					required.Remove(extension);
				}
			}
			if (required.Count > 0)
			{
				throw new NotSupportedException("Your graphics card does not support the extensions " + string.Join(", ", required) + " which is required to start the game");
			}
		}
		CheckGlError("testhwinfo");
	}

	public override string GetGraphicCardInfos()
	{
		return "GC Vendor: " + GL.GetString((StringName)7936) + "\nGC Version: " + GL.GetString((StringName)7938) + "\nGC Renderer: " + GL.GetString((StringName)7937) + "\nGC ShaderVersion: " + GL.GetString((StringName)35724);
	}

	public override string GetFrameworkInfos()
	{
		return ".net " + Environment.Version;
	}

	public override bool IsExitAvailable()
	{
		return true;
	}

	public override void SetWindowSize(int width, int height)
	{
		window.ClientSize = new Vector2i(width, height);
		Window_Resize();
	}

	public override BitmapRef CreateBitmap(int width, int height)
	{
		return new BitmapExternal(width, height);
	}

	public override void SetBitmapPixelsArgb(BitmapRef bmp, int[] pixels)
	{
		BitmapExternal bitmapExternal = (BitmapExternal)bmp;
		int width = bitmapExternal.bmp.Width;
		int height = bitmapExternal.bmp.Height;
		FastBitmap fastBitmap = new FastBitmap();
		fastBitmap.bmp = bitmapExternal.bmp;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				fastBitmap.SetPixel(i, j, pixels[i + j * width]);
			}
		}
	}

	public override BitmapRef CreateBitmapFromPng(IAsset asset)
	{
		using MemoryStream stream = new MemoryStream(asset.Data);
		return new BitmapExternal(stream, Logger, asset.Location);
	}

	public override BitmapRef CreateBitmapFromPng(byte[] data)
	{
		return CreateBitmapFromPng(data, data.Length);
	}

	public override BitmapRef CreateBitmapFromPng(byte[] data, int dataLength)
	{
		return new BitmapExternal(data, dataLength, Logger);
	}

	public override BitmapRef CreateBitmapFromPixels(int[] pixels, int width, int height)
	{
		nint scan = GCHandle.Alloc(pixels, GCHandleType.Pinned).AddrOfPinnedObject();
		return new BitmapExternal(SKBitmap.FromImage(SKImage.FromPixels(new SKImageInfo(width, height), scan)));
	}

	public override IAviWriter CreateAviWriter(float framerate, string codec)
	{
		return base.XPlatInterface.GetAviWriter(ClientSettings.RecordingBufferSize, framerate, codec);
	}

	public override AvailableCodec[] GetAvailableCodecs()
	{
		return base.XPlatInterface.AvailableCodecs();
	}

	public override string GetGameVersion()
	{
		return "1.22.6";
	}

	public void SetServerExitInterface(GameExitState exit)
	{
		gameexit = exit;
	}

	public override void ThreadSpinWait(int iterations)
	{
		Thread.SpinWait(iterations);
	}

	public override void LoadAssets()
	{
		if (assetManager == null)
		{
			assetManager = new AssetManager(GamePaths.AssetsPath, EnumAppSide.Client);
		}
		logger.Notification("Start discovering assets");
		int count = assetManager.InitAndLoadBaseAssets(logger, "textures");
		logger.Notification("Found {0} base assets in total", count);
	}

	public void Start()
	{
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Invalid comparison between Unknown and I4
		window.FocusedChanged += window_FocusChanged;
		window.KeyDown += game_KeyDown;
		window.KeyUp += game_KeyUp;
		window.TextInput += game_KeyPress;
		window.MouseDown += Mouse_ButtonDown;
		window.MouseUp += Mouse_ButtonUp;
		window.MouseMove += Mouse_Move;
		window.MouseWheel += Mouse_WheelChanged;
		window.RenderFrame += window_RenderFrame;
		window.Closing += window_Closing;
		window.Resize += OnWindowResize;
		window.Title = "Vintage Story";
		window.FileDrop += Window_FileDrop;
		frameBuffers = SetupDefaultFrameBuffers();
		minimalGuiShaderProgram = new ShaderProgramMinimalGui();
		minimalGuiShaderProgram.Compile();
		windowsize.Width = window.ClientSize.X;
		windowsize.Height = window.ClientSize.Y;
		GL.LineWidth(1.5f);
		ErrorCode code = GL.GetError();
		SupportsThickLines = (int)code != 1281;
		cpuCoreCount = Environment.ProcessorCount;
	}

	public override void RebuildFrameBuffers()
	{
		if (!ShaderRegistry.SupressShaderAndBufferReloads)
		{
			List<FrameBufferRef> oldFrameBuffers = frameBuffers;
			List<FrameBufferRef> newFrameBuffers = SetupDefaultFrameBuffers();
			frameBuffers = newFrameBuffers;
			DisposeFrameBuffers(oldFrameBuffers);
		}
	}

	private void Window_FileDrop(FileDropEventArgs e)
	{
		fileDropEventHandler?.Invoke(e.FileNames[0]);
	}

	private void OnWindowResize(ResizeEventArgs e)
	{
		doResize = Environment.TickCount + 40;
	}

	public override void WindowFocus()
	{
		if (window.WindowState == WindowState.Minimized)
		{
			window.WindowState = WindowState.Normal;
		}
		window.Focus();
	}

	private void Window_Resize()
	{
		doResize = 0;
		if (window.WindowState != WindowState.Minimized)
		{
			Vector2i windowSize = window.ClientSize;
			if (window.ClientSize.X < 600)
			{
				windowSize.X = 600;
			}
			if (window.ClientSize.Y < 400)
			{
				windowSize.Y = 400;
			}
			if (window.ClientSize.Y < 400 || window.ClientSize.X < 600)
			{
				window.ClientSize = windowSize;
			}
		}
		if (window.ClientSize.X == 0 || window.ClientSize.Y == 0)
		{
			logger.Notification("Window was resized to {0} {1}? Window probably got minimized. Will not rebuild frame buffers", window.ClientSize.X, window.ClientSize.Y);
		}
		else if (window.ClientSize.X != windowsize.Width || window.ClientSize.Y != windowsize.Height)
		{
			logger.Notification("Window was resized to {0} {1}, rebuilding framebuffers...", window.ClientSize.X, window.ClientSize.Y);
			RebuildFrameBuffers();
			windowsize.Width = window.ClientSize.X;
			windowsize.Height = window.ClientSize.Y;
			if (window.WindowState == WindowState.Normal)
			{
				ClientSettings.ScreenWidth = window.Size.X;
				ClientSettings.ScreenHeight = window.Size.Y;
			}
			int windowState = window.WindowState switch
			{
				WindowState.Maximized => 2, 
				WindowState.Fullscreen => (ClientSettings.GameWindowMode != 3) ? 1 : 3, 
				_ => 0, 
			};
			if (ClientSettings.GameWindowMode != windowState)
			{
				ClientSettings.GameWindowMode = windowState;
			}
			TriggerWindowResized(window.ClientSize.X, window.ClientSize.Y);
		}
	}

	private void window_Closing(CancelEventArgs e)
	{
		try
		{
			gameexit.Mode = EnumExitMode.HardExit;
			if (windowClosedHandler())
			{
				e.Cancel = true;
				gameexit.Mode = EnumExitMode.None;
			}
		}
		catch (Exception)
		{
		}
	}

	public override void SetVSync(bool enabled)
	{
		window.VSync = (enabled ? VSyncMode.On : VSyncMode.Off);
	}

	public unsafe override void SetDirectMouseMode(bool enabled)
	{
		GLFW.SetInputMode(window.WindowPtr, RawMouseMotionAttribute.RawMouseMotion, enabled);
	}

	public override string SaveScreenshot(string path = null, string filename = null, bool withAlpha = false, bool flip = false, string metaDataStr = null)
	{
		screenshot.d_GameWindow = window;
		FrameBufferRef currentFrameBuffer = CurrentFrameBuffer;
		Size2i size = ((currentFrameBuffer == null) ? new Size2i(window.ClientSize.X, window.ClientSize.Y) : new Size2i(currentFrameBuffer.Width, currentFrameBuffer.Height));
		return screenshot.SaveScreenshot(this, size, path, filename, withAlpha, flip, metaDataStr);
	}

	public override BitmapRef GrabScreenshot(bool withAlpha = false, bool scale = false)
	{
		screenshot.d_GameWindow = window;
		FrameBufferRef currentFrameBuffer = CurrentFrameBuffer;
		Size2i size = ((currentFrameBuffer == null) ? new Size2i(window.ClientSize.X, window.ClientSize.Y) : new Size2i(currentFrameBuffer.Width, currentFrameBuffer.Height));
		return new BitmapExternal(screenshot.GrabScreenshot(size, scale, flip: false, withAlpha));
	}

	public override BitmapRef GrabScreenshot(int width, int height, bool scaleScreenshot, bool flip, bool withAlpha = false)
	{
		screenshot.d_GameWindow = window;
		return new BitmapExternal(screenshot.GrabScreenshot(new Size2i(width, height), scaleScreenshot, flip, withAlpha));
	}

	public override void WindowExit(string reason, EnumExitMode mode)
	{
		logger.Notification("Exiting game now. Server running=" + serverRunning + ". Exit reason: {0}", reason);
		base.IsShuttingDown = true;
		if (gameexit != null)
		{
			gameexit.Mode = mode;
		}
		try
		{
			UriHandler.Instance.Dispose();
			window?.Close();
		}
		catch (Exception)
		{
			Environment.Exit(0);
		}
	}

	public override void SetTitle(string applicationname)
	{
		window.Title = applicationname;
	}

	public override WindowState GetWindowState()
	{
		return window.WindowState;
	}

	public override void SetWindowState(WindowState value)
	{
		window.WindowState = value;
		if (window.Location.Y < 0)
		{
			window.Location = new Vector2i(window.Location.X, 0);
		}
	}

	public unsafe override void SetWindowAttribute(WindowAttribute attribute, bool value)
	{
		GLFW.SetWindowAttrib(window.WindowPtr, attribute, value);
	}

	public override void StartSinglePlayerServer(StartServerArgs serverargs)
	{
		ServerExitState = new GameExitState();
		OnStartSinglePlayerServer(serverargs);
	}

	public override void ExitSinglePlayerServer(EnumExitMode exitMode)
	{
		ServerExitState.Mode = exitMode;
	}

	public override bool IsLoadedSinglePlayerServer()
	{
		return singlePlayerServerLoaded;
	}

	public override DummyNetwork[] GetSinglePlayerServerNetwork()
	{
		return singlePlayerServerDummyNetwork;
	}

	public override void SetFileDropHandler(Action<string> handler)
	{
		fileDropEventHandler = handler;
	}

	public override void RegisterOnFocusChange(OnFocusChanged handler)
	{
		focusChangedDelegates.Add(handler);
	}

	private void window_FocusChanged(FocusedChangedEventArgs e)
	{
		foreach (OnFocusChanged focusChangedDelegate in focusChangedDelegates)
		{
			focusChangedDelegate(window.IsFocused);
		}
	}

	public override void SetWindowClosedHandler(ActionBoolReturn handler)
	{
		windowClosedHandler = handler;
	}

	public override void SetFrameHandler(NewFrameHandler handler)
	{
		frameHandler = handler;
	}

	public override void RegisterKeyboardEvent(KeyEventHandler handler)
	{
		keyEventHandlers.Add(handler);
	}

	public override void RegisterMouseEvent(MouseEventHandler handler)
	{
		mouseEventHandlers.Add(handler);
	}

	public override void AddOnCrash(OnCrashHandler handler)
	{
		crashreporter.OnCrash = OnCrash;
		onCrashHandler = handler;
	}

	public override void ClearOnCrash()
	{
		onCrashHandler = null;
		crashreporter.OnCrash = null;
	}

	private void OnCrash()
	{
		if (onCrashHandler != null)
		{
			onCrashHandler.OnCrash();
		}
	}

	public override void RenderMesh(MeshRef modelRef)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		RuntimeStats.drawCallsCount++;
		if (debugDrawCalls)
		{
			drawCallStacks.Add(Environment.StackTrace);
		}
		VAO vao = (VAO)modelRef;
		if (vao.VaoId == 0 || vao.Disposed)
		{
			if (vao.VaoId == 0)
			{
				throw new ArgumentException("Fatal: Trying to render an uninitialized mesh");
			}
			throw new ArgumentException("Fatal: Trying to render a disposed mesh");
		}
		GL.BindVertexArray(vao.VaoId);
		GL.BindBuffer((BufferTarget)34963, vao.vboIdIndex);
		GL.DrawElements(vao.drawMode, vao.IndicesCount, (DrawElementsType)5125, 0);
		GL.BindBuffer((BufferTarget)34963, 0);
		GL.BindVertexArray(0);
	}

	public void RenderFullscreenTriangle(MeshRef modelRef)
	{
		RuntimeStats.drawCallsCount++;
		GL.BindVertexArray(((VAO)modelRef).VaoId);
		GL.DrawArrays((PrimitiveType)4, 0, 3);
		GL.BindVertexArray(0);
	}

	public override void RenderMesh(MeshRef modelRef, int[] indices, int[] indicesSizes, int groupCount)
	{
		RenderMesh(modelRef, indices, indicesSizes, groupCount, useSSBOs: false);
	}

	public override void RenderMesh(MeshRef modelRef, int[] indices, int[] indicesSizes, int groupCount, bool useSSBOs)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		RuntimeStats.drawCallsCount++;
		VAO vao = (VAO)modelRef;
		GL.BindVertexArray(vao.VaoId);
		if (useSSBOs)
		{
			GL.BindBuffer((BufferTarget)34963, ClientPlatformAbstract.singleIndexBufferId);
			GL.BindBufferBase((BufferRangeTarget)37074, 3, vao.xyzVboId);
			GL.MultiDrawElements<int>(vao.drawMode, indicesSizes, (DrawElementsType)5125, indices, groupCount);
			GL.BindBufferBase((BufferRangeTarget)37074, 3, 0);
		}
		else
		{
			GL.BindBuffer((BufferTarget)34963, vao.vboIdIndex);
			GL.MultiDrawElements<int>(vao.drawMode, indicesSizes, (DrawElementsType)5125, indices, groupCount);
		}
		GL.BindBuffer((BufferTarget)34963, 0);
		GL.BindVertexArray(0);
	}

	public override void RenderMeshInstanced(MeshRef modelRef, int quantity = 1)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		RuntimeStats.drawCallsCount++;
		VAO vao = (VAO)modelRef;
		GL.BindVertexArray(vao.VaoId);
		GL.BindBuffer((BufferTarget)34963, vao.vboIdIndex);
		GL.DrawElementsInstanced(vao.drawMode, vao.IndicesCount, (DrawElementsType)5125, (IntPtr)IntPtr.Zero, quantity);
		GL.BindBuffer((BufferTarget)34963, 0);
		GL.BindVertexArray(0);
	}

	public override void SetGamePausedState(bool paused)
	{
		gamepause = paused;
	}

	public override void ResetGamePauseAndUptimeState()
	{
		uptimeStopWatch.Start();
	}

	public override void ToggleOffscreenBuffer(bool enable)
	{
		OffscreenBuffer = enable;
	}

	public override void DisposeFrameBuffer(FrameBufferRef frameBuffer, bool disposeTextures = true)
	{
		if (frameBuffer == null)
		{
			return;
		}
		if (disposeTextures)
		{
			for (int i = 0; i < frameBuffer.ColorTextureIds.Length; i++)
			{
				GLDeleteTexture(frameBuffer.ColorTextureIds[i]);
			}
			if (frameBuffer.DepthTextureId > 0)
			{
				GLDeleteTexture(frameBuffer.DepthTextureId);
			}
		}
		GL.DeleteFramebuffer(frameBuffer.FboId);
	}

	public override FrameBufferRef CreateFramebuffer(FramebufferAttrs fbAttrs)
	{
		FrameBufferRef framebuffer = (CurrentFrameBufferKeepVw = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fbAttrs.Width,
			Height = fbAttrs.Height
		});
		List<DrawBuffersEnum> drawBuffers = new List<DrawBuffersEnum>();
		List<int> colorTextureIds = new List<int>();
		FramebufferAttrsAttachment[] attachments = fbAttrs.Attachments;
		foreach (FramebufferAttrsAttachment fbAtt in attachments)
		{
			RawTexture tex = fbAtt.Texture;
			int textureId = ((tex.TextureId == 0) ? GL.GenTexture() : tex.TextureId);
			if (tex.TextureId == 0)
			{
				GL.BindTexture((TextureTarget)3553, textureId);
				GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)tex.PixelInternalFormat, tex.Width, tex.Height, 0, (PixelFormat)tex.PixelFormat, (PixelType)5126, (IntPtr)IntPtr.Zero);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, (int)tex.MinFilter);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, (int)tex.MagFilter);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, (int)tex.WrapS);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, (int)tex.WrapT);
			}
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)fbAtt.AttachmentType, (TextureTarget)3553, textureId, 0);
			if (fbAtt.AttachmentType == EnumFramebufferAttachment.DepthAttachment)
			{
				GL.DepthFunc((DepthFunction)513);
				framebuffer.DepthTextureId = (tex.TextureId = textureId);
			}
			else
			{
				drawBuffers.Add((DrawBuffersEnum)fbAtt.AttachmentType);
				colorTextureIds.Add(tex.TextureId = textureId);
			}
		}
		framebuffer.ColorTextureIds = colorTextureIds.ToArray();
		GL.DrawBuffers(drawBuffers.Count, drawBuffers.ToArray());
		CheckFboStatus((FramebufferTarget)36160, fbAttrs.Name);
		CurrentFrameBufferKeepVw = null;
		GL.BindTexture((TextureTarget)3553, 0);
		return framebuffer;
	}

	public List<FrameBufferRef> SetupDefaultFrameBuffers()
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		SetupSSAO = ClientSettings.SSAOQuality > 0;
		if (ClientSettings.IsNewSettingsFile && window.ClientSize.X > 1920)
		{
			ClientSettings.SSAA = 0.5f;
		}
		List<FrameBufferRef> framebuffers = new List<FrameBufferRef>(31);
		for (int i = 0; i <= 24; i++)
		{
			framebuffers.Add(null);
		}
		ShadowMapQuality = ClientSettings.ShadowMapQuality;
		ssaaLevel = ClientSettings.SSAA;
		int fullWidth = (int)((float)window.ClientSize.X * ssaaLevel);
		int fullHeight = (int)((float)window.ClientSize.Y * ssaaLevel);
		if (fullWidth == 0 || fullHeight == 0)
		{
			return framebuffers;
		}
		PixelFormat rgbaFormat = (PixelFormat)6408;
		CheckGlError("sdfb-begin");
		FrameBufferRef frameBufferRef = (framebuffers[0] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth,
			Height = fullHeight
		});
		FrameBufferRef frameBuffer = frameBufferRef;
		frameBuffer.DepthTextureId = GL.GenTexture();
		if (frameBuffer.FboId == 0)
		{
			base.XPlatInterface.ShowMessageBox("Fatal error", "Unable to generate a new framebuffer. This shouldn't happen, ever. Maybe a restart resolves the problem?");
		}
		CurrentFrameBufferKeepVw = frameBuffer;
		GL.BindTexture((TextureTarget)3553, frameBuffer.DepthTextureId);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)33191, fullWidth, fullHeight, 0, (PixelFormat)6402, (PixelType)5126, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9728);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9728);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33071);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33071);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36096, (TextureTarget)3553, frameBuffer.DepthTextureId, 0);
		GL.DepthFunc((DepthFunction)513);
		frameBuffer.ColorTextureIds = ArrayUtil.CreateFilled(SetupSSAO ? 4 : 2, (int n) => GL.GenTexture());
		GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[0]);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)32856, fullWidth, fullHeight, 0, rgbaFormat, (PixelType)5123, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, (ssaaLevel <= 1f) ? 9728 : 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, (ssaaLevel <= 1f) ? 9728 : 9729);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, frameBuffer.ColorTextureIds[0], 0);
		GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[1]);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)32856, fullWidth, fullHeight, 0, rgbaFormat, (PixelType)5121, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, (ssaaLevel <= 1f) ? 9728 : 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, (ssaaLevel <= 1f) ? 9728 : 9729);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36065, (TextureTarget)3553, frameBuffer.ColorTextureIds[1], 0);
		if (SetupSSAO)
		{
			GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[2]);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)34842, fullWidth, fullHeight, 0, rgbaFormat, (PixelType)5126, (IntPtr)IntPtr.Zero);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)4100, new float[4] { 1f, 1f, 1f, 1f });
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33069);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33069);
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36066, (TextureTarget)3553, frameBuffer.ColorTextureIds[2], 0);
			GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[3]);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)34842, fullWidth, fullHeight, 0, (PixelFormat)6408, (PixelType)5126, (IntPtr)IntPtr.Zero);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)4100, new float[4] { 1f, 1f, 1f, 1f });
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33069);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33069);
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36067, (TextureTarget)3553, frameBuffer.ColorTextureIds[3], 0);
			DrawBuffersEnum[] array = new DrawBuffersEnum[4];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			DrawBuffersEnum[] bufs = (DrawBuffersEnum[])(object)array;
			GL.DrawBuffers(4, bufs);
		}
		else
		{
			DrawBuffersEnum[] bufs2 = (DrawBuffersEnum[])(object)new DrawBuffersEnum[2]
			{
				(DrawBuffersEnum)36064,
				(DrawBuffersEnum)36065
			};
			GL.DrawBuffers(2, bufs2);
		}
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.Primary);
		frameBufferRef = (framebuffers[1] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth,
			Height = fullHeight
		});
		frameBuffer = frameBufferRef;
		frameBuffer.ColorTextureIds = new int[3]
		{
			GL.GenTexture(),
			GL.GenTexture(),
			GL.GenTexture()
		};
		CurrentFrameBufferKeepVw = frameBuffer;
		GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[0]);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)34842, fullWidth, fullHeight, 0, rgbaFormat, (PixelType)5123, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, frameBuffer.ColorTextureIds[0], 0);
		GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[1]);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)33325, fullWidth, fullHeight, 0, (PixelFormat)6403, (PixelType)5123, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36065, (TextureTarget)3553, frameBuffer.ColorTextureIds[1], 0);
		GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[2]);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)32856, fullWidth, fullHeight, 0, rgbaFormat, (PixelType)5121, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36066, (TextureTarget)3553, frameBuffer.ColorTextureIds[2], 0);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36096, (TextureTarget)3553, framebuffers[0].DepthTextureId, 0);
		DrawBuffersEnum[] array2 = new DrawBuffersEnum[3];
		RuntimeHelpers.InitializeArray(array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		DrawBuffersEnum[] bufs3 = (DrawBuffersEnum[])(object)array2;
		GL.DrawBuffers(3, bufs3);
		ClearFrameBuffer(EnumFrameBuffer.Transparent);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.Transparent);
		if (SetupSSAO)
		{
			_ = ClientSettings.SSAOQuality;
			float ssaoSizeFq = 0.5f;
			FrameBufferRef obj = new FrameBufferRef
			{
				FboId = GL.GenFramebuffer(),
				Width = (int)((float)fullWidth * ssaoSizeFq),
				Height = (int)((float)fullHeight * ssaoSizeFq)
			};
			frameBufferRef = obj;
			framebuffers[13] = obj;
			frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
			frameBuffer.ColorTextureIds = new int[2]
			{
				GL.GenTexture(),
				GL.GenTexture()
			};
			GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[0]);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)6407, frameBuffer.Width, frameBuffer.Height, 0, (PixelFormat)6407, (PixelType)5126, (IntPtr)IntPtr.Zero);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9728);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9728);
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, frameBuffer.ColorTextureIds[0], 0);
			Random rand = new Random(5);
			_ = frameBuffer.ColorTextureIds[1];
			int size = 16;
			float[] vecs = new float[size * size * 3];
			Vec3f tmpvec = new Vec3f();
			for (int i2 = 0; i2 < size * size; i2++)
			{
				tmpvec.Set((float)rand.NextDouble() * 2f - 1f, (float)rand.NextDouble() * 2f - 1f, 0f).Normalize();
				vecs[i2 * 3] = tmpvec.X;
				vecs[i2 * 3 + 1] = tmpvec.Y;
				vecs[i2 * 3 + 2] = tmpvec.Z;
			}
			GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[1]);
			GL.TexImage2D<float>((TextureTarget)3553, 0, (PixelInternalFormat)34836, size, size, 0, (PixelFormat)6407, (PixelType)5126, vecs);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9728);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9728);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 10497);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 10497);
			for (int i3 = 0; i3 < 64; i3++)
			{
				Vec3f vec = new Vec3f((float)rand.NextDouble() * 2f - 1f, (float)rand.NextDouble() * 2f - 1f, (float)rand.NextDouble());
				vec.Normalize();
				vec *= (float)rand.NextDouble();
				float scale = (float)i3 / 64f;
				scale = GameMath.Lerp(0.1f, 1f, scale * scale);
				vec *= scale;
				ssaoKernel[i3 * 3] = vec.X;
				ssaoKernel[i3 * 3 + 1] = vec.Y;
				ssaoKernel[i3 * 3 + 2] = vec.Z;
			}
			GL.DrawBuffer((DrawBufferMode)36064);
			CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.SSAO);
			EnumFrameBuffer[] array3 = new EnumFrameBuffer[2]
			{
				EnumFrameBuffer.SSAOBlurVertical,
				EnumFrameBuffer.SSAOBlurHorizontal
			};
			foreach (EnumFrameBuffer val in array3)
			{
				frameBufferRef = (framebuffers[(int)val] = new FrameBufferRef
				{
					FboId = GL.GenFramebuffer(),
					Width = (int)((float)fullWidth * ssaoSizeFq),
					Height = (int)((float)fullHeight * ssaoSizeFq)
				});
				frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
				frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
				setupAttachment(frameBuffer, frameBuffer.Width, frameBuffer.Height, 0, rgbaFormat, (PixelInternalFormat)32856);
				GL.DrawBuffer((DrawBufferMode)36064);
				CheckFboStatus((FramebufferTarget)36160, val);
			}
		}
		frameBufferRef = (framebuffers[2] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth / 2,
			Height = fullHeight / 2
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth / 2, fullHeight / 2, 0, rgbaFormat, (PixelInternalFormat)32856);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.BlurHorizontalMedRes);
		frameBufferRef = (framebuffers[3] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth / 2,
			Height = fullHeight / 2
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth / 2, fullHeight / 2, 0, rgbaFormat, (PixelInternalFormat)32856);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.BlurVerticalMedRes);
		frameBufferRef = (framebuffers[9] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth / 4,
			Height = fullHeight / 4
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth / 4, fullHeight / 4, 0, rgbaFormat, (PixelInternalFormat)32856);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.BlurHorizontalLowRes);
		frameBufferRef = (framebuffers[8] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer()
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth / 4, fullHeight / 4, 0, rgbaFormat, (PixelInternalFormat)32856);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.BlurVerticalLowRes);
		frameBufferRef = (framebuffers[4] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth,
			Height = fullHeight
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth, fullHeight, 0, rgbaFormat, (PixelInternalFormat)34842);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.FindBright);
		frameBufferRef = (framebuffers[7] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth / 2,
			Height = fullHeight / 2
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth / 2, fullHeight / 2, 0, rgbaFormat, (PixelInternalFormat)34842);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.GodRays);
		frameBufferRef = (framebuffers[10] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth,
			Height = fullHeight
		});
		frameBuffer = (CurrentFrameBufferKeepVw = frameBufferRef);
		frameBuffer.ColorTextureIds = new int[1] { GL.GenTexture() };
		setupAttachment(frameBuffer, fullWidth, fullHeight, 0, rgbaFormat, (PixelInternalFormat)34842);
		GL.DrawBuffer((DrawBufferMode)36064);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.Luma);
		frameBufferRef = (framebuffers[5] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = fullWidth / 4,
			Height = fullHeight / 4
		});
		frameBuffer = frameBufferRef;
		frameBuffer.ColorTextureIds = Array.Empty<int>();
		CheckGlError("sdfb-lide");
		CurrentFrameBufferKeepVw = frameBuffer;
		frameBuffer.DepthTextureId = GL.GenTexture();
		GL.BindTexture((TextureTarget)3553, frameBuffer.DepthTextureId);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)33191, frameBuffer.Width, frameBuffer.Height, 0, (PixelFormat)6402, (PixelType)5126, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)34892, 0);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33071);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33071);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36096, (TextureTarget)3553, frameBuffer.DepthTextureId, 0);
		GL.DepthFunc((DepthFunction)513);
		GL.DrawBuffer((DrawBufferMode)0);
		GL.ReadBuffer((ReadBufferMode)0);
		CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.LiquidDepth);
		int shadowMapWidth = Math.Max(4, ShadowMapQuality + 2) * 1024;
		int shadowMapHeight = Math.Max(4, ShadowMapQuality + 2) * 1024;
		frameBufferRef = (framebuffers[11] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = shadowMapWidth,
			Height = shadowMapHeight
		});
		frameBuffer = frameBufferRef;
		frameBuffer.ColorTextureIds = Array.Empty<int>();
		CheckGlError("sdfb-fsm");
		if (ShadowMapQuality > 0)
		{
			CurrentFrameBufferKeepVw = frameBuffer;
			frameBuffer.DepthTextureId = GL.GenTexture();
			GL.BindTexture((TextureTarget)3553, frameBuffer.DepthTextureId);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)33191, shadowMapWidth, shadowMapHeight, 0, (PixelFormat)6402, (PixelType)5126, (IntPtr)IntPtr.Zero);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)34892, 34894);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)34893, 515);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)4100, new float[4] { 1f, 1f, 1f, 1f });
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33069);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33069);
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36096, (TextureTarget)3553, frameBuffer.DepthTextureId, 0);
			GL.DepthFunc((DepthFunction)513);
			GL.DrawBuffer((DrawBufferMode)0);
			GL.ReadBuffer((ReadBufferMode)0);
			CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.ShadowmapFar);
		}
		frameBufferRef = (framebuffers[12] = new FrameBufferRef
		{
			FboId = GL.GenFramebuffer(),
			Width = shadowMapWidth,
			Height = shadowMapHeight
		});
		frameBuffer = frameBufferRef;
		frameBuffer.ColorTextureIds = Array.Empty<int>();
		CheckGlError("sdfb-nsm-before");
		if (ShadowMapQuality > 1)
		{
			CurrentFrameBufferKeepVw = frameBuffer;
			frameBuffer.DepthTextureId = GL.GenTexture();
			GL.BindTexture((TextureTarget)3553, frameBuffer.DepthTextureId);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)33191, shadowMapWidth, shadowMapHeight, 0, (PixelFormat)6402, (PixelType)5126, (IntPtr)IntPtr.Zero);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)34892, 34894);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)34893, 515);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)4100, new float[4] { 1f, 1f, 1f, 1f });
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33069);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33069);
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36096, (TextureTarget)3553, frameBuffer.DepthTextureId, 0);
			GL.DepthFunc((DepthFunction)513);
			GL.DrawBuffer((DrawBufferMode)0);
			GL.ReadBuffer((ReadBufferMode)0);
			CheckFboStatus((FramebufferTarget)36160, EnumFrameBuffer.ShadowmapNear);
		}
		CheckGlError("sdfb-nsm-after");
		PixelPackBuffer = new GLBuffer[3];
		for (int i4 = 0; i4 < PixelPackBuffer.Length; i4++)
		{
			PixelPackBuffer[i4] = new GLBuffer
			{
				BufferId = GL.GenBuffer()
			};
			GL.BindBuffer((BufferTarget)35051, PixelPackBuffer[i4].BufferId);
			int size2 = 4 * sampleCount;
			GL.BufferData((BufferTarget)35051, size2, (IntPtr)IntPtr.Zero, (BufferUsageHint)35041);
		}
		GL.BindBuffer((BufferTarget)35051, 0);
		MeshData quad = QuadMeshUtil.GetCustomQuadModelData(-1f, -1f, 0f, 2f, 2f);
		quad.Normals = null;
		quad.Rgba = null;
		quad.Uv = null;
		if (screenQuad != null)
		{
			screenQuad.Dispose();
		}
		screenQuad = UploadMesh(quad);
		if (OffscreenBuffer)
		{
			CurrentFrameBufferKeepVw = framebuffers[0];
		}
		else
		{
			CurrentFrameBufferKeepVw = null;
			GL.DrawBuffer((DrawBufferMode)1029);
		}
		logger.Notification("(Re-)loaded frame buffers");
		return framebuffers;
	}

	private void setupAttachment(FrameBufferRef frameBuffer, int width, int height, int index, PixelFormat rgbaFormat, PixelInternalFormat dataFormat)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		GL.BindTexture((TextureTarget)3553, frameBuffer.ColorTextureIds[index]);
		GL.TexImage2D((TextureTarget)3553, 0, dataFormat, width, height, 0, rgbaFormat, (PixelType)(((int)dataFormat == 34842) ? 5123 : 5121), (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9729);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, frameBuffer.ColorTextureIds[index], 0);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, Convert.ToInt32((object)(TextureWrapMode)33071));
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, Convert.ToInt32((object)(TextureWrapMode)33071));
	}

	public void DisposeFrameBuffers(List<FrameBufferRef> buffers)
	{
		for (int i = 0; i < buffers.Count; i++)
		{
			if (buffers[i] != null)
			{
				GL.DeleteFramebuffer(buffers[i].FboId);
				GL.DeleteTexture(buffers[i].DepthTextureId);
				for (int j = 0; j < buffers[i].ColorTextureIds.Length; j++)
				{
					GL.DeleteTexture(buffers[i].ColorTextureIds[j]);
				}
				buffers[i].Disposed = true;
			}
		}
	}

	public override void ClearFrameBuffer(FrameBufferRef framebuffer, bool clearDepth = true)
	{
		ClearFrameBuffer(framebuffer, clearColor, clearDepth);
	}

	public override void ClearFrameBuffer(FrameBufferRef framebuffer, float[] clearColor, bool clearDepthBuffer = true, bool clearColorBuffers = true)
	{
		CurrentFrameBufferKeepVw = framebuffer;
		if (clearColorBuffers)
		{
			for (int i = 0; i < framebuffer.ColorTextureIds.Length; i++)
			{
				GL.ClearBuffer((ClearBuffer)6144, i, clearColor);
			}
		}
		if (clearDepthBuffer)
		{
			float clearval = 1f;
			GL.ClearBuffer((ClearBuffer)6145, 0, ref clearval);
		}
	}

	public override void LoadFrameBuffer(FrameBufferRef frameBuffer, int textureId)
	{
		CurrentFrameBuffer = frameBuffer;
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, textureId, 0);
	}

	public override void UnloadFrameBuffer(FrameBufferRef frameBuffer)
	{
		LoadFrameBuffer(EnumFrameBuffer.Primary);
	}

	public override void ClearFrameBuffer(EnumFrameBuffer framebuffer)
	{
		switch (framebuffer)
		{
		case EnumFrameBuffer.Default:
			CurrentFrameBufferKeepVw = null;
			GL.DrawBuffer((DrawBufferMode)1029);
			GL.Clear((ClearBufferMask)16640);
			CurrentFrameBufferKeepVw = frameBuffers[0];
			break;
		case EnumFrameBuffer.Primary:
		{
			GL.ClearBuffer((ClearBuffer)6144, 0, new float[4] { 0f, 0f, 0f, 1f });
			GL.ClearBuffer((ClearBuffer)6144, 1, new float[4] { 0f, 0f, 0f, 1f });
			if (RenderSSAO)
			{
				GL.ClearBuffer((ClearBuffer)6144, 2, new float[4] { 0f, 0f, 0f, 1f });
				GL.ClearBuffer((ClearBuffer)6144, 3, new float[4] { 0f, 0f, 0f, 1f });
			}
			float clearval2 = 1f;
			GL.ClearBuffer((ClearBuffer)6145, 0, ref clearval2);
			break;
		}
		case EnumFrameBuffer.LiquidDepth:
		case EnumFrameBuffer.ShadowmapFar:
		case EnumFrameBuffer.ShadowmapNear:
		{
			FrameBufferRef fb = FrameBuffers[(int)framebuffer];
			float clearval = 1f;
			GL.Viewport(0, 0, fb.Width, fb.Height);
			GL.ClearBuffer((ClearBuffer)6145, 0, ref clearval);
			break;
		}
		case EnumFrameBuffer.Transparent:
			GL.ClearBuffer((ClearBuffer)6144, 0, new float[4]);
			GL.ClearBuffer((ClearBuffer)6144, 1, new float[4] { 1f, 0f, 0f, 0f });
			GL.ClearBuffer((ClearBuffer)6144, 2, new float[4]);
			break;
		}
	}

	public override void LoadFrameBuffer(EnumFrameBuffer framebuffer)
	{
		switch (framebuffer)
		{
		case EnumFrameBuffer.Transparent:
		{
			CurrentFrameBufferKeepVw = frameBuffers[1];
			ScreenManager.FrameProfiler.Mark("rendTransp-fbbound");
			GlDisableCullFace();
			GlDepthMask(flag: false);
			GlEnableDepthTest();
			ScreenManager.FrameProfiler.Mark("rendTransp-dbset");
			DrawBuffersEnum[] array = new DrawBuffersEnum[3];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			DrawBuffersEnum[] bufs = (DrawBuffersEnum[])(object)array;
			GL.DrawBuffers(3, bufs);
			GL.Enable((EnableCap)3042);
			GL.BlendEquation(0, (BlendEquationMode)32774);
			GL.BlendFunc(0, (BlendingFactorSrc)1, (BlendingFactorDest)1);
			GL.BlendEquation(1, (BlendEquationMode)32774);
			GL.BlendFunc(1, (BlendingFactorSrc)0, (BlendingFactorDest)769);
			GL.BlendEquation(2, (BlendEquationMode)32774);
			GL.BlendFunc(2, (BlendingFactorSrc)770, (BlendingFactorDest)771);
			break;
		}
		case EnumFrameBuffer.Default:
			CurrentFrameBufferKeepVw = null;
			GL.Viewport(0, 0, window.ClientSize.X, window.ClientSize.Y);
			GL.DrawBuffer((DrawBufferMode)1029);
			break;
		case EnumFrameBuffer.BlurHorizontalMedRes:
		case EnumFrameBuffer.BlurVerticalMedRes:
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X / 2f), (int)(ssaaLevel * (float)window.ClientSize.Y / 2f));
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.SSAOBlurVertical:
		case EnumFrameBuffer.SSAOBlurHorizontal:
		case EnumFrameBuffer.SSAOBlurVerticalHalfRes:
		case EnumFrameBuffer.SSAOBlurHorizontalHalfRes:
		{
			FrameBufferRef fb2 = frameBuffers[(int)framebuffer];
			GL.Viewport(0, 0, fb2.Width, fb2.Height);
			CurrentFrameBufferKeepVw = fb2;
			break;
		}
		case EnumFrameBuffer.BlurVerticalLowRes:
		case EnumFrameBuffer.BlurHorizontalLowRes:
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X / 4f), (int)(ssaaLevel * (float)window.ClientSize.Y / 4f));
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.FindBright:
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.GodRays:
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X / 2f), (int)(ssaaLevel * (float)window.ClientSize.Y / 2f));
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.Luma:
			GL.Disable((EnableCap)3042);
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.SSAO:
		{
			FrameBufferRef fb = frameBuffers[(int)framebuffer];
			GL.Viewport(0, 0, fb.Width, fb.Height);
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		}
		case EnumFrameBuffer.ShadowmapFar:
		case EnumFrameBuffer.ShadowmapNear:
			GlDepthMask(flag: true);
			GlEnableDepthTest();
			GlToggleBlend(on: true);
			GlEnableCullFace();
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.LiquidDepth:
			GlDepthMask(flag: true);
			GlEnableDepthTest();
			GlToggleBlend(on: true);
			GlEnableCullFace();
			CurrentFrameBufferKeepVw = frameBuffers[(int)framebuffer];
			break;
		case EnumFrameBuffer.Primary:
			if (OffscreenBuffer)
			{
				CurrentFrameBufferKeepVw = frameBuffers[0];
				GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X), (int)(ssaaLevel * (float)window.ClientSize.Y));
			}
			else
			{
				CurrentFrameBufferKeepVw = null;
				GL.DrawBuffer((DrawBufferMode)1029);
			}
			break;
		case (EnumFrameBuffer)6:
			break;
		}
	}

	public override void UnloadFrameBuffer(EnumFrameBuffer framebuffer)
	{
		if (framebuffer == EnumFrameBuffer.Transparent)
		{
			GlDepthMask(flag: true);
		}
		GL.Viewport(0, 0, (int)((float)window.ClientSize.X * ssaaLevel), (int)((float)window.ClientSize.Y * ssaaLevel));
		if (OffscreenBuffer)
		{
			CurrentFrameBufferKeepVw = frameBuffers[0];
			return;
		}
		CurrentFrameBufferKeepVw = null;
		GL.DrawBuffer((DrawBufferMode)1029);
	}

	public override void MergeTransparentRenderPass()
	{
		if (OffscreenBuffer)
		{
			CurrentFrameBufferKeepVw = frameBuffers[0];
		}
		else
		{
			CurrentFrameBufferKeepVw = null;
			GL.DrawBuffer((DrawBufferMode)1029);
		}
		GL.Disable((EnableCap)2929);
		GL.Enable((EnableCap)3042);
		GL.BlendFunc((BlendingFactor)770, (BlendingFactor)771);
		ShaderProgramTransparentcompose transparentcompose = ShaderPrograms.Transparentcompose;
		transparentcompose.Use();
		transparentcompose.Revealage2D = frameBuffers[1].ColorTextureIds[1];
		transparentcompose.Accumulation2D = frameBuffers[1].ColorTextureIds[0];
		transparentcompose.InGlow2D = frameBuffers[1].ColorTextureIds[2];
		RenderFullscreenTriangle(screenQuad);
		transparentcompose.Stop();
	}

	public override void RenderPostprocessingEffects(float[] projectMatrix)
	{
		if (!OffscreenBuffer)
		{
			return;
		}
		int width = window.ClientSize.X;
		int height = window.ClientSize.Y;
		if (RenderBloom)
		{
			GlToggleBlend(on: false);
			LoadFrameBuffer(EnumFrameBuffer.FindBright);
			ShaderProgramFindbright findbright = ShaderPrograms.Findbright;
			findbright.Use();
			findbright.ColorTex2D = frameBuffers[0].ColorTextureIds[0];
			findbright.GlowTex2D = frameBuffers[0].ColorTextureIds[1];
			findbright.AmbientBloomLevel = ClientSettings.AmbientBloomLevel / 100f + ShaderUniforms.AmbientBloomLevelAdd[0] + ShaderUniforms.AmbientBloomLevelAdd[1] + ShaderUniforms.AmbientBloomLevelAdd[2] + ShaderUniforms.AmbientBloomLevelAdd[3];
			findbright.ExtraBloom = ShaderUniforms.ExtraBloom;
			RenderFullscreenTriangle(screenQuad);
			findbright.Stop();
			ShaderProgramBlur blur = ShaderPrograms.Blur;
			blur.Use();
			blur.Uniform("frameSize", (float)width * ssaaLevel, (float)height * ssaaLevel);
			LoadFrameBuffer(EnumFrameBuffer.BlurHorizontalMedRes);
			blur.IsVertical = 0;
			blur.InputTexture2D = frameBuffers[4].ColorTextureIds[0];
			RenderFullscreenTriangle(screenQuad);
			LoadFrameBuffer(EnumFrameBuffer.BlurVerticalMedRes);
			blur.IsVertical = 1;
			blur.InputTexture2D = frameBuffers[2].ColorTextureIds[0];
			RenderFullscreenTriangle(screenQuad);
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X / 4f), (int)(ssaaLevel * (float)window.ClientSize.Y / 4f));
			LoadFrameBuffer(EnumFrameBuffer.BlurHorizontalLowRes);
			blur.IsVertical = 0;
			blur.InputTexture2D = frameBuffers[3].ColorTextureIds[0];
			RenderFullscreenTriangle(screenQuad);
			LoadFrameBuffer(EnumFrameBuffer.BlurVerticalLowRes);
			blur.IsVertical = 1;
			blur.InputTexture2D = frameBuffers[9].ColorTextureIds[0];
			RenderFullscreenTriangle(screenQuad);
			blur.Stop();
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X), (int)(ssaaLevel * (float)window.ClientSize.Y));
			GlToggleBlend(on: true);
		}
		if (RenderGodRays)
		{
			LoadFrameBuffer(EnumFrameBuffer.GodRays);
			ShaderProgramGodrays godrays = ShaderPrograms.Godrays;
			godrays.Use();
			godrays.Uniform("invFrameSizeIn", 1f / ((float)width * ssaaLevel), 1f / ((float)height * ssaaLevel));
			godrays.SunPosScreenIn = ShaderUniforms.SunPositionScreen;
			godrays.SunPos3dIn = ShaderUniforms.LightPosition3D;
			godrays.PlayerViewVector = ShaderUniforms.PlayerViewVector;
			godrays.Dusk = ShaderUniforms.Dusk;
			godrays.IGlobalTimeIn = (float)EllapsedMs / 1000f;
			godrays.InputTexture2D = frameBuffers[0].ColorTextureIds[0];
			godrays.GlowParts2D = frameBuffers[0].ColorTextureIds[1];
			RenderFullscreenTriangle(screenQuad);
			godrays.Stop();
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X), (int)(ssaaLevel * (float)window.ClientSize.Y));
		}
		if (RenderSSAO && projectMatrix != null)
		{
			GlToggleBlend(on: false);
			LoadFrameBuffer(EnumFrameBuffer.SSAO);
			GL.ClearBuffer((ClearBuffer)6144, 0, new float[4] { 1f, 1f, 1f, 1f });
			ShaderProgramSsao ssao = ShaderPrograms.Ssao;
			ssao.Use();
			ssao.GNormal2D = frameBuffers[0].ColorTextureIds[2];
			ssao.GPosition2D = frameBuffers[0].ColorTextureIds[3];
			ssao.TexNoise2D = frameBuffers[13].ColorTextureIds[1];
			float ssaoSizeFq = ((ssaaLevel == 1f) ? 0.5f : 1f);
			ssao.Uniform("screenSize", ssaaLevel * (float)width * ssaoSizeFq, ssaaLevel * (float)height * ssaoSizeFq);
			ssao.Revealage2D = frameBuffers[1].ColorTextureIds[1];
			ssao.Projection = projectMatrix;
			ssao.SamplesArray(64, ssaoKernel);
			RenderFullscreenTriangle(screenQuad);
			ssao.Stop();
			ShaderProgramBilateralblur progblur = ShaderPrograms.Bilateralblur;
			progblur.Use();
			int q = ((ClientSettings.SSAOQuality == 1) ? 1 : 3);
			for (int i = 0; i < q; i++)
			{
				FrameBufferRef fb = frameBuffers[15];
				LoadFrameBuffer(EnumFrameBuffer.SSAOBlurHorizontal);
				progblur.Uniform("frameSize", fb.Width, fb.Height);
				progblur.IsVertical = 0;
				progblur.InputTexture2D = frameBuffers[(i == 0) ? 13 : 14].ColorTextureIds[0];
				progblur.DepthTexture2D = frameBuffers[0].DepthTextureId;
				RenderFullscreenTriangle(screenQuad);
				LoadFrameBuffer(EnumFrameBuffer.SSAOBlurVertical);
				progblur.IsVertical = 1;
				progblur.Uniform("frameSize", fb.Width, fb.Height);
				progblur.InputTexture2D = frameBuffers[15].ColorTextureIds[0];
				RenderFullscreenTriangle(screenQuad);
			}
			progblur.Stop();
			GlToggleBlend(on: true);
			GL.Viewport(0, 0, (int)(ssaaLevel * (float)window.ClientSize.X), (int)(ssaaLevel * (float)window.ClientSize.Y));
		}
		if (RenderFXAA)
		{
			LoadFrameBuffer(EnumFrameBuffer.Luma);
			ShaderProgramLuma luma = ShaderPrograms.Luma;
			luma.Use();
			luma.Scene2D = frameBuffers[0].ColorTextureIds[0];
			RenderFullscreenTriangle(screenQuad);
			luma.Stop();
		}
		else
		{
			LoadFrameBuffer(EnumFrameBuffer.Luma);
			ShaderProgramBlit blit = ShaderPrograms.Blit;
			blit.Use();
			blit.Scene2D = frameBuffers[0].ColorTextureIds[0];
			RenderFullscreenTriangle(screenQuad);
			blit.Stop();
		}
		GL.Enable((EnableCap)3042);
		LoadFrameBuffer(EnumFrameBuffer.Primary);
		ScreenManager.Platform.CheckGlError();
	}

	public override void RenderFinalComposition()
	{
		if (OffscreenBuffer)
		{
			int bloomPartsTexId = frameBuffers[8].ColorTextureIds[0];
			int godrayPartsTexId = frameBuffers[7].ColorTextureIds[0];
			int primarySceneTexId = frameBuffers[10].ColorTextureIds[0];
			if (RenderBloom)
			{
				_ = frameBuffers[8].ColorTextureIds[0];
			}
			DrawBuffersEnum[] bufs = (DrawBuffersEnum[])(object)new DrawBuffersEnum[1] { (DrawBuffersEnum)36064 };
			GL.DrawBuffers(1, bufs);
			GL.Disable((EnableCap)2929);
			GlToggleBlend(on: true);
			ShaderProgramFinal progf = ShaderPrograms.Final;
			progf.Use();
			progf.PrimaryScene2D = primarySceneTexId;
			progf.BloomParts2D = bloomPartsTexId;
			progf.GlowParts2D = frameBuffers[0].ColorTextureIds[1];
			progf.GodrayParts2D = godrayPartsTexId;
			progf.AmbientBloomLevel = ClientSettings.AmbientBloomLevel / 100f + ShaderUniforms.AmbientBloomLevelAdd[0] + ShaderUniforms.AmbientBloomLevelAdd[1] + ShaderUniforms.AmbientBloomLevelAdd[2] + ShaderUniforms.AmbientBloomLevelAdd[3];
			if (RenderSSAO)
			{
				progf.SsaoScene2D = frameBuffers[14].ColorTextureIds[0];
			}
			progf.Uniform("invFrameSizeIn", 1f / ((float)window.ClientSize.X * ssaaLevel), 1f / ((float)window.ClientSize.Y * ssaaLevel));
			progf.GammaLevel = ClientSettings.GammaLevel;
			progf.ExtraGamma = ClientSettings.ExtraGammaLevel;
			progf.ContrastLevel = ShaderUniforms.ExtraContrastLevel;
			progf.BrightnessLevel = ClientSettings.BrightnessLevel + Math.Max(0f, ShaderUniforms.DropShadowIntensity * 2f - 1.66f) / 3f;
			progf.SepiaLevel = ShaderUniforms.SepiaLevel + ShaderUniforms.ExtraSepia;
			progf.WindWaveCounter = ShaderUniforms.WindWaveCounter;
			progf.GlitchEffectStrength = ShaderUniforms.GlitchStrength;
			if (RenderGodRays)
			{
				progf.SunPosScreenIn = ShaderUniforms.SunPositionScreen;
				progf.SunPos3dIn = ShaderUniforms.SunPosition3D;
				progf.PlayerViewVector = ShaderUniforms.PlayerViewVector;
			}
			progf.DamageVignetting = ShaderUniforms.DamageVignetting;
			progf.DamageVignettingSide = ShaderUniforms.DamageVignettingSide;
			progf.FrostVignetting = ShaderUniforms.FrostVignetting;
			RenderFullscreenTriangle(screenQuad);
			progf.Stop();
			if (RenderSSAO)
			{
				DrawBuffersEnum[] array = new DrawBuffersEnum[4];
				RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				bufs = (DrawBuffersEnum[])(object)array;
				GL.DrawBuffers(4, bufs);
			}
			else
			{
				bufs = (DrawBuffersEnum[])(object)new DrawBuffersEnum[2]
				{
					(DrawBuffersEnum)36064,
					(DrawBuffersEnum)36065
				};
				GL.DrawBuffers(2, bufs);
			}
		}
	}

	private void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, nint message, nint userParam)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		if ((int)type != 33361)
		{
			string messageString = Marshal.PtrToStringAnsi(message, length);
			Logger.Notification("{0} {1} | {2}", severity, type, messageString);
			if ((int)type == 33356)
			{
				throw new Exception(messageString);
			}
		}
	}

	public override void BlitPrimaryToDefault()
	{
		if (OffscreenBuffer)
		{
			int textureId = frameBuffers[0].ColorTextureIds[0];
			LoadFrameBuffer(EnumFrameBuffer.Default);
			GL.Viewport(0, 0, window.ClientSize.X, window.ClientSize.Y);
			ShaderProgramBlit blit = ShaderPrograms.Blit;
			blit.Use();
			blit.Scene2D = textureId;
			RenderFullscreenTriangle(screenQuad);
			blit.Stop();
		}
	}

	private void CheckFboStatus(FramebufferTarget target, EnumFrameBuffer fbtype)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		CheckFboStatus(target, fbtype.ToString() ?? "");
	}

	private unsafe void CheckFboStatus(FramebufferTarget target, string fbtype)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected I4, but got Unknown
		FramebufferErrorCode err = Ext.CheckFramebufferStatus(target);
		switch (err - 36053)
		{
		case 0:
			return;
		case 1:
			throw new Exception("FBO " + fbtype + ": One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
		case 2:
			throw new Exception("FBO " + fbtype + ": There are no attachments.");
		case 4:
			throw new Exception("FBO " + fbtype + ": Attachments are of different size. All attachments must have the same width and height.");
		case 5:
			throw new Exception("FBO " + fbtype + ": The color attachments have different format. All color attachments must have the same format.");
		case 6:
			throw new Exception("FBO " + fbtype + ": An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
		case 7:
			throw new Exception("FBO " + fbtype + ": The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
		case 8:
			throw new Exception("FBO " + fbtype + ": This particular FBO configuration is not supported by the implementation.");
		}
		throw new Exception("FBO " + fbtype + ": Framebuffer unknown error (" + ((object)(*(FramebufferErrorCode*)(&err))/*cast due to constrained. prefix*/).ToString() + ")");
	}

	public override void CheckGlError(string errmsg = null)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (GlErrorChecking)
		{
			ErrorCode err = GL.GetError();
			if ((int)err != 0)
			{
				throw new Exception(string.Format("{0} - OpenGL threw an error: {1}", (errmsg == null) ? "" : (errmsg + " "), err));
			}
		}
	}

	public override void CheckGlErrorAlways(string errmsg = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		ErrorCode err = GL.GetError();
		if ((int)err != 0)
		{
			string note = (ClientSettings.GlDebugMode ? "" : ". Enable Gl Debug Mode in the settings or clientsettings.json to track this error");
			string logmsg = string.Format("{0} - OpenGL threw an error: {1}{2}", (errmsg == null) ? "" : (errmsg + " "), err, note);
			Logger.Error(logmsg);
		}
		if ((int)err == 1285)
		{
			throw new OutOfMemoryException("Either the graphics card or the OS ran out of memory! Please close other programs and reduce your view distance to prevent the game from crashing.");
		}
	}

	public unsafe override string GlGetError()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ErrorCode err = GL.GetError();
		if ((int)err != 0)
		{
			return ((object)(*(ErrorCode*)(&err))/*cast due to constrained. prefix*/).ToString();
		}
		return null;
	}

	public override string GetGLShaderVersionString()
	{
		return GL.GetString((StringName)35724);
	}

	public override int GenSampler(bool linear)
	{
		int num = GL.GenSampler();
		GL.SamplerParameter(num, (SamplerParameterName)10240, linear ? 9729 : 9728);
		GL.SamplerParameter(num, (SamplerParameterName)10241, 9986);
		return num;
	}

	public override void GLWireframes(bool toggle)
	{
		GL.PolygonMode((TriangleFace)1032, (PolygonMode)(toggle ? 6913 : 6914));
	}

	public override void GlViewport(int x, int y, int width, int height)
	{
		GL.Viewport(x, y, width, height);
	}

	public override void GlScissor(int x, int y, int width, int height)
	{
		GL.Scissor(x, y, width, height);
	}

	public override void GlScissorFlag(bool enable)
	{
		if (enable)
		{
			GL.Enable((EnableCap)3089);
		}
		else
		{
			GL.Disable((EnableCap)3089);
		}
	}

	public override void GlEnableDepthTest()
	{
		GL.Enable((EnableCap)2929);
	}

	public override void GlDisableDepthTest()
	{
		GL.Disable((EnableCap)2929);
	}

	public override void BindTexture2d(int texture)
	{
		GL.ActiveTexture((TextureUnit)33984);
		GL.BindTexture((TextureTarget)3553, texture);
	}

	public override void BindTextureCubeMap(int texture)
	{
		GL.BindTexture((TextureTarget)34067, texture);
	}

	public override void UnBindTextureCubeMap()
	{
		GL.BindTexture((TextureTarget)34067, 0);
	}

	public override void GlToggleBlend(bool on, EnumBlendMode blendMode = EnumBlendMode.Standard)
	{
		if (on)
		{
			GL.Enable((EnableCap)3042);
			switch (blendMode)
			{
			case EnumBlendMode.Brighten:
				GL.BlendFunc((BlendingFactor)774, (BlendingFactor)1);
				return;
			case EnumBlendMode.Multiply:
				GL.BlendFuncSeparate((BlendingFactorSrc)0, (BlendingFactorDest)771, (BlendingFactorSrc)1, (BlendingFactorDest)771);
				return;
			case EnumBlendMode.PremultipliedAlpha:
				GL.BlendFunc((BlendingFactor)1, (BlendingFactor)771);
				return;
			case EnumBlendMode.Glow:
				GL.BlendFuncSeparate((BlendingFactorSrc)770, (BlendingFactorDest)1, (BlendingFactorSrc)1, (BlendingFactorDest)0);
				return;
			case EnumBlendMode.Overlay:
				GL.BlendFuncSeparate((BlendingFactorSrc)770, (BlendingFactorDest)771, (BlendingFactorSrc)1, (BlendingFactorDest)1);
				return;
			}
			GL.BlendFunc((BlendingFactor)770, (BlendingFactor)771);
			if (RenderSSAO)
			{
				GL.BlendEquation(2, (BlendEquationMode)32774);
				GL.BlendFunc(2, (BlendingFactorSrc)1, (BlendingFactorDest)0);
				GL.BlendEquation(3, (BlendEquationMode)32774);
				GL.BlendFunc(3, (BlendingFactorSrc)1, (BlendingFactorDest)0);
			}
		}
		else
		{
			GL.Disable((EnableCap)3042);
		}
	}

	public override void GlDisableCullFace()
	{
		GL.Disable((EnableCap)2884);
	}

	public override void GlEnableCullFace()
	{
		GL.Enable((EnableCap)2884);
	}

	public override void GlClearColorRgbaf(float r, float g, float b, float a)
	{
		GL.ClearColor(r, g, b, a);
	}

	public override void GLLineWidth(float width)
	{
		if (RuntimeEnv.OS != OS.Mac)
		{
			GL.LineWidth(width);
		}
	}

	public override void SmoothLines(bool on)
	{
		if (RuntimeEnv.OS != OS.Mac)
		{
			if (on)
			{
				GL.Enable((EnableCap)2848);
				GL.Hint((HintTarget)3154, (HintMode)4354);
			}
			else
			{
				GL.Disable((EnableCap)2848);
				GL.Hint((HintTarget)3154, (HintMode)4352);
			}
		}
	}

	public override void GlDepthMask(bool flag)
	{
		GL.DepthMask(flag);
	}

	public override void GlDepthFunc(EnumDepthFunction depthFunc)
	{
		GL.DepthFunc((DepthFunction)depthFunc);
	}

	public override void GlCullFaceBack()
	{
		GL.CullFace((TriangleFace)1029);
	}

	public override void GlCullFaceFront()
	{
		GL.CullFace((TriangleFace)1028);
	}

	public override void GlEnableStencilTest()
	{
		GL.Enable((EnableCap)2960);
	}

	public override void GlDisableStencilTest()
	{
		GL.Disable((EnableCap)2960);
	}

	public override void GlStencilMask(int mask)
	{
		GL.StencilMask(mask);
	}

	public override void GlStencilFunc(int func, int refVal, int mask)
	{
		GL.StencilFunc((StencilFunction)func, refVal, mask);
	}

	public override void GlStencilOp(int sfail, int dpfail, int dppass)
	{
		GL.StencilOp((StencilOp)sfail, (StencilOp)dpfail, (StencilOp)dppass);
	}

	public override void GlColorMask(bool r, bool g, bool b, bool a)
	{
		GL.ColorMask(r, g, b, a);
	}

	public override void GlClearStencil()
	{
		GL.Clear((ClearBufferMask)1024);
	}

	public override void GlGenerateTex2DMipmaps()
	{
		GL.GenerateMipmap((GenerateMipmapTarget)3553);
	}

	public override int LoadCairoTexture(ImageSurface surface, bool linearMag)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Texture uploads must happen in the main thread. We only have one OpenGL context.");
		}
		int id = GL.GenTexture();
		GL.BindTexture((TextureTarget)3553, id);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, linearMag ? 9729 : 9728);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)32856, surface.Width, surface.Height, 0, (PixelFormat)32993, (PixelType)5121, (IntPtr)surface.DataPtr);
		return id;
	}

	public override void GenTexture(RawTexture tex)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Texture uploads must happen in the main thread. We only have one OpenGL context.");
		}
		int textureId = GL.GenTexture();
		GL.BindTexture((TextureTarget)3553, textureId);
		GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)tex.PixelInternalFormat, tex.Width, tex.Height, 0, (PixelFormat)tex.PixelFormat, (PixelType)5126, (IntPtr)IntPtr.Zero);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, (int)tex.MinFilter);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, (int)tex.MagFilter);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, (int)tex.WrapS);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, (int)tex.WrapT);
		tex.TextureId = textureId;
	}

	public override void LoadOrUpdateCairoTexture(ImageSurface surface, bool linearMag, ref LoadedTexture intoTexture)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Texture uploads must happen in the main thread. We only have one OpenGL context.");
		}
		if (intoTexture.TextureId == 0 || intoTexture.Width != surface.Width || intoTexture.Height != surface.Height)
		{
			if (intoTexture.TextureId != 0)
			{
				GL.DeleteTexture(intoTexture.TextureId);
			}
			intoTexture.TextureId = GL.GenTexture();
			intoTexture.Width = surface.Width;
			intoTexture.Height = surface.Height;
			GL.BindTexture((TextureTarget)3553, intoTexture.TextureId);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, linearMag ? 9729 : 9728);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)32856, surface.Width, surface.Height, 0, (PixelFormat)32993, (PixelType)5121, (IntPtr)surface.DataPtr);
		}
		else
		{
			GL.BindTexture((TextureTarget)3553, intoTexture.TextureId);
			GL.TexSubImage2D((TextureTarget)3553, 0, 0, 0, surface.Width, surface.Height, (PixelFormat)32993, (PixelType)5121, (IntPtr)surface.DataPtr);
		}
		CheckGlError("LoadOrUpdateCairoTexture");
	}

	public override int LoadTexture(SKBitmap bmp, bool linearMag = false, int clampMode = 0, bool generateMipmaps = false)
	{
		if (bmp == null)
		{
			throw new ArgumentNullException("bmp", "Trying to load texture from null bitmap");
		}
		return LoadTexture(new BitmapExternal(bmp), linearMag, clampMode, generateMipmaps);
	}

	public override void LoadIntoTexture(IBitmap srcBmp, int targetTextureId, int destX, int destY, bool generateMipmaps = false)
	{
		GL.BindTexture((TextureTarget)3553, targetTextureId);
		if (srcBmp is BitmapExternal bmpExt)
		{
			GL.TexSubImage2D((TextureTarget)3553, 0, destX, destY, srcBmp.Width, srcBmp.Height, (PixelFormat)32993, (PixelType)5121, (IntPtr)bmpExt.PixelsPtrAndLock);
		}
		else
		{
			GL.TexSubImage2D<int>((TextureTarget)3553, 0, destX, destY, srcBmp.Width, srcBmp.Height, (PixelFormat)32993, (PixelType)5121, srcBmp.Pixels);
		}
		if (ENABLE_MIPMAPS && generateMipmaps)
		{
			BuildMipMaps(targetTextureId);
		}
	}

	public override int LoadTexture(IBitmap bmp, bool linearMag = false, int clampMode = 0, bool generateMipmaps = false)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Texture uploads must happen in the main thread. We only have one OpenGL context.");
		}
		int id = GL.GenTexture();
		GL.BindTexture((TextureTarget)3553, id);
		if (ENABLE_ANISOTROPICFILTERING)
		{
			float maxAniso = GL.GetFloat((GetPName)34047);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)34046, maxAniso);
		}
		switch (clampMode)
		{
		case 1:
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33071);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33071);
			break;
		case 2:
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 10497);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 10497);
			break;
		}
		if (bmp is BitmapExternal bitmapExternal)
		{
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)6408, bmp.Width, bmp.Height, 0, (PixelFormat)32993, (PixelType)5121, (IntPtr)bitmapExternal.PixelsPtrAndLock);
		}
		else
		{
			GL.TexImage2D<int>((TextureTarget)3553, 0, (PixelInternalFormat)6408, bmp.Width, bmp.Height, 0, (PixelFormat)32993, (PixelType)5121, bmp.Pixels);
		}
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, linearMag ? 9729 : 9728);
		if (ENABLE_MIPMAPS && generateMipmaps)
		{
			BuildMipMaps(id);
		}
		return id;
	}

	public override void LoadOrUpdateTextureFromBgra_DeferMipMap(int[] rgbaPixels, bool linearMag, int clampMode, ref LoadedTexture intoTexture)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		PixelFormat format = (PixelFormat)32993;
		LoadOrUpdateTextureFromPixels(rgbaPixels, linearMag, clampMode, ref intoTexture, format, makeMipMap: false);
	}

	public override void LoadOrUpdateTextureFromBgra(int[] rgbaPixels, bool linearMag, int clampMode, ref LoadedTexture intoTexture)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		PixelFormat format = (PixelFormat)32993;
		LoadOrUpdateTextureFromPixels(rgbaPixels, linearMag, clampMode, ref intoTexture, format, makeMipMap: true);
	}

	public override void LoadOrUpdateTextureFromRgba(int[] rgbaPixels, bool linearMag, int clampMode, ref LoadedTexture intoTexture)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		PixelFormat format = (PixelFormat)6408;
		LoadOrUpdateTextureFromPixels(rgbaPixels, linearMag, clampMode, ref intoTexture, format, makeMipMap: true);
	}

	private void LoadOrUpdateTextureFromPixels(int[] rgbaPixels, bool linearMag, int clampMode, ref LoadedTexture intoTexture, PixelFormat format, bool makeMipMap)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Texture uploads must happen in the main thread. We only have one OpenGL context.");
		}
		if (intoTexture.TextureId == 0 || intoTexture.Width * intoTexture.Height != rgbaPixels.Length)
		{
			if (intoTexture.TextureId != 0)
			{
				GL.DeleteTexture(intoTexture.TextureId);
			}
			intoTexture.TextureId = GL.GenTexture();
			GL.BindTexture((TextureTarget)3553, intoTexture.TextureId);
			if (ENABLE_ANISOTROPICFILTERING)
			{
				float maxAniso = GL.GetFloat((GetPName)34047);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)34046, maxAniso);
			}
			if (clampMode == 1)
			{
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33071);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33071);
			}
			GL.TexImage2D<int>((TextureTarget)3553, 0, (PixelInternalFormat)6408, intoTexture.Width, intoTexture.Height, 0, format, (PixelType)5121, rgbaPixels);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9729);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, linearMag ? 9729 : 9728);
			if (makeMipMap)
			{
				BuildMipMaps(intoTexture.TextureId);
			}
		}
		else
		{
			GL.BindTexture((TextureTarget)3553, intoTexture.TextureId);
			GL.TexSubImage2D<int>((TextureTarget)3553, 0, 0, 0, intoTexture.Width, intoTexture.Height, format, (PixelType)5121, rgbaPixels);
		}
	}

	public override void BuildMipMaps(int textureId)
	{
		if (ENABLE_MIPMAPS)
		{
			GL.BindTexture((TextureTarget)3553, textureId);
			int MipMapCount = default(int);
			GL.GetTexParameter((TextureTarget)3553, (GetTextureParameter)33085, ref MipMapCount);
			if (MipMapCount > 0)
			{
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9986);
				GL.GenerateMipmap((GenerateMipmapTarget)3553);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)34049, 0f);
				GL.TexParameter((TextureTarget)3553, (TextureParameterName)33085, ClientSettings.MipMapLevel);
			}
		}
	}

	public override int Load3DTextureCube(BitmapRef[] bmps)
	{
		GL.ActiveTexture((TextureUnit)33984);
		int textureId = GL.GenTexture();
		GL.BindTexture((TextureTarget)34067, textureId);
		for (int i = 0; i < 6; i++)
		{
			Load3DTextureSide((BitmapExternal)bmps[i], (TextureTarget)(34069 + i));
		}
		GL.TexParameter((TextureTarget)34067, (TextureParameterName)10241, 9729);
		GL.TexParameter((TextureTarget)34067, (TextureParameterName)10240, 9729);
		GL.TexParameter((TextureTarget)34067, (TextureParameterName)32882, Convert.ToInt32((object)(TextureWrapMode)33071));
		GL.TexParameter((TextureTarget)34067, (TextureParameterName)10242, Convert.ToInt32((object)(TextureWrapMode)33071));
		GL.TexParameter((TextureTarget)34067, (TextureParameterName)10243, Convert.ToInt32((object)(TextureWrapMode)33071));
		return textureId;
	}

	private void Load3DTextureSide(BitmapExternal bmp, TextureTarget target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		GL.TexImage2D(target, 0, (PixelInternalFormat)6408, bmp.Width, bmp.Height, 0, (PixelFormat)32993, (PixelType)5121, (IntPtr)bmp.PixelsPtrAndLock);
	}

	public override void GLDeleteTexture(int id)
	{
		GL.DeleteTexture(id);
	}

	public override int GlGetMaxTextureSize()
	{
		int size = 1024;
		try
		{
			GL.GetInteger((GetPName)3379, ref size);
		}
		catch
		{
		}
		return size;
	}

	public override UBORef CreateUBO(int shaderProgramId, int bindingPoint, string blockName, int size)
	{
		int handle = GL.GenBuffer();
		GL.BindBuffer((BufferTarget)35345, handle);
		GL.BufferData((BufferTarget)35345, size, (IntPtr)IntPtr.Zero, (BufferUsageHint)35048);
		ScreenManager.Platform.CheckGlError();
		int uboIndex = GL.GetUniformBlockIndex(shaderProgramId, blockName);
		ScreenManager.Platform.CheckGlError();
		GL.UniformBlockBinding(shaderProgramId, uboIndex, bindingPoint);
		GL.BindBufferBase((BufferRangeTarget)35345, bindingPoint, handle);
		ScreenManager.Platform.CheckGlError();
		UBO uBO = new UBO();
		uBO.Handle = handle;
		uBO.Size = size;
		uBO.Unbind();
		ScreenManager.Platform.CheckGlError();
		return uBO;
	}

	public override void UpdateMesh(MeshRef modelRef, MeshData data)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		VAO vao = (VAO)modelRef;
		BufferAccessMask flags = (BufferAccessMask)34;
		if (vao.Persistent)
		{
			flags = (BufferAccessMask)(flags | 0x50);
		}
		bool pers = vao.Persistent;
		if (data.xyz != null)
		{
			updateVAO(data.xyz, data.XyzOffset, data.XyzCount, vao.xyzVboId, vao.xyzPtr, pers);
		}
		if (data.Normals != null && data.VerticesCount > 0)
		{
			updateVAO(data.Normals, data.NormalsOffset, data.VerticesCount, vao.normalsVboId, vao.normalsPtr, pers);
		}
		if (data.Uv != null && data.UvCount > 0)
		{
			updateVAO(data.Uv, data.UvOffset, data.UvCount, vao.uvVboId, vao.uvPtr, pers);
		}
		if (data.Rgba != null && data.RgbaCount > 0)
		{
			updateVAO(data.Rgba, data.RgbaOffset, data.RgbaCount, vao.rgbaVboId, vao.rgbaPtr, pers);
		}
		if (data.Flags != null && data.FlagsCount > 0)
		{
			updateVAO(data.Flags, data.FlagsOffset, data.FlagsCount, vao.flagsVboId, vao.flagsPtr, pers);
		}
		if (data.CustomFloats != null && data.CustomFloats.Count > 0)
		{
			updateVAO(data.CustomFloats.Values, data.CustomFloats.BaseOffset, data.CustomFloats.Count, vao.customDataFloatVboId, vao.customDataFloatPtr, pers);
		}
		if (data.CustomShorts != null && data.CustomShorts.Count > 0)
		{
			updateVAO(data.CustomShorts.Values, data.CustomShorts.BaseOffset, data.CustomShorts.Count, vao.customDataShortVboId, vao.customDataShortPtr, pers);
		}
		if (data.CustomInts != null && data.CustomInts.Count > 0)
		{
			updateVAO(data.CustomInts.Values, data.CustomInts.BaseOffset, data.CustomInts.Count, vao.customDataIntVboId, vao.customDataIntPtr, pers);
		}
		if (data.CustomBytes != null && data.CustomBytes.Count > 0)
		{
			updateVAO(data.CustomBytes.Values, data.CustomBytes.BaseOffset, data.CustomBytes.Count, vao.customDataByteVboId, vao.customDataBytePtr, pers);
		}
		GL.BindBuffer((BufferTarget)34962, 0);
		updateIndices(data.Indices, data.IndicesOffset, data.IndicesCount, vao, pers);
		if (GlErrorChecking && GlDebugMode)
		{
			global::_003C_003Ey__InlineArray4<object> buffer = default(global::_003C_003Ey__InlineArray4<object>);
			buffer[0] = data.XyzCount;
			buffer[1] = data.RgbaCount;
			buffer[2] = data.UvCount;
			buffer[3] = data.IndicesCount;
			CheckGlError(string.Format("Error when trying to update vao indices, modeldata xyz/rgba/uv/indices sizes: {0}/{1}/{2}/{3}", (ReadOnlySpan<object?>)buffer));
		}
	}

	private unsafe void updateVAO(float[] data, int offset, int count, int vboId, nint vboPtr, bool pers)
	{
		GL.BindBuffer((BufferTarget)34962, vboId);
		if (pers)
		{
			float* ptr = (float*)vboPtr;
			ptr += offset / 4;
			for (int i = 0; i < count; i++)
			{
				*(ptr++) = data[i];
			}
		}
		else
		{
			GL.BufferSubData<float>((BufferTarget)34962, (IntPtr)offset, 4 * count, data);
		}
	}

	private unsafe void updateVAO(int[] data, int offset, int count, int vboId, nint vboPtr, bool pers)
	{
		GL.BindBuffer((BufferTarget)34962, vboId);
		if (pers)
		{
			int* ptr = (int*)vboPtr;
			ptr += offset / 4;
			for (int i = 0; i < count; i++)
			{
				*(ptr++) = data[i];
			}
		}
		else
		{
			GL.BufferSubData<int>((BufferTarget)34962, (IntPtr)offset, 4 * count, data);
		}
	}

	private unsafe void updateVAO(short[] data, int offset, int count, int vboId, nint vboPtr, bool pers)
	{
		GL.BindBuffer((BufferTarget)34962, vboId);
		if (pers)
		{
			short* ptr = (short*)vboPtr;
			ptr += offset / 2;
			for (int i = 0; i < count; i++)
			{
				*(ptr++) = data[i];
			}
		}
		else
		{
			GL.BufferSubData<short>((BufferTarget)34962, (IntPtr)offset, 2 * count, data);
		}
	}

	private unsafe void updateVAO(ushort[] data, int offset, int count, int vboId, nint vboPtr, bool pers)
	{
		GL.BindBuffer((BufferTarget)34962, vboId);
		if (pers)
		{
			ushort* ptr = (ushort*)vboPtr;
			ptr += offset / 2;
			for (int i = 0; i < count; i++)
			{
				*(ptr++) = data[i];
			}
		}
		else
		{
			GL.BufferSubData<ushort>((BufferTarget)34962, (IntPtr)offset, 2 * count, data);
		}
	}

	private unsafe void updateVAO(byte[] data, int offset, int count, int vboId, nint vboPtr, bool pers)
	{
		GL.BindBuffer((BufferTarget)34962, vboId);
		if (pers)
		{
			byte* ptr = (byte*)vboPtr;
			ptr += offset / 1;
			for (int i = 0; i < count; i++)
			{
				*(ptr++) = data[i];
			}
		}
		else
		{
			GL.BufferSubData<byte>((BufferTarget)34962, (IntPtr)offset, count, data);
		}
	}

	private unsafe void updateIndices(int[] Indices, int IndicesOffset, int IndicesCount, VAO vao, bool pers)
	{
		if (Indices == null)
		{
			return;
		}
		GL.BindBuffer((BufferTarget)34963, vao.vboIdIndex);
		if (pers)
		{
			int* ptr = (int*)vao.indicesPtr;
			ptr += IndicesOffset / 4;
			for (int i = 0; i < IndicesCount; i++)
			{
				*(ptr++) = Indices[i];
			}
		}
		else
		{
			GL.BufferSubData<int>((BufferTarget)34963, (IntPtr)IndicesOffset, 4 * IndicesCount, Indices);
		}
		GL.BindBuffer((BufferTarget)34963, 0);
		vao.IndicesCount = IndicesCount;
	}

	public override MeshRef AllocateEmptyMesh(int xyzSize, int normalsSize, int uvSize, int rgbaSize, int flagsSize, int indicesSize, CustomMeshDataPartFloat customFloats, CustomMeshDataPartShort customShorts, CustomMeshDataPartByte customBytes, CustomMeshDataPartInt customInts, EnumDrawMode drawMode = EnumDrawMode.Triangles, bool staticDraw = true)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		VAO vao = new VAO();
		int vaoId = GL.GenVertexArray();
		int vaoSlotNumber = 0;
		GL.BindVertexArray(vaoId);
		int xyzVboId = 0;
		int normalsVboId = 0;
		int uvVboId = 0;
		int rgbaVboId = 0;
		int flagsVboId = 0;
		BufferUsageHint usageHint = (BufferUsageHint)(staticDraw ? 35044 : 35048);
		bool doPStorage = allowPStorage && supportsPersistentMapping && !staticDraw;
		BufferStorageFlags flags = (BufferStorageFlags)450;
		MapBufferAccessMask mapflags = (MapBufferAccessMask)194;
		if (xyzSize > 0)
		{
			xyzVboId = GenArrayBuffer(xyzSize, ref vao.xyzPtr, doPStorage, flags, mapflags, usageHint);
			GL.VertexAttribPointer(vaoSlotNumber++, 3, (VertexAttribPointerType)5126, false, 0, 0);
		}
		CheckGlError("Failed loading model");
		if (normalsSize > 0)
		{
			normalsVboId = GenArrayBuffer(normalsSize, ref vao.normalsPtr, doPStorage, flags, mapflags, usageHint);
			GL.VertexAttribPointer(vaoSlotNumber++, 4, (VertexAttribPointerType)36255, true, 0, 0);
		}
		if (uvSize > 0)
		{
			uvVboId = GenArrayBuffer(uvSize, ref vao.uvPtr, doPStorage, flags, mapflags, usageHint);
			GL.VertexAttribPointer(vaoSlotNumber++, 2, (VertexAttribPointerType)5126, false, 0, 0);
		}
		if (rgbaSize > 0)
		{
			rgbaVboId = GenArrayBuffer(rgbaSize, ref vao.rgbaPtr, doPStorage, flags, mapflags, usageHint);
			GL.VertexAttribPointer(vaoSlotNumber++, 4, (VertexAttribPointerType)5121, true, 0, 0);
		}
		if (flagsSize > 0)
		{
			flagsVboId = GenArrayBuffer(flagsSize, ref vao.flagsPtr, doPStorage, flags, mapflags, usageHint);
			GL.VertexAttribIPointer(vaoSlotNumber++, 1, (VertexAttribIntegerType)5125, 0, (IntPtr)IntPtr.Zero);
		}
		vaoSlotNumber = AddCustoms(vao, vaoSlotNumber, customFloats, customShorts, customInts, customBytes, doPStorage, flags, mapflags, 0);
		GL.BindBuffer((BufferTarget)34962, 0);
		int vboIdIndex = GL.GenBuffer();
		GL.BindBuffer((BufferTarget)34963, vboIdIndex);
		if (doPStorage)
		{
			GL.BufferStorage((BufferTarget)34963, indicesSize, (IntPtr)IntPtr.Zero, flags);
			vao.indicesPtr = GL.MapBufferRange((BufferTarget)34963, (IntPtr)IntPtr.Zero, indicesSize, mapflags);
		}
		else
		{
			GL.BufferData((BufferTarget)34963, indicesSize, (IntPtr)IntPtr.Zero, (BufferUsageHint)35044);
		}
		GL.BindBuffer((BufferTarget)34963, 0);
		for (int i = 0; i < vaoSlotNumber; i++)
		{
			GL.EnableVertexAttribArray(i);
		}
		GL.BindVertexArray(0);
		CheckGlError("Failed loading model");
		vao.Persistent = doPStorage;
		vao.VaoId = vaoId;
		vao.IndicesCount = indicesSize;
		vao.vaoSlotNumber = vaoSlotNumber;
		vao.vboIdIndex = vboIdIndex;
		vao.normalsVboId = normalsVboId;
		vao.xyzVboId = xyzVboId;
		vao.uvVboId = uvVboId;
		vao.rgbaVboId = rgbaVboId;
		vao.flagsVboId = flagsVboId;
		vao.drawMode = DrawModeToPrimiteType(drawMode);
		return vao;
	}

	private int AddCustoms(VAO vao, int vaoSlotNumber, CustomMeshDataPartFloat customFloats, CustomMeshDataPartShort customShorts, CustomMeshDataPartInt customInts, CustomMeshDataPartByte customBytes, bool doPStorage, BufferStorageFlags flags, MapBufferAccessMask mapflags, int pruneCustomInts)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		int customDataFloatsVboId = 0;
		int customDataBytesVboId = 0;
		int customDataIntsVboId = 0;
		int customDataShortsVboId = 0;
		if (customFloats != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(customFloats.StaticDraw ? 35044 : 35048);
			customDataFloatsVboId = GenArrayBuffer(4 * customFloats.AllocationSize, ref vao.customDataFloatPtr, doPStorage, flags, mapflags, hint);
			int[] interleaveSizes = customFloats.InterleaveSizes;
			for (int i = 0; i < interleaveSizes.Length; i++)
			{
				GL.VertexAttribPointer(vaoSlotNumber, interleaveSizes[i], (VertexAttribPointerType)5126, false, customFloats.InterleaveStride, customFloats.InterleaveOffsets[i]);
				if (customFloats.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		if (customShorts != null)
		{
			BufferUsageHint hint2 = (BufferUsageHint)(customShorts.StaticDraw ? 35044 : 35048);
			customDataShortsVboId = GenArrayBuffer(2 * customShorts.AllocationSize, ref vao.customDataShortPtr, doPStorage, flags, mapflags, hint2);
			int[] interleaveSizes2 = customShorts.InterleaveSizes;
			for (int j = 0; j < interleaveSizes2.Length; j++)
			{
				if (customShorts.Conversion == DataConversion.Integer)
				{
					GL.VertexAttribIPointer(vaoSlotNumber, interleaveSizes2[j], (VertexAttribIntegerType)5122, customShorts.InterleaveStride, (IntPtr)customShorts.InterleaveOffsets[j]);
				}
				else
				{
					GL.VertexAttribPointer(vaoSlotNumber, interleaveSizes2[j], (VertexAttribPointerType)5123, customShorts.Conversion == DataConversion.NormalizedFloat, customShorts.InterleaveStride, customShorts.InterleaveOffsets[j]);
				}
				if (customShorts.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		if (customInts != null && (pruneCustomInts == 0 || customInts.InterleaveStride > 4))
		{
			int prunedDivisor = 1 + pruneCustomInts;
			BufferUsageHint hint3 = (BufferUsageHint)(customInts.StaticDraw ? 35044 : 35048);
			customDataIntsVboId = GenArrayBuffer(4 * customInts.AllocationSize / prunedDivisor, ref vao.customDataIntPtr, doPStorage, flags, mapflags, hint3);
			int[] interleaveSizes3 = customInts.InterleaveSizes;
			for (int k = 0; k < interleaveSizes3.Length; k++)
			{
				if (k - pruneCustomInts != -1)
				{
					if (customInts.Conversion == DataConversion.Integer)
					{
						GL.VertexAttribIPointer(vaoSlotNumber, interleaveSizes3[k], (VertexAttribIntegerType)5125, customInts.InterleaveStride / prunedDivisor, (IntPtr)customInts.InterleaveOffsets[k - pruneCustomInts]);
					}
					else
					{
						GL.VertexAttribPointer(vaoSlotNumber, interleaveSizes3[k], (VertexAttribPointerType)5125, customInts.Conversion == DataConversion.NormalizedFloat, customInts.InterleaveStride / prunedDivisor, customInts.InterleaveOffsets[k - pruneCustomInts]);
					}
					if (customInts.Instanced)
					{
						GL.VertexAttribDivisor(vaoSlotNumber, 1);
					}
					vaoSlotNumber++;
				}
			}
		}
		if (customBytes != null)
		{
			BufferUsageHint hint4 = (BufferUsageHint)(customBytes.StaticDraw ? 35044 : 35048);
			customDataBytesVboId = GenArrayBuffer(customBytes.AllocationSize, ref vao.customDataBytePtr, doPStorage, flags, mapflags, hint4);
			int[] interleaveSizes4 = customBytes.InterleaveSizes;
			for (int l = 0; l < interleaveSizes4.Length; l++)
			{
				if (customBytes.Conversion == DataConversion.Integer)
				{
					GL.VertexAttribIPointer(vaoSlotNumber, interleaveSizes4[l], (VertexAttribIntegerType)5121, customBytes.InterleaveStride, (IntPtr)customBytes.InterleaveOffsets[l]);
				}
				else
				{
					GL.VertexAttribPointer(vaoSlotNumber, interleaveSizes4[l], (VertexAttribPointerType)5121, customBytes.Conversion == DataConversion.NormalizedFloat, customBytes.InterleaveStride, customBytes.InterleaveOffsets[l]);
				}
				if (customBytes.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		vao.customDataFloatVboId = customDataFloatsVboId;
		vao.customDataByteVboId = customDataBytesVboId;
		vao.customDataIntVboId = customDataIntsVboId;
		vao.customDataShortVboId = customDataShortsVboId;
		return vaoSlotNumber;
	}

	private int GenArrayBuffer(int dataSize, ref nint ptr, bool doPStorage, BufferStorageFlags flags, MapBufferAccessMask mapflags, BufferUsageHint usageHint)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		int id = GL.GenBuffer();
		GL.BindBuffer((BufferTarget)34962, id);
		if (doPStorage)
		{
			GL.BufferStorage((BufferTarget)34962, dataSize, (IntPtr)IntPtr.Zero, flags);
			ptr = GL.MapBufferRange((BufferTarget)34962, (IntPtr)IntPtr.Zero, dataSize, mapflags);
		}
		else
		{
			GL.BufferData((BufferTarget)34962, dataSize, (IntPtr)IntPtr.Zero, usageHint);
		}
		return id;
	}

	public override MeshRef UploadMesh(MeshData data)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		int vaoId = GL.GenVertexArray();
		int vaoSlotNumber = 0;
		GL.BindVertexArray(vaoId);
		int xyzVboId = 0;
		int normalsVboId = 0;
		int uvVboId = 0;
		int rgbaVboId = 0;
		int customDataFloatVboId = 0;
		int customDataShortVboId = 0;
		int customDataIntVboId = 0;
		int customDataByteVboId = 0;
		int flagsVboId = 0;
		if (data.xyz != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.XyzStatic ? 35044 : 35048);
			xyzVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, xyzVboId);
			GL.BufferData<float>((BufferTarget)34962, 4 * data.XyzCount, data.xyz, hint);
			GL.VertexAttribPointer(vaoSlotNumber, 3, (VertexAttribPointerType)5126, false, 0, 0);
			if (data.XyzInstanced)
			{
				GL.VertexAttribDivisor(vaoSlotNumber, 1);
			}
			vaoSlotNumber++;
		}
		if (data.Normals != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.XyzStatic ? 35044 : 35048);
			normalsVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, normalsVboId);
			GL.BufferData<int>((BufferTarget)34962, 4 * data.VerticesCount, data.Normals, hint);
			GL.VertexAttribPointer(vaoSlotNumber, 4, (VertexAttribPointerType)36255, true, 0, 0);
			if (data.XyzInstanced)
			{
				GL.VertexAttribDivisor(vaoSlotNumber, 1);
			}
			vaoSlotNumber++;
		}
		if (data.Uv != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.UvStatic ? 35044 : 35048);
			uvVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, uvVboId);
			GL.BufferData<float>((BufferTarget)34962, 4 * data.UvCount, data.Uv, hint);
			GL.VertexAttribPointer(vaoSlotNumber, 2, (VertexAttribPointerType)5126, false, 0, 0);
			if (data.UvInstanced)
			{
				GL.VertexAttribDivisor(vaoSlotNumber, 1);
			}
			vaoSlotNumber++;
		}
		if (data.Rgba != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.RgbaStatic ? 35044 : 35048);
			rgbaVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, rgbaVboId);
			GL.BufferData<byte>((BufferTarget)34962, data.RgbaCount, data.Rgba, hint);
			GL.VertexAttribPointer(vaoSlotNumber, 4, (VertexAttribPointerType)5121, true, 0, 0);
			if (data.RgbaInstanced)
			{
				GL.VertexAttribDivisor(vaoSlotNumber, 1);
			}
			vaoSlotNumber++;
		}
		if (data.Flags != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.FlagsStatic ? 35044 : 35048);
			flagsVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, flagsVboId);
			GL.BufferData<int>((BufferTarget)34962, 4 * data.Flags.Length, data.Flags, hint);
			GL.VertexAttribIPointer(vaoSlotNumber, 1, (VertexAttribIntegerType)5125, 0, (IntPtr)IntPtr.Zero);
			if (data.FlagsInstanced)
			{
				GL.VertexAttribDivisor(vaoSlotNumber, 1);
			}
			vaoSlotNumber++;
		}
		if (data.CustomFloats != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.CustomFloats.StaticDraw ? 35044 : 35048);
			customDataFloatVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, customDataFloatVboId);
			GL.BufferData<float>((BufferTarget)34962, 4 * data.CustomFloats.AllocationSize, data.CustomFloats.Values, hint);
			for (int i = 0; i < data.CustomFloats.InterleaveSizes.Length; i++)
			{
				GL.VertexAttribPointer(vaoSlotNumber, data.CustomFloats.InterleaveSizes[i], (VertexAttribPointerType)5126, false, data.CustomFloats.InterleaveStride, data.CustomFloats.InterleaveOffsets[i]);
				if (data.CustomFloats.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		if (data.CustomShorts != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.CustomShorts.StaticDraw ? 35044 : 35048);
			customDataShortVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, customDataShortVboId);
			GL.BufferData<short>((BufferTarget)34962, 2 * data.CustomShorts.AllocationSize, data.CustomShorts.Values, hint);
			for (int j = 0; j < data.CustomShorts.InterleaveSizes.Length; j++)
			{
				if (data.CustomShorts.Conversion == DataConversion.Integer)
				{
					GL.VertexAttribIPointer(vaoSlotNumber, data.CustomShorts.InterleaveSizes[j], (VertexAttribIntegerType)5122, data.CustomShorts.InterleaveStride, (IntPtr)IntPtr.Zero);
				}
				else
				{
					GL.VertexAttribPointer(vaoSlotNumber, data.CustomShorts.InterleaveSizes[j], (VertexAttribPointerType)5122, data.CustomShorts.Conversion == DataConversion.NormalizedFloat, data.CustomShorts.InterleaveStride, data.CustomShorts.InterleaveOffsets[j]);
				}
				if (data.CustomShorts.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		if (data.CustomInts != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.CustomInts.StaticDraw ? 35044 : 35048);
			customDataIntVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, customDataIntVboId);
			GL.BufferData<int>((BufferTarget)34962, 4 * data.CustomInts.AllocationSize, data.CustomInts.Values, hint);
			for (int k = 0; k < data.CustomInts.InterleaveSizes.Length; k++)
			{
				GL.VertexAttribIPointer(vaoSlotNumber, data.CustomInts.InterleaveSizes[k], (VertexAttribIntegerType)5124, data.CustomInts.InterleaveStride, (IntPtr)IntPtr.Zero);
				if (data.CustomInts.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		if (data.CustomBytes != null)
		{
			BufferUsageHint hint = (BufferUsageHint)(data.CustomBytes.StaticDraw ? 35044 : 35048);
			customDataByteVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34962, customDataByteVboId);
			GL.BufferData<byte>((BufferTarget)34962, data.CustomBytes.AllocationSize, data.CustomBytes.Values, hint);
			for (int l = 0; l < data.CustomBytes.InterleaveSizes.Length; l++)
			{
				if (data.CustomBytes.Conversion == DataConversion.Integer)
				{
					GL.VertexAttribIPointer(vaoSlotNumber, data.CustomBytes.InterleaveSizes[l], (VertexAttribIntegerType)5121, data.CustomBytes.InterleaveStride, (IntPtr)IntPtr.Zero);
				}
				else
				{
					GL.VertexAttribPointer(vaoSlotNumber, data.CustomBytes.InterleaveSizes[l], (VertexAttribPointerType)5121, data.CustomBytes.Conversion == DataConversion.NormalizedFloat, data.CustomBytes.InterleaveStride, data.CustomBytes.InterleaveOffsets[l]);
				}
				if (data.CustomBytes.Instanced)
				{
					GL.VertexAttribDivisor(vaoSlotNumber, 1);
				}
				vaoSlotNumber++;
			}
		}
		GL.BindBuffer((BufferTarget)34962, 0);
		int vboIdIndex = GL.GenBuffer();
		GL.BindBuffer((BufferTarget)34963, vboIdIndex);
		GL.BufferData<int>((BufferTarget)34963, 4 * data.IndicesCount, data.Indices, (BufferUsageHint)(data.IndicesStatic ? 35044 : 35048));
		GL.BindBuffer((BufferTarget)34963, 0);
		for (int m = 0; m < vaoSlotNumber; m++)
		{
			GL.EnableVertexAttribArray(m);
		}
		GL.BindVertexArray(0);
		CheckGlError("Something failed during mesh upload");
		return new VAO
		{
			VaoId = vaoId,
			IndicesCount = data.IndicesCount,
			vaoSlotNumber = vaoSlotNumber,
			vboIdIndex = vboIdIndex,
			normalsVboId = normalsVboId,
			xyzVboId = xyzVboId,
			uvVboId = uvVboId,
			rgbaVboId = rgbaVboId,
			customDataFloatVboId = customDataFloatVboId,
			customDataIntVboId = customDataIntVboId,
			customDataByteVboId = customDataByteVboId,
			customDataShortVboId = customDataShortVboId,
			flagsVboId = flagsVboId,
			drawMode = DrawModeToPrimiteType(data.mode)
		};
	}

	public override void DeleteMesh(MeshRef modelref)
	{
		if (modelref != null)
		{
			((VAO)modelref).Dispose();
		}
	}

	public override void UpdateSSBOMesh(MeshRef modelRef, MeshData data)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (data.xyz == null)
		{
			return;
		}
		VAO vao = (VAO)modelRef;
		BufferAccessMask flags = (BufferAccessMask)34;
		if (vao.Persistent)
		{
			flags = (BufferAccessMask)(flags | 0x50);
		}
		bool pers = vao.Persistent;
		int count = data.VerticesCount;
		if (facedataBuffer == null || facedataBuffer.Length < count / 4)
		{
			facedataBuffer = new FaceData[count / 4];
		}
		float[] src = data.xyz;
		float[] uv = data.Uv;
		int[] rflags = data.Flags;
		int[] colordataInts = ((data.CustomInts != null && data.CustomInts.Count > 0) ? data.CustomInts.Values : null);
		int colordataStride = ((data.CustomInts == null || data.CustomInts.Count <= 0) ? 1 : (data.CustomInts.InterleaveStride / 4));
		FaceData[] facesBuffer = facedataBuffer;
		for (int i = 0; i < count; i += 4)
		{
			float u0 = uv[i * 2];
			float v0 = uv[i * 2 + 1];
			float v1 = uv[i * 2 + 3];
			float u2 = uv[i * 2 + 4];
			float v2 = uv[i * 2 + 5];
			if (u0 < -1.5E-05f || u0 > 1.000015f || v0 < -1.5E-05f || v0 > 1.000015f)
			{
				u0 = 0f;
				v0 = 0f;
			}
			if (u2 < -1.5E-05f || u2 > 1.000015f || v2 < -1.5E-05f || v2 > 1.000015f)
			{
				u2 = 0f;
				v2 = 0f;
			}
			bool rotatedUv;
			if (rotatedUv = v0 == v1)
			{
				float u3 = uv[i * 2 + 2];
				if (u2 != u3)
				{
					rotatedUv = false;
				}
			}
			facesBuffer[i / 4] = new FaceData(src, i * 3, u0, v0, u2 - u0, v2 - v0, rflags, i, (colordataInts != null) ? colordataInts[i * colordataStride] : 0, rotatedUv);
		}
		int offset = data.XyzOffset / 12 * 16;
		GL.BindBuffer((BufferTarget)37074, vao.xyzVboId);
		GL.BufferSubData<FaceData>((BufferTarget)37074, (IntPtr)offset, 16 * count, facedataBuffer);
		GL.BindBuffer((BufferTarget)37074, 0);
		if (data.Rgba != null && data.RgbaCount > 0)
		{
			updateVAO(data.Rgba, data.RgbaOffset, data.RgbaCount, vao.rgbaVboId, vao.rgbaPtr, pers);
		}
		if (data.CustomFloats != null && data.CustomFloats.Count > 0)
		{
			updateVAO(data.CustomFloats.Values, data.CustomFloats.BaseOffset, data.CustomFloats.Count, vao.customDataFloatVboId, vao.customDataFloatPtr, pers);
		}
		if (data.CustomShorts != null && data.CustomShorts.Count > 0)
		{
			updateVAO(data.CustomShorts.Values, data.CustomShorts.BaseOffset, data.CustomShorts.Count, vao.customDataShortVboId, vao.customDataShortPtr, pers);
		}
		if (data.CustomInts != null && data.CustomInts.Count > 0 && data.CustomInts.InterleaveStride > 4)
		{
			int[] customInts = data.CustomInts.Values;
			if (data.CustomInts.InterleaveStride / 4 != 2)
			{
				throw new Exception("We are assuming 2 customInts per vertex if it is not 1");
			}
			int ciCount = data.CustomInts.Count / 2;
			if (customIntsPruned == null || customIntsPruned.Length < ciCount)
			{
				customIntsPruned = new int[ciCount];
			}
			int[] customIntsCopy = customIntsPruned;
			for (int j = 0; j < ciCount; j++)
			{
				customIntsCopy[j] = customInts[j * 2 + 1];
			}
			updateVAO(customIntsPruned, data.CustomInts.BaseOffset / 2, ciCount, vao.customDataIntVboId, vao.customDataIntPtr, pers);
		}
		if (data.CustomBytes != null && data.CustomBytes.Count > 0)
		{
			updateVAO(data.CustomBytes.Values, data.CustomBytes.BaseOffset, data.CustomBytes.Count, vao.customDataByteVboId, vao.customDataBytePtr, pers);
		}
		GL.BindBuffer((BufferTarget)34962, 0);
		vao.IndicesCount = data.IndicesCount;
		if (GlErrorChecking && GlDebugMode)
		{
			global::_003C_003Ey__InlineArray4<object> buffer = default(global::_003C_003Ey__InlineArray4<object>);
			buffer[0] = data.XyzCount;
			buffer[1] = data.RgbaCount;
			buffer[2] = data.UvCount;
			buffer[3] = data.IndicesCount;
			CheckGlError(string.Format("Error when trying to update vao indices, modeldata xyz/rgba/uv/indices sizes: {0}/{1}/{2}/{3}", (ReadOnlySpan<object?>)buffer));
		}
	}

	public override MeshRef AllocateEmptySSBOMesh(int xyzSize, int normalsSize, int uvSize, int rgbaSize, int flagsSize, int indicesSize, CustomMeshDataPartFloat customFloats, CustomMeshDataPartShort customShorts, CustomMeshDataPartByte customBytes, CustomMeshDataPartInt customInts, EnumDrawMode drawMode = EnumDrawMode.Triangles, bool staticDraw = true)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		VAO vao = new VAO();
		int vaoId = GL.GenVertexArray();
		int vaoSlotNumber = 0;
		GL.BindVertexArray(vaoId);
		int xyzVboId = 0;
		int rgbaVboId = 0;
		BufferUsageHint usageHint = (BufferUsageHint)(staticDraw ? 35044 : 35048);
		BufferStorageFlags flags = (BufferStorageFlags)450;
		MapBufferAccessMask mapflags = (MapBufferAccessMask)194;
		if (xyzSize > 0)
		{
			xyzVboId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)37074, xyzVboId);
			GL.BufferStorage((BufferTarget)37074, xyzSize / 12 * 16, (IntPtr)IntPtr.Zero, (BufferStorageFlags)256);
		}
		CheckGlError("Failed loading model");
		if (rgbaSize > 0)
		{
			rgbaVboId = GenArrayBuffer(rgbaSize, ref vao.rgbaPtr, doPStorage: false, flags, mapflags, usageHint);
			GL.VertexAttribPointer(vaoSlotNumber++, 4, (VertexAttribPointerType)5121, true, 0, 0);
		}
		vaoSlotNumber = AddCustoms(vao, vaoSlotNumber, customFloats, customShorts, customInts, customBytes, doPStorage: false, flags, mapflags, 1);
		GL.BindBuffer((BufferTarget)34962, 0);
		if (ClientPlatformAbstract.singleIndexBufferSize < indicesSize)
		{
			ClientPlatformAbstract.singleIndexBufferSize = indicesSize;
			if (ClientPlatformAbstract.singleIndexBufferSize != 0)
			{
				ClientPlatformAbstract.DisposeIndexBuffer();
			}
			ClientPlatformAbstract.singleIndexBufferId = GL.GenBuffer();
			GL.BindBuffer((BufferTarget)34963, ClientPlatformAbstract.singleIndexBufferId);
			int[] indicesStd = GC.AllocateUninitializedArray<int>((indicesSize + 23) / 24 * 6);
			for (int i = 0; i < indicesStd.Length; i += 6)
			{
				int vtx = (indicesStd[i] = i / 6 * 4);
				indicesStd[i + 1] = vtx + 1;
				indicesStd[i + 2] = vtx + 2;
				indicesStd[i + 3] = vtx;
				indicesStd[i + 4] = vtx + 2;
				indicesStd[i + 5] = vtx + 3;
			}
			GL.BufferData<int>((BufferTarget)34963, indicesSize, indicesStd, (BufferUsageHint)35044);
			GL.BindBuffer((BufferTarget)34963, 0);
		}
		for (int j = 0; j < vaoSlotNumber; j++)
		{
			GL.EnableVertexAttribArray(j);
		}
		GL.BindVertexArray(0);
		CheckGlError("Failed loading model");
		vao.Persistent = false;
		vao.VaoId = vaoId;
		vao.IndicesCount = indicesSize;
		vao.vaoSlotNumber = vaoSlotNumber;
		vao.vboIdIndex = ClientPlatformAbstract.singleIndexBufferId;
		vao.normalsVboId = 0;
		vao.xyzVboId = xyzVboId;
		vao.uvVboId = 0;
		vao.rgbaVboId = rgbaVboId;
		vao.flagsVboId = 0;
		vao.drawMode = DrawModeToPrimiteType(drawMode);
		return vao;
	}

	private PrimitiveType DrawModeToPrimiteType(EnumDrawMode drawmode)
	{
		return (PrimitiveType)(drawmode switch
		{
			EnumDrawMode.Lines => 1, 
			EnumDrawMode.LineStrip => 3, 
			_ => 4, 
		});
	}

	public override bool LoadMouseCursor(string cursorCoode, int hotx, int hoty, BitmapRef bmpRef)
	{
		try
		{
			SKBitmap bmp = ((BitmapExternal)bmpRef).bmp;
			if (bmp.Width > 32 || bmp.Height > 32)
			{
				return false;
			}
			float gUIScale = ClientSettings.GUIScale;
			if (gUIScale != 1f)
			{
				bmp = bmp.Resize(new SKImageInfo((int)((float)bmp.Width * gUIScale), (int)((float)bmp.Height * gUIScale)), new SKSamplingOptions(SKCubicResampler.Mitchell));
			}
			int i = 0;
			byte[] data = new byte[bmp.BytesPerPixel * bmp.Width * bmp.Height];
			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					SKColor color = bmp.GetPixel(x, y);
					data[i] = color.Red;
					data[i + 1] = color.Green;
					data[i + 2] = color.Blue;
					data[i + 3] = color.Alpha;
					i += 4;
				}
			}
			preLoadedCursors[cursorCoode] = new MouseCursor(hotx, hoty, bmp.Width, bmp.Height, data);
			bmp.Dispose();
		}
		catch (Exception e)
		{
			Logger.Error("Failed loading mouse cursor {0}:", cursorCoode);
			Logger.Error(e);
			RestoreWindowCursor();
			return false;
		}
		return true;
	}

	public override void UseMouseCursor(string cursorCode, bool forceUpdate = false)
	{
		if ((cursorCode == null || cursorCode == CurrentMouseCursor) && !forceUpdate)
		{
			return;
		}
		try
		{
			window.Cursor = preLoadedCursors[cursorCode];
			CurrentMouseCursor = cursorCode;
		}
		catch
		{
			RestoreWindowCursor();
		}
	}

	public override void RestoreWindowCursor()
	{
		window.Cursor = MouseCursor.Default;
	}

	private void UpdateMousePosition()
	{
		if (!window.IsFocused || window.MouseState.Position == previousMousePosition)
		{
			return;
		}
		float xdelta = window.MouseState.Position.X - previousMousePosition.X;
		float ydelta = window.MouseState.Position.Y - previousMousePosition.Y;
		if (previousCursorState == CursorState.Normal && window.CursorState == CursorState.Grabbed)
		{
			ignoreGrabbedMouseMoveEvents = 2;
		}
		if (ignoreGrabbedMouseMoveEvents > 0)
		{
			xdelta = 0f;
			ydelta = 0f;
			ignoreGrabbedMouseMoveEvents--;
		}
		if (window.CursorState == CursorState.Grabbed)
		{
			ignoreNormalMouseMoveEvent = true;
			SetMousePosition((float)window.ClientSize.X / 2f, (float)window.ClientSize.Y / 2f);
		}
		else
		{
			ignoreNormalMouseMoveEvent = false;
		}
		previousMousePosition = window.MouseState.Position;
		previousCursorState = window.CursorState;
		foreach (MouseEventHandler mouseEventHandler in mouseEventHandlers)
		{
			MouseEvent args = new MouseEvent((int)mouseX, (int)mouseY, (int)xdelta, (int)ydelta);
			mouseEventHandler.OnMouseMove(args);
		}
	}

	private void Mouse_Move(MouseMoveEventArgs e)
	{
		if (!ignoreNormalMouseMoveEvent)
		{
			SetMousePosition(e.X, e.Y);
		}
	}

	private void SetMousePosition(float x, float y)
	{
		mouseX = x;
		mouseY = y;
		if (RuntimeEnv.OS == OS.Mac)
		{
			mouseY += ClientSettings.WeirdMacOSMouseYOffset;
		}
	}

	private void Mouse_WheelChanged(OpenTK.Windowing.Common.MouseWheelEventArgs e)
	{
		foreach (MouseEventHandler mouseEventHandler in mouseEventHandlers)
		{
			float delta = e.OffsetY * ClientSettings.MouseWheelSensivity;
			if (RuntimeEnv.OS == OS.Mac)
			{
				delta = GameMath.Clamp(delta, -1f, 1f);
			}
			prevWheelValue += delta;
			Vintagestory.API.Client.MouseWheelEventArgs e2 = new Vintagestory.API.Client.MouseWheelEventArgs
			{
				delta = (int)delta,
				deltaPrecise = (int)delta,
				value = (int)prevWheelValue,
				valuePrecise = prevWheelValue
			};
			mouseEventHandler.OnMouseWheel(e2);
		}
	}

	private void Mouse_ButtonDown(MouseButtonEventArgs e)
	{
		EnumMouseButton enumMouseButton = MouseButtonConverter.ToEnumMouseButton(e.Button);
		foreach (MouseEventHandler mouseEventHandler in mouseEventHandlers)
		{
			MouseEvent args = new MouseEvent((int)mouseX, (int)mouseY, enumMouseButton, (int)e.Modifiers);
			mouseEventHandler.OnMouseDown(args);
		}
	}

	private void Mouse_ButtonUp(MouseButtonEventArgs e)
	{
		EnumMouseButton enumMouseButton = MouseButtonConverter.ToEnumMouseButton(e.Button);
		foreach (MouseEventHandler mouseEventHandler in mouseEventHandlers)
		{
			MouseEvent e2 = new MouseEvent((int)mouseX, (int)mouseY, enumMouseButton, (int)e.Modifiers);
			mouseEventHandler.OnMouseUp(e2);
		}
	}

	private void game_KeyPress(TextInputEventArgs e)
	{
		foreach (KeyEventHandler keyEventHandler in keyEventHandlers)
		{
			keyEventHandler.OnKeyPress(new KeyEvent
			{
				KeyCode = e.Unicode,
				KeyChar = (char)e.Unicode
			});
		}
	}

	private void game_KeyDown(KeyboardKeyEventArgs e)
	{
		if (e.Key == Keys.Unknown)
		{
			return;
		}
		int key = KeyConverter.NewKeysToGlKeys[(int)e.Key];
		foreach (KeyEventHandler keyEventHandler in keyEventHandlers)
		{
			KeyEvent args = new KeyEvent
			{
				KeyCode = key
			};
			if (EllapsedMs - lastKeyUpMs <= 200)
			{
				args.KeyCode2 = lastKeyUpKey;
			}
			args.CommandPressed = e.Command;
			args.CtrlPressed = e.Control;
			args.ShiftPressed = e.Shift;
			args.AltPressed = e.Alt;
			keyEventHandler.OnKeyDown(args);
		}
	}

	private void game_KeyUp(KeyboardKeyEventArgs e)
	{
		if (e.Key == Keys.Unknown)
		{
			return;
		}
		int key = KeyConverter.NewKeysToGlKeys[(int)e.Key];
		lastKeyUpMs = EllapsedMs;
		lastKeyUpKey = key;
		foreach (KeyEventHandler keyEventHandler in keyEventHandlers)
		{
			KeyEvent args = new KeyEvent
			{
				KeyCode = key
			};
			args.CommandPressed = e.Command;
			args.CtrlPressed = e.Control;
			args.ShiftPressed = e.Shift;
			args.AltPressed = e.Alt;
			keyEventHandler.OnKeyUp(args);
		}
	}

	public override MouseEvent CreateMouseEvent(EnumMouseButton button)
	{
		return new MouseEvent((int)mouseX, (int)mouseY, button, 0);
	}

	public override int GetUniformLocation(ShaderProgram program, string name)
	{
		return GL.GetUniformLocation(program.ProgramId, name);
	}

	public override bool CompileShader(Shader shader)
	{
		int shaderId = (shader.ShaderId = GL.CreateShader((ShaderType)shader.shaderType));
		string shaderCode = shader.Code;
		if (shaderCode != null)
		{
			if (shaderCode.IndexOfOrdinal("#version") == -1)
			{
				logger.Warning("Shader {0}: Is not defining a shader version via #version", shader.Filename);
			}
			if (RuntimeEnv.OS == OS.Mac)
			{
				shaderCode = Regex.Replace(shaderCode, "#version \\d+", "#version 330");
			}
			else if (ScreenManager.Platform.UseSSBOs && shader.UsesSSBOs())
			{
				shaderCode = Regex.Replace(shaderCode, "#version \\d+", "#version 430");
			}
			int startIndex = shaderCode.IndexOf('\n', Math.Max(0, shaderCode.IndexOfOrdinal("#version"))) + 1;
			shaderCode = shaderCode.Insert(startIndex, shader.PrefixCode);
		}
		GL.ShaderSource(shaderId, shaderCode);
		GL.CompileShader(shaderId);
		int outval = default(int);
		GL.GetShader(shaderId, (ShaderParameter)35713, ref outval);
		if (outval != 1)
		{
			string logText = GL.GetShaderInfoLog(shaderId);
			logger.Error("Shader compile error in {0} {1}", shader.Filename, logText.TrimEnd());
			logger.VerboseDebug("{0}", shaderCode);
			return false;
		}
		return true;
	}

	public override bool CreateShaderProgram(ShaderProgram program)
	{
		bool ok = true;
		int programId = (program.ProgramId = GL.CreateProgram());
		GL.AttachShader(programId, program.VertexShader.ShaderId);
		GL.AttachShader(programId, program.FragmentShader.ShaderId);
		if (program.GeometryShader != null)
		{
			GL.AttachShader(programId, program.GeometryShader.ShaderId);
		}
		foreach (KeyValuePair<int, string> val in program.attributes)
		{
			GL.BindAttribLocation(program.ProgramId, val.Key, val.Value);
		}
		GL.LinkProgram(programId);
		int outval = default(int);
		GL.GetProgram(programId, (GetProgramParameterName)35714, ref outval);
		string logText = GL.GetProgramInfoLog(programId);
		if (outval != 1)
		{
			logger.Error("Link error in shader program for pass {0}: {1}", program.PassName, logText.TrimEnd());
			ok = false;
		}
		else
		{
			logger.Notification("Loaded Shaderprogramm for render pass {0}.", program.PassName);
		}
		return ok;
	}
}
