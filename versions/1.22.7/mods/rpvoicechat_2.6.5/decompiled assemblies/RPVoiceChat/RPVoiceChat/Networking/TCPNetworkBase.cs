using System;
using System.Threading;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000075 RID: 117
	public abstract class TCPNetworkBase : IDisposable
	{
		// Token: 0x060002FA RID: 762 RVA: 0x000117C0 File Offset: 0x0000F9C0
		public TCPNetworkBase(Logger logger)
		{
			this.logger = logger;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000117CF File Offset: 0x0000F9CF
		public string GetTransportID()
		{
			return "CustomTCP";
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000117D6 File Offset: 0x0000F9D6
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

		// Token: 0x0400015A RID: 346
		protected Logger logger;

		// Token: 0x0400015B RID: 347
		protected CancellationTokenSource _readinessProbeCTS;

		// Token: 0x0400015C RID: 348
		protected int port;

		// Token: 0x0400015D RID: 349
		protected const string _transportID = "CustomTCP";

		// Token: 0x0400015E RID: 350
		protected bool isReady;
	}
}
