using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200003C RID: 60
	public class EntityPSAgent : EntityAgent
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0001CF08 File Offset: 0x0001B108
		public override string GetInfoText()
		{
			string result = base.GetInfoText();
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				result = result + "\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>\n\n";
			}
			return result;
		}
	}
}
