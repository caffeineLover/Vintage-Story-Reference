using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000041 RID: 65
	[NullableContext(1)]
	[Nullable(0)]
	internal static class InventorySafeguardsPatches
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x000176E4 File Offset: 0x000158E4
		public static void Patch(string harmonyId)
		{
			Harmony harmony = new Harmony(harmonyId);
			Harmony harmony2 = harmony;
			MethodBase method = typeof(PlayerInventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryUpdate)
			});
			Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryUpdate, bool> func;
			if ((func = InventorySafeguardsPatches.<>O.<0>__UpdateFromPacket) == null)
			{
				func = (InventorySafeguardsPatches.<>O.<0>__UpdateFromPacket = new Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryUpdate, bool>(InventorySafeguardsPatches.UpdateFromPacket));
			}
			harmony2.Patch(method, new HarmonyMethod(func), null, null, null);
			Harmony harmony3 = harmony;
			MethodBase method2 = typeof(InventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryUpdate)
			});
			Func<InventoryNetworkUtil, IWorldAccessor, Packet_InventoryUpdate, bool> func2;
			if ((func2 = InventorySafeguardsPatches.<>O.<1>__UpdateFromPacket2) == null)
			{
				func2 = (InventorySafeguardsPatches.<>O.<1>__UpdateFromPacket2 = new Func<InventoryNetworkUtil, IWorldAccessor, Packet_InventoryUpdate, bool>(InventorySafeguardsPatches.UpdateFromPacket2));
			}
			harmony3.Patch(method2, new HarmonyMethod(func2), null, null, null);
			Harmony harmony4 = harmony;
			MethodBase method3 = typeof(InventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryDoubleUpdate)
			});
			Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryDoubleUpdate, bool> func3;
			if ((func3 = InventorySafeguardsPatches.<>O.<2>__UpdateFromPacketDouble) == null)
			{
				func3 = (InventorySafeguardsPatches.<>O.<2>__UpdateFromPacketDouble = new Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryDoubleUpdate, bool>(InventorySafeguardsPatches.UpdateFromPacketDouble));
			}
			harmony4.Patch(method3, new HarmonyMethod(func3), null, null, null);
			Harmony harmony5 = harmony;
			MethodBase method4 = typeof(InventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryContents)
			});
			Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryContents, bool> func4;
			if ((func4 = InventorySafeguardsPatches.<>O.<3>__UpdateFromInventoryContents) == null)
			{
				func4 = (InventorySafeguardsPatches.<>O.<3>__UpdateFromInventoryContents = new Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryContents, bool>(InventorySafeguardsPatches.UpdateFromInventoryContents));
			}
			harmony5.Patch(method4, new HarmonyMethod(func4), null, null, null);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00017878 File Offset: 0x00015A78
		public static void Unpatch(string harmonyId)
		{
			Harmony harmony = new Harmony(harmonyId);
			harmony.Unpatch(typeof(PlayerInventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryUpdate)
			}), 1, "*");
			harmony.Unpatch(typeof(InventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryUpdate)
			}), 1, "*");
			harmony.Unpatch(typeof(InventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryDoubleUpdate)
			}), 1, "*");
			harmony.Unpatch(typeof(InventoryNetworkUtil).GetMethod("UpdateFromPacket", 20, new Type[]
			{
				typeof(IWorldAccessor),
				typeof(Packet_InventoryContents)
			}), 1, "*");
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017994 File Offset: 0x00015B94
		private static bool UpdateFromPacket(PlayerInventoryNetworkUtil __instance, IWorldAccessor world, Packet_InventoryUpdate packet)
		{
			if (InventorySafeguardsPatches._skipUpdateFromPacket)
			{
				return true;
			}
			InventorySafeguardsPatches._skipUpdateFromPacket = true;
			try
			{
				__instance.UpdateFromPacket(world, packet);
			}
			catch (Exception exception)
			{
				ICoreAPI api = __instance.Api;
				Type typeFromHandle = typeof(InventorySafeguardsPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error in 'UpdateFromPacket' (single).\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Verbose(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			InventorySafeguardsPatches._skipUpdateFromPacket = false;
			return false;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00017A10 File Offset: 0x00015C10
		private static bool UpdateFromPacket2(InventoryNetworkUtil __instance, IWorldAccessor resolver, Packet_InventoryUpdate packet)
		{
			if (InventorySafeguardsPatches._skipUpdateFromPacket2)
			{
				return true;
			}
			InventorySafeguardsPatches._skipUpdateFromPacket2 = true;
			try
			{
				__instance.UpdateFromPacket(resolver, packet);
			}
			catch (Exception exception)
			{
				ICoreAPI api = __instance.Api;
				Type typeFromHandle = typeof(InventorySafeguardsPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error in 'UpdateFromPacket' (single).\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Verbose(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			InventorySafeguardsPatches._skipUpdateFromPacket2 = false;
			return false;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00017A8C File Offset: 0x00015C8C
		private static bool UpdateFromPacketDouble(PlayerInventoryNetworkUtil __instance, IWorldAccessor resolver, Packet_InventoryDoubleUpdate packet)
		{
			if (InventorySafeguardsPatches._skipUpdateFromDoublePacket)
			{
				return true;
			}
			InventorySafeguardsPatches._skipUpdateFromDoublePacket = true;
			try
			{
				__instance.UpdateFromPacket(resolver, packet);
			}
			catch (Exception exception)
			{
				ICoreAPI api = __instance.Api;
				Type typeFromHandle = typeof(InventorySafeguardsPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error in 'UpdateFromPacket' (double).\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Verbose(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			InventorySafeguardsPatches._skipUpdateFromDoublePacket = false;
			return false;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00017B08 File Offset: 0x00015D08
		private static bool UpdateFromInventoryContents(PlayerInventoryNetworkUtil __instance, IWorldAccessor resolver, Packet_InventoryContents packet)
		{
			if (InventorySafeguardsPatches._skipUpdateFromInventoryContents)
			{
				return true;
			}
			InventorySafeguardsPatches._skipUpdateFromInventoryContents = true;
			try
			{
				__instance.UpdateFromPacket(resolver, packet);
			}
			catch (Exception exception)
			{
				ICoreAPI api = __instance.Api;
				Type typeFromHandle = typeof(InventorySafeguardsPatches);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(51, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error in 'UpdateFromPacket' (contents).\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Verbose(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			InventorySafeguardsPatches._skipUpdateFromInventoryContents = false;
			return false;
		}

		// Token: 0x04000188 RID: 392
		[ThreadStatic]
		private static bool _skipUpdateFromPacket;

		// Token: 0x04000189 RID: 393
		[ThreadStatic]
		private static bool _skipUpdateFromPacket2;

		// Token: 0x0400018A RID: 394
		[ThreadStatic]
		private static bool _skipUpdateFromDoublePacket;

		// Token: 0x0400018B RID: 395
		[ThreadStatic]
		private static bool _skipUpdateFromInventoryContents;

		// Token: 0x020001E9 RID: 489
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000B26 RID: 2854
			[Nullable(new byte[]
			{
				0,
				1,
				1,
				1
			})]
			public static Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryUpdate, bool> <0>__UpdateFromPacket;

			// Token: 0x04000B27 RID: 2855
			[Nullable(new byte[]
			{
				0,
				1,
				1,
				1
			})]
			public static Func<InventoryNetworkUtil, IWorldAccessor, Packet_InventoryUpdate, bool> <1>__UpdateFromPacket2;

			// Token: 0x04000B28 RID: 2856
			[Nullable(new byte[]
			{
				0,
				1,
				1,
				1
			})]
			public static Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryDoubleUpdate, bool> <2>__UpdateFromPacketDouble;

			// Token: 0x04000B29 RID: 2857
			[Nullable(new byte[]
			{
				0,
				1,
				1,
				1
			})]
			public static Func<PlayerInventoryNetworkUtil, IWorldAccessor, Packet_InventoryContents, bool> <3>__UpdateFromInventoryContents;
		}
	}
}
