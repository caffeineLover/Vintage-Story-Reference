using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Util
{
	// Token: 0x02000029 RID: 41
	public class Logger
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x0000792C File Offset: 0x00005B2C
		public Logger(ICoreAPI api)
		{
			this.api = api;
			if (api is ICoreServerAPI)
			{
				Logger.server = this;
			}
			if (api is ICoreClientAPI)
			{
				Logger.client = this;
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007978 File Offset: 0x00005B78
		public void Error(string message)
		{
			this.Error(message, new object[0]);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007988 File Offset: 0x00005B88
		public void Error(string message, params object[] args)
		{
			object obj = this.main_file_lock;
			lock (obj)
			{
				ILogger logger = this.api.Logger;
				if (logger != null)
				{
					logger.Error("[RPVoiceChat] " + message, args);
				}
			}
			obj = this.debug_file_lock;
			lock (obj)
			{
				ILogger logger2 = this.api.Logger;
				if (logger2 != null)
				{
					logger2.VerboseDebug("[Error] [RPVoiceChat] " + message, args);
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007A30 File Offset: 0x00005C30
		public void Debug(string message)
		{
			this.Debug(message, new object[0]);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00007A40 File Offset: 0x00005C40
		public void Debug(string message, params object[] args)
		{
			object obj = this.debug_file_lock;
			lock (obj)
			{
				ILogger logger = this.api.Logger;
				if (logger != null)
				{
					logger.Debug("[RPVoiceChat] " + message, args);
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007A9C File Offset: 0x00005C9C
		public void VerboseDebug(string message)
		{
			this.VerboseDebug(message, new object[0]);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007AAC File Offset: 0x00005CAC
		public void VerboseDebug(string message, params object[] args)
		{
			object obj = this.debug_file_lock;
			lock (obj)
			{
				ILogger logger = this.api.Logger;
				if (logger != null)
				{
					logger.VerboseDebug("[RPVoiceChat] " + message, args);
				}
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007B08 File Offset: 0x00005D08
		public void Warning(string message)
		{
			this.Warning(message, new object[0]);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007B18 File Offset: 0x00005D18
		public void Warning(string message, params object[] args)
		{
			object obj = this.main_file_lock;
			lock (obj)
			{
				ILogger logger = this.api.Logger;
				if (logger != null)
				{
					logger.Warning("[RPVoiceChat] " + message, args);
				}
			}
			obj = this.debug_file_lock;
			lock (obj)
			{
				ILogger logger2 = this.api.Logger;
				if (logger2 != null)
				{
					logger2.VerboseDebug("[Warning] [RPVoiceChat] " + message, args);
				}
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public void Notification(string message)
		{
			this.Notification(message, new object[0]);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007BD0 File Offset: 0x00005DD0
		public void Notification(string message, params object[] args)
		{
			object obj = this.main_file_lock;
			lock (obj)
			{
				ILogger logger = this.api.Logger;
				if (logger != null)
				{
					logger.Notification("[RPVoiceChat] " + message, args);
				}
			}
			obj = this.debug_file_lock;
			lock (obj)
			{
				ILogger logger2 = this.api.Logger;
				if (logger2 != null)
				{
					logger2.VerboseDebug("[Notification] [RPVoiceChat] " + message, args);
				}
			}
		}

		// Token: 0x0400005F RID: 95
		public ICoreAPI api;

		// Token: 0x04000060 RID: 96
		public static Logger server;

		// Token: 0x04000061 RID: 97
		public static Logger client;

		// Token: 0x04000062 RID: 98
		private object debug_file_lock = new object();

		// Token: 0x04000063 RID: 99
		private object main_file_lock = new object();
	}
}
