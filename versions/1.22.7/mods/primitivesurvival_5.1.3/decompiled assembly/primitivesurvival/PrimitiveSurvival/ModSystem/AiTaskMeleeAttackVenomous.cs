using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000032 RID: 50
	public class AiTaskMeleeAttackVenomous : AiTaskMeleeAttack
	{
		// Token: 0x06000242 RID: 578 RVA: 0x0001AF38 File Offset: 0x00019138
		public AiTaskMeleeAttackVenomous(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			VenomState.Venomed = "";
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0001AFBC File Offset: 0x000191BC
		public override bool ShouldExecute()
		{
			bool result;
			try
			{
				long ellapsedMs = this.entity.World.ElapsedMilliseconds;
				if (ellapsedMs - this.lastCheckOrAttackMs < (long)this.attackDurationMs || this.cooldownUntilMs > ellapsedMs)
				{
					result = false;
				}
				else if (this.WhenInEmotionStates != null)
				{
					result = false;
				}
				else
				{
					Vec3d pos = this.entity.Pos.XYZ.Add(0.0, (double)(this.entity.CollisionBox.Y2 / 2f), 0.0).Ahead((double)(this.entity.CollisionBox.XSize / 2f), 0f, this.entity.Pos.Yaw);
					int generation = this.entity.WatchedAttributes.GetInt("generation", 0);
					float fearReductionFactor = Math.Max(0f, (this.tamingGenerations - (float)generation) / this.tamingGenerations);
					if (this.WhenInEmotionStates != null)
					{
						fearReductionFactor = 1f;
					}
					if (fearReductionFactor <= 0f)
					{
						result = false;
					}
					else
					{
						this.targetEntity = this.entity.World.GetNearestEntity(pos, 3f * fearReductionFactor, 3f * fearReductionFactor, delegate(Entity e)
						{
							if (e == null)
							{
								return false;
							}
							if (!e.Alive || !e.IsInteractable || e.EntityId == this.entity.EntityId)
							{
								return false;
							}
							for (int i = 0; i < this.seekEntityCodesExact.Length; i++)
							{
								if (e.Code.Path == this.seekEntityCodesExact[i])
								{
									if (e.Code.Path == "player")
									{
										IPlayer player = this.entity.World.PlayerByUid(((EntityPlayer)e).PlayerUID);
										return (player == null || (player.WorldData.CurrentGameMode != 2 && player.WorldData.CurrentGameMode != 3 && (player as IServerPlayer).ConnectionState == 4)) && this.HasDirectContact(e);
									}
									if (this.HasDirectContact(e))
									{
										return true;
									}
								}
							}
							for (int j = 0; j < this.seekEntityCodesBeginsWith.Length; j++)
							{
								if (StringUtil.StartsWithFast(e.Code.Path, this.seekEntityCodesBeginsWith[j]) && this.HasDirectContact(e))
								{
									return true;
								}
							}
							return false;
						});
						this.lastCheckOrAttackMs = this.entity.World.ElapsedMilliseconds;
						this.damageInflicted = false;
						result = (this.targetEntity != null);
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0001B148 File Offset: 0x00019348
		public override bool ContinueExecute(float dt)
		{
			EntityPos own = this.entity.Pos;
			EntityPos his = this.targetEntity.Pos;
			float desiredYaw = (float)Math.Atan2(his.X - own.X, his.Z - own.Z);
			float yawDist = GameMath.AngleRadDistance(this.entity.Pos.Yaw, desiredYaw);
			this.entity.Pos.Yaw += GameMath.Clamp(yawDist, -this.curTurnRadPerSec * dt * GlobalConstants.OverallSpeedMultiplier, this.curTurnRadPerSec * dt * GlobalConstants.OverallSpeedMultiplier);
			this.entity.Pos.Yaw %= 6.2831855f;
			if (this.lastCheckOrAttackMs + (long)this.damagePlayerAtMs > this.entity.World.ElapsedMilliseconds)
			{
				return true;
			}
			if (!this.damageInflicted)
			{
				if (!this.HasDirectContact(this.targetEntity))
				{
					return false;
				}
				bool alive = this.targetEntity.Alive;
				this.targetEntity.ReceiveDamage(new DamageSource
				{
					Source = 8,
					SourceEntity = this.entity,
					Type = this.damageType,
					DamageTier = this.damageTier
				}, this.damage * GlobalConstants.CreatureDamageModifier);
				if (alive && !this.targetEntity.Alive)
				{
					EntityBehaviorEmotionStates behavior = this.entity.GetBehavior<EntityBehaviorEmotionStates>();
					if (behavior != null)
					{
						behavior.TryTriggerState("saturated", 1L);
					}
				}
				this.damageInflicted = true;
				VenomState.Venomed = this.targetEntity.GetName();
			}
			return this.lastCheckOrAttackMs + (long)this.attackDurationMs > this.entity.World.ElapsedMilliseconds;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0001B2F4 File Offset: 0x000194F4
		private bool HasDirectContact(Entity targetEntity)
		{
			Cuboidd cuboidd = targetEntity.CollisionBox.ToDouble().Translate(targetEntity.Pos.X, targetEntity.Pos.Y, targetEntity.Pos.Z);
			Vec3d pos = this.entity.Pos.XYZ.Add(0.0, (double)(this.entity.CollisionBox.Y2 / 2f), 0.0).Ahead((double)(this.entity.CollisionBox.XSize / 2f), 0f, this.entity.Pos.Yaw);
			double dist = cuboidd.ShortestDistanceFrom(pos);
			double vertDist = Math.Abs(cuboidd.ShortestVerticalDistanceFrom(pos.Y));
			if (dist >= (double)this.minDist || vertDist >= (double)this.minVerDist)
			{
				return false;
			}
			Vec3d rayTraceFrom = this.entity.Pos.XYZ;
			rayTraceFrom.Y += 0.03125;
			Vec3d rayTraceTo = targetEntity.Pos.XYZ;
			rayTraceTo.Y += 0.03125;
			this.entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref this.blockSel, ref this.entitySel, null, null);
			bool directContact = this.blockSel == null;
			if (!directContact)
			{
				rayTraceFrom.Y += (double)(this.entity.CollisionBox.Y2 * 7f / 16f);
				rayTraceTo.Y += (double)(targetEntity.CollisionBox.Y2 * 7f / 16f);
				this.entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref this.blockSel, ref this.entitySel, null, null);
				directContact = (this.blockSel == null);
			}
			if (!directContact)
			{
				rayTraceFrom.Y += (double)(this.entity.CollisionBox.Y2 * 7f / 16f);
				rayTraceTo.Y += (double)(targetEntity.CollisionBox.Y2 * 7f / 16f);
				this.entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref this.blockSel, ref this.entitySel, null, null);
				directContact = (this.blockSel == null);
			}
			return directContact;
		}

		// Token: 0x040000F6 RID: 246
		private Entity targetEntity;

		// Token: 0x040000F7 RID: 247
		private long lastCheckOrAttackMs;

		// Token: 0x040000F8 RID: 248
		private readonly float damage = 4f;

		// Token: 0x040000F9 RID: 249
		private readonly float minDist = 1.5f;

		// Token: 0x040000FA RID: 250
		private readonly float minVerDist = 1f;

		// Token: 0x040000FB RID: 251
		private bool damageInflicted;

		// Token: 0x040000FC RID: 252
		private readonly int attackDurationMs = 1500;

		// Token: 0x040000FD RID: 253
		private readonly int damagePlayerAtMs = 500;

		// Token: 0x040000FE RID: 254
		private readonly string[] seekEntityCodesExact = new string[]
		{
			"player"
		};

		// Token: 0x040000FF RID: 255
		private readonly string[] seekEntityCodesBeginsWith = new string[0];

		// Token: 0x04000100 RID: 256
		private readonly float tamingGenerations = 10f;

		// Token: 0x04000101 RID: 257
		private float curTurnRadPerSec;
	}
}
