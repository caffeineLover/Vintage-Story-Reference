using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Xabe.FFmpeg.Events;
using Xabe.FFmpeg.Exceptions;

namespace Xabe.FFmpeg
{
	// Token: 0x02000011 RID: 17
	internal class FFmpegWrapper : FFmpeg
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600005D RID: 93 RVA: 0x00003E74 File Offset: 0x00002074
		// (remove) Token: 0x0600005E RID: 94 RVA: 0x00003EAC File Offset: 0x000020AC
		internal event ConversionProgressEventHandler OnProgress;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600005F RID: 95 RVA: 0x00003EE4 File Offset: 0x000020E4
		// (remove) Token: 0x06000060 RID: 96 RVA: 0x00003F1C File Offset: 0x0000211C
		internal event DataReceivedEventHandler OnDataReceived;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000061 RID: 97 RVA: 0x00003F54 File Offset: 0x00002154
		// (remove) Token: 0x06000062 RID: 98 RVA: 0x00003F8C File Offset: 0x0000218C
		internal event VideoDataEventHandler OnVideoDataReceived;

		// Token: 0x06000063 RID: 99 RVA: 0x00003FC4 File Offset: 0x000021C4
		internal Task<bool> RunProcess(string args, CancellationToken cancellationToken, ProcessPriorityClass? priority)
		{
			FFmpegWrapper.<>c__DisplayClass14_0 CS$<>8__locals1 = new FFmpegWrapper.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.args = args;
			CS$<>8__locals1.priority = priority;
			CS$<>8__locals1.cancellationToken = cancellationToken;
			return Task.Factory.StartNew<bool>(delegate()
			{
				FFmpegWrapper.<>c__DisplayClass14_1 CS$<>8__locals2 = new FFmpegWrapper.<>c__DisplayClass14_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals1.<>4__this._outputLog = new List<string>();
				bool flag = CS$<>8__locals1.<>4__this.OnVideoDataReceived != null;
				CS$<>8__locals2.process = CS$<>8__locals1.<>4__this.RunProcess(CS$<>8__locals1.args, CS$<>8__locals1.<>4__this.FFmpegPath, CS$<>8__locals1.priority, true, flag, true);
				CS$<>8__locals2.processId = CS$<>8__locals2.process.Id;
				using (CS$<>8__locals2.process)
				{
					CS$<>8__locals2.process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
					{
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.ProcessOutputData(e, CS$<>8__locals2.CS$<>8__locals1.args, CS$<>8__locals2.processId);
					};
					CS$<>8__locals2.process.BeginErrorReadLine();
					if (flag)
					{
						Task.Run(delegate()
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.ProcessVideoData(CS$<>8__locals2.process, CS$<>8__locals2.CS$<>8__locals1.cancellationToken);
						}, CS$<>8__locals1.cancellationToken);
					}
					using (CS$<>8__locals1.cancellationToken.Register(delegate()
					{
						FFmpegWrapper.<>c__DisplayClass14_1.<<RunProcess>b__3>d <<RunProcess>b__3>d;
						<<RunProcess>b__3>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<RunProcess>b__3>d.<>4__this = CS$<>8__locals2;
						<<RunProcess>b__3>d.<>1__state = -1;
						<<RunProcess>b__3>d.<>t__builder.Start<FFmpegWrapper.<>c__DisplayClass14_1.<<RunProcess>b__3>d>(ref <<RunProcess>b__3>d);
					}))
					{
						using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
						{
							WaitHandleExtensions.SetSafeWaitHandle(manualResetEvent, new SafeWaitHandle(CS$<>8__locals2.process.Handle, false));
							int num = WaitHandle.WaitAny(new WaitHandle[]
							{
								manualResetEvent,
								CS$<>8__locals1.cancellationToken.WaitHandle
							});
							if (num == 1 && !CS$<>8__locals2.process.HasExited)
							{
								CS$<>8__locals2.process.CloseMainWindow();
								CS$<>8__locals2.process.Kill();
								CS$<>8__locals1.<>4__this._wasKilled = true;
							}
							else if (num == 0 && !CS$<>8__locals2.process.HasExited)
							{
								CS$<>8__locals2.process.WaitForExit();
							}
						}
						CS$<>8__locals1.cancellationToken.ThrowIfCancellationRequested();
						if (CS$<>8__locals1.<>4__this._wasKilled)
						{
							throw new ConversionException("Cannot stop process. Killed it.", CS$<>8__locals1.args);
						}
						if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						string text = string.Join(Environment.NewLine, CS$<>8__locals1.<>4__this._outputLog.ToArray());
						new FFmpegExceptionCatcher().CatchFFmpegErrors(text, CS$<>8__locals1.args);
						if (CS$<>8__locals2.process.ExitCode != 0 && Enumerable.Any<string>(CS$<>8__locals1.<>4__this._outputLog) && !Enumerable.Last<string>(CS$<>8__locals1.<>4__this._outputLog).Contains("dummy"))
						{
							throw new ConversionException(text, CS$<>8__locals1.args);
						}
					}
				}
				return true;
			}, CS$<>8__locals1.cancellationToken, 2, TaskScheduler.Default);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004018 File Offset: 0x00002218
		private void ProcessOutputData(DataReceivedEventArgs e, string args, int processId)
		{
			if (e.Data == null)
			{
				return;
			}
			DataReceivedEventHandler onDataReceived = this.OnDataReceived;
			if (onDataReceived != null)
			{
				onDataReceived.Invoke(this, e);
			}
			this._outputLog.Add(e.Data);
			if (this.OnProgress == null)
			{
				return;
			}
			this.CalculateTime(e, args, processId);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004064 File Offset: 0x00002264
		private void ProcessVideoData(Process process, CancellationToken cancellationToken)
		{
			BinaryReader binaryReader = new BinaryReader(process.StandardOutput.BaseStream);
			byte[] data;
			while ((data = binaryReader.ReadBytes(4096)).Length != 0)
			{
				VideoDataEventArgs args = new VideoDataEventArgs(data);
				VideoDataEventHandler onVideoDataReceived = this.OnVideoDataReceived;
				if (onVideoDataReceived != null)
				{
					onVideoDataReceived(this, args);
				}
				cancellationToken.ThrowIfCancellationRequested();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000040B8 File Offset: 0x000022B8
		private void CalculateTime(DataReceivedEventArgs e, string args, int processId)
		{
			if (e.Data.Contains("Duration: N/A"))
			{
				return;
			}
			if (e.Data.Contains("Duration"))
			{
				this.GetDuration(e, FFmpegWrapper._timeFormatRegex, args);
				return;
			}
			if (e.Data.Contains("size"))
			{
				Match match = FFmpegWrapper._timeFormatRegex.Match(e.Data);
				TimeSpan timeSpanValue = this.GetTimeSpanValue(match);
				if (timeSpanValue.TotalMilliseconds > 0.0)
				{
					this.OnProgress(this, new ConversionProgressEventArgs(timeSpanValue, this._totalTime, processId));
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004150 File Offset: 0x00002350
		private void GetDuration(DataReceivedEventArgs e, Regex regex, string args)
		{
			string argumentValue = this.GetArgumentValue("-t", args);
			if (!string.IsNullOrWhiteSpace(argumentValue) && argumentValue != "1")
			{
				this._totalTime = TimeSpan.Parse(argumentValue);
				return;
			}
			Match match = regex.Match(e.Data);
			if (!match.Success)
			{
				return;
			}
			this._totalTime = this._totalTime.Add(TimeSpan.Parse(match.Value));
			string argumentValue2 = this.GetArgumentValue("-ss", args);
			if (!string.IsNullOrWhiteSpace(argumentValue2))
			{
				this._totalTime -= TimeSpan.Parse(argumentValue2);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000041EC File Offset: 0x000023EC
		private string GetArgumentValue(string option, string args)
		{
			List<string> list = Enumerable.ToList<string>(args.Split(new char[]
			{
				' '
			}));
			int num = list.IndexOf(option);
			if (num >= 0)
			{
				return list[num + 1];
			}
			return string.Empty;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000422C File Offset: 0x0000242C
		private TimeSpan GetTimeSpanValue(Match match)
		{
			if (!match.Success)
			{
				return new TimeSpan(0, 0, 0);
			}
			TimeSpan result;
			if (TimeSpan.TryParse(match.Value, ref result))
			{
				return result;
			}
			return this.GetTimeSpanValue(match.NextMatch());
		}

		// Token: 0x04000270 RID: 624
		private const string TIME_FORMAT_PATTERN = "\\w\\w:\\w\\w:\\w\\w";

		// Token: 0x04000271 RID: 625
		private static readonly Regex _timeFormatRegex = new Regex("\\w\\w:\\w\\w:\\w\\w", 8);

		// Token: 0x04000272 RID: 626
		private List<string> _outputLog;

		// Token: 0x04000273 RID: 627
		private TimeSpan _totalTime;

		// Token: 0x04000274 RID: 628
		private bool _wasKilled;
	}
}
