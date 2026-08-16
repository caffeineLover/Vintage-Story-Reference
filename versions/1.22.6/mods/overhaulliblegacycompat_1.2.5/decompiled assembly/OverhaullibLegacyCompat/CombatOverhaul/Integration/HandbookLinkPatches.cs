using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000040 RID: 64
	[NullableContext(1)]
	[Nullable(0)]
	internal static class HandbookLinkPatches
	{
		// Token: 0x06000420 RID: 1056 RVA: 0x00017524 File Offset: 0x00015724
		public static void Patch(string harmonyId)
		{
			MethodInfo method = HandbookLinkPatches.GetPageCodeMethod();
			if (method == null)
			{
				return;
			}
			new Harmony(harmonyId).Patch(method, null, new HarmonyMethod(AccessTools.Method(typeof(HandbookLinkPatches), "GetPageCodeForStackPostfix", null, null)), null, null);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001756C File Offset: 0x0001576C
		public static void Unpatch(string harmonyId)
		{
			MethodInfo method = HandbookLinkPatches.GetPageCodeMethod();
			if (method == null)
			{
				return;
			}
			new Harmony(harmonyId).Unpatch(method, 2, harmonyId);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00017597 File Offset: 0x00015797
		[NullableContext(2)]
		private static MethodInfo GetPageCodeMethod()
		{
			return AccessTools.Method(typeof(CollectibleBehaviorHandbookTextAndExtraInfo), "getPageCodeForStack", new Type[]
			{
				typeof(ICoreClientAPI),
				typeof(ItemStack)
			}, null);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000175D0 File Offset: 0x000157D0
		private static void GetPageCodeForStackPostfix(ICoreClientAPI capi, ItemStack stack, ref string __result)
		{
			if (capi != null)
			{
				ItemStack stack2 = stack;
				if (((stack2 != null) ? stack2.Collectible : null) != null && !string.IsNullOrEmpty(__result))
				{
					ItemStack[] handbookStacks = ObjectCacheUtil.TryGet<ItemStack[]>(capi, "handbookallstacks");
					if (handbookStacks == null || handbookStacks.Length == 0)
					{
						return;
					}
					if (HandbookLinkPatches.HandbookContainsPageCode(capi, handbookStacks, __result))
					{
						return;
					}
					ItemStack matchingPageStack = Enumerable.FirstOrDefault<ItemStack>(handbookStacks, (ItemStack candidate) => ((candidate != null) ? candidate.Collectible : null) != null && candidate.Satisfies(stack));
					if (matchingPageStack != null)
					{
						__result = HandbookLinkPatches.GetPageCode(capi, matchingPageStack);
					}
					return;
				}
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001764C File Offset: 0x0001584C
		private static bool HandbookContainsPageCode(ICoreClientAPI capi, IEnumerable<ItemStack> handbookStacks, string pageCode)
		{
			return Enumerable.Any<ItemStack>(handbookStacks, (ItemStack stack) => ((stack != null) ? stack.Collectible : null) != null && HandbookLinkPatches.GetPageCode(capi, stack) == pageCode);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001767F File Offset: 0x0001587F
		private static string GetPageCode(ICoreClientAPI capi, ItemStack stack)
		{
			IHandBookPageCodeProvider collectibleInterface = stack.Collectible.GetCollectibleInterface<IHandBookPageCodeProvider>();
			return ((collectibleInterface != null) ? collectibleInterface.HandbookPageCodeForStack(capi.World, stack) : null) ?? GuiHandbookItemStackPage.PageCodeForStack(stack);
		}
	}
}
