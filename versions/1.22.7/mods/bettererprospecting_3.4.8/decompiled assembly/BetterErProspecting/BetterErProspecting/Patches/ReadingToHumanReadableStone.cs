using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using BetterErProspecting.Item;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(PropickReading), "ToHumanReadable")]
	[HarmonyPatchCategory("Always")]
	public class ReadingToHumanReadableStone
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003B20 File Offset: 0x00001D20
		private unsafe static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new CodeMatcher(instructions, null);
			FieldInfo totalFactorField = AccessTools.Field(typeof(OreReading), "TotalFactor");
			ConstructorInfo kvpCtor = AccessTools.Constructor(typeof(KeyValuePair<double, string>), new Type[]
			{
				typeof(double),
				typeof(string)
			}, false);
			MethodInfo listAddMethod = AccessTools.Method(typeof(List<KeyValuePair<double, string>>), "Add", null, null);
			MethodInfo helperMethod = AccessTools.Method(typeof(ReadingToHumanReadableStone), "ProcessStoneReading", null, null);
			MethodInfo getKeyMethod = AccessTools.PropertyGetter(typeof(KeyValuePair<string, OreReading>), "Key");
			matcher.Start().MatchStartForward(new CodeMatch[]
			{
				new CodeMatch(new OpCode?(OpCodes.Ldloca_S), null, null),
				new CodeMatch(new OpCode?(OpCodes.Call), getKeyMethod, null)
			});
			if (matcher.IsInvalid)
			{
				ILogger logger = BetterErProspect.Logger;
				if (logger != null)
				{
					logger.Error("[PropickReadingRockDisplayPatch] Failed to find oreReading1 local");
				}
				return instructions;
			}
			object oreReading1LocalOperand = *matcher.Operand;
			CodeMatcher codeMatcher = matcher.Start();
			CodeMatch[] array = new CodeMatch[6];
			array[0] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Ldloc_0 || i.opcode == OpCodes.Ldloc || i.opcode == OpCodes.Ldloc_S, null);
			array[1] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Ldloc_S || i.opcode == OpCodes.Ldloc, null);
			array[2] = new CodeMatch(new OpCode?(OpCodes.Ldfld), totalFactorField, null);
			array[3] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Ldloc_S || i.opcode == OpCodes.Ldloc, null);
			array[4] = new CodeMatch(new OpCode?(OpCodes.Newobj), kvpCtor, null);
			array[5] = new CodeMatch(new OpCode?(OpCodes.Callvirt), listAddMethod, null);
			codeMatcher.MatchStartForward(array);
			if (matcher.IsInvalid)
			{
				ILogger logger2 = BetterErProspect.Logger;
				if (logger2 != null)
				{
					logger2.Error("[PropickReadingRockDisplayPatch] Failed to find source.Add pattern");
				}
				return instructions;
			}
			matcher.Advance(3);
			object str2LocalOperand = *matcher.Operand;
			matcher.Advance(-3);
			matcher.Insert(new CodeInstruction[]
			{
				new CodeInstruction(OpCodes.Ldarg_1, null),
				new CodeInstruction(OpCodes.Ldloca_S, oreReading1LocalOperand),
				new CodeInstruction(OpCodes.Ldobj, typeof(KeyValuePair<string, OreReading>)),
				new CodeInstruction(OpCodes.Ldloc_S, str2LocalOperand),
				new CodeInstruction(OpCodes.Call, helperMethod),
				new CodeInstruction(OpCodes.Stloc_S, str2LocalOperand)
			});
			MethodInfo stringFormatMethod = AccessTools.Method(typeof(string), "Format", new Type[]
			{
				typeof(string),
				typeof(object),
				typeof(object)
			}, null);
			MethodInfo stringBuilderAppendMethod = AccessTools.Method(typeof(StringBuilder), "Append", new Type[]
			{
				typeof(string)
			}, null);
			MethodInfo traceHelperMethod = AccessTools.Method(typeof(ReadingToHumanReadableStone), "ProcessTraceReading", null, null);
			CodeMatcher codeMatcher2 = matcher.Start();
			CodeMatch[] array2 = new CodeMatch[5];
			array2[0] = new CodeMatch(new OpCode?(OpCodes.Call), stringFormatMethod, null);
			array2[1] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Stloc_S || i.opcode == OpCodes.Stloc, null);
			array2[2] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Ldloc_S || i.opcode == OpCodes.Ldloc, null);
			array2[3] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Ldloc_S || i.opcode == OpCodes.Ldloc, null);
			array2[4] = new CodeMatch(new OpCode?(OpCodes.Callvirt), stringBuilderAppendMethod, null);
			codeMatcher2.MatchStartForward(array2);
			if (matcher.IsInvalid)
			{
				ILogger logger3 = BetterErProspect.Logger;
				if (logger3 != null)
				{
					logger3.Error("[PropickReadingRockDisplayPatch] Failed to find str4 assignment pattern");
				}
				return matcher.InstructionEnumeration();
			}
			matcher.Advance(1);
			object str4LocalOperand = *matcher.Operand;
			MethodInfo stringConcatMethod = AccessTools.Method(typeof(string), "Concat", new Type[]
			{
				typeof(string),
				typeof(string)
			}, null);
			CodeMatcher keySearchMatcher = new CodeMatcher(instructions, null);
			CodeMatcher codeMatcher3 = keySearchMatcher.Start();
			CodeMatch[] array3 = new CodeMatch[3];
			array3[0] = new CodeMatch(new OpCode?(OpCodes.Ldstr), "ore-", null);
			array3[1] = new CodeMatch((CodeInstruction i) => i.opcode == OpCodes.Ldloc_S || i.opcode == OpCodes.Ldloc, null);
			array3[2] = new CodeMatch(new OpCode?(OpCodes.Call), stringConcatMethod, null);
			codeMatcher3.MatchStartForward(array3);
			if (keySearchMatcher.IsInvalid)
			{
				ILogger logger4 = BetterErProspect.Logger;
				if (logger4 != null)
				{
					logger4.Error("[PropickReadingRockDisplayPatch] Failed to find key local");
				}
				return matcher.InstructionEnumeration();
			}
			keySearchMatcher.Advance(1);
			object keyLocalOperand = *keySearchMatcher.Operand;
			matcher.Advance(1);
			matcher.Insert(new CodeInstruction[]
			{
				new CodeInstruction(OpCodes.Ldarg_1, null),
				new CodeInstruction(OpCodes.Ldloc_S, keyLocalOperand),
				new CodeInstruction(OpCodes.Ldloc_S, str4LocalOperand),
				new CodeInstruction(OpCodes.Call, traceHelperMethod),
				new CodeInstruction(OpCodes.Stloc_S, str4LocalOperand)
			});
			return matcher.InstructionEnumeration();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004044 File Offset: 0x00002244
		private static string ProcessTraceReading(string languageCode, string key, string currentValue)
		{
			if (!key.StartsWith("rock-"))
			{
				return currentValue;
			}
			return ItemBetterErProspectingPick.getHandbookLinkOrName(BetterErProspect.Api.World, languageCode, key, null, null);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004068 File Offset: 0x00002268
		private static string ProcessStoneReading(string languageCode, [Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<string, OreReading> oreReading1, string currentValue)
		{
			if (!oreReading1.Key.StartsWith("rock-"))
			{
				return currentValue;
			}
			OreReading reading = oreReading1.Value;
			string oreName = Lang.GetL(languageCode, reading.DepositCode, Array.Empty<object>());
			return reading.TotalFactor.ToString("0.##%") + " " + oreName;
		}
	}
}
