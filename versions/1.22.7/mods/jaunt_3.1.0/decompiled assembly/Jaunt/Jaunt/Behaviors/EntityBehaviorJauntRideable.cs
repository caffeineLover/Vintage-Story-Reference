using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x02000013 RID: 19
	public class EntityBehaviorJauntRideable : EntityBehaviorRideable
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004FCB File Offset: 0x000031CB
		public bool CanFlyRidden
		{
			get
			{
				return this.RideableGaitOrder.ContainsKey(2);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004FD9 File Offset: 0x000031D9
		protected static JauntModSystem ModSystem
		{
			get
			{
				return JauntModSystem.Instance;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004FE0 File Offset: 0x000031E0
		protected static string AttributeKey
		{
			get
			{
				return EntityBehaviorJauntRideable.ModSystem.ModId + ":rideable";
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004FF6 File Offset: 0x000031F6
		public EntityBehaviorJauntRideable(Entity entity) : base(entity)
		{
			this.onGroundSinceMs = entity.World.ElapsedMilliseconds;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005010 File Offset: 0x00003210
		protected override IMountableSeat CreateSeat(string seatId, SeatConfig config)
		{
			EntityRideableSeat seat = new EntityRideableSeat(this, seatId, config);
			seat.controls.OnAction = delegate(EnumEntityAction action, bool on, ref EnumHandling handled)
			{
				this.OnJauntControls(seat, action, on, ref handled);
			};
			return seat;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000505C File Offset: 0x0000325C
		public void OnJauntControls(EntityRideableSeat seat, EnumEntityAction action, bool on, ref EnumHandling handled)
		{
			if (action == 5 && on && (this.entity.OnGround || this.entity.Swimming) && this.entity.World.ElapsedMilliseconds - this.onGroundSinceMs > 500L)
			{
				EntityAgent entityAgent = seat.Passenger as EntityAgent;
				if (entityAgent != null)
				{
					entityAgent.TryUnmount();
				}
				seat.controls.StopAllMovement();
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000050CC File Offset: 0x000032CC
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			this.smoothTurning = attributes["SmoothTurning"].AsBool(true);
			this.smoothTurningOnGround = attributes["smoothTurningConfig"]["onGround"].AsBool(false);
			this.smoothTurningInFlight = attributes["smoothTurningConfig"]["inFlight"].AsBool(true);
			this.baseAcceleration = attributes["smoothTurningConfig"]["baseAcceleration"].AsDouble(0.10000000149011612);
			this.yawMultiplierEffectiveness = attributes["smoothTurningConfig"]["yawMultiplierEffectiveness"].AsDouble(5.0);
			base.Initialize(properties, attributes);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000518C File Offset: 0x0000338C
		protected override bool IsSprinting()
		{
			return false;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005190 File Offset: 0x00003390
		public void AirToGround()
		{
			this.entity.Pos.Roll = 0f;
			this.eagent.Controls.IsFlying = false;
			this.eagent.Controls.Down = (this.eagent.Controls.Up = false);
			this.onGroundSinceMs = this.entity.World.ElapsedMilliseconds;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005200 File Offset: 0x00003400
		public override double SeatsToMotion(float dt)
		{
			double angularMotion = base.SeatsToMotion(dt);
			foreach (IMountableSeat seat in base.Seats)
			{
				if (base.Controller == seat.Passenger)
				{
					EntityControls controls = seat.Controls;
					if (base.RemainingSaddleBreaks > 0)
					{
						return angularMotion;
					}
					CanRideDelegate canRideEvent = (CanRideDelegate)typeof(EntityBehaviorRideable).GetField("CanRide", 36).GetValue(this);
					if (canRideEvent != null && (controls.Jump || controls.TriesToMove))
					{
						using (IEnumerator<CanRideDelegate> enumerator = Enumerable.Cast<CanRideDelegate>(canRideEvent.GetInvocationList()).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string errMsg;
								if (!enumerator.Current.Invoke(seat, ref errMsg))
								{
									return angularMotion;
								}
							}
						}
					}
					this.jumpNow = false;
					if (controls.Jump)
					{
						if (this.entity.OnGround || this.coyoteTimer > 0f)
						{
							this.lastJumpMs = this.entity.World.ElapsedMilliseconds;
							this.jumpNow = true;
						}
						else if (this.CanFlyRidden && (double)this.coyoteTimer < -0.15)
						{
							this.eagent.Controls.IsFlying = true;
						}
					}
					if (this.eagent.Controls.IsFlying)
					{
						EntityBehaviorJauntGait ebjg = this.ebg as EntityBehaviorJauntGait;
						if (ebjg != null)
						{
							this.eagent.Controls.Down = (controls.Sneak && !controls.Jump && this.entity.Pos.Y > 0.0);
							this.eagent.Controls.Up = (controls.Jump && !controls.Sneak && this.entity.Pos.Y < (double)(this.entity.World.BlockAccessor.MapSizeY - 1));
							JauntGaitMeta currentJauntGait = this.ebg.CurrentGait as JauntGaitMeta;
							float verticalMoveSpeed = Math.Min(0.2f, dt) * controls.MovespeedMultiplier / 2f;
							ebjg.VerticalSpeed = (double)(verticalMoveSpeed * (this.eagent.Controls.Up ? ((currentJauntGait != null) ? currentJauntGait.AscendSpeed : 1f) : (this.eagent.Controls.Down ? (-((currentJauntGait != null) ? currentJauntGait.DescendSpeed : 5f)) : 0f)));
							if (this.eagent.Controls.Down && !(PosUtil.GetBlockBelow(this.entity.World.BlockAccessor, this.entity.Pos.AsBlockPos, 1, 0).Code == "air"))
							{
								this.AirToGround();
							}
						}
					}
				}
			}
			return angularMotion;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000054F8 File Offset: 0x000036F8
		public void OnGaitChangedForEnvironment()
		{
			this.BehaviorGait_OnGaitChangedForEnvironment();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005500 File Offset: 0x00003700
		protected override void UpdateAngleAndMotion(float dt)
		{
			dt = Math.Min(0.5f, dt);
			bool IsFlying = this.eagent.Controls.IsFlying;
			if (this.smoothTurning && ((IsFlying && this.smoothTurningInFlight) || (!IsFlying && this.smoothTurningOnGround)))
			{
				double target = this.SeatsToMotion(GlobalConstants.PhysicsFrameTime);
				float yawMultiplier = this.eagent.GetBehavior<EntityBehaviorGait>().CurrentGait.YawMultiplier;
				double response = this.baseAcceleration + (double)yawMultiplier * this.yawMultiplierEffectiveness;
				this.smoothedYawDelta += (target - this.smoothedYawDelta) * response * (double)dt;
				float yawStep = (float)(this.smoothedYawDelta * (double)dt * 30.0);
				this.entity.Pos.Yaw += yawStep;
				this.entity.Pos.Yaw %= 6.2831855f;
				this.ebg.AngularVelocity = (double)yawStep;
				return;
			}
			base.UpdateAngleAndMotion(dt);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005600 File Offset: 0x00003800
		public override void GetInfoText(StringBuilder infotext)
		{
			if (base.RemainingSaddleBreaks > 0)
			{
				infotext.AppendLine(Lang.Get("jaunt:infotext-saddlebreaks-remaining", new object[]
				{
					base.RemainingSaddleBreaks
				}));
				if (this.entity.World.Calendar.TotalDays - base.LastSaddleBreakTotalDays > (double)this.saddleBreakDayInterval)
				{
					infotext.AppendLine(Lang.Get("jaunt:infotext-saddlebreak-ready", Array.Empty<object>()));
					return;
				}
				if ((double)this.saddleBreakDayInterval - (this.entity.World.Calendar.TotalDays - base.LastSaddleBreakTotalDays) >= 1.0)
				{
					infotext.AppendLine(Lang.Get("jaunt:infotext-saddlebreak-cooldown-days", new object[]
					{
						Math.Round((double)this.saddleBreakDayInterval - (this.entity.World.Calendar.TotalDays - base.LastSaddleBreakTotalDays), 1)
					}));
					return;
				}
				infotext.AppendLine(Lang.Get("jaunt:infotext-saddlebreak-cooldown-hours", new object[]
				{
					Math.Round(24.0 * ((double)this.saddleBreakDayInterval - (this.entity.World.Calendar.TotalDays - base.LastSaddleBreakTotalDays)), 1)
				}));
			}
		}

		// Token: 0x0400005C RID: 92
		protected long onGroundSinceMs;

		// Token: 0x0400005D RID: 93
		private bool smoothTurning;

		// Token: 0x0400005E RID: 94
		private bool smoothTurningOnGround;

		// Token: 0x0400005F RID: 95
		private bool smoothTurningInFlight;

		// Token: 0x04000060 RID: 96
		private double yawMultiplierEffectiveness;

		// Token: 0x04000061 RID: 97
		private double baseAcceleration;

		// Token: 0x04000062 RID: 98
		private double smoothedYawDelta;
	}
}
