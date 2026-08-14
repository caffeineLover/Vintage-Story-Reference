using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace BetterErProspecting.Extensions
{
	// Token: 0x02000013 RID: 19
	[NullableContext(1)]
	[Nullable(0)]
	public static class ServerPlayerExtensions
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00005E47 File Offset: 0x00004047
		public static string L(this IServerPlayer player, string key, params object[] args)
		{
			return Lang.GetL(player.LanguageCode, ServerPlayerExtensions.prefix(key), args);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005E5B File Offset: 0x0000405B
		public static void Info(this IServerPlayer player, string message, params object[] args)
		{
			player.SendLocalized(message, GlobalConstants.InfoLogChatGroup, args);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005E6A File Offset: 0x0000406A
		public static void Info(this IServerPlayer player, string message, int chatGroup, params object[] args)
		{
			player.SendLocalized(message, chatGroup, args);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005E75 File Offset: 0x00004075
		public static void General(this IServerPlayer player, string message, params object[] args)
		{
			player.SendLocalized(message, GlobalConstants.AllChatGroups, args);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005E84 File Offset: 0x00004084
		public static void SendLocalized(this IServerPlayer player, string message, int groupType, params object[] args)
		{
			player.SendLocalisedMessage(groupType, ServerPlayerExtensions.prefix(message), args);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005E94 File Offset: 0x00004094
		private static string prefix(string message)
		{
			if (!message.Contains(':'))
			{
				message = "bettererprospecting:" + message;
			}
			return message;
		}

		// Token: 0x0400002A RID: 42
		private const string modid = "bettererprospecting";
	}
}
