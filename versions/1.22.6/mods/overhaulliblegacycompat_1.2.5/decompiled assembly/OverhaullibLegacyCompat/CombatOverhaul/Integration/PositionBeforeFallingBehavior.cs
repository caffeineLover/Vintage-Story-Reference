using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000048 RID: 72
	[NullableContext(1)]
	[Nullable(0)]
	public class PositionBeforeFallingBehavior : EntityBehavior
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x0001A120 File Offset: 0x00018320
		public PositionBeforeFallingBehavior(Entity entity) : base(entity)
		{
			EntityAgent entityAgent = entity as EntityAgent;
			if (entityAgent == null)
			{
				throw new ArgumentException("[PositionBeforeFallingBehavior] Entity should be EntityAgent", "entity");
			}
			this._player = entityAgent;
			this._api = entity.Api;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001A176 File Offset: 0x00018376
		public override string PropertyName()
		{
			return "PositionBeforeFallingBehavior";
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001A17D File Offset: 0x0001837D
		public double LastFallHeight
		{
			get
			{
				return this.GetFallHeight();
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0001A185 File Offset: 0x00018385
		public IEnumerable<PlayerPositionData> PositionsData
		{
			get
			{
				return this._positions;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001A190 File Offset: 0x00018390
		public override void OnGameTick(float deltaTime)
		{
			long currentTime = this._api.World.ElapsedMilliseconds;
			double height = this.entity.SidedPos.Y;
			bool onGround = PositionBeforeFallingBehavior.IsNotInFreeFall(this._player);
			this._positions.Enqueue(new PlayerPositionData(height, onGround, currentTime));
			if (this._positions.Count > 512)
			{
				PlayerPositionData lastData = this._positions.Dequeue();
				if (lastData.OnGround)
				{
					this._lastOnGroundHeight = lastData.Height;
				}
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001A210 File Offset: 0x00018410
		private double GetFallHeight()
		{
			PlayerPositionData[] positions = Enumerable.ToArray<PlayerPositionData>(Enumerable.Reverse<PlayerPositionData>(this._positions));
			bool foundInAir = false;
			double prevHeight = this._lastOnGroundHeight;
			double height = this.CurrentBelowBlockHeight();
			for (int index = 0; index < positions.Length; index++)
			{
				if (!positions[index].OnGround)
				{
					foundInAir = true;
				}
				if (!foundInAir && positions[index].OnGround)
				{
					height = positions[index].Height;
				}
				if (foundInAir && positions[index].OnGround)
				{
					prevHeight = positions[index].Height;
					return prevHeight - height;
				}
			}
			return prevHeight - height;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001A2A8 File Offset: 0x000184A8
		private static bool IsNotInFreeFall(EntityAgent player)
		{
			bool collidedVertically = player.CollidedVertically;
			bool mounted = player.MountedOn != null;
			bool notInAir = player.OnGround || player.FeetInLiquid || player.Swimming || player.InLava || player.ServerControls.IsStepping;
			bool gliding = player.ServerControls.Gliding;
			bool movingUp = player.SidedPos.Motion.Y > 0.01;
			return collidedVertically || mounted || notInAir || gliding || movingUp;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001A324 File Offset: 0x00018524
		private double CurrentBelowBlockHeight()
		{
			double height = this._player.SidedPos.Y;
			IBlockAccessor accessor = this._api.World.GetBlockAccessor(false, false, false, false);
			int heightDiff = 1;
			while ((double)heightDiff < height)
			{
				BlockPos blockPos = this._player.SidedPos.AsBlockPos;
				blockPos.Y -= heightDiff;
				BlockPos bp0 = blockPos.Copy();
				BlockPos bp = blockPos.Copy();
				BlockPos bp2 = blockPos.Copy();
				BlockPos bp3 = blockPos.Copy();
				Vec3d entityPosPos = this._player.SidedPos.XYZ;
				float xDiff = this._player.CollisionBox.XSize / 2f;
				float zDiff = this._player.CollisionBox.ZSize / 2f;
				bp0.X = (int)(entityPosPos.X - (double)xDiff);
				bp0.Z = (int)(entityPosPos.Z - (double)zDiff);
				bp.X = (int)(entityPosPos.X + (double)xDiff);
				bp.Z = (int)(entityPosPos.Z - (double)zDiff);
				bp2.X = (int)(entityPosPos.X - (double)xDiff);
				bp2.Z = (int)(entityPosPos.Z + (double)zDiff);
				bp3.X = (int)(entityPosPos.X + (double)xDiff);
				bp3.Z = (int)(entityPosPos.Z + (double)zDiff);
				Block[] blocks = new Block[]
				{
					accessor.GetBlock(bp0),
					accessor.GetBlock(bp),
					accessor.GetBlock(bp2),
					accessor.GetBlock(bp3)
				};
				if (Enumerable.Any<Block>(blocks, (Block block) => ((block != null) ? block.CollisionBoxes : null) != null && block.CollisionBoxes.Length != 0))
				{
					return (double)((float)blockPos.Y + Enumerable.Max(Enumerable.Select<Block, float>(Enumerable.Where<Block>(blocks, (Block block) => ((block != null) ? block.CollisionBoxes : null) != null && block.CollisionBoxes.Length != 0), (Block block) => Enumerable.Max(Enumerable.Select<Cuboidf, float>(block.CollisionBoxes, (Cuboidf box) => box.MaxY)))));
				}
				heightDiff++;
			}
			return 0.0;
		}

		// Token: 0x040001AA RID: 426
		private const int _maxPositionsStored = 512;

		// Token: 0x040001AB RID: 427
		private const double _moveUpThreshold = 0.01;

		// Token: 0x040001AC RID: 428
		private static bool _drawPlots;

		// Token: 0x040001AD RID: 429
		private readonly Queue<PlayerPositionData> _positions = new Queue<PlayerPositionData>();

		// Token: 0x040001AE RID: 430
		private readonly Queue<double> _fallHeights = new Queue<double>();

		// Token: 0x040001AF RID: 431
		private readonly ICoreAPI _api;

		// Token: 0x040001B0 RID: 432
		private readonly EntityAgent _player;

		// Token: 0x040001B1 RID: 433
		private double _fallHeight;

		// Token: 0x040001B2 RID: 434
		private double _lastOnGroundHeight;
	}
}
