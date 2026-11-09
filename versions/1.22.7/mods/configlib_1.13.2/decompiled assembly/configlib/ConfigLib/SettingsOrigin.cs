using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.Common;

namespace ConfigLib
{
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	internal class SettingsOrigin : IAssetOrigin
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000D4C6 File Offset: 0x0000B6C6
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000D4CE File Offset: 0x0000B6CE
		public string OriginPath { get; protected set; }

		// Token: 0x06000256 RID: 598 RVA: 0x0000D4D7 File Offset: 0x0000B6D7
		public SettingsOrigin(byte[] data, AssetLocation location)
		{
			this._data = data;
			this._location = location;
			this.OriginPath = this._location.Path;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00002176 File Offset: 0x00000376
		public void LoadAsset(IAsset asset)
		{
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000206C File Offset: 0x0000026C
		public bool TryLoadAsset(IAsset asset)
		{
			return true;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D4FE File Offset: 0x0000B6FE
		public List<IAsset> GetAssets(AssetCategory category, bool shouldLoad = true)
		{
			List<IAsset> list = new List<IAsset>();
			list.Add(new Asset(this._data, this._location, this));
			return list;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D4FE File Offset: 0x0000B6FE
		public List<IAsset> GetAssets(AssetLocation baseLocation, bool shouldLoad = true)
		{
			List<IAsset> list = new List<IAsset>();
			list.Add(new Asset(this._data, this._location, this));
			return list;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000206C File Offset: 0x0000026C
		public virtual bool IsAllowedToAffectGameplay()
		{
			return true;
		}

		// Token: 0x0400011A RID: 282
		private readonly byte[] _data;

		// Token: 0x0400011B RID: 283
		private readonly AssetLocation _location;
	}
}
