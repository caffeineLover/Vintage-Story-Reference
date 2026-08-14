using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000CB RID: 203
	public class CircularAudioBuffer : IDisposable
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000765 RID: 1893 RVA: 0x000291B4 File Offset: 0x000273B4
		// (remove) Token: 0x06000766 RID: 1894 RVA: 0x000291EC File Offset: 0x000273EC
		public event Action OnEmptyingQueue;

		// Token: 0x06000767 RID: 1895 RVA: 0x00029224 File Offset: 0x00027424
		public CircularAudioBuffer(int source, int bufferCount)
		{
			this.source = source;
			this.buffers = OALW.GenBuffers(bufferCount);
			this.availableBuffers.AddRange(this.buffers);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00029288 File Offset: 0x00027488
		public void QueueAudio(byte[] audio, ALFormat format, int frequency)
		{
			this.FreeProcessedBuffers();
			if (this.availableBuffers.Count == 0)
			{
				Logger.client.Debug("CircularAudioBuffer had to skip queuing audio");
				return;
			}
			object obj = this.buffer_queue_lock;
			lock (obj)
			{
				int currentBuffer = this.availableBuffers[0];
				this.availableBuffers.RemoveAt(0);
				OALW.ClearError();
				OALW.BufferData(currentBuffer, format, audio, frequency);
				ALError bufferError = AL.GetError();
				if (bufferError != null)
				{
					Logger client = Logger.client;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("OpenAL error while setting buffer data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ALError>(bufferError);
					client.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					this.availableBuffers.Add(currentBuffer);
				}
				else
				{
					OALW.SourceQueueBuffer(this.source, currentBuffer);
					ALError queueError = AL.GetError();
					if (queueError != null)
					{
						Logger client2 = Logger.client;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("OpenAL error while queuing buffer: ");
						defaultInterpolatedStringHandler2.AppendFormatted<ALError>(queueError);
						client2.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
						this.availableBuffers.Add(currentBuffer);
					}
					else
					{
						this.queuedBuffers.Add(currentBuffer);
					}
				}
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000293B4 File Offset: 0x000275B4
		private void FreeProcessedBuffers()
		{
			object obj = this.buffer_queue_lock;
			bool sourceHasStopped;
			lock (obj)
			{
				ALSourceState sourceState = OALW.GetSourceState(this.source);
				sourceHasStopped = (sourceState == 4116);
				if (sourceHasStopped && sourceState == this.previousSourceState)
				{
					return;
				}
				this.previousSourceState = sourceState;
				this.TryDequeueBuffers();
			}
			if (sourceHasStopped)
			{
				Action onEmptyingQueue = this.OnEmptyingQueue;
				if (onEmptyingQueue == null)
				{
					return;
				}
				onEmptyingQueue.Invoke();
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00029434 File Offset: 0x00027634
		private void TryDequeueBuffers()
		{
			if (this.queuedBuffers.Count == 0)
			{
				return;
			}
			OALW.ClearError();
			int buffersProcessed;
			OALW.GetSource(this.source, 4118, out buffersProcessed);
			if (buffersProcessed == 0)
			{
				return;
			}
			int i = 0;
			while (i < buffersProcessed && this.queuedBuffers.Count > 0)
			{
				int buffer = OALW.SourceUnqueueBuffer(this.source);
				if (buffer == 0)
				{
					break;
				}
				ALError error = AL.GetError();
				if (error != null)
				{
					Logger client = Logger.client;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(37, 1);
					defaultInterpolatedStringHandler.AppendLiteral("OpenAL error while dequeuing buffer: ");
					defaultInterpolatedStringHandler.AppendFormatted<ALError>(error);
					client.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
				this.queuedBuffers.Remove(buffer);
				this.availableBuffers.Add(buffer);
				i++;
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000294E8 File Offset: 0x000276E8
		public void Dispose()
		{
			object obj = this.buffer_queue_lock;
			lock (obj)
			{
				try
				{
					OALW.SourceStop(this.source);
					while (this.queuedBuffers.Count > 0)
					{
						int buffer = OALW.SourceUnqueueBuffer(this.source);
						if (buffer == 0)
						{
							break;
						}
						this.queuedBuffers.Remove(buffer);
					}
				}
				catch (Exception e)
				{
					Logger.client.Warning("Error cleaning up audio buffers: " + e.Message);
				}
				OALW.DeleteBuffers(this.buffers);
			}
		}

		// Token: 0x04000356 RID: 854
		private List<int> availableBuffers = new List<int>();

		// Token: 0x04000357 RID: 855
		private List<int> queuedBuffers = new List<int>();

		// Token: 0x04000358 RID: 856
		private int[] buffers;

		// Token: 0x04000359 RID: 857
		private int source;

		// Token: 0x0400035A RID: 858
		private ALSourceState previousSourceState = 4113;

		// Token: 0x0400035B RID: 859
		private object buffer_queue_lock = new object();
	}
}
