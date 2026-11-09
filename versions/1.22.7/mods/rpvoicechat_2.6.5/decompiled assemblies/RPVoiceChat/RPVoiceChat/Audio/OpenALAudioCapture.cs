using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000101 RID: 257
	public class OpenALAudioCapture : IAudioCapture, IDisposable
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00033CB0 File Offset: 0x00031EB0
		public int AvailableSamples
		{
			get
			{
				int result;
				ALC.GetInteger(this._captureDevice, 786, 1, ref result);
				return result;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00033CD1 File Offset: 0x00031ED1
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x00033CD9 File Offset: 0x00031ED9
		public string CurrentDevice { get; private set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00033CE2 File Offset: 0x00031EE2
		public int Frequency { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00033CEA File Offset: 0x00031EEA
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x00033CF2 File Offset: 0x00031EF2
		public ALFormat SampleFormat { get; private set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00033CFB File Offset: 0x00031EFB
		public int BufferSize { get; }

		// Token: 0x06000956 RID: 2390 RVA: 0x00033D04 File Offset: 0x00031F04
		public OpenALAudioCapture(string deviceName, int frequency, ALFormat captureFormat, int bufferSize)
		{
			if (deviceName == "Default")
			{
				deviceName = null;
			}
			this.Frequency = frequency;
			this.BufferSize = bufferSize;
			string[] deviceNames = new string[]
			{
				deviceName,
				default(string),
				OpenALAudioCapture.DefaultDevice
			};
			ALFormat[] formats = new ALFormat[]
			{
				captureFormat,
				4355,
				4353
			};
			this.TryOpenDevice(deviceNames, formats);
			if (this._captureDevice == IntPtr.Zero)
			{
				throw new Exception("All attempts to open capture devices returned IntPtr.Zero");
			}
			if (this.CheckError(this._captureDevice))
			{
				throw new Exception("Capture device returned an exception");
			}
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00033DA2 File Offset: 0x00031FA2
		public void Start()
		{
			ALC.CaptureStart(this._captureDevice);
			this.IsRunning = true;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00033DB6 File Offset: 0x00031FB6
		public void Stop()
		{
			ALC.CaptureStop(this._captureDevice);
			this.IsRunning = false;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00033DCA File Offset: 0x00031FCA
		public void ReadSamples(byte[] buffer, int count)
		{
			ALC.CaptureSamples<byte>(this._captureDevice, buffer, count);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00033DD9 File Offset: 0x00031FD9
		public static List<string> GetAvailableDevices()
		{
			return ALC.GetString(ALDevice.Null, 784) ?? new List<string>();
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00033DF4 File Offset: 0x00031FF4
		private void TryOpenDevice(string[] deviceNames, ALFormat[] formats)
		{
			foreach (string device in deviceNames)
			{
				foreach (ALFormat format in formats)
				{
					if (this.TryOpenDevice(device, format))
					{
						return;
					}
				}
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00033E3C File Offset: 0x0003203C
		private bool TryOpenDevice(string deviceName, ALFormat format)
		{
			this.CurrentDevice = deviceName;
			this.SampleFormat = format;
			this._captureDevice = ALC.CaptureOpenDevice(deviceName, this.Frequency, format, this.BufferSize);
			this.LogError();
			return this._captureDevice != IntPtr.Zero;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00033E8B File Offset: 0x0003208B
		private void LogError()
		{
			this.CheckError(ALCaptureDevice.Null);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00033E9C File Offset: 0x0003209C
		private bool CheckError(ALCaptureDevice device)
		{
			AlcError ALCError = ALC.GetError(new ALDevice(device));
			if (ALCError == null)
			{
				return false;
			}
			Logger client = Logger.client;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(50, 5);
			defaultInterpolatedStringHandler.AppendLiteral("[Internal] Failed to open capture device: ");
			defaultInterpolatedStringHandler.AppendFormatted<AlcError>(ALCError);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(this.CurrentDevice ?? "Default");
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Frequency);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<ALFormat>(this.SampleFormat);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.BufferSize);
			client.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			return true;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00033F5C File Offset: 0x0003215C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00033F6B File Offset: 0x0003216B
		public void Dispose(bool manual)
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this._captureDevice != IntPtr.Zero)
			{
				if (this.IsRunning)
				{
					this.Stop();
				}
				ALC.CaptureCloseDevice(this._captureDevice);
			}
			this.IsDisposed = true;
		}

		// Token: 0x0400042C RID: 1068
		public static string DefaultDevice = ALC.GetString(ALDevice.Null, 785);

		// Token: 0x0400042D RID: 1069
		private ALCaptureDevice _captureDevice;

		// Token: 0x0400042E RID: 1070
		private bool IsDisposed;

		// Token: 0x0400042F RID: 1071
		private bool IsRunning;
	}
}
