using System;
using System.Threading;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000058 RID: 88
	public abstract class TCPNetworkBase : IDisposable
	{
		// Token: 0x0600025F RID: 607 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
		public TCPNetworkBase(Logger logger)
		{
			this.logger = logger;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000DD03 File Offset: 0x0000BF03
		public string GetTransportID()
		{
			return "CustomTCP";
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000DD0A File Offset: 0x0000BF0A
		public virtual void Dispose()
		{
			CancellationTokenSource readinessProbeCTS = this._readinessProbeCTS;
			if (readinessProbeCTS != null)
			{
				readinessProbeCTS.Cancel();
			}
			CancellationTokenSource readinessProbeCTS2 = this._readinessProbeCTS;
			if (readinessProbeCTS2 == null)
			{
				return;
			}
			readinessProbeCTS2.Dispose();
		}

		// Token: 0x04000108 RID: 264
		protected Logger logger;

		// Token: 0x04000109 RID: 265
		protected CancellationTokenSource _readinessProbeCTS;

		// Token: 0x0400010A RID: 266
		protected int port;

		// Token: 0x0400010B RID: 267
		protected const string _transportID = "CustomTCP";

		// Token: 0x0400010C RID: 268
		protected bool isReady;
	}
}
