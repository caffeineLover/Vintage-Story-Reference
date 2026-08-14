using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace RPVoiceChat
{
	// Token: 0x02000019 RID: 25
	internal class HelveHammerPatch
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004994 File Offset: 0x00002B94
		internal static void Patch(Harmony harmony)
		{
			MethodInfo originalMethod = AccessTools.Method(typeof(ItemWorkItem), "GetHelveWorkableMode", null, null);
			MethodInfo prefixMethod = AccessTools.Method(typeof(HelveHammerPatch), "GetHelveWorkableMode", null, null);
			harmony.Patch(originalMethod, new HarmonyMethod(prefixMethod), null, null, null);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000049E0 File Offset: 0x00002BE0
		public static bool GetHelveWorkableMode(ref EnumHelveWorkableMode __result, ref BlockEntityAnvil beAnvil)
		{
			try
			{
				JsonItemStack jsonOutput = beAnvil.SelectedRecipe.Output;
				if (jsonOutput != null && jsonOutput.Code != null)
				{
					ICoreAPI api = beAnvil.Api;
					if (((api != null) ? api.World : null) != null)
					{
						Item collectible = beAnvil.Api.World.GetItem(jsonOutput.Code);
						if (collectible != null)
						{
							JsonObject attr = collectible.Attributes;
							if (attr != null && attr["helvehammerworkable"].AsBool(false))
							{
								__result = 2;
								return false;
							}
						}
					}
				}
			}
			catch
			{
				return true;
			}
			return true;
		}
	}
}
