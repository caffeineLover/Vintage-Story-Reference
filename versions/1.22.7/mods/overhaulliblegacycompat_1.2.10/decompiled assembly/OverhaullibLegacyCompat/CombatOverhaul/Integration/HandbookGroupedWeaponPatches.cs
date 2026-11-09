using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(0)]
	internal static class HandbookGroupedWeaponPatches
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x00017170 File Offset: 0x00015370
		public static void Patch(string harmonyId, ICoreClientAPI api)
		{
			MethodInfo method = AccessTools.Method(typeof(ModSystemSurvivalHandbook), "onCreatePagesAsync", null, null);
			MethodInfo createdByMethod = AccessTools.Method(typeof(CollectibleBehaviorHandbookTextAndExtraInfo), "addCreatedByInfo", null, null);
			if (method == null)
			{
				return;
			}
			HandbookGroupedWeaponPatches._api = api;
			Harmony harmony = new Harmony(harmonyId);
			harmony.Patch(method, null, new HarmonyMethod(AccessTools.Method(typeof(HandbookGroupedWeaponPatches), "OnCreatePagesAsyncPostfix", null, null)), null, null);
			if (createdByMethod != null)
			{
				harmony.Patch(createdByMethod, null, null, new HarmonyMethod(AccessTools.Method(typeof(HandbookGroupedWeaponPatches), "AddCreatedByInfoTranspiler", null, null)), null);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00017218 File Offset: 0x00015418
		public static void Unpatch(string harmonyId)
		{
			MethodInfo method = AccessTools.Method(typeof(ModSystemSurvivalHandbook), "onCreatePagesAsync", null, null);
			if (method != null)
			{
				new Harmony(harmonyId).Unpatch(method, 2, harmonyId);
			}
			MethodInfo createdByMethod = AccessTools.Method(typeof(CollectibleBehaviorHandbookTextAndExtraInfo), "addCreatedByInfo", null, null);
			if (createdByMethod != null)
			{
				new Harmony(harmonyId).Unpatch(createdByMethod, 3, harmonyId);
			}
			HandbookGroupedWeaponPatches._api = null;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00017287 File Offset: 0x00015487
		private static IEnumerable<CodeInstruction> AddCreatedByInfoTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			HandbookGroupedWeaponPatches.<AddCreatedByInfoTranspiler>d__5 <AddCreatedByInfoTranspiler>d__ = new HandbookGroupedWeaponPatches.<AddCreatedByInfoTranspiler>d__5(-2);
			<AddCreatedByInfoTranspiler>d__.<>3__instructions = instructions;
			return <AddCreatedByInfoTranspiler>d__;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00017297 File Offset: 0x00015497
		[NullableContext(2)]
		public static bool SatisfiesForGroupedHandbookPage(ItemStack candidate, ItemStack pageStack)
		{
			return candidate != null && pageStack != null && (candidate.Satisfies(pageStack) || TextureAttributeHandbook.MatchesIgnoringTextureAttributes(pageStack, candidate));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000172B4 File Offset: 0x000154B4
		private static void OnCreatePagesAsyncPostfix(ref List<GuiHandbookPage> __result)
		{
			ICoreClientAPI api = HandbookGroupedWeaponPatches._api;
			if (api == null || __result.Count == 0)
			{
				return;
			}
			Dictionary<string, List<ItemStack>> stacksByGroup = new Dictionary<string, List<ItemStack>>(StringComparer.Ordinal);
			Dictionary<string, HashSet<string>> seenStacksByGroup = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
			Dictionary<GuiHandbookPage, string> groupByPage = new Dictionary<GuiHandbookPage, string>();
			foreach (GuiHandbookPage page in __result)
			{
				GuiHandbookItemStackPage stackPage = page as GuiHandbookItemStackPage;
				if (stackPage != null && TextureAttributeHandbook.ShouldUseGroupedHandbookPage(stackPage.Stack))
				{
					string groupPageCode = TextureAttributeHandbook.VisualAttributePageCodeForStack(stackPage.Stack);
					groupByPage[page] = groupPageCode;
					List<ItemStack> stacks;
					if (!stacksByGroup.TryGetValue(groupPageCode, ref stacks))
					{
						stacks = new List<ItemStack>();
						stacksByGroup[groupPageCode] = stacks;
						seenStacksByGroup[groupPageCode] = new HashSet<string>(StringComparer.Ordinal);
					}
					string concretePageCode = GuiHandbookItemStackPage.PageCodeForStack(stackPage.Stack);
					if (seenStacksByGroup[groupPageCode].Add(concretePageCode))
					{
						stacks.Add(stackPage.Stack);
					}
				}
			}
			if (groupByPage.Count == 0)
			{
				return;
			}
			HashSet<string> insertedGroups = new HashSet<string>(StringComparer.Ordinal);
			List<GuiHandbookPage> pages = new List<GuiHandbookPage>(__result.Count);
			foreach (GuiHandbookPage page2 in __result)
			{
				string groupPageCode2;
				List<ItemStack> stacks2;
				if (!groupByPage.TryGetValue(page2, ref groupPageCode2))
				{
					pages.Add(page2);
				}
				else if (stacksByGroup.TryGetValue(groupPageCode2, ref stacks2) && stacks2.Count != 0)
				{
					if (stacks2.Count == 1)
					{
						pages.Add(page2);
					}
					else if (insertedGroups.Add(groupPageCode2))
					{
						pages.Add(new HandbookGroupedWeaponPatches.CyclingWeaponHandbookPage(api, groupPageCode2, stacks2)
						{
							Visible = page2.Visible
						});
					}
				}
			}
			__result = pages;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00017490 File Offset: 0x00015690
		[NullableContext(2)]
		private static void LogTooltipFailureOnce(ICoreClientAPI capi, ItemStack stack, [Nullable(1)] Exception exception)
		{
			string text;
			if (stack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code2 = collectible.Code;
					text = ((code2 != null) ? code2.ToString() : null);
				}
			}
			string code = text ?? "<unknown>";
			object loggedTooltipFailuresLock = HandbookGroupedWeaponPatches.LoggedTooltipFailuresLock;
			lock (loggedTooltipFailuresLock)
			{
				if (!HandbookGroupedWeaponPatches.LoggedTooltipFailures.Add(code))
				{
					return;
				}
			}
			if (capi != null)
			{
				ILogger logger = capi.Logger;
				if (logger == null)
				{
					return;
				}
				logger.Warning("Skipping unsafe handbook tooltip text for grouped page stack {0}: {1}: {2}", new object[]
				{
					code,
					exception.GetType().Name,
					exception.Message
				});
			}
		}

		// Token: 0x04000185 RID: 389
		[Nullable(2)]
		private static ICoreClientAPI _api;

		// Token: 0x04000186 RID: 390
		private static readonly object LoggedTooltipFailuresLock = new object();

		// Token: 0x04000187 RID: 391
		private static readonly HashSet<string> LoggedTooltipFailures = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x020001E5 RID: 485
		[Nullable(0)]
		private sealed class CyclingWeaponHandbookPage : GuiHandbookPage
		{
			// Token: 0x17000531 RID: 1329
			// (get) Token: 0x0600157F RID: 5503 RVA: 0x00068BF4 File Offset: 0x00066DF4
			public List<ItemStack> Stacks { get; }

			// Token: 0x17000532 RID: 1330
			// (get) Token: 0x06001580 RID: 5504 RVA: 0x00068BFC File Offset: 0x00066DFC
			public string TextCacheTitle { get; }

			// Token: 0x17000533 RID: 1331
			// (get) Token: 0x06001581 RID: 5505 RVA: 0x00068C04 File Offset: 0x00066E04
			public string TextCacheAll { get; }

			// Token: 0x17000534 RID: 1332
			// (get) Token: 0x06001582 RID: 5506 RVA: 0x00068C0C File Offset: 0x00066E0C
			public override string PageCode
			{
				get
				{
					return this._pageCode;
				}
			}

			// Token: 0x17000535 RID: 1333
			// (get) Token: 0x06001583 RID: 5507 RVA: 0x00068C14 File Offset: 0x00066E14
			public override string CategoryCode
			{
				get
				{
					return "stack";
				}
			}

			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x06001584 RID: 5508 RVA: 0x00068C1B File Offset: 0x00066E1B
			public override bool IsDuplicate
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x06001585 RID: 5509 RVA: 0x00068C1E File Offset: 0x00066E1E
			public override float SearchWeightOffset
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x06001586 RID: 5510 RVA: 0x00068C28 File Offset: 0x00066E28
			public CyclingWeaponHandbookPage(ICoreClientAPI capi, string pageCode, IEnumerable<ItemStack> stacks)
			{
				this._pageCode = pageCode;
				this.Stacks = Enumerable.ToList<ItemStack>(Enumerable.Select<ItemStack, ItemStack>(stacks, (ItemStack stack) => stack.Clone()));
				this._displayName = ((this.Stacks.Count > 0) ? this.Stacks[0].GetName() : pageCode);
				this._dummySlot = ((this.Stacks.Count > 0) ? new DummySlot(this.Stacks[0]) : new DummySlot());
				this.TextCacheTitle = StringUtil.ToSearchFriendly(this._displayName);
				string[] array = new string[5];
				array[0] = this._displayName;
				array[1] = " ";
				array[2] = string.Join(" ", Enumerable.Select<ItemStack, string>(this.Stacks, (ItemStack stack) => stack.GetName()));
				array[3] = " ";
				array[4] = this.GetSafeDescription(capi, Enumerable.FirstOrDefault<ItemStack>(this.Stacks));
				this.TextCacheAll = StringUtil.ToSearchFriendly(string.Concat(array));
			}

			// Token: 0x06001587 RID: 5511 RVA: 0x00068D54 File Offset: 0x00066F54
			public override void RenderListEntryTo(ICoreClientAPI capi, float dt, double x, double y, double cellWidth, double cellHeight)
			{
				if (this.Stacks.Count == 0)
				{
					return;
				}
				float iconSize = (float)GuiElement.scaled(25.0);
				float leftPadding = (float)GuiElement.scaled(10.0);
				if (this._texture == null)
				{
					this.RecomposeLabel(capi);
				}
				int index = (int)(capi.ElapsedMilliseconds / 1000L % (long)this.Stacks.Count);
				this._dummySlot.Itemstack = this.Stacks[index];
				if (this._scissorBounds == null)
				{
					this._scissorBounds = ElementBounds.FixedSize(50.0, 50.0);
				}
				this._scissorBounds.ParentBounds = capi.Gui.WindowBounds;
				this._scissorBounds.fixedX = ((double)leftPadding + x - (double)(iconSize / 2f)) / (double)RuntimeEnv.GUIScale;
				this._scissorBounds.fixedY = (y - (double)(iconSize / 2f)) / (double)RuntimeEnv.GUIScale;
				this._scissorBounds.CalcWorldBounds();
				if (this._scissorBounds.InnerWidth <= 0.0 || this._scissorBounds.InnerHeight <= 0.0)
				{
					return;
				}
				capi.Render.PushScissor(this._scissorBounds, true);
				capi.Render.RenderItemstackToGui(this._dummySlot, x + (double)leftPadding + (double)(iconSize / 2f), y + (double)(iconSize / 2f), 100.0, iconSize, -1, true, false, false);
				capi.Render.PopScissor();
				if (this._texture != null)
				{
					capi.Render.Render2DTexturePremultipliedAlpha(this._texture.TextureId, x + (double)iconSize + GuiElement.scaled(25.0), y + (double)(iconSize / 4f) - GuiElement.scaled(3.0), (double)this._texture.Width, (double)this._texture.Height, 50f, null);
				}
			}

			// Token: 0x06001588 RID: 5512 RVA: 0x00068F40 File Offset: 0x00067140
			public override void ComposePage(GuiComposer detailViewGui, ElementBounds textBounds, ItemStack[] allstacks, ActionConsumable<string> openDetailPageFor)
			{
				RichTextComponentBase[] pageText = this.GetPageText(detailViewGui.Api, allstacks, openDetailPageFor);
				GuiComposerHelpers.AddRichtext(detailViewGui, pageText, textBounds, "richtext");
			}

			// Token: 0x06001589 RID: 5513 RVA: 0x00068F6C File Offset: 0x0006716C
			public override PageText GetPageText()
			{
				PageText result = default(PageText);
				result.Title = this.TextCacheTitle;
				result.Text = this.TextCacheAll;
				return result;
			}

			// Token: 0x0600158A RID: 5514 RVA: 0x00068F9C File Offset: 0x0006719C
			public override void Dispose()
			{
				LoadedTexture texture = this._texture;
				if (texture != null)
				{
					texture.Dispose();
				}
				this._texture = null;
			}

			// Token: 0x0600158B RID: 5515 RVA: 0x00068FB8 File Offset: 0x000671B8
			private RichTextComponentBase[] GetPageText(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor)
			{
				if (this.Stacks.Count == 0)
				{
					return Array.Empty<RichTextComponentBase>();
				}
				this._dummySlot.Itemstack = this.Stacks[0];
				RichTextComponentBase[] result;
				try
				{
					CollectibleBehaviorHandbookTextAndExtraInfo behavior = this.Stacks[0].Collectible.GetBehavior<CollectibleBehaviorHandbookTextAndExtraInfo>();
					result = (((behavior != null) ? behavior.GetHandbookInfo(this._dummySlot, capi, allStacks, openDetailPageFor) : null) ?? Array.Empty<RichTextComponentBase>());
				}
				catch (Exception exception)
				{
					HandbookGroupedWeaponPatches.LogTooltipFailureOnce(capi, this.Stacks[0], exception);
					string fallbackText = this.GetSafeDescription(capi, this.Stacks[0]);
					result = (string.IsNullOrWhiteSpace(fallbackText) ? Array.Empty<RichTextComponentBase>() : VtmlUtil.Richtextify(capi, fallbackText, CairoFont.WhiteSmallText(), null));
				}
				return result;
			}

			// Token: 0x0600158C RID: 5516 RVA: 0x00069080 File Offset: 0x00067280
			private void RecomposeLabel(ICoreClientAPI capi)
			{
				LoadedTexture texture = this._texture;
				if (texture != null)
				{
					texture.Dispose();
				}
				this._texture = new TextTextureUtil(capi).GenTextTexture(this._displayName, CairoFont.WhiteSmallText(), null);
			}

			// Token: 0x0600158D RID: 5517 RVA: 0x000690B0 File Offset: 0x000672B0
			private string GetSafeDescription(ICoreClientAPI capi, [Nullable(2)] ItemStack stack)
			{
				if (stack == null)
				{
					return "";
				}
				this._dummySlot.Itemstack = stack;
				string result;
				try
				{
					result = stack.GetDescription(capi.World, this._dummySlot, false);
				}
				catch (Exception exception)
				{
					HandbookGroupedWeaponPatches.LogTooltipFailureOnce(capi, stack, exception);
					result = "";
				}
				return result;
			}

			// Token: 0x04000B13 RID: 2835
			private readonly string _pageCode;

			// Token: 0x04000B14 RID: 2836
			private readonly string _displayName;

			// Token: 0x04000B15 RID: 2837
			private readonly DummySlot _dummySlot;

			// Token: 0x04000B16 RID: 2838
			[Nullable(2)]
			private ElementBounds _scissorBounds;

			// Token: 0x04000B17 RID: 2839
			[Nullable(2)]
			private LoadedTexture _texture;
		}
	}
}
