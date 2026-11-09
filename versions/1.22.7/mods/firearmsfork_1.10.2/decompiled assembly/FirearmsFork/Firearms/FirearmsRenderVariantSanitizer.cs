using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	internal static class FirearmsRenderVariantSanitizer
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00002CEC File Offset: 0x00000EEC
		public static void Start(ICoreClientAPI api)
		{
			Type rendererType = AccessTools.TypeByName("Vintagestory.Client.NoObf.InventoryItemRenderer");
			Type clientMainType = AccessTools.TypeByName("Vintagestory.Client.NoObf.ClientMain");
			if (rendererType == null || clientMainType == null)
			{
				api.Logger.Warning("[maltiezfirearms] Could not find vanilla item renderer types. Invalid renderVariant stacks may still crash in GUI rendering.");
				return;
			}
			MethodInfo method = AccessTools.Method(rendererType, "GetItemStackRenderInfo", new Type[]
			{
				clientMainType,
				typeof(ItemSlot),
				typeof(EnumItemRenderTarget),
				typeof(float)
			}, null);
			if (method == null)
			{
				api.Logger.Warning("[maltiezfirearms] Could not patch item render info creation. Invalid renderVariant stacks may still crash in GUI rendering.");
				return;
			}
			if (FirearmsRenderVariantSanitizer._harmony == null)
			{
				FirearmsRenderVariantSanitizer._harmony = new Harmony("firearmsfork.render-variant-sanitizer");
			}
			FirearmsRenderVariantSanitizer._harmony.Patch(method, new HarmonyMethod(typeof(FirearmsRenderVariantSanitizer), "SanitizeRenderVariantPrefix", null), null, null, null);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public static void Stop()
		{
			Harmony harmony = FirearmsRenderVariantSanitizer._harmony;
			if (harmony != null)
			{
				harmony.UnpatchAll("firearmsfork.render-variant-sanitizer");
			}
			FirearmsRenderVariantSanitizer._harmony = null;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002DE1 File Offset: 0x00000FE1
		private static void SanitizeRenderVariantPrefix(ItemSlot inSlot)
		{
			FirearmRenderVariantUtil.SanitizeRenderVariant(inSlot);
		}

		// Token: 0x04000030 RID: 48
		private const string HarmonyId = "firearmsfork.render-variant-sanitizer";

		// Token: 0x04000031 RID: 49
		[Nullable(2)]
		private static Harmony _harmony;
	}
}
