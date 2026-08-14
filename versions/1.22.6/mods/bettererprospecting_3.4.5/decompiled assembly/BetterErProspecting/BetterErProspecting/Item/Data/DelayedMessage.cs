using System;
using System.Runtime.CompilerServices;
using BetterErProspecting.Extensions;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace BetterErProspecting.Item.Data
{
	// Token: 0x02000011 RID: 17
	[NullableContext(1)]
	[Nullable(0)]
	public class DelayedMessage
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00005D1F File Offset: 0x00003F1F
		internal DelayedMessage(string message, int chatGroup, params object[] args)
		{
			this.chatGroup = chatGroup;
			this.message = message;
			this.args = args;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005D52 File Offset: 0x00003F52
		internal DelayedMessage(string message, params object[] args)
		{
			this.message = message;
			this.args = args;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005D7E File Offset: 0x00003F7E
		internal DelayedMessage(string message)
		{
			this.message = message;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005DA3 File Offset: 0x00003FA3
		public void Send(IServerPlayer sp)
		{
			if (this.args == null)
			{
				this.args = Array.Empty<object>();
			}
			sp.Info(this.message, this.chatGroup, this.args);
		}

		// Token: 0x04000023 RID: 35
		private int chatGroup = GlobalConstants.InfoLogChatGroup;

		// Token: 0x04000024 RID: 36
		private string message;

		// Token: 0x04000025 RID: 37
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private object[] args = Array.Empty<object>();
	}
}
