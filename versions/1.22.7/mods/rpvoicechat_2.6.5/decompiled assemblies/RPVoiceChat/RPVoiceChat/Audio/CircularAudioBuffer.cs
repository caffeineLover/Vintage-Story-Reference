using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000104 RID: 260
	public class CircularAudioBuffer : IDisposable
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000988 RID: 2440 RVA: 0x00034A08 File Offset: 0x00032C08
		// (remove) Token: 0x06000989 RID: 2441 RVA: 0x00034A40 File Offset: 0x00032C40
		public event Action OnEmptyingQueue;

		// Token: 0x0600098A RID: 2442 RVA: 0x00034A78 File Offset: 0x00032C78
		public CircularAudioBuffer(int source, int bufferCount)
		{
			this.source = source;
			this.buffers = OALW.GenBuffers(bufferCount);
			this.availableBuffers.AddRange(this.buffers);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00034ADB File Offset: 0x00032CDB
		public void QueueAudio(byte[] audio, ALFormat format, int frequency)
		{
			if (!this.TryQueueAudio(audio, format, frequency))
			{
				Logger.client.Debug("CircularAudioBuffer had to skip queuing audio");
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00034AF8 File Offset: 0x00032CF8
		public bool TryQueueAudio(byte[] audio, ALFormat format, int frequency)
		{
			this.FreeProcessedBuffers();
			if (this.availableBuffers.Count == 0)
			{
				return false;
			}
			object obj = this.buffer_queue_lock;
			bool result;
			lock (obj)
			{
				if (this.availableBuffers.Count == 0)
				{
					result = false;
				}
				else
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
						result = false;
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
							result = false;
						}
						else
						{
							this.queuedBuffers.Add(currentBuffer);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00034C38 File Offset: 0x00032E38
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

		// Token: 0x0600098E RID: 2446 RVA: 0x00034CB8 File Offset: 0x00032EB8
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

		// Token: 0x0600098F RID: 2447 RVA: 0x00034D6C File Offset: 0x00032F6C
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

		// Token: 0x04000437 RID: 1079
		private List<int> availableBuffers = new List<int>();

		// Token: 0x04000438 RID: 1080
		private List<int> queuedBuffers = new List<int>();

		// Token: 0x04000439 RID: 1081
		private int[] buffers;

		// Token: 0x0400043A RID: 1082
		private int source;

		// Token: 0x0400043B RID: 1083
		private ALSourceState previousSourceState = 4113;

		// Token: 0x0400043C RID: 1084
		private object buffer_queue_lock = new object();
	}
}
