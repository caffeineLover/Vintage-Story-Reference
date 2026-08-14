using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000009 RID: 9
	public class BEFireflies : BlockEntity
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00003CD8 File Offset: 0x00001ED8
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (EnumAppSideExtensions.IsServer(api.Side))
			{
				this.RegisterGameTickListener(new Action<float>(this.OnScanSurroundingArea), api.World.Rand.Next(5000) + this.listenerModifier, 0);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003D2C File Offset: 0x00001F2C
		private void OnScanSurroundingArea(float dt)
		{
			if (this.Api.Side == 2)
			{
				return;
			}
			if (this.scanIteration == 0)
			{
				this.scanQuantityNearbyFlowers = 0;
			}
			int minX = -8 + 8 * (this.scanIteration / 2);
			int minZ = -8 + 8 * (this.scanIteration % 2);
			int size = 8;
			int baseX = this.Pos.X;
			int baseY = this.Pos.Y;
			int baseZ = this.Pos.Z;
			int dim = this.Pos.dimension;
			BlockPos minPos = this.tmpScanMinPos;
			BlockPos maxPos = this.tmpScanMaxPos;
			minPos.dimension = dim;
			maxPos.dimension = dim;
			minPos.Set(baseX + minX, baseY - 5, baseZ + minZ);
			maxPos.Set(baseX + minX + size - 1, baseY + 5, baseZ + minZ + size - 1);
			this.Api.World.BlockAccessor.WalkBlocks(minPos, maxPos, delegate(Block block, int x, int y, int z)
			{
				if (block.Id == 0)
				{
					return;
				}
				JsonObject attributes = block.Attributes;
				if (attributes != null && attributes.IsTrue("beeFeed"))
				{
					this.scanQuantityNearbyFlowers++;
				}
			}, false);
			this.scanIteration++;
			if (this.scanIteration == 4)
			{
				this.scanIteration = 0;
				this.OnScanComplete();
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003E44 File Offset: 0x00002044
		private void OnScanComplete()
		{
			this.quantityNearbyFlowers = this.scanQuantityNearbyFlowers;
			this.MarkDirty(false, null);
			if (this.quantityNearbyFlowers >= this.flowersRequired)
			{
				ClimateCondition climateAt = this.Api.World.BlockAccessor.GetClimateAt(this.Pos, 1, 0.0);
				float hourOfDay = this.Api.World.Calendar.HourOfDay;
				if (climateAt.Temperature >= (float)this.tempRequired && hourOfDay <= (float)this.hourRequired && this.Api.World.Rand.Next(100) < this.firefliesCatchPercent)
				{
					int fftype = this.Api.World.Rand.Next(Enumerable.Count<string>(this.fireflyTypes));
					string thisBlockPath = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0).Code.Path;
					string location = "primitivesurvival:fireflies-" + this.fireflyTypes[fftype];
					EntityProperties type = this.Api.World.GetEntityType(new AssetLocation(location));
					if (type == null)
					{
						this.Api.World.Logger.Error("ItemCreature: No such entity - {0}", new object[]
						{
							location
						});
						if (this.Api.World.Side == 2)
						{
							(this.Api as ICoreClientAPI).TriggerIngameError(this, "nosuchentity", "No such entity '{0}' loaded.");
						}
						return;
					}
					Entity entity = this.Api.World.ClassRegistry.CreateEntity(type);
					if (entity != null)
					{
						entity.Pos.X = (double)((float)this.Pos.X + 0.5f);
						entity.Pos.Y = (double)this.Pos.Y;
						entity.Pos.Z = (double)((float)this.Pos.Z + 0.5f);
						if (thisBlockPath.Contains("angled"))
						{
							entity.Pos.Yaw = 0.78f;
						}
						entity.Pos.SetFrom(entity.Pos);
						entity.PositionBeforeFalling.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
						entity.Attributes.SetString("origin", "playerplaced");
						this.Api.World.BlockAccessor.SetBlock(0, this.Pos);
						this.Api.World.SpawnEntity(entity);
					}
					this.MarkDirty(false, null);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000040E0 File Offset: 0x000022E0
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetInt("scanIteration", this.scanIteration);
			tree.SetInt("quantityNearbyFlowers", this.quantityNearbyFlowers);
			tree.SetInt("scanQuantityNearbyFlowers", this.scanQuantityNearbyFlowers);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000411C File Offset: 0x0000231C
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.scanIteration = tree.GetInt("scanIteration", 0);
			this.quantityNearbyFlowers = tree.GetInt("quantityNearbyFlowers", 0);
			this.scanQuantityNearbyFlowers = tree.GetInt("scanQuantityNearbyFlowers", 0);
			this.MarkDirty(true, null);
		}

		// Token: 0x04000019 RID: 25
		private readonly int flowersRequired = 10;

		// Token: 0x0400001A RID: 26
		private readonly int tempRequired = 15;

		// Token: 0x0400001B RID: 27
		private readonly int hourRequired = 3;

		// Token: 0x0400001C RID: 28
		private readonly int listenerModifier = 30000;

		// Token: 0x0400001D RID: 29
		private readonly int firefliesCatchPercent = 10;

		// Token: 0x0400001E RID: 30
		private readonly string[] fireflyTypes = new string[]
		{
			"treetop",
			"mysticlantern",
			"blueghost",
			"rover",
			"fairyring",
			"candle",
			"marshimp"
		};

		// Token: 0x0400001F RID: 31
		private readonly BlockPos tmpScanMinPos = new BlockPos(0);

		// Token: 0x04000020 RID: 32
		private readonly BlockPos tmpScanMaxPos = new BlockPos(0);

		// Token: 0x04000021 RID: 33
		private int scanIteration;

		// Token: 0x04000022 RID: 34
		private int quantityNearbyFlowers;

		// Token: 0x04000023 RID: 35
		private int scanQuantityNearbyFlowers;
	}
}
