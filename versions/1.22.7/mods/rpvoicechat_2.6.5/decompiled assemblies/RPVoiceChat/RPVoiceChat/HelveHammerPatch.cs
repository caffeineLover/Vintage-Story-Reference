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
		// Token: 0x06000083 RID: 131 RVA: 0x00004AB8 File Offset: 0x00002CB8
		internal static void Patch(Harmony harmony)
		{
			MethodInfo originalMethod = AccessTools.Method(typeof(ItemWorkItem), "GetHelveWorkableMode", null, null);
			MethodInfo prefixMethod = AccessTools.Method(typeof(HelveHammerPatch), "GetHelveWorkableMode", null, null);
			harmony.Patch(originalMethod, new HarmonyMethod(prefixMethod), null, null, null);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004B04 File Offset: 0x00002D04
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
