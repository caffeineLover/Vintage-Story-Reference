using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C1 RID: 193
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ProjectileMotionContext : IEquatable<ProjectileMotionContext>
	{
		// Token: 0x06000A07 RID: 2567 RVA: 0x00036151 File Offset: 0x00034351
		public ProjectileMotionContext(ProjectileEntity Projectile, EntityPos Position, float FlightSeconds)
		{
			this.Projectile = Projectile;
			this.Position = Position;
			this.FlightSeconds = FlightSeconds;
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00036168 File Offset: 0x00034368
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x00036170 File Offset: 0x00034370
		public ProjectileEntity Projectile { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00036179 File Offset: 0x00034379
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00036181 File Offset: 0x00034381
		public EntityPos Position { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0003618A File Offset: 0x0003438A
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x00036192 File Offset: 0x00034392
		public float FlightSeconds { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0003619B File Offset: 0x0003439B
		public IWorldAccessor World
		{
			get
			{
				return this.Projectile.World;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x000361A8 File Offset: 0x000343A8
		[Nullable(2)]
		public Entity Shooter
		{
			[NullableContext(2)]
			get
			{
				return this.World.GetEntityById(this.Projectile.ShooterId);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x000361C0 File Offset: 0x000343C0
		[Nullable(2)]
		public Entity Owner
		{
			[NullableContext(2)]
			get
			{
				return this.World.GetEntityById(this.Projectile.OwnerId);
			}
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x000361D8 File Offset: 0x000343D8
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProjectileMotionContext");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00036224 File Offset: 0x00034424
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Projectile = ");
			builder.Append(this.Projectile);
			builder.Append(", Position = ");
			builder.Append(this.Position);
			builder.Append(", FlightSeconds = ");
			builder.Append(this.FlightSeconds.ToString());
			builder.Append(", World = ");
			builder.Append(this.World);
			builder.Append(", Shooter = ");
			builder.Append(this.Shooter);
			builder.Append(", Owner = ");
			builder.Append(this.Owner);
			return true;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000362D6 File Offset: 0x000344D6
		[CompilerGenerated]
		public static bool operator !=(ProjectileMotionContext left, ProjectileMotionContext right)
		{
			return !(left == right);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x000362E2 File Offset: 0x000344E2
		[CompilerGenerated]
		public static bool operator ==(ProjectileMotionContext left, ProjectileMotionContext right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000362EC File Offset: 0x000344EC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<ProjectileEntity>.Default.GetHashCode(this.<Projectile>k__BackingField) * -1521134295 + EqualityComparer<EntityPos>.Default.GetHashCode(this.<Position>k__BackingField)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(this.<FlightSeconds>k__BackingField);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0003632C File Offset: 0x0003452C
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is ProjectileMotionContext && this.Equals((ProjectileMotionContext)obj);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00036344 File Offset: 0x00034544
		[CompilerGenerated]
		public bool Equals(ProjectileMotionContext other)
		{
			return EqualityComparer<ProjectileEntity>.Default.Equals(this.<Projectile>k__BackingField, other.<Projectile>k__BackingField) && EqualityComparer<EntityPos>.Default.Equals(this.<Position>k__BackingField, other.<Position>k__BackingField) && EqualityComparer<float>.Default.Equals(this.<FlightSeconds>k__BackingField, other.<FlightSeconds>k__BackingField);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00036399 File Offset: 0x00034599
		[CompilerGenerated]
		public void Deconstruct(out ProjectileEntity Projectile, out EntityPos Position, out float FlightSeconds)
		{
			Projectile = this.Projectile;
			Position = this.Position;
			FlightSeconds = this.FlightSeconds;
		}
	}
}
