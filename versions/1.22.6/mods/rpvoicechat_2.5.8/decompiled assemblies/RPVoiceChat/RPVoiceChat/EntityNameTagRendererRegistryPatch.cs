using System;
using System.Reflection;
using HarmonyLib;
using RPVoiceChat.Gui;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace RPVoiceChat
{
	// Token: 0x02000015 RID: 21
	internal static class EntityNameTagRendererRegistryPatch
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004208 File Offset: 0x00002408
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = RuntimeReflectionExtensions.GetMethodInfo((NameTagRendererDelegate)AccessTools.Field(typeof(EntityNameTagRendererRegistry), "DefaultNameTagRenderer").GetValue(null));
			MethodInfo PrefixMethod = AccessTools.Method(typeof(EntityNameTagRendererRegistryPatch), "DefaultNameTagRenderer", null, null);
			harmony.Patch(OriginalMethod, new HarmonyMethod(PrefixMethod), null, null, null);
			MethodInfo OriginalMethod2 = AccessTools.Method(typeof(EntityNameTagRendererRegistry.DefaultEntitlementTagRenderer), "renderTag", null, null);
			MethodInfo PrefixMethod2 = AccessTools.Method(typeof(EntityNameTagRendererRegistryPatch), "DefaultEntitlementTagRenderer_renderTag", null, null);
			harmony.Patch(OriginalMethod2, new HarmonyMethod(PrefixMethod2), null, null, null);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000042A4 File Offset: 0x000024A4
		public static bool DefaultEntitlementTagRenderer_renderTag(ref LoadedTexture __result, double[] ___color, TextBackground ___background, Entity entity)
		{
			if (entity != null)
			{
				EntityPlayer playerEntity = entity as EntityPlayer;
				if (playerEntity != null)
				{
					__result = PlayerNameTagRenderer.GetRenderer(playerEntity, ___color, ___background);
					return __result == null;
				}
			}
			return true;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000042D0 File Offset: 0x000024D0
		public static bool DefaultNameTagRenderer(ref LoadedTexture __result, Entity entity)
		{
			if (entity != null)
			{
				EntityPlayer playerEntity = entity as EntityPlayer;
				if (playerEntity != null)
				{
					__result = PlayerNameTagRenderer.GetRenderer(playerEntity, null, null);
					return __result == null;
				}
			}
			return true;
		}
	}
}
