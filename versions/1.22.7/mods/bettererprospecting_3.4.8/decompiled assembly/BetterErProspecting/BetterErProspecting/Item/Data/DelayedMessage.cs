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
		// Token: 0x0600006F RID: 111 RVA: 0x00005D23 File Offset: 0x00003F23
		internal DelayedMessage(string message, int chatGroup, params object[] args)
		{
			this.chatGroup = chatGroup;
			this.message = message;
			this.args = args;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005D56 File Offset: 0x00003F56
		internal DelayedMessage(string message, params object[] args)
		{
			this.message = message;
			this.args = args;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005D82 File Offset: 0x00003F82
		internal DelayedMessage(string message)
		{
			this.message = message;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public void Send(IServerPlayer sp)
		{
			if (this.args == null)
			{
				this.args = Array.Empty<object>();
			}
			if (this.externalMod)
			{
				this.message = "!:" + this.message;
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

		// Token: 0x04000026 RID: 38
		public bool externalMod;
	}
}
