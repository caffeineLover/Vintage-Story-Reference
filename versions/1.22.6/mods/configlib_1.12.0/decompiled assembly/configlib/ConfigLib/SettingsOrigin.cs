using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.Common;

namespace ConfigLib
{
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	internal class SettingsOrigin : IAssetOrigin
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000D1AA File Offset: 0x0000B3AA
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000D1B2 File Offset: 0x0000B3B2
		public string OriginPath { get; protected set; }

		// Token: 0x06000231 RID: 561 RVA: 0x0000D1BB File Offset: 0x0000B3BB
		public SettingsOrigin(byte[] data, AssetLocation location)
		{
			this._data = data;
			this._location = location;
			this.OriginPath = this._location.Path;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00007270 File Offset: 0x00005470
		public void LoadAsset(IAsset asset)
		{
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public bool TryLoadAsset(IAsset asset)
		{
			return true;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000D1E5 File Offset: 0x0000B3E5
		public List<IAsset> GetAssets(AssetCategory category, bool shouldLoad = true)
		{
			List<IAsset> list = new List<IAsset>();
			list.Add(new Asset(this._data, this._location, this));
			return list;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000D1E5 File Offset: 0x0000B3E5
		public List<IAsset> GetAssets(AssetLocation baseLocation, bool shouldLoad = true)
		{
			List<IAsset> list = new List<IAsset>();
			list.Add(new Asset(this._data, this._location, this));
			return list;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public virtual bool IsAllowedToAffectGameplay()
		{
			return true;
		}

		// Token: 0x04000117 RID: 279
		private readonly byte[] _data;

		// Token: 0x04000118 RID: 280
		private readonly AssetLocation _location;
	}
}
