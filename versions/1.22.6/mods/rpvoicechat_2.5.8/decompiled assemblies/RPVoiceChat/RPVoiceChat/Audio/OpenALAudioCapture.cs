using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C8 RID: 200
	public class OpenALAudioCapture : IAudioCapture, IDisposable
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00028664 File Offset: 0x00026864
		public int AvailableSamples
		{
			get
			{
				int result;
				ALC.GetInteger(this._captureDevice, 786, 1, ref result);
				return result;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00028685 File Offset: 0x00026885
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0002868D File Offset: 0x0002688D
		public string CurrentDevice { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00028696 File Offset: 0x00026896
		public int Frequency { get; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0002869E File Offset: 0x0002689E
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x000286A6 File Offset: 0x000268A6
		public ALFormat SampleFormat { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x000286AF File Offset: 0x000268AF
		public int BufferSize { get; }

		// Token: 0x06000736 RID: 1846 RVA: 0x000286B8 File Offset: 0x000268B8
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

		// Token: 0x06000737 RID: 1847 RVA: 0x00028756 File Offset: 0x00026956
		public void Start()
		{
			ALC.CaptureStart(this._captureDevice);
			this.IsRunning = true;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002876A File Offset: 0x0002696A
		public void Stop()
		{
			ALC.CaptureStop(this._captureDevice);
			this.IsRunning = false;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002877E File Offset: 0x0002697E
		public void ReadSamples(byte[] buffer, int count)
		{
			ALC.CaptureSamples<byte>(this._captureDevice, buffer, count);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002878D File Offset: 0x0002698D
		public static List<string> GetAvailableDevices()
		{
			return ALC.GetString(ALDevice.Null, 784);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000287A0 File Offset: 0x000269A0
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

		// Token: 0x0600073C RID: 1852 RVA: 0x000287E8 File Offset: 0x000269E8
		private bool TryOpenDevice(string deviceName, ALFormat format)
		{
			this.CurrentDevice = deviceName;
			this.SampleFormat = format;
			this._captureDevice = ALC.CaptureOpenDevice(deviceName, this.Frequency, format, this.BufferSize);
			this.LogError();
			return this._captureDevice != IntPtr.Zero;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00028837 File Offset: 0x00026A37
		private void LogError()
		{
			this.CheckError(ALCaptureDevice.Null);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00028848 File Offset: 0x00026A48
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

		// Token: 0x0600073F RID: 1855 RVA: 0x00028908 File Offset: 0x00026B08
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00028917 File Offset: 0x00026B17
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

		// Token: 0x0400034B RID: 843
		public static string DefaultDevice = ALC.GetString(ALDevice.Null, 785);

		// Token: 0x0400034C RID: 844
		private ALCaptureDevice _captureDevice;

		// Token: 0x0400034D RID: 845
		private bool IsDisposed;

		// Token: 0x0400034E RID: 846
		private bool IsRunning;
	}
}
