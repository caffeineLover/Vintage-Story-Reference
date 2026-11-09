using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectilePhysicsBehavior : EntityBehaviorPassivePhysics
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x00035A18 File Offset: 0x00033C18
		public ProjectilePhysicsBehavior(Entity entity) : base(entity)
		{
			this.ModSettings = entity.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00035A84 File Offset: 0x00033C84
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			this.Config = attributes.AsObject<ProjectilePhysicsBehaviorConfig>(null);
			if (ProjectilePhysicsBehavior.EntityBehaviorPassivePhysics_airDragValue == null)
			{
				ProjectilePhysicsBehavior.EntityBehaviorPassivePhysics_airDragValue = typeof(EntityBehaviorPassivePhysics).GetField("airDragValue", 36);
			}
			FieldInfo entityBehaviorPassivePhysics_airDragValue = ProjectilePhysicsBehavior.EntityBehaviorPassivePhysics_airDragValue;
			if (entityBehaviorPassivePhysics_airDragValue == null)
			{
				return;
			}
			entityBehaviorPassivePhysics_airDragValue.SetValue(this, 1);
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00035ADE File Offset: 0x00033CDE
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x00035AE6 File Offset: 0x00033CE6
		public bool Stuck { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x00035AEF File Offset: 0x00033CEF
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x00035AF7 File Offset: 0x00033CF7
		public ProjectilePhysicsBehaviorConfig Config { get; set; } = new ProjectilePhysicsBehaviorConfig();

		// Token: 0x06000A05 RID: 2565 RVA: 0x00035B00 File Offset: 0x00033D00
		protected override void applyCollision(EntityPos pos, float dtFactor)
		{
			Vector3d CurrentPosition;
			CurrentPosition..ctor(pos.X, pos.Y, pos.Z);
			if (CurrentPosition.LengthSquared == 0.0)
			{
				return;
			}
			Vector3d PositionDelta;
			PositionDelta..ctor(pos.Motion.X * (double)dtFactor, pos.Motion.Y * (double)dtFactor, pos.Motion.Z * (double)dtFactor);
			Vector3d NextPosition = CurrentPosition + PositionDelta;
			if (this.ModSettings.DebugProjectilesTrailsParticles && pos.Motion.Length() > 0.1)
			{
				ICoreAPI api = this.entity.Api;
				if (api != null)
				{
					api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(255, 100, 100, 125), new Vec3d(CurrentPosition.X, CurrentPosition.Y, CurrentPosition.Z), new Vec3d(CurrentPosition.X, CurrentPosition.Y, CurrentPosition.Z), new Vec3f(), new Vec3f(), 0.1f, 0f, 0.7f, 1, null);
				}
			}
			Vector3d intersection;
			Vector3d normal;
			BlockFacing facing;
			Block block;
			BlockPos blockPosition;
			if (!CuboidAABBCollider.CollideWithTerrain(this.entity.Api.World.BlockAccessor, NextPosition, CurrentPosition, this.Config.ColliderRadius, out intersection, out normal, out facing, out block, out blockPosition))
			{
				this.entity.CollidedHorizontally = false;
				this.entity.CollidedVertically = false;
				this.newPos.Set(NextPosition.X, NextPosition.Y, NextPosition.Z);
				return;
			}
			float angleDeg = Math.Abs(Angle.BetweenVectors(PositionDelta, normal).Degrees);
			if (angleDeg > 90f - this.Config.MaxRicochetAngleDeg && angleDeg < 90f + this.Config.MaxRicochetAngleDeg && pos.Motion.Length() > (double)this.Config.MinSpeedToRicochet)
			{
				switch (facing.Index)
				{
				case 0:
				case 2:
					pos.Motion.Z *= (double)(-(double)this.Config.RicochetNormalSpeedFactor);
					break;
				case 1:
				case 3:
					pos.Motion.X *= (double)(-(double)this.Config.RicochetNormalSpeedFactor);
					break;
				case 4:
				case 5:
					pos.Motion.Y *= (double)(-(double)this.Config.RicochetNormalSpeedFactor);
					break;
				}
				PositionDelta..ctor(pos.Motion.X * (double)dtFactor, pos.Motion.Y * (double)dtFactor, pos.Motion.Z * (double)dtFactor);
				NextPosition = intersection + PositionDelta * (1.0 - (CurrentPosition - intersection).Length / (CurrentPosition - NextPosition).Length);
				switch (facing.Index)
				{
				case 0:
					NextPosition.Z = Math.Min(intersection.Z - this.Config.ColliderRadius, NextPosition.Z);
					break;
				case 1:
					NextPosition.X = Math.Max(intersection.X + this.Config.ColliderRadius, NextPosition.X);
					break;
				case 2:
					NextPosition.Z = Math.Max(intersection.Z + this.Config.ColliderRadius, NextPosition.Z);
					break;
				case 3:
					NextPosition.X = Math.Min(intersection.X - this.Config.ColliderRadius, NextPosition.X);
					break;
				case 4:
					NextPosition.Y = Math.Max(intersection.Y + this.Config.ColliderRadius, NextPosition.Y);
					break;
				case 5:
					NextPosition.Y = Math.Min(intersection.Y - this.Config.ColliderRadius, NextPosition.Y);
					break;
				}
				this.newPos.Set(NextPosition.X, NextPosition.Y, NextPosition.Z);
				this.entity.CollidedHorizontally = false;
				this.entity.CollidedVertically = false;
				ProjectileEntity projectileEntity = this.entity as ProjectileEntity;
				if (projectileEntity != null)
				{
					projectileEntity.SetRotation();
				}
				pos.Motion *= this.Config.RicochetSpeedFactor;
				SoundAttributes? soundAttributes;
				SoundAttributes? soundAttributes2;
				if (block == null)
				{
					soundAttributes = default(SoundAttributes?);
					soundAttributes2 = soundAttributes;
				}
				else
				{
					BlockSounds sounds = block.Sounds;
					if (sounds == null)
					{
						soundAttributes = default(SoundAttributes?);
						soundAttributes2 = soundAttributes;
					}
					else
					{
						soundAttributes2 = new SoundAttributes?(sounds.Hit);
					}
				}
				SoundAttributes? soundAttributes3 = soundAttributes2;
				SoundAttributes? soundAttributes5;
				if (soundAttributes3 == null)
				{
					SoundAttributes? soundAttributes4;
					if (block == null)
					{
						soundAttributes4 = default(SoundAttributes?);
					}
					else
					{
						BlockSounds sounds2 = block.Sounds;
						if (sounds2 == null)
						{
							soundAttributes4 = default(SoundAttributes?);
						}
						else
						{
							Dictionary<EnumTool, BlockSounds> byTool = sounds2.ByTool;
							if (byTool == null)
							{
								soundAttributes4 = default(SoundAttributes?);
							}
							else
							{
								Dictionary<EnumTool, BlockSounds>.ValueCollection values = byTool.Values;
								if (values == null)
								{
									soundAttributes4 = default(SoundAttributes?);
								}
								else
								{
									BlockSounds blockSounds = Enumerable.FirstOrDefault<BlockSounds>(values);
									soundAttributes4 = ((blockSounds != null) ? new SoundAttributes?(blockSounds.Hit) : default(SoundAttributes?));
								}
							}
						}
					}
					soundAttributes = soundAttributes4;
					if (soundAttributes == null)
					{
						if (block == null)
						{
							soundAttributes5 = default(SoundAttributes?);
						}
						else
						{
							BlockSounds sounds3 = block.Sounds;
							soundAttributes5 = ((sounds3 != null) ? new SoundAttributes?(sounds3.Break) : default(SoundAttributes?));
						}
					}
					else
					{
						soundAttributes5 = soundAttributes;
					}
				}
				else
				{
					soundAttributes5 = soundAttributes3;
				}
				soundAttributes = soundAttributes5;
				AssetLocation soundLoc = ((soundAttributes != null) ? soundAttributes.GetValueOrDefault().Location : null) ?? new AssetLocation("game:sounds/player/destruct");
				this.entity.Api.World.PlaySoundAt(soundLoc, intersection.X, intersection.Y, intersection.Z, null, true, 32f, 1f);
				return;
			}
			this.newPos.Set(intersection.X, intersection.Y, intersection.Z);
			this.entity.WatchedAttributes.SetBool("stuck", true);
			this.entity.CollidedHorizontally = true;
			this.entity.CollidedVertically = true;
			if (block != null)
			{
				block.OnEntityCollide(this.entity.Api.World, this.entity, blockPosition, facing, pos.Motion, true);
			}
			pos.Motion *= 0f;
		}

		// Token: 0x040004A0 RID: 1184
		protected BlockPos MinPos = new BlockPos(0);

		// Token: 0x040004A1 RID: 1185
		protected BlockPos MaxPos = new BlockPos(0);

		// Token: 0x040004A2 RID: 1186
		protected BlockPos PosBuffer = new BlockPos(0);

		// Token: 0x040004A3 RID: 1187
		protected Cuboidd EntityBox = new Cuboidd();

		// Token: 0x040004A4 RID: 1188
		protected Settings ModSettings;

		// Token: 0x040004A5 RID: 1189
		[Nullable(2)]
		protected static FieldInfo EntityBehaviorPassivePhysics_airDragValue = typeof(EntityBehaviorPassivePhysics).GetField("airDragValue", 36);
	}
}
