using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Util
{
	// Token: 0x02000029 RID: 41
	public class Logger
	{
		// Token: 0x060000FE RID: 254 RVA: 0x0000816C File Offset: 0x0000636C
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

		// Token: 0x060000FF RID: 255 RVA: 0x000081B8 File Offset: 0x000063B8
		public void Error(string message)
		{
			this.Error(message, new object[0]);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000081C8 File Offset: 0x000063C8
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

		// Token: 0x06000101 RID: 257 RVA: 0x00008270 File Offset: 0x00006470
		public void Debug(string message)
		{
			this.Debug(message, new object[0]);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008280 File Offset: 0x00006480
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

		// Token: 0x06000103 RID: 259 RVA: 0x000082DC File Offset: 0x000064DC
		public void VerboseDebug(string message)
		{
			this.VerboseDebug(message, new object[0]);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000082EC File Offset: 0x000064EC
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

		// Token: 0x06000105 RID: 261 RVA: 0x00008348 File Offset: 0x00006548
		public void Warning(string message)
		{
			this.Warning(message, new object[0]);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008358 File Offset: 0x00006558
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

		// Token: 0x06000107 RID: 263 RVA: 0x00008400 File Offset: 0x00006600
		public void Notification(string message)
		{
			this.Notification(message, new object[0]);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00008410 File Offset: 0x00006610
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

		// Token: 0x04000066 RID: 102
		public ICoreAPI api;

		// Token: 0x04000067 RID: 103
		public static Logger server;

		// Token: 0x04000068 RID: 104
		public static Logger client;

		// Token: 0x04000069 RID: 105
		private object debug_file_lock = new object();

		// Token: 0x0400006A RID: 106
		private object main_file_lock = new object();
	}
}
