using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000002 RID: 2
	public class BlockEntityPetNest : BlockEntity, IPointOfInterest
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Vec3d Position
		{
			get
			{
				return this.Pos.ToVec3d();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000205D File Offset: 0x0000025D
		public Vec3d MiddlePostion
		{
			get
			{
				return this.Pos.ToVec3d().AddCopy(this.MiddleOffset);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		public Vec3d MiddleOffset
		{
			get
			{
				return new Vec3d(base.Block.Attributes["x"].AsDouble(0.0), base.Block.Attributes["y"].AsDouble(0.0), base.Block.Attributes["z"].AsDouble(0.0));
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020F3 File Offset: 0x000002F3
		public string Type
		{
			get
			{
				return "petnest";
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FC File Offset: 0x000002FC
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			ICoreServerAPI sapi = api as ICoreServerAPI;
			if (sapi != null)
			{
				sapi.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000212C File Offset: 0x0000032C
		public override void OnBlockBroken(IPlayer byPlayer = null)
		{
			base.OnBlockBroken(byPlayer);
			ICoreServerAPI coreServerAPI = this.Api as ICoreServerAPI;
			if (coreServerAPI == null)
			{
				return;
			}
			coreServerAPI.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002156 File Offset: 0x00000356
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			ICoreServerAPI coreServerAPI = this.Api as ICoreServerAPI;
			if (coreServerAPI == null)
			{
				return;
			}
			coreServerAPI.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
		}
	}
}
