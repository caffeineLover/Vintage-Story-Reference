using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public abstract class EntityParticle : ParticleBase
{
	protected float dirNormalizedX;

	protected float dirNormalizedY;

	protected float dirNormalizedZ;

	protected bool SwimOnLiquid;

	public abstract string Type { get; }

	protected float Size
	{
		set
		{
			float num = (SizeZ = value);
			float sizeX = (SizeY = num);
			SizeX = sizeX;
		}
	}

	protected float SizeX { get; set; } = 1f;

	protected float SizeY { get; set; } = 1f;

	protected float SizeZ { get; set; } = 1f;

	protected float GravityStrength { get; set; } = 1f;

	public void OnSpawned(ParticlePhysics physicsSim)
	{
		lightrgbs = physicsSim.BlockAccess.GetLightRGBsAsInt((int)Position.X, (int)Position.Y, (int)Position.Z);
	}

	public override void TickNow(float dt, float physicsdt, ICoreClientAPI api, ParticlePhysics physicsSim)
	{
		motion.Set(Velocity.X * dt, Velocity.Y * dt, Velocity.Z * dt);
		float s = SizeY / 4f;
		updatePositionWithCollision(dt, api, physicsSim, s);
		Velocity.Y -= GravityStrength * dt;
		physicsSim.HandleBoyancy(Position, Velocity, SwimOnLiquid, GravityStrength, dt, s);
		tickCount++;
		if (tickCount > 2)
		{
			doSlowTick(physicsSim, dt * 3f);
		}
		if ((double)Velocity.Length() > 0.05)
		{
			dirNormalizedX = Velocity.X;
			dirNormalizedY = Velocity.Y;
			dirNormalizedZ = Velocity.Z;
		}
	}

	protected virtual void doSlowTick(ParticlePhysics physicsSim, float dt)
	{
		lightrgbs = physicsSim.BlockAccess.GetLightRGBsAsInt((int)Position.X, (int)Position.Y, (int)Position.Z);
		tickCount = 0;
	}

	public override void UpdateBuffers(MeshData buffer, Vec3d cameraPos, ref int posPosition, ref int rgbaPosition, ref int flagPosition)
	{
		float f = 1f - prevPosAdvance;
		float[] CustomFloats = buffer.CustomFloats.Values;
		CustomFloats[posPosition++] = (float)(Position.X - (double)(prevPosDeltaX * f) - cameraPos.X);
		CustomFloats[posPosition++] = (float)(Position.Y - (double)(prevPosDeltaY * f) - cameraPos.Y);
		CustomFloats[posPosition++] = (float)(Position.Z - (double)(prevPosDeltaZ * f) - cameraPos.Z);
		CustomFloats[posPosition++] = SizeX;
		CustomFloats[posPosition++] = SizeY;
		CustomFloats[posPosition++] = SizeZ;
		posPosition = UpdateAngles(CustomFloats, posPosition);
		byte[] values = buffer.CustomBytes.Values;
		values[rgbaPosition++] = (byte)lightrgbs;
		values[rgbaPosition++] = (byte)(lightrgbs >> 8);
		values[rgbaPosition++] = (byte)(lightrgbs >> 16);
		values[rgbaPosition++] = (byte)(lightrgbs >> 24);
		values[rgbaPosition++] = ColorBlue;
		values[rgbaPosition++] = ColorGreen;
		values[rgbaPosition++] = ColorRed;
		values[rgbaPosition++] = ColorAlpha;
		buffer.Flags[flagPosition++] = VertexFlags;
	}

	public virtual int UpdateAngles(float[] customFloats, int posPosition)
	{
		customFloats[posPosition++] = dirNormalizedX;
		customFloats[posPosition++] = dirNormalizedY;
		customFloats[posPosition++] = dirNormalizedZ;
		customFloats[posPosition++] = 0f;
		return posPosition;
	}
}
