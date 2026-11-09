using System;
using System.Reflection;
using HarmonyLib;
using RPVoiceChat.Systems;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat
{
	// Token: 0x02000014 RID: 20
	internal static class BlockCanPlacePatch
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00004260 File Offset: 0x00002460
		internal static void Patch(Harmony harmony)
		{
			MethodInfo original = AccessTools.Method(typeof(Block), "CanPlaceBlock", null, null);
			MethodInfo prefix = AccessTools.Method(typeof(BlockCanPlacePatch), "CanPlaceBlock_Prefix", null, null);
			harmony.Patch(original, new HarmonyMethod(prefix), null, null, null);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000042AC File Offset: 0x000024AC
		public static bool CanPlaceBlock_Prefix(Block __instance, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode, ref bool __result)
		{
			if (((world != null) ? world.BlockAccessor : null) == null || blockSel == null)
			{
				return true;
			}
			BlockPos placePos = (world.BlockAccessor.GetBlock(blockSel.Position).Replaceable >= 6000) ? blockSel.Position : blockSel.Position.AddCopy(blockSel.Face);
			if (WarningBeaconStructure.IsInReservedZone(world.BlockAccessor, placePos))
			{
				failureCode = RPVoiceChatMod.modID + ":WarningBeacon.ReservedZone";
				__result = false;
				return false;
			}
			return true;
		}
	}
}
