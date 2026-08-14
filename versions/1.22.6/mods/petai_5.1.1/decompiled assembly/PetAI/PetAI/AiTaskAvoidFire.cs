using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000005 RID: 5
	public class AiTaskAvoidFire : AiTaskBase
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000021C0 File Offset: 0x000003C0
		public AiTaskAvoidFire(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			JsonObject jsonObject = taskConfig["movespeed"];
			this.moveSpeed = ((jsonObject != null) ? jsonObject.AsFloat(0.01f) : 0.01f);
			JsonObject jsonObject2 = taskConfig["mindistance"];
			this.minDistance = ((jsonObject2 != null) ? jsonObject2.AsFloat(5f) : 5f);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002224 File Offset: 0x00000424
		public override bool ShouldExecute()
		{
			long currentTime = this.entity.World.ElapsedMilliseconds;
			if (this.lastCheck + 2000f > (float)currentTime)
			{
				return false;
			}
			this.lastCheck = (float)currentTime;
			int[][] array = new int[9][];
			array[0] = new int[]
			{
				-1,
				-1
			};
			int num = 1;
			int[] array2 = new int[2];
			array2[0] = -1;
			array[num] = array2;
			array[2] = new int[]
			{
				-1,
				1
			};
			array[3] = new int[]
			{
				default(int),
				-1
			};
			array[4] = new int[2];
			array[5] = new int[]
			{
				default(int),
				1
			};
			array[6] = new int[]
			{
				1,
				-1
			};
			int num2 = 7;
			int[] array3 = new int[2];
			array3[0] = 1;
			array[num2] = array3;
			array[8] = new int[]
			{
				1,
				1
			};
			BlockPos entityPos = this.entity.Pos.AsBlockPos;
			IBlockAccessor blockAccessor = this.entity.World.BlockAccessor;
			BlockPos nearestFire = Enumerable.MinBy<BlockPos, double>(Enumerable.ToList<BlockPos>(Enumerable.Select<KeyValuePair<BlockPos, BlockEntity>, BlockPos>(Enumerable.Where<KeyValuePair<BlockPos, BlockEntity>>(Enumerable.Where<KeyValuePair<BlockPos, BlockEntity>>(Enumerable.Where<KeyValuePair<BlockPos, BlockEntity>>(Enumerable.SelectMany<IWorldChunk, KeyValuePair<BlockPos, BlockEntity>>(Enumerable.Select<int[], IWorldChunk>(array, (int[] offset) => blockAccessor.GetChunk(entityPos.X / 32 + offset[0], entityPos.Y / 32, entityPos.Z / 32 + offset[1])), (IWorldChunk chunk) => ((chunk != null) ? chunk.BlockEntities : null) ?? new Dictionary<BlockPos, BlockEntity>()), (KeyValuePair<BlockPos, BlockEntity> entry) => entry.Value != null && entry.Key != null), (KeyValuePair<BlockPos, BlockEntity> entry) => entry.Value.Block.HasBehavior<BlockBehaviorHeatSource>(false)), (KeyValuePair<BlockPos, BlockEntity> entry) => entry.Key.DistanceSqToNearerEdge((double)entityPos.X, (double)entityPos.Y, (double)entityPos.Z) < (double)(this.minDistance * this.minDistance)), (KeyValuePair<BlockPos, BlockEntity> entry) => entry.Key)), (BlockPos firePos) => firePos.DistanceSqToNearerEdge((double)entityPos.X, (double)entityPos.Y, (double)entityPos.Z));
			if (nearestFire != null)
			{
				Vec3i direction = entityPos.AddCopy(-nearestFire.X, -nearestFire.Y, -nearestFire.Z).AsVec3i;
				this.target = entityPos.Add(direction).Add(direction).ToVec3d();
				return true;
			}
			return false;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002438 File Offset: 0x00000638
		public override void StartExecute()
		{
			base.StartExecute();
			this.pathTraverser.WalkTowards(this.target, this.moveSpeed, 1f, delegate()
			{
			}, delegate()
			{
			}, 0);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000024A7 File Offset: 0x000006A7
		public override bool ContinueExecute(float dt)
		{
			return this.pathTraverser.Active;
		}

		// Token: 0x04000005 RID: 5
		private Vec3d target;

		// Token: 0x04000006 RID: 6
		private readonly float moveSpeed;

		// Token: 0x04000007 RID: 7
		private readonly float minDistance;

		// Token: 0x04000008 RID: 8
		private float lastCheck;
	}
}
