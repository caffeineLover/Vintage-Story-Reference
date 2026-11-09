using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches.Stone
{
	// Token: 0x0200000B RID: 11
	[HarmonyPatch(typeof(OreMapLayer), "ComposeDialogExtras")]
	[HarmonyPatchCategory("Always")]
	public class ComposeDialogExtrasPatch
	{
		// Token: 0x06000045 RID: 69 RVA: 0x000040C8 File Offset: 0x000022C8
		[NullableContext(1)]
		private static bool Prefix(OreMapLayer __instance, GuiDialogWorldMap guiDialogWorldMap, GuiComposer compo)
		{
			ComposeDialogExtrasPatch.<>c__DisplayClass0_0 CS$<>8__locals1 = new ComposeDialogExtrasPatch.<>c__DisplayClass0_0();
			CS$<>8__locals1.__instance = __instance;
			CS$<>8__locals1.guiDialogWorldMap = guiDialogWorldMap;
			bool stoneReadingsEnabled = BetterErProspect.Config.StoneSearchCreatesReadings;
			ICoreClientAPI capi = (ICoreClientAPI)AccessTools.Field(typeof(OreMapLayer), "capi").GetValue(CS$<>8__locals1.__instance);
			string filterByOreCode = (string)AccessTools.Field(typeof(OreMapLayer), "filterByOreCode").GetValue(CS$<>8__locals1.__instance);
			CS$<>8__locals1.key = "worldmap-layer-" + CS$<>8__locals1.__instance.LayerGroupCode;
			HashSet<string> orecodes = new HashSet<string>();
			foreach (PropickReading propickReading in CS$<>8__locals1.__instance.ownPropickReadings)
			{
				foreach (KeyValuePair<string, OreReading> oreReading in propickReading.OreReadings)
				{
					orecodes.Add(oreReading.Key);
				}
			}
			List<string> list = new List<string>();
			list.Add(null);
			List<string> valuesList = list;
			List<string> list2 = new List<string>();
			list2.Add(Lang.Get("worldmap-ores-everything", Array.Empty<object>()));
			List<string> namesList = list2;
			if (stoneReadingsEnabled)
			{
				valuesList.AddRange(new <>z__ReadOnlyArray<string>(new string[]
				{
					"ore-",
					"rock-"
				}));
				namesList.AddRange(new <>z__ReadOnlyArray<string>(new string[]
				{
					Lang.Get("bettererprospecting:worldmap-ores-only", Array.Empty<object>()),
					Lang.Get("bettererprospecting:worldmap-rocks-only", Array.Empty<object>())
				}));
			}
			List<ValueTuple<string, string>> oreEntries = new List<ValueTuple<string, string>>();
			foreach (string code in orecodes)
			{
				if (code.StartsWith("rock-"))
				{
					if (stoneReadingsEnabled)
					{
						oreEntries.Add(new ValueTuple<string, string>(code, "[" + Lang.Get("bettererprospecting:R", Array.Empty<object>()) + "]" + Lang.Get(code, Array.Empty<object>())));
					}
				}
				else
				{
					oreEntries.Add(new ValueTuple<string, string>(code, Lang.Get("ore-" + code, Array.Empty<object>())));
				}
			}
			foreach (ValueTuple<string, string> valueTuple in Enumerable.ThenBy<ValueTuple<string, string>, string>(Enumerable.OrderBy<ValueTuple<string, string>, bool>(oreEntries, ([TupleElementNames(new string[]
			{
				"value",
				"name"
			})] ValueTuple<string, string> e) => e.Item1.StartsWith("rock-")), ([TupleElementNames(new string[]
			{
				"value",
				"name"
			})] ValueTuple<string, string> e) => e.Item2, StringComparer.CurrentCulture))
			{
				string value = valueTuple.Item1;
				string name = valueTuple.Item2;
				valuesList.Add(value);
				namesList.Add(name);
			}
			string[] values = valuesList.ToArray();
			string[] names = namesList.ToArray();
			ElementBounds dlgBounds = ElementStdBounds.AutosizedMainDialog.WithFixedPosition((compo.Bounds.renderX + compo.Bounds.OuterWidth) / (double)RuntimeEnv.GUIScale + 10.0, compo.Bounds.renderY / (double)RuntimeEnv.GUIScale).WithAlignment(0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			CS$<>8__locals1.filterByOreCodeField = AccessTools.Field(typeof(OreMapLayer), "filterByOreCode");
			CS$<>8__locals1.guiDialogWorldMap.Composers[CS$<>8__locals1.key] = GuiComposerHelpers.AddDropDown(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(capi.Gui.CreateCompo(CS$<>8__locals1.key, dlgBounds), bgBounds, false, 5.0, 0.75f), Lang.Get("maplayer-prospecting", Array.Empty<object>()), delegate()
			{
				CS$<>8__locals1.guiDialogWorldMap.Composers[CS$<>8__locals1.key].Enabled = false;
			}, null, null, null).BeginChildElements(bgBounds), values, names, Math.Max(0, ArrayExtensions.IndexOf<string>(values, filterByOreCode)), new SelectionChangedDelegate(CS$<>8__locals1.<Prefix>g__onSelectionChanged|0), ElementBounds.Fixed(0.0, 30.0, 160.0, 35.0), null).EndChildElements().Compose(true);
			CS$<>8__locals1.guiDialogWorldMap.Composers[CS$<>8__locals1.key].Enabled = false;
			return false;
		}
	}
}
