using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x0200004C RID: 76
	[HarmonyPatch(typeof(GridRecipe), "Matches")]
	internal static class GridRecipeQuenchStateMatchPatch
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x0001A909 File Offset: 0x00018B09
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return QuenchablePatchGate.Prepare();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001A910 File Offset: 0x00018B10
		[NullableContext(1)]
		private static void Postfix(ItemSlot[] ingredients, ref bool __result)
		{
			if (!__result)
			{
				return;
			}
			int? expectedState = default(int?);
			bool? expectedDirect = default(bool?);
			int? expectedClayQuenchIteration = default(int?);
			int? expectedClayTemperIteration = default(int?);
			foreach (ItemSlot slot in ingredients)
			{
				if (!slot.Empty && QuenchableStateUtil.IsArmorOrArmorComponent(slot.Itemstack) && QuenchableStateUtil.IsFerrous(slot.Itemstack))
				{
					int state = QuenchableStateUtil.GetArmorQuenchState(slot.Itemstack);
					int num = expectedState.GetValueOrDefault();
					if (expectedState == null)
					{
						num = state;
						expectedState = new int?(num);
					}
					if (expectedState.Value != state)
					{
						__result = false;
						return;
					}
					bool direct = QuenchableStateUtil.HasDirectArmorQuench(slot.Itemstack);
					int clayQuenchIteration = (QuenchableStateUtil.GetArmorQuenchMode(slot.Itemstack) == "clay") ? slot.Itemstack.Attributes.GetInt("quenchIteration", 0) : 0;
					int clayTemperIteration = (QuenchableStateUtil.GetArmorQuenchMode(slot.Itemstack) == "clay") ? slot.Itemstack.Attributes.GetInt("temperIteration", 0) : 0;
					bool flag = expectedDirect.GetValueOrDefault();
					if (expectedDirect == null)
					{
						flag = direct;
						expectedDirect = new bool?(flag);
					}
					num = expectedClayQuenchIteration.GetValueOrDefault();
					if (expectedClayQuenchIteration == null)
					{
						num = clayQuenchIteration;
						expectedClayQuenchIteration = new int?(num);
					}
					num = expectedClayTemperIteration.GetValueOrDefault();
					if (expectedClayTemperIteration == null)
					{
						num = clayTemperIteration;
						expectedClayTemperIteration = new int?(num);
					}
					if (expectedDirect.Value != direct || expectedClayQuenchIteration.Value != clayQuenchIteration || expectedClayTemperIteration.Value != clayTemperIteration)
					{
						__result = false;
						return;
					}
				}
			}
		}
	}
}
