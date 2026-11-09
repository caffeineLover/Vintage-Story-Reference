using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011E RID: 286
	[NullableContext(1)]
	[Nullable(0)]
	public static class DebugParticleSpawner
	{
		// Token: 0x06000DD2 RID: 3538 RVA: 0x00042814 File Offset: 0x00040A14
		public static void SpawnDebugBlockParticles(ICoreServerAPI api, Entity player, DamageBlockStats stats)
		{
			Vector3d center = (player.Pos.XYZ + player.LocalEyePos).ToOpenTK();
			Vector3d viewDirection = player.Pos.GetViewVector().ToOpenTK();
			foreach (ValueTuple<Vector3d, DirectionOffset> valueTuple in DebugParticleSpawner.GetSurroundingEntitiesWithDirections(api, player, 8f))
			{
				Vector3d position = valueTuple.Item1;
				DirectionOffset direction = valueTuple.Item2;
				Color4 color = stats.Directions.Check(direction) ? Color4.Green : Color4.Red;
				DebugParticleSpawner.SpawnParticlesRay(api, color, center, position, 25, 5, 0.5f);
			}
			Color4 bordersColor = Color4.Yellow;
			DirectionConstrain constraint = stats.Directions.Expand(Angle.FromDegrees(0f));
			DebugParticleSpawner.SpawnParticlesArc(api, bordersColor, viewDirection, center, new DirectionOffset(constraint.PitchTop, constraint.YawRight), new DirectionOffset(constraint.PitchTop, constraint.YawLeft), 16, 0, 0.5f);
			DebugParticleSpawner.SpawnParticlesArc(api, bordersColor, viewDirection, center, new DirectionOffset(constraint.PitchBottom, constraint.YawRight), new DirectionOffset(constraint.PitchBottom, constraint.YawLeft), 16, 0, 0.5f);
			DebugParticleSpawner.SpawnParticlesArc(api, bordersColor, viewDirection, center, new DirectionOffset(constraint.PitchTop, constraint.YawRight), new DirectionOffset(constraint.PitchBottom, constraint.YawRight), 16, 0, 0.5f);
			DebugParticleSpawner.SpawnParticlesArc(api, bordersColor, viewDirection, center, new DirectionOffset(constraint.PitchTop, constraint.YawLeft), new DirectionOffset(constraint.PitchBottom, constraint.YawLeft), 16, 0, 0.5f);
			DebugParticleSpawner.SpawnParticlesArc(api, bordersColor, viewDirection, center, new DirectionOffset(constraint.PitchTop, constraint.YawRight), new DirectionOffset(constraint.PitchBottom, constraint.YawLeft), 32, 0, 0.2f);
			DebugParticleSpawner.SpawnParticlesArc(api, bordersColor, viewDirection, center, new DirectionOffset(constraint.PitchBottom, constraint.YawRight), new DirectionOffset(constraint.PitchTop, constraint.YawLeft), 32, 0, 0.2f);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00042A24 File Offset: 0x00040C24
		[NullableContext(0)]
		private static ValueTuple<Vector3d, Vector3d, Vector3d, Vector3d> GetConstraintVertices(DirectionConstrain constraint, Vector3d direction)
		{
			Vector3d[] result = DebugParticleSpawner.BuildConstraintVectors(direction, (double)constraint.YawLeft.Radians, (double)constraint.YawRight.Radians, (double)constraint.PitchBottom.Radians, (double)constraint.PitchTop.Radians);
			return new ValueTuple<Vector3d, Vector3d, Vector3d, Vector3d>(result[0], result[1], result[3], result[2]);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00042A90 File Offset: 0x00040C90
		private static Vector3d[] BuildConstraintVectors(Vector3d direction, double h0, double h1, double v0, double v1)
		{
			direction = direction.Normalized();
			Vector3d up = Vector3d.UnitY;
			Vector3d right = Vector3d.Cross(up, direction);
			if (right.LengthSquared < 1E-12)
			{
				right = Vector3d.UnitX;
			}
			right.Normalize();
			up = Vector3d.Cross(direction, right).Normalized();
			Vector3d[] result = new Vector3d[4];
			int i = 0;
			foreach (double h2 in new double[]
			{
				h0,
				h1
			})
			{
				foreach (double v2 in new double[]
				{
					v0,
					v1
				})
				{
					Vector3d vec = direction + Math.Tan(h2) * right + Math.Tan(v2) * up;
					result[i++] = vec.Normalized();
				}
			}
			return result;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00042B7C File Offset: 0x00040D7C
		private static void SpawnParticlesRay(ICoreServerAPI api, Color4 color, Vector3d start, Vector3d end, int particlesNumber, int startFrom = 1, float size = 0.5f)
		{
			Vector3d direction = end - start;
			for (int i = startFrom; i <= particlesNumber; i++)
			{
				Vector3d particlePosition = start + direction * (double)i / (double)particlesNumber;
				DebugParticleSpawner.SpawnDebugParticle(api, color, particlePosition, size);
			}
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00042BC0 File Offset: 0x00040DC0
		private static void SpawnParticlesArc(ICoreServerAPI api, DirectionConstrain constraint, Vector3d view, Vector3d center, Vector3d start, Vector3d end, int particlesNumber, int startFrom = 1, float size = 0.5f)
		{
			Vector3d direction = end - start;
			for (int i = startFrom; i <= particlesNumber; i++)
			{
				Vector3d particlePosition = start + direction * (double)i / (double)particlesNumber;
				particlePosition = Vector3d.Normalize(particlePosition - center) + center;
				Vector3d particleDirection = Vector3d.Normalize(particlePosition - center);
				DirectionOffset offset = DirectionOffset.GetDirectionWithRespectToCamera(view, particleDirection);
				Color4 color = constraint.Check(offset) ? Color4.Green : Color4.Red;
				DebugParticleSpawner.SpawnDebugParticle(api, color, particlePosition, size);
			}
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00042C4C File Offset: 0x00040E4C
		private static void SpawnParticlesArc(ICoreServerAPI api, Color4 color, Vector3d view, Vector3d center, DirectionOffset start, DirectionOffset end, int particlesNumber, int startFrom = 1, float size = 0.5f)
		{
			DirectionOffset direction = end - start;
			for (int i = startFrom; i <= particlesNumber; i++)
			{
				DirectionOffset particleAngles = start + direction * (float)i / (float)particlesNumber;
				Vector3d particleDirection = DirectionOffset.GetDirectionVectorWithRespectToWorld(view, DirectionOffset.DirectionFromYawPitch(particleAngles));
				Vector3d particlePosition = center + particleDirection;
				DebugParticleSpawner.SpawnDebugParticle(api, color, particlePosition, size);
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00042CAC File Offset: 0x00040EAC
		private static void SpawnParticlesArc(ICoreServerAPI api, Color4 color, Vector3d center, Vector3d start, Vector3d end, int particlesNumber, int startFrom = 1, float size = 0.5f)
		{
			Vector3d direction = end - start;
			for (int i = startFrom; i <= particlesNumber; i++)
			{
				Vector3d particlePosition = start + direction * (double)i / (double)particlesNumber;
				particlePosition = Vector3d.Normalize(particlePosition - center) + center;
				DebugParticleSpawner.SpawnDebugParticle(api, color, particlePosition, size);
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00042D04 File Offset: 0x00040F04
		private static void SpawnDebugParticle(ICoreServerAPI api, Color4 color, Vector3d position, float size = 0.5f)
		{
			api.World.SpawnParticles(1f, DebugParticleSpawner.ToVanillaParticleColor(color), position.ToVanilla().ToVec3d(), position.ToVanilla().ToVec3d(), new Vec3f(), new Vec3f(), 0.008f, 0f, size, 1, null);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00042D5A File Offset: 0x00040F5A
		private static int ToVanillaParticleColor(Color4 color)
		{
			return ColorUtil.ColorFromRgba((int)(color.B * 255f), (int)(color.G * 255f), (int)(color.R * 255f), (int)(color.A * 255f));
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00042D98 File Offset: 0x00040F98
		[return: TupleElementNames(new string[]
		{
			"position",
			"direction"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0
		})]
		private static IEnumerable<ValueTuple<Vector3d, DirectionOffset>> GetSurroundingEntitiesWithDirections(ICoreServerAPI api, Entity player, float range)
		{
			return Enumerable.Select<Entity, ValueTuple<Vector3d, DirectionOffset>>(Enumerable.Where<Entity>(api.World.GetEntitiesAround(player.Pos.XYZ, range, range, (Entity entity) => entity.IsCreature), (Entity target) => target != player), (Entity target) => new ValueTuple<Vector3d, DirectionOffset>(DebugParticleSpawner.GetEyesPosition(target), DirectionOffset.GetDirectionWithRespectToCamera(player, target)));
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00042E10 File Offset: 0x00041010
		private static Vector3d GetEyesPosition(Entity target)
		{
			return (target.Pos.XYZ + target.LocalEyePos).ToOpenTK();
		}
	}
}
