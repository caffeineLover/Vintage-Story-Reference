using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200003A RID: 58
	[HarmonyPatch(typeof(BlockGrindingWheel), "OnBlockInteractStart")]
	internal static class GrindingWheelStartPatch
	{
		// Token: 0x060003DB RID: 987 RVA: 0x00012089 File Offset: 0x00010289
		[NullableContext(1)]
		private static void Postfix(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref bool __result)
		{
			IPlayerInventoryManager inventoryManager = byPlayer.InventoryManager;
			if (!GrindingWheelCompat.TryEnableGrindingWheelBuff((inventoryManager != null) ? inventoryManager.ActiveHotbarSlot : null))
			{
				return;
			}
			__result = GrindingWheelCompat.TryStartGrindingWeapon(world, byPlayer, blockSel);
		}
	}
}
