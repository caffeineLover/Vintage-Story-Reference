using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x0200005A RID: 90
	internal static class DidModifyItemSlotPatches
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x0001C34A File Offset: 0x0001A54A
		[NullableContext(1)]
		private static void OnInvalidSlot(InventoryBase inventory, ItemSlot slot)
		{
			LoggerUtil.Verbose(inventory.Api, typeof(DidModifyItemSlotPatches), "Supplied slot is not part of this inventory (" + inventory.InventoryID + ")");
		}

		// Token: 0x020001EC RID: 492
		[HarmonyPatch(typeof(InventoryBase), "DidModifyItemSlot")]
		[HarmonyPatchCategory("combatoverhaul")]
		public static class InventoryBase_DidModifyItemSlot_Transpiler
		{
			// Token: 0x06001560 RID: 5472 RVA: 0x000684EC File Offset: 0x000666EC
			[NullableContext(1)]
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				DidModifyItemSlotPatches.InventoryBase_DidModifyItemSlot_Transpiler.<Transpiler>d__0 <Transpiler>d__ = new DidModifyItemSlotPatches.InventoryBase_DidModifyItemSlot_Transpiler.<Transpiler>d__0(-2);
				<Transpiler>d__.<>3__instructions = instructions;
				return <Transpiler>d__;
			}
		}
	}
}
