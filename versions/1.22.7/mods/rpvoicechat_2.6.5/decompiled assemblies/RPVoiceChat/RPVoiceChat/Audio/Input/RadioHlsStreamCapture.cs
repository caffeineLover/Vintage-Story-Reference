using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio.Input
{
	// Token: 0x02000109 RID: 265
	public sealed class RadioHlsStreamCapture : IDisposable
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x000366CC File Offset: 0x000348CC
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x000366D4 File Offset: 0x000348D4
		public bool IsRunning { get; private set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x000366DD File Offset: 0x000348DD
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x000366E5 File Offset: 0x000348E5
		public string LastError { get; private set; } = "";

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060009D8 RID: 2520 RVA: 0x000366F0 File Offset: 0x000348F0
		// (remove) Token: 0x060009D9 RID: 2521 RVA: 0x00036728 File Offset: 0x00034928
		public event Action<short[]> OnPcmFrame;

		// Token: 0x060009DA RID: 2522 RVA: 0x00036760 File Offset: 0x00034960
		public bool TryStart(string streamUrl)
		{
			this.Stop();
			if (string.IsNullOrWhiteSpace(streamUrl))
			{
				this.LastError = "Empty HLS URL.";
				return false;
			}
			Uri uri;
			if (!Uri.TryCreate(streamUrl.Trim(), 1, ref uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
			{
				this.LastError = "HLS URL must start with http:// or https://.";
				return false;
			}
			if (!FfmpegLocator.IsAvailable())
			{
				FfmpegLocator.TryEnsureAvailable(TimeSpan.FromMinutes(2L));
			}
			string ffmpegExecutable = FfmpegLocator.ResolveExecutable();
			if (ffmpegExecutable == null)
			{
				this.LastError = FfmpegLocator.GetMissingMessage();
				return false;
			}
			bool result;
			try
			{
				this.cancellation = new CancellationTokenSource();
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					FileName = ffmpegExecutable,
					Arguments = RadioHlsStreamCapture.BuildFfmpegArguments(uri.AbsoluteUri),
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				};
				this.ffmpegProcess = Process.Start(startInfo);
				if (this.ffmpegProcess == null)
				{
					this.LastError = "Failed to start FFmpeg.";
					result = false;
				}
				else
				{
					this.ffmpegProcess.EnableRaisingEvents = true;
					this.ffmpegProcess.ErrorDataReceived += delegate(object _, DataReceivedEventArgs _)
					{
					};
					this.ffmpegProcess.BeginErrorReadLine();
					this.readerThread = new Thread(new ParameterizedThreadStart(this.ReadLoop))
					{
						IsBackground = true,
						Name = "RPVC-HLS",
						Priority = 3
					};
					this.readerThread.Start(this.cancellation.Token);
					this.IsRunning = true;
					this.LastError = "";
					result = true;
				}
			}
			catch (Exception ex)
			{
				this.LastError = ex.Message;
				this.Stop();
				result = false;
			}
			return result;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00036938 File Offset: 0x00034B38
		public void Stop()
		{
			object obj = this.sync;
			lock (obj)
			{
				try
				{
					CancellationTokenSource cancellationTokenSource = this.cancellation;
					if (cancellationTokenSource != null)
					{
						cancellationTokenSource.Cancel();
					}
				}
				catch
				{
				}
				if (this.ffmpegProcess != null)
				{
					try
					{
						if (!this.ffmpegProcess.HasExited)
						{
							this.ffmpegProcess.Kill(true);
						}
					}
					catch
					{
					}
					try
					{
						this.ffmpegProcess.Dispose();
					}
					catch
					{
					}
					this.ffmpegProcess = null;
				}
				try
				{
					Thread thread = this.readerThread;
					if (thread != null)
					{
						thread.Join(500);
					}
				}
				catch
				{
				}
				this.readerThread = null;
				CancellationTokenSource cancellationTokenSource2 = this.cancellation;
				if (cancellationTokenSource2 != null)
				{
					cancellationTokenSource2.Dispose();
				}
				this.cancellation = null;
				this.IsRunning = false;
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00036A3C File Offset: 0x00034C3C
		public void Dispose()
		{
			this.Stop();
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00036A44 File Offset: 0x00034C44
		private void ReadLoop(object tokenObj)
		{
			if (tokenObj is CancellationToken)
			{
				CancellationToken token = (CancellationToken)tokenObj;
				Process process = this.ffmpegProcess;
				bool flag;
				if (process == null)
				{
					flag = (null != null);
				}
				else
				{
					StreamReader standardOutput = process.StandardOutput;
					flag = (((standardOutput != null) ? standardOutput.BaseStream : null) != null);
				}
				if (flag)
				{
					byte[] frameBuffer = new byte[960];
					int frameOffset = 0;
					byte[] readBuffer = new byte[8192];
					int discardedFrames = 0;
					Stopwatch paceClock = null;
					double nextFrameAtMs = 0.0;
					try
					{
						Stream stdout = this.ffmpegProcess.StandardOutput.BaseStream;
						while (!token.IsCancellationRequested && !this.ffmpegProcess.HasExited)
						{
							int read = stdout.Read(readBuffer, 0, readBuffer.Length);
							if (read <= 0)
							{
								break;
							}
							int sourceOffset = 0;
							while (sourceOffset < read)
							{
								if (token.IsCancellationRequested)
								{
									return;
								}
								int copy = Math.Min(960 - frameOffset, read - sourceOffset);
								Buffer.BlockCopy(readBuffer, sourceOffset, frameBuffer, frameOffset, copy);
								frameOffset += copy;
								sourceOffset += copy;
								if (frameOffset >= 960)
								{
									frameOffset = 0;
									if (discardedFrames < 50)
									{
										discardedFrames++;
									}
									else
									{
										if (paceClock == null)
										{
											paceClock = Stopwatch.StartNew();
											nextFrameAtMs = 0.0;
										}
										RadioHlsStreamCapture.PaceToRealtime(paceClock, ref nextFrameAtMs, token);
										if (token.IsCancellationRequested)
										{
											return;
										}
										this.EmitFrame(frameBuffer);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						if (!token.IsCancellationRequested)
						{
							this.LastError = ex.Message;
							Logger server = Logger.server;
							if (server != null)
							{
								server.Warning("[RadioHlsStreamCapture] Stream read failed: " + ex.Message);
							}
						}
					}
					finally
					{
						this.IsRunning = false;
					}
					return;
				}
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00036C00 File Offset: 0x00034E00
		private static void PaceToRealtime(Stopwatch paceClock, ref double nextFrameAtMs, CancellationToken token)
		{
			nextFrameAtMs += 10.0;
			double aheadMs = nextFrameAtMs - paceClock.Elapsed.TotalMilliseconds;
			while (aheadMs > 1.0 && !token.IsCancellationRequested)
			{
				int sleepMs = (int)Math.Min(aheadMs, 25.0);
				try
				{
					Thread.Sleep(sleepMs);
				}
				catch
				{
					return;
				}
				aheadMs = nextFrameAtMs - paceClock.Elapsed.TotalMilliseconds;
			}
			if (aheadMs < -250.0)
			{
				nextFrameAtMs = paceClock.Elapsed.TotalMilliseconds;
				return;
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00036CA0 File Offset: 0x00034EA0
		private void EmitFrame(byte[] frameBuffer)
		{
			short[] samples = new short[480];
			MemoryMarshal.Cast<byte, short>(MemoryExtensions.AsSpan<byte>(frameBuffer, 0, 960)).CopyTo(samples);
			Action<short[]> onPcmFrame = this.OnPcmFrame;
			if (onPcmFrame == null)
			{
				return;
			}
			onPcmFrame.Invoke(samples);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00036CE8 File Offset: 0x00034EE8
		private static string BuildFfmpegArguments(string streamUrl)
		{
			string text = "-hide_banner -loglevel error -reconnect 1 -reconnect_streamed 1 -reconnect_delay_max 5 -fflags nobuffer -flags low_delay -probesize 32k -analyzeduration 0 ";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(35, 3);
			defaultInterpolatedStringHandler.AppendLiteral("-i \"");
			defaultInterpolatedStringHandler.AppendFormatted(streamUrl);
			defaultInterpolatedStringHandler.AppendLiteral("\" -vn -ac ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(1);
			defaultInterpolatedStringHandler.AppendLiteral(" -ar ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(48000);
			defaultInterpolatedStringHandler.AppendLiteral(" -f s16le pipe:1");
			return text + defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400046D RID: 1133
		public const int SampleRate = 48000;

		// Token: 0x0400046E RID: 1134
		public const int Channels = 1;

		// Token: 0x0400046F RID: 1135
		public const int BytesPerSample = 2;

		// Token: 0x04000470 RID: 1136
		public const int FrameSamples = 480;

		// Token: 0x04000471 RID: 1137
		public const int FrameBytes = 960;

		// Token: 0x04000472 RID: 1138
		private const double FrameDurationMs = 10.0;

		// Token: 0x04000473 RID: 1139
		private const int PrerollDiscardFrames = 50;

		// Token: 0x04000474 RID: 1140
		private Process ffmpegProcess;

		// Token: 0x04000475 RID: 1141
		private Thread readerThread;

		// Token: 0x04000476 RID: 1142
		private CancellationTokenSource cancellation;

		// Token: 0x04000477 RID: 1143
		private readonly object sync = new object();
	}
}
