using System;
using System.Collections.Generic;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000037 RID: 55
	public sealed class RadioProgramMicBuffer
	{
		// Token: 0x06000157 RID: 343 RVA: 0x00009E64 File Offset: 0x00008064
		public void EnqueueSamples(IReadOnlyList<short> pcm)
		{
			if (pcm == null || pcm.Count == 0)
			{
				return;
			}
			object obj = this.sync;
			lock (obj)
			{
				for (int i = 0; i < pcm.Count; i++)
				{
					this.samples.Enqueue(pcm[i]);
				}
				while (this.samples.Count > 96000)
				{
					this.samples.Dequeue();
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00009EF0 File Offset: 0x000080F0
		public unsafe void ReadFrame(int frameSamples, Span<short> destination)
		{
			destination.Clear();
			if (frameSamples <= 0)
			{
				return;
			}
			object obj = this.sync;
			lock (obj)
			{
				int toRead = Math.Min(frameSamples, this.samples.Count);
				for (int i = 0; i < toRead; i++)
				{
					*destination[i] = this.samples.Dequeue();
				}
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009F68 File Offset: 0x00008168
		public void Clear()
		{
			object obj = this.sync;
			lock (obj)
			{
				this.samples.Clear();
			}
		}

		// Token: 0x04000089 RID: 137
		private readonly Queue<short> samples = new Queue<short>();

		// Token: 0x0400008A RID: 138
		private readonly object sync = new object();
	}
}
