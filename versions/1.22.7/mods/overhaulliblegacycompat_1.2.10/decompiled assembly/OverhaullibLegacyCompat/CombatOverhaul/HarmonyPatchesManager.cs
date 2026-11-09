using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	internal static class HarmonyPatchesManager
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x000042DC File Offset: 0x000024DC
		public static void Patch(ICoreAPI api)
		{
			HarmonyPatchesManager._api = api;
			HarmonyPatchesManager.PatchUniversalSide(api);
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi != null)
			{
				HarmonyPatchesManager.PatchClientSide(clientApi);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004305 File Offset: 0x00002505
		public static void Unpatch()
		{
			HarmonyPatchesManager.UnpatchUniversalSide();
			HarmonyPatchesManager.UnpatchClientSide();
			HarmonyPatchesManager._api = null;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004317 File Offset: 0x00002517
		public static void ConfigureDiagnostics(ICoreAPI api, Settings settings)
		{
			SlowDiagnostics.Configure(api, settings, "OverhaulLib:Diagnostics");
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004328 File Offset: 0x00002528
		private static void PatchClientSide(ICoreClientAPI api)
		{
			if (HarmonyPatchesManager._patchedClientSide)
			{
				return;
			}
			HarmonyPatchesManager._patchedClientSide = true;
			AimingPatches.Patch("OverhaulLib:Aiming");
			MouseWheelPatch.Patch("OverhaulLib:MouseWheel", api);
			GuiDialogPatches.Patch("OverhaulLib:GuiDialog", api);
			HandbookLinkPatches.Patch("OverhaulLib:Handbook");
			HandbookGroupedWeaponPatches.Patch("OverhaulLib:Handbook", api);
			NightVisionSlotPatches.Patch("OverhaulLib:NightVision", api);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004384 File Offset: 0x00002584
		private static void UnpatchClientSide()
		{
			if (!HarmonyPatchesManager._patchedClientSide)
			{
				return;
			}
			HarmonyPatchesManager._patchedClientSide = false;
			AimingPatches.Unpatch("OverhaulLib:Aiming");
			MouseWheelPatch.Unpatch("OverhaulLib:MouseWheel");
			GuiDialogPatches.Unpatch("OverhaulLib:GuiDialog");
			HandbookLinkPatches.Unpatch("OverhaulLib:Handbook");
			HandbookGroupedWeaponPatches.Unpatch("OverhaulLib:Handbook");
			NightVisionSlotPatches.Unpatch("OverhaulLib:NightVision");
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000043DC File Offset: 0x000025DC
		private static void PatchUniversalSide(ICoreAPI api)
		{
			if (HarmonyPatchesManager._patchedUniversalSide)
			{
				return;
			}
			HarmonyPatchesManager._patchedUniversalSide = true;
			QuenchablePatchGate.Enabled = QuenchablePatchGate.IsCombatOverhaulEnabled(api);
			new Harmony("OverhaulLib:Transpilers").PatchAll();
			InventorySafeguardsPatches.Patch("OverhaulLib:Inventory");
			HarmonyPatches.Patch("OverhaulLib:General", api);
			AnimationPatches.Patch("OverhaulLib:Animation", api);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004434 File Offset: 0x00002634
		private static void UnpatchUniversalSide()
		{
			if (!HarmonyPatchesManager._patchedUniversalSide)
			{
				return;
			}
			HarmonyPatchesManager._patchedUniversalSide = false;
			new Harmony("OverhaulLib:Transpilers").UnpatchAll(null);
			QuenchablePatchGate.Enabled = false;
			InventorySafeguardsPatches.Unpatch("OverhaulLib:Inventory");
			SlowDiagnostics.Shutdown("OverhaulLib:Diagnostics");
			if (HarmonyPatchesManager._api != null)
			{
				HarmonyPatches.Unpatch("OverhaulLib:General", HarmonyPatchesManager._api);
				AnimationPatches.Unpatch("OverhaulLib:Animation", HarmonyPatchesManager._api);
			}
		}

		// Token: 0x04000041 RID: 65
		private const string _harmonyId = "OverhaulLib:";

		// Token: 0x04000042 RID: 66
		private const string _harmonyIdAiming = "OverhaulLib:Aiming";

		// Token: 0x04000043 RID: 67
		private const string _harmonyIdMouseWheel = "OverhaulLib:MouseWheel";

		// Token: 0x04000044 RID: 68
		private const string _harmonyIdGuiDialog = "OverhaulLib:GuiDialog";

		// Token: 0x04000045 RID: 69
		private const string _harmonyIdHandbook = "OverhaulLib:Handbook";

		// Token: 0x04000046 RID: 70
		private const string _harmonyIdNightVision = "OverhaulLib:NightVision";

		// Token: 0x04000047 RID: 71
		private const string _harmonyIdTranspilers = "OverhaulLib:Transpilers";

		// Token: 0x04000048 RID: 72
		private const string _harmonyIdInventory = "OverhaulLib:Inventory";

		// Token: 0x04000049 RID: 73
		private const string _harmonyIdAnimation = "OverhaulLib:Animation";

		// Token: 0x0400004A RID: 74
		private const string _harmonyIdGeneral = "OverhaulLib:General";

		// Token: 0x0400004B RID: 75
		private const string _harmonyIdDiagnostics = "OverhaulLib:Diagnostics";

		// Token: 0x0400004C RID: 76
		[Nullable(2)]
		private static ICoreAPI _api;

		// Token: 0x0400004D RID: 77
		private static bool _patchedUniversalSide;

		// Token: 0x0400004E RID: 78
		private static bool _patchedClientSide;
	}
}
