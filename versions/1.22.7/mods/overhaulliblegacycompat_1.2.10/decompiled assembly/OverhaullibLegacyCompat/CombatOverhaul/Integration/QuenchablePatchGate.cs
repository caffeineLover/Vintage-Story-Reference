using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	[Nullable(0)]
	internal static class QuenchablePatchGate
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001A579 File Offset: 0x00018779
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x0001A580 File Offset: 0x00018780
		internal static bool Enabled { get; set; }

		// Token: 0x06000487 RID: 1159 RVA: 0x0001A588 File Offset: 0x00018788
		internal static bool IsCombatOverhaulEnabled(ICoreAPI api)
		{
			return api.ModLoader.IsModEnabled("combatoverhaul") || api.ModLoader.IsModEnabled("combatoverhaulfork");
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001A5B0 File Offset: 0x000187B0
		internal static void DisableCustomQuenchRecipeAssetsIfDisabled(ICoreAPI api)
		{
			if (QuenchablePatchGate.Enabled)
			{
				return;
			}
			byte[] emptyRecipeArray = Encoding.UTF8.GetBytes("[]");
			foreach (string recipeName in QuenchablePatchGate.CustomQuenchRecipeNames)
			{
				IAsset asset = api.Assets.TryGet(new AssetLocation("game", "recipes/grid/" + recipeName + ".json"), true);
				if (asset != null)
				{
					asset.Data = emptyRecipeArray;
				}
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001A644 File Offset: 0x00018844
		internal static void RemoveCustomQuenchRecipesIfDisabled(ICoreAPI api)
		{
			if (QuenchablePatchGate.Enabled)
			{
				return;
			}
			IWorldAccessor world = api.World;
			List<GridRecipe> recipes = (world != null) ? world.GridRecipes : null;
			if (recipes == null)
			{
				return;
			}
			List<GridRecipe> list = recipes;
			Predicate<GridRecipe> predicate;
			if ((predicate = QuenchablePatchGate.<>O.<0>__IsCustomQuenchRecipe) == null)
			{
				predicate = (QuenchablePatchGate.<>O.<0>__IsCustomQuenchRecipe = new Predicate<GridRecipe>(QuenchablePatchGate.IsCustomQuenchRecipe));
			}
			int removed = list.RemoveAll(predicate);
			if (removed > 0)
			{
				Type typeFromHandle = typeof(QuenchablePatchGate);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(69, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removed);
				defaultInterpolatedStringHandler.AppendLiteral(" custom quench recipes because Combat Overhaul is not loaded.");
				LoggerUtil.Verbose(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001A6D7 File Offset: 0x000188D7
		internal static bool Prepare()
		{
			return QuenchablePatchGate.Enabled;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001A6E0 File Offset: 0x000188E0
		private static bool IsCustomQuenchRecipe(GridRecipe recipe)
		{
			string text;
			if (recipe == null)
			{
				text = null;
			}
			else
			{
				AssetLocation name2 = recipe.Name;
				text = ((name2 != null) ? name2.Path : null);
			}
			string text2;
			if ((text2 = text) == null)
			{
				string text3;
				if (recipe == null)
				{
					text3 = null;
				}
				else
				{
					AssetLocation name3 = recipe.Name;
					text3 = ((name3 != null) ? name3.ToString() : null);
				}
				text2 = (text3 ?? "");
			}
			string name = text2;
			return QuenchablePatchGate.CustomQuenchRecipeNames.Contains(name);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001A736 File Offset: 0x00018936
		// Note: this type is marked as 'beforefieldinit'.
		static QuenchablePatchGate()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("claycovering-armoryparts");
			hashSet.Add("claycovering-coblades");
			hashSet.Add("claycovering-cospears");
			QuenchablePatchGate.CustomQuenchRecipeNames = hashSet;
		}

		// Token: 0x040001B3 RID: 435
		private static readonly HashSet<string> CustomQuenchRecipeNames;

		// Token: 0x020001F4 RID: 500
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000B59 RID: 2905
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Predicate<GridRecipe> <0>__IsCustomQuenchRecipe;
		}
	}
}
