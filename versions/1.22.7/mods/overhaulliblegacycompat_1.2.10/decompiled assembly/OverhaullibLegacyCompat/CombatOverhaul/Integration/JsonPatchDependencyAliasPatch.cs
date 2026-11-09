using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(ModJsonPatchLoader), "ApplyPatches")]
	internal static class JsonPatchDependencyAliasPatch
	{
		// Token: 0x0600042C RID: 1068 RVA: 0x00017B84 File Offset: 0x00015D84
		private static void Prefix(ModJsonPatchLoader __instance)
		{
			HashSet<string> aliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			ICoreAPI api = JsonPatchDependencyAliasPatch.ApiField.GetValue(__instance) as ICoreAPI;
			if (api != null)
			{
				foreach (Mod mod in api.ModLoader.Mods)
				{
					JsonPatchDependencyAliasPatch.AddModIdAliases(aliases, mod.Info.ModID);
				}
				foreach (AssetLocation location in api.Assets.GetLocations("compatibility/", null))
				{
					string text;
					if (JsonPatchDependencyAliasPatch.TryGetCompatibilityPatchTarget(location.Path, out text))
					{
						string domain = location.Domain;
						if (!string.IsNullOrWhiteSpace(domain))
						{
							aliases.Add(domain);
						}
					}
				}
			}
			object aliasLock = JsonPatchDependencyAliasPatch.AliasLock;
			lock (aliasLock)
			{
				JsonPatchDependencyAliasPatch._loadedAliases = aliases;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00017CA8 File Offset: 0x00015EA8
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			JsonPatchDependencyAliasPatch.<Transpiler>d__9 <Transpiler>d__ = new JsonPatchDependencyAliasPatch.<Transpiler>d__9(-2);
			<Transpiler>d__.<>3__instructions = instructions;
			return <Transpiler>d__;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00017CB8 File Offset: 0x00015EB8
		private static List<IAsset> GetManyWithCompatibilityPatches(IAssetManager assets, string pathBegins, [Nullable(2)] string domain, bool loadAsset)
		{
			List<IAsset> patches = assets.GetMany(pathBegins, domain, loadAsset);
			if (!string.Equals(pathBegins, "patches/", 5) || domain != null)
			{
				return patches;
			}
			object aliasLock = JsonPatchDependencyAliasPatch.AliasLock;
			HashSet<string> aliases;
			lock (aliasLock)
			{
				aliases = new HashSet<string>(JsonPatchDependencyAliasPatch._loadedAliases, StringComparer.OrdinalIgnoreCase);
			}
			HashSet<string> patchLocations = Enumerable.ToHashSet<string>(Enumerable.Select<IAsset, string>(patches, (IAsset asset) => asset.Location.ToString()), StringComparer.OrdinalIgnoreCase);
			foreach (AssetLocation location in assets.GetLocations("compatibility/", null))
			{
				string compatibilityTarget;
				if (JsonPatchDependencyAliasPatch.TryGetCompatibilityPatchTarget(location.Path, out compatibilityTarget) && aliases.Contains(compatibilityTarget))
				{
					IAsset asset2 = assets.TryGet(location, loadAsset);
					if (asset2 != null && patchLocations.Add(asset2.Location.ToString()))
					{
						patches.Add(asset2);
					}
				}
			}
			return patches;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00017DDC File Offset: 0x00015FDC
		private static bool ContainsModOrAlias(HashSet<string> modIds, string modId)
		{
			if (JsonPatchDependencyAliasPatch.ContainsModId(modIds, modId))
			{
				return true;
			}
			object aliasLock = JsonPatchDependencyAliasPatch.AliasLock;
			bool result;
			lock (aliasLock)
			{
				result = JsonPatchDependencyAliasPatch._loadedAliases.Contains(modId);
			}
			return result;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00017E30 File Offset: 0x00016030
		private static void AddModIdAliases(HashSet<string> aliases, string modId)
		{
			aliases.Add(modId);
			foreach (KeyValuePair<string, string[]> keyValuePair in JsonPatchDependencyAliasPatch.ExplicitAliases)
			{
				string text;
				string[] array;
				keyValuePair.Deconstruct(ref text, ref array);
				string originalModId = text;
				foreach (string forkModId in array)
				{
					if (string.Equals(modId, forkModId, 5))
					{
						aliases.Add(originalModId);
						break;
					}
				}
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00017EC4 File Offset: 0x000160C4
		private static bool ContainsModId(HashSet<string> modIds, string modId)
		{
			if (modIds.Contains(modId))
			{
				return true;
			}
			using (HashSet<string>.Enumerator enumerator = modIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (string.Equals(enumerator.Current, modId, 5))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00017F28 File Offset: 0x00016128
		private static bool TryGetCompatibilityPatchTarget(string path, out string compatibilityTarget)
		{
			compatibilityTarget = "";
			string normalizedPath = path.Replace('\\', '/');
			if (!normalizedPath.StartsWith("compatibility/", 5))
			{
				return false;
			}
			int targetEnd = normalizedPath.IndexOf('/', "compatibility/".Length);
			if (targetEnd <= "compatibility/".Length)
			{
				return false;
			}
			string text = normalizedPath;
			int num = targetEnd + 1;
			string remainingPath = text.Substring(num, text.Length - num);
			if (!remainingPath.StartsWith("patches/", 5) || !remainingPath.EndsWith(".json", 5))
			{
				return false;
			}
			string text2 = normalizedPath;
			num = "compatibility/".Length;
			compatibilityTarget = text2.Substring(num, targetEnd - num);
			return compatibilityTarget.Length > 0;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00017FD4 File Offset: 0x000161D4
		// Note: this type is marked as 'beforefieldinit'.
		static JsonPatchDependencyAliasPatch()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
			dictionary["armory"] = new string[]
			{
				"armoryfork"
			};
			dictionary["combatoverhaul"] = new string[]
			{
				"combatoverhaulfork"
			};
			dictionary["crossbows"] = new string[]
			{
				"crossbowsfork"
			};
			dictionary["firearms"] = new string[]
			{
				"firearmsfork"
			};
			dictionary["quiversandsheaths"] = new string[]
			{
				"quiversfork"
			};
			dictionary["toolanimations"] = new string[]
			{
				"toolsanimationsfork"
			};
			dictionary["toolsanimations"] = new string[]
			{
				"toolsanimationsfork"
			};
			JsonPatchDependencyAliasPatch.ExplicitAliases = dictionary;
		}

		// Token: 0x0400018C RID: 396
		private static readonly FieldInfo ApiField = AccessTools.Field(typeof(ModJsonPatchLoader), "api");

		// Token: 0x0400018D RID: 397
		private static readonly MethodInfo AssetGetMany = AccessTools.Method(typeof(IAssetManager), "GetMany", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(bool)
		}, null);

		// Token: 0x0400018E RID: 398
		private static readonly MethodInfo HashSetContains = AccessTools.Method(typeof(HashSet<string>), "Contains", new Type[]
		{
			typeof(string)
		}, null);

		// Token: 0x0400018F RID: 399
		private static readonly MethodInfo GetManyWithCompatibilityPatchesMethod = AccessTools.Method(typeof(JsonPatchDependencyAliasPatch), "GetManyWithCompatibilityPatches", null, null);

		// Token: 0x04000190 RID: 400
		private static readonly MethodInfo ContainsModOrAliasMethod = AccessTools.Method(typeof(JsonPatchDependencyAliasPatch), "ContainsModOrAlias", null, null);

		// Token: 0x04000191 RID: 401
		private static readonly object AliasLock = new object();

		// Token: 0x04000192 RID: 402
		private static HashSet<string> _loadedAliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000193 RID: 403
		private static readonly Dictionary<string, string[]> ExplicitAliases;
	}
}
