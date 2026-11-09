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
	// Token: 0x02000030 RID: 48
	public class AiTaskMeleeAttackCrab : AiTaskMeleeAttack
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0001A7FC File Offset: 0x000189FC
		public AiTaskMeleeAttackCrab(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0001A874 File Offset: 0x00018A74
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

		// Token: 0x0600023C RID: 572 RVA: 0x0001AA00 File Offset: 0x00018C00
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
			}
			return this.lastCheckOrAttackMs + (long)this.attackDurationMs > this.entity.World.ElapsedMilliseconds;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0001AB9C File Offset: 0x00018D9C
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

		// Token: 0x040000E9 RID: 233
		private Entity targetEntity;

		// Token: 0x040000EA RID: 234
		private long lastCheckOrAttackMs;

		// Token: 0x040000EB RID: 235
		private readonly float damage = 3f;

		// Token: 0x040000EC RID: 236
		private readonly float minDist = 1.5f;

		// Token: 0x040000ED RID: 237
		private readonly float minVerDist = 1.5f;

		// Token: 0x040000EE RID: 238
		private bool damageInflicted;

		// Token: 0x040000EF RID: 239
		private readonly int attackDurationMs = 500;

		// Token: 0x040000F0 RID: 240
		private readonly int damagePlayerAtMs = 300;

		// Token: 0x040000F1 RID: 241
		private readonly string[] seekEntityCodesExact = new string[]
		{
			"player"
		};

		// Token: 0x040000F2 RID: 242
		private readonly string[] seekEntityCodesBeginsWith = new string[0];

		// Token: 0x040000F3 RID: 243
		private readonly float tamingGenerations = 10f;

		// Token: 0x040000F4 RID: 244
		private float curTurnRadPerSec;
	}
}
