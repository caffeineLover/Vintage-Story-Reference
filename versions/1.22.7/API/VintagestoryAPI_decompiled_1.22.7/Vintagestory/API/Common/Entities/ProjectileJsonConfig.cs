namespace Vintagestory.API.Common.Entities;

public class ProjectileJsonConfig : IProjectileJsonConfig
{
	/// <summary>
	/// Projectile damage when hitting an entity.
	/// </summary>
	public float Damage { get; set; } = 1f;

	/// <summary>
	/// Projectile damage tier when hitting an entity.
	/// </summary>
	public int DamageTier { get; set; }

	/// <summary>
	/// Projectile damage type when hitting an entity.
	/// </summary>
	public EnumDamageType DamageType { get; set; } = EnumDamageType.PiercingAttack;

	/// <summary>
	/// If damage source done by projectile will ignore invincibility frames.
	/// </summary>
	public bool IgnoreInvFrames { get; set; } = true;

	/// <summary>
	/// Chance for projectile to not be destroyed on impact.
	/// </summary>
	public float DropOnImpactChance { get; set; }

	/// <summary>
	/// How much <see cref="P:Vintagestory.API.Common.Entities.IProjectile.ProjectileStack" /> should be damage on impact.
	/// </summary>
	public bool DamageStackOnImpact { get; set; }

	/// <summary>
	/// If projectile can be collected when stuck.
	/// </summary>
	public bool Collectible { get; set; } = true;

	/// <summary>
	/// Projectile weight used for knockback calculations.
	/// </summary>
	public float Weight { get; set; } = -1f;
}
