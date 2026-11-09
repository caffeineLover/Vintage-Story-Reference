using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C4 RID: 196
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CurvedFlightBehavior : EntityBehavior, IProjectileMotionModifier
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x00036479 File Offset: 0x00034679
		public CurvedFlightBehavior(Entity entity) : base(entity)
		{
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0003648D File Offset: 0x0003468D
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x00036495 File Offset: 0x00034695
		public CurvedFlightConfig Config { get; private set; } = new CurvedFlightConfig();

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003649E File Offset: 0x0003469E
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000364A2 File Offset: 0x000346A2
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			this.Config = (attributes.AsObject<CurvedFlightConfig>(null) ?? new CurvedFlightConfig());
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000364C4 File Offset: 0x000346C4
		public void ModifyMotion(ProjectileMotionContext context, float dt)
		{
			if (dt <= 0f || context.Projectile.Stuck)
			{
				return;
			}
			if (this.Config.ReturnAfterSeconds >= 0f && context.FlightSeconds >= this.Config.ReturnAfterSeconds)
			{
				this.ApplyReturnMotion(context, dt);
				return;
			}
			if (context.FlightSeconds < Math.Max(0f, this.Config.StartAfterSeconds))
			{
				return;
			}
			CurvedFlightMath.RotateHorizontal(context.Position.Motion, Math.Abs(this.Config.HorizontalTurnRate), this.Config.Direction, dt);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0003656B File Offset: 0x0003476B
		public override string PropertyName()
		{
			return "combatOverhaulCurvedFlight";
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00036574 File Offset: 0x00034774
		private void ApplyReturnMotion(ProjectileMotionContext context, float dt)
		{
			Entity owner = context.Owner;
			if (owner == null || !owner.Alive)
			{
				return;
			}
			Vec3d toOwner = new Vec3d(owner.ServerPos.X, owner.ServerPos.Y + (double)this.Config.ReturnTargetHeight, owner.ServerPos.Z).SubCopy(context.Position.XYZ);
			double distance = toOwner.Length();
			if (this.Config.CatchRadius > 0f && distance <= (double)this.Config.CatchRadius && ProjectileCatch.TryCatch(context.Projectile, owner))
			{
				return;
			}
			if (distance <= 1E-06)
			{
				return;
			}
			double currentSpeed = context.Position.Motion.Length();
			double targetSpeed = (this.Config.MaximumReturnSpeed > 0f) ? ((double)this.Config.MaximumReturnSpeed) : currentSpeed;
			if (targetSpeed <= 1E-06)
			{
				return;
			}
			Vec3d desiredVelocity = toOwner.Mul(targetSpeed / distance);
			CurvedFlightMath.SteerToward(context.Position.Motion, desiredVelocity, (double)(Math.Max(0f, this.Config.ReturnAcceleration) * dt));
		}
	}
}
