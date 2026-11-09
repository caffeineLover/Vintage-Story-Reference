using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public sealed class ParticleGeneric : ParticleBase
{
	private static EvolvingNatFloat AccelerationX = EvolvingNatFloat.create(EnumTransformFunction.SINUS, (float)Math.PI * 2f);

	private static EvolvingNatFloat AccelerationZ = EvolvingNatFloat.create(EnumTransformFunction.COSINUS, 7.539823f);

	public FastVec3f StartingVelocity = new FastVec3f(0f, 0f, 0f);

	public FastVec3f ParentVelocity;

	public float ParentVelocityWeight;

	public float SizeMultiplier = 1f;

	public float ParticleHeight;

	public EvolvingNatFloat SizeEvolve = EvolvingNatFloat.NoValueSet;

	public EvolvingNatFloat[] VelocityEvolve;

	public EvolvingNatFloat OpacityEvolve = EvolvingNatFloat.NoValueSet;

	public EvolvingNatFloat GreenEvolve = EvolvingNatFloat.NoValueSet;

	public EvolvingNatFloat RedEvolve = EvolvingNatFloat.NoValueSet;

	public EvolvingNatFloat BlueEvolve = EvolvingNatFloat.NoValueSet;

	public int LightEmission;

	public float GravityStrength;

	private SmallBoolArray bools;

	public IParticlePropertiesProvider[] SecondaryParticles;

	public float[] SecondarySpawnTimers;

	public IParticlePropertiesProvider[] DeathParticles;

	private uint dirNormalized;

	private float seq;

	private float dir = 1f;

	public bool TerrainCollision
	{
		get
		{
			return bools[0];
		}
		set
		{
			bools[0] = value;
		}
	}

	public bool SelfPropelled
	{
		get
		{
			return bools[1];
		}
		set
		{
			bools[1] = value;
		}
	}

	public bool DieInLiquid
	{
		get
		{
			return bools[2];
		}
		set
		{
			bools[2] = value;
		}
	}

	public bool DieInAir
	{
		get
		{
			return bools[3];
		}
		set
		{
			bools[3] = value;
		}
	}

	public bool DieOnRainHeightmap
	{
		get
		{
			return bools[4];
		}
		set
		{
			bools[4] = value;
		}
	}

	public bool SwimOnLiquid
	{
		get
		{
			return bools[5];
		}
		set
		{
			bools[5] = value;
		}
	}

	public bool RandomVelocityChange
	{
		get
		{
			return bools[6];
		}
		set
		{
			bools[6] = value;
		}
	}

	private byte dirNormalizedX
	{
		get
		{
			return (byte)dirNormalized;
		}
		set
		{
			dirNormalized = (dirNormalized & 0xFFFF00) + value;
		}
	}

	private byte dirNormalizedY
	{
		get
		{
			return (byte)(dirNormalized >> 8);
		}
		set
		{
			dirNormalized = (dirNormalized & 0xFF00FF) + (uint)(value << 8);
		}
	}

	private byte dirNormalizedZ
	{
		get
		{
			return (byte)(dirNormalized >> 16);
		}
		set
		{
			dirNormalized = (dirNormalized & 0xFFFF) + (uint)(value << 16);
		}
	}

	public ParticleGeneric()
	{
		SecondarySpawnTimers = new float[4];
	}

	public override void TickNow(float lifedt, float pdt, ICoreClientAPI api, ParticlePhysics physicsSim)
	{
		SecondsAlive += lifedt;
		if (SecondaryParticles != null)
		{
			for (int i = 0; i < SecondaryParticles.Length; i++)
			{
				SecondarySpawnTimers[i] += pdt;
				IParticlePropertiesProvider particleProps = SecondaryParticles[i];
				if (SecondarySpawnTimers[i] > particleProps.SecondarySpawnInterval)
				{
					SecondarySpawnTimers[i] = 0f;
					particleProps.PrepareForSecondarySpawn(this);
					api.World.SpawnParticles(particleProps);
				}
			}
		}
		if (TerrainCollision && SelfPropelled)
		{
			Velocity.X += (StartingVelocity.X - Velocity.X) * 0.02f;
			Velocity.Y += (StartingVelocity.Y - Velocity.Y) * 0.02f;
			Velocity.Z += (StartingVelocity.Z - Velocity.Z) * 0.02f;
		}
		Velocity.Y -= GravityStrength * pdt;
		float height = ParticleHeight * SizeMultiplier;
		physicsSim.HandleBoyancy(Position, Velocity, SwimOnLiquid, GravityStrength, pdt, height);
		if (VelocityEvolve != null)
		{
			float relLife = SecondsAlive / LifeLength;
			motion.Set(Velocity.X * VelocityEvolve[0].nextFloat(0f, relLife) * pdt, Velocity.Y * VelocityEvolve[1].nextFloat(0f, relLife) * pdt, Velocity.Z * VelocityEvolve[2].nextFloat(0f, relLife) * pdt);
		}
		else
		{
			motion.Set(Velocity.X * pdt, Velocity.Y * pdt, Velocity.Z * pdt);
		}
		if (ParentVelocity.NotZero())
		{
			motion.Add(ParentVelocity.X * ParentVelocityWeight * pdt * tdragnow, ParentVelocity.Y * ParentVelocityWeight * pdt * tdragnow, ParentVelocity.Z * ParentVelocityWeight * pdt * tdragnow);
		}
		if (TerrainCollision)
		{
			updatePositionWithCollision(pdt, api, physicsSim, height);
		}
		else
		{
			Position.X += motion.X;
			Position.Y += motion.Y;
			Position.Z += motion.Z;
		}
		if (RandomVelocityChange)
		{
			if (seq > 0f)
			{
				Velocity.X += dir * AccelerationX.nextFloat(0f, seq) * pdt * 4f * SizeMultiplier;
				Velocity.Z += AccelerationZ.nextFloat(0f, seq) * pdt * 3f * SizeMultiplier;
				Velocity.Y += (dir * AccelerationX.nextFloat(0f, seq) * pdt * 10f * SizeMultiplier - dir * AccelerationZ.nextFloat(0f, seq) * pdt * 3f * SizeMultiplier) / 10f;
				seq += pdt / 3f;
				if (seq > 2f)
				{
					seq = 0f;
				}
				if (api.World.Rand.NextDouble() < 0.005)
				{
					seq = 0f;
				}
			}
			else
			{
				Velocity.X += (StartingVelocity.X - Velocity.X) * pdt;
				Velocity.Z += (StartingVelocity.Z - Velocity.Z) * pdt;
			}
			if (api.World.Rand.NextDouble() < 0.005)
			{
				seq = (float)api.World.Rand.NextDouble() * 0.5f;
				dir = api.World.Rand.Next(2) * 2 - 1;
			}
		}
		Alive = SecondsAlive < LifeLength && (!DieInAir || physicsSim.BlockAccess.GetBlockRaw((int)Position.X, (int)(Position.Y + 0.15000000596046448), (int)Position.Z, 2).IsLiquid());
		tickCount++;
		if (tickCount > 2)
		{
			lightrgbs = ((LightEmission == int.MaxValue) ? LightEmission : physicsSim.BlockAccess.GetLightRGBsAsInt((int)Position.X, (int)Position.Y, (int)Position.Z));
			if (LightEmission != 0)
			{
				lightrgbs = Math.Max(lightrgbs & 0xFF, LightEmission & 0xFF) | Math.Max(lightrgbs & 0xFF00, LightEmission & 0xFF00) | Math.Max(lightrgbs & 0xFF0000, LightEmission & 0xFF0000);
			}
			if (DieOnRainHeightmap)
			{
				float f = 1f - prevPosAdvance;
				double veloX = prevPosDeltaX * f * pdt * 8f;
				double veloY = prevPosDeltaY * f * pdt * 12f;
				double veloZ = prevPosDeltaZ * f * pdt * 8f;
				Alive &= (double)physicsSim.BlockAccess.GetRainMapHeightAt((int)(Position.X + veloX), (int)(Position.Z + veloZ)) - veloY < Position.Y;
			}
			Alive &= !DieInLiquid || !physicsSim.BlockAccess.GetBlockRaw((int)Position.X, (int)(Position.Y + 0.15000000596046448), (int)Position.Z, 2).IsLiquid();
			tickCount = 0;
			float len = Velocity.Length();
			dirNormalizedX = (byte)(Velocity.X / len * 128f);
			dirNormalizedY = (byte)(Velocity.Y / len * 128f);
			dirNormalizedZ = (byte)(Velocity.Z / len * 128f);
		}
		if (!Alive && DeathParticles != null)
		{
			for (int j = 0; j < DeathParticles.Length; j++)
			{
				IParticlePropertiesProvider particleProps2 = DeathParticles[j];
				particleProps2.PrepareForSecondarySpawn(this);
				api.World.SpawnParticles(particleProps2);
			}
		}
	}

	public override void UpdateBuffers(MeshData buffer, Vec3d cameraPos, ref int posPosition, ref int rgbaPosition, ref int flagPosition)
	{
		float relLife = SecondsAlive / LifeLength;
		byte[] values = buffer.CustomBytes.Values;
		float[] values2 = buffer.CustomFloats.Values;
		float f = 1f - prevPosAdvance;
		values2[posPosition++] = (float)(Position.X - (double)(prevPosDeltaX * f) - cameraPos.X);
		values2[posPosition++] = (float)(Position.Y - (double)(prevPosDeltaY * f) - cameraPos.Y);
		values2[posPosition++] = (float)(Position.Z - (double)(prevPosDeltaZ * f) - cameraPos.Z);
		values2[posPosition++] = ((SizeEvolve != EvolvingNatFloat.NoValueSet) ? SizeEvolve.nextFloat(SizeMultiplier, relLife) : SizeMultiplier);
		byte alpha = ColorAlpha;
		if (OpacityEvolve != EvolvingNatFloat.NoValueSet)
		{
			alpha = (byte)GameMath.Clamp(OpacityEvolve.nextFloat((int)alpha, relLife), 0f, 255f);
		}
		values[rgbaPosition++] = dirNormalizedX;
		values[rgbaPosition++] = dirNormalizedY;
		values[rgbaPosition++] = dirNormalizedZ;
		rgbaPosition++;
		values[rgbaPosition++] = (byte)lightrgbs;
		values[rgbaPosition++] = (byte)(lightrgbs >> 8);
		values[rgbaPosition++] = (byte)(lightrgbs >> 16);
		values[rgbaPosition++] = (byte)(lightrgbs >> 24);
		values[rgbaPosition++] = (byte)((float)(int)ColorBlue + BlueEvolve.nextFloat((int)ColorBlue, relLife));
		values[rgbaPosition++] = (byte)((float)(int)ColorGreen + GreenEvolve.nextFloat((int)ColorGreen, relLife));
		values[rgbaPosition++] = (byte)((float)(int)ColorRed + RedEvolve.nextFloat((int)ColorRed, relLife));
		values[rgbaPosition++] = alpha;
		buffer.Flags[flagPosition++] = VertexFlags;
	}

	public void Spawned(ParticlePhysics physicsSim)
	{
		Alive = true;
		SecondsAlive = 0f;
		accum = physicsSim.PhysicsTickTime;
		lightrgbs = physicsSim.BlockAccess.GetLightRGBsAsInt((int)Position.X, (int)Position.Y, (int)Position.Z);
		if (SecondaryParticles != null)
		{
			for (int i = 0; i < SecondaryParticles.Length; i++)
			{
				SecondarySpawnTimers[i] = 0f;
			}
		}
	}
}
