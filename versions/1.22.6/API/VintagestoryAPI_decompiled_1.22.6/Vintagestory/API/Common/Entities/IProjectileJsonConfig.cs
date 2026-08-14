namespace Vintagestory.API.Common.Entities;

public interface IProjectileJsonConfig
{
	/// <summary>
	/// Projectile damage when hitting an entity.
	/// </summary>
	float Damage { get; }

	/// <summary>
	/// Projectile damage tier when hitting an entity.
	/// </summary>
	int DamageTier { get; }

	/// <summary>
	/// Projectile damage type when hitting an entity.
	/// </summary>
	EnumDamageType DamageType { get; }

	/// <summary>
	/// If damage source done by projectile will ignore invincibility frames.
	/// </summary>
	bool IgnoreInvFrames { get; }

	/// <summary>
	/// Chance for projectile to not be destroyed on impact.
	/// </summary>
	float DropOnImpactChance { get; }

	/// <summary>
	/// How much <see cref="P:Vintagestory.API.Common.Entities.IProjectile.ProjectileStack" /> should be damage on impact.
	/// </summary>
	bool DamageStackOnImpact { get; }

	/// <summary>
	/// If projectile can be collected when stuck.
	/// </summary>
	bool Collectible { get; }

	/// <summary>
	/// Projectile weight used for knockback calculations.
	/// </summary>
	float Weight { get; }
}
