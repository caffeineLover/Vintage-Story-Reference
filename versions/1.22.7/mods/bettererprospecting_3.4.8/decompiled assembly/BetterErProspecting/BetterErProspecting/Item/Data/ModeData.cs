using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace BetterErProspecting.Item.Data
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	public class ModeData
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00005E00 File Offset: 0x00004000
		public ModeData(string mode, string assetPath, Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int> execute = null, string nameKey = null, string domain = "game")
		{
			this.TextureAssetLocation = domain + ":" + assetPath;
			this.Skill = new SkillItem
			{
				Code = new AssetLocation(domain + ":" + mode)
			};
			this.Execute = execute;
			if (nameKey != null)
			{
				this.Skill.Name = Lang.Get(nameKey, Array.Empty<object>());
			}
		}

		// Token: 0x04000027 RID: 39
		public AssetLocation TextureAssetLocation;

		// Token: 0x04000028 RID: 40
		public SkillItem Skill;

		// Token: 0x04000029 RID: 41
		public bool Enabled = true;

		// Token: 0x0400002A RID: 42
		public Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int> Execute;
	}
}
