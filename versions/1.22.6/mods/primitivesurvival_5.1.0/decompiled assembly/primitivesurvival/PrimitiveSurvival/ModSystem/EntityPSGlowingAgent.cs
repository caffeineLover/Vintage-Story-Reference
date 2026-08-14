using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200003D RID: 61
	public class EntityPSGlowingAgent : EntityAgent
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0001CF49 File Offset: 0x0001B149
		public override byte[] LightHsv
		{
			get
			{
				return this.lightHsv;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0001CF51 File Offset: 0x0001B151
		public override void Initialize(EntityProperties properties, ICoreAPI api, long inChunkIndex3d)
		{
			base.Initialize(properties, api, inChunkIndex3d);
			this.lightHsv = properties.Attributes["lightHsv"].AsObject<byte[]>(null);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001CF78 File Offset: 0x0001B178
		public override string GetInfoText()
		{
			string result = base.GetInfoText();
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				result = result + "\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>\n\n";
			}
			return result;
		}

		// Token: 0x04000117 RID: 279
		private byte[] lightHsv = new byte[3];
	}
}
