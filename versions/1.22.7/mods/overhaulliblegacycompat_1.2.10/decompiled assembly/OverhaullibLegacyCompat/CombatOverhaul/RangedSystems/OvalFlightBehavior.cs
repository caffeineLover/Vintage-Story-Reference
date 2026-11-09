using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C6 RID: 198
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class OvalFlightBehavior : EntityBehavior, IProjectileMotionModifier
	{
		// Token: 0x06000A41 RID: 2625 RVA: 0x00036751 File Offset: 0x00034951
		public OvalFlightBehavior(Entity entity) : base(entity)
		{
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00036770 File Offset: 0x00034970
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x00036778 File Offset: 0x00034978
		public OvalFlightConfig Config { get; private set; } = new OvalFlightConfig();

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00036781 File Offset: 0x00034981
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00036785 File Offset: 0x00034985
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			this.Config = (attributes.AsObject<OvalFlightConfig>(null) ?? new OvalFlightConfig());
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x000367A8 File Offset: 0x000349A8
		public void ModifyMotion(ProjectileMotionContext context, float dt)
		{
			if (dt <= 0f || context.Projectile.Stuck)
			{
				return;
			}
			if (!this._initialized && !this.InitializePath(context, dt))
			{
				return;
			}
			double duration = Math.Max(0.1, (double)this.Config.FlightDurationSeconds);
			double progress = GameMath.Clamp((double)(context.FlightSeconds - this._startFlightSeconds) / duration, 0.0, 1.0);
			double forwardOffset;
			double lateralOffset;
			OvalFlightMath.Sample(this.Config.MaximumDistance, this.Config.LateralRadius, progress, out forwardOffset, out lateralOffset);
			lateralOffset *= (double)Math.Sign((this.Config.Direction == 0) ? 1 : this.Config.Direction);
			Vec3d target = new Vec3d(this._origin.X + this._forwardX * forwardOffset + this._lateralX * lateralOffset, this._origin.Y, this._origin.Z + this._forwardZ * forwardOffset + this._lateralZ * lateralOffset);
			if (progress >= (double)GameMath.Clamp(this.Config.CatchAfterProgress, 0f, 1f) && this.TryCatchOnOwnerCollision(context, target))
			{
				return;
			}
			if (progress >= 1.0 && (double)context.Position.XYZ.SquareDistanceTo(target) <= 1E-06)
			{
				context.Position.Motion.Set(0.0, 0.0, 0.0);
				return;
			}
			double dtFactor = (double)(dt * 60f);
			context.Position.Motion.Set((target.X - context.Position.X) / dtFactor, (target.Y - context.Position.Y) / dtFactor, (target.Z - context.Position.Z) / dtFactor);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00036997 File Offset: 0x00034B97
		public override string PropertyName()
		{
			return "combatOverhaulOvalFlight";
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000369A0 File Offset: 0x00034BA0
		private bool InitializePath(ProjectileMotionContext context, float dt)
		{
			double horizontalLength = Math.Sqrt(context.Position.Motion.X * context.Position.Motion.X + context.Position.Motion.Z * context.Position.Motion.Z);
			if (horizontalLength <= 1E-08)
			{
				return false;
			}
			this._origin.Set(context.Position.X, context.Position.Y, context.Position.Z);
			this._forwardX = context.Position.Motion.X / horizontalLength;
			this._forwardZ = context.Position.Motion.Z / horizontalLength;
			this._lateralX = this._forwardZ;
			this._lateralZ = -this._forwardX;
			this._startFlightSeconds = Math.Max(0f, context.FlightSeconds - dt);
			this._initialized = true;
			return true;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00036AA4 File Offset: 0x00034CA4
		private bool TryCatchOnOwnerCollision(ProjectileMotionContext context, Vec3d target)
		{
			Entity owner = context.Owner;
			if (owner == null || !owner.Alive)
			{
				return false;
			}
			Cuboidf box = owner.CollisionBox;
			EntityPos ownerPosition = owner.Pos;
			double padding = (double)Math.Max(0f, context.Projectile.ColliderRadius + this.Config.CatchCollisionPadding);
			return OvalFlightMath.SegmentIntersectsAabb(context.Position.XYZ, target, (double)box.X1 + ownerPosition.X - padding, (double)box.Y1 + ownerPosition.Y - padding, (double)box.Z1 + ownerPosition.Z - padding, (double)box.X2 + ownerPosition.X + padding, (double)box.Y2 + ownerPosition.Y + padding, (double)box.Z2 + ownerPosition.Z + padding) && ProjectileCatch.TryCatch(context.Projectile, owner);
		}

		// Token: 0x040004B8 RID: 1208
		private readonly Vec3d _origin = new Vec3d();

		// Token: 0x040004B9 RID: 1209
		private double _forwardX;

		// Token: 0x040004BA RID: 1210
		private double _forwardZ;

		// Token: 0x040004BB RID: 1211
		private double _lateralX;

		// Token: 0x040004BC RID: 1212
		private double _lateralZ;

		// Token: 0x040004BD RID: 1213
		private float _startFlightSeconds;

		// Token: 0x040004BE RID: 1214
		private bool _initialized;
	}
}
