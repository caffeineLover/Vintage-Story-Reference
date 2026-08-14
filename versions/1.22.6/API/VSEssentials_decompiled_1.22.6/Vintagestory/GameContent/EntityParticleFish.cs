using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityParticleFish : EntityParticle
{
	protected ICoreClientAPI capi;

	protected float dieAccum;

	protected static Random rand = new Random();

	private Vec3d StartingPosition = new Vec3d();

	private bool flee;

	private float maxspeed;

	public FastVec3i StartPos;

	private double wiggle;

	public static int[][] Colors = new int[7][]
	{
		new int[4] { 189, 187, 59, 255 },
		new int[4] { 192, 135, 53, 255 },
		new int[4] { 184, 88, 26, 255 },
		new int[4] { 180, 65, 47, 255 },
		new int[4] { 56, 125, 163, 255 },
		new int[4] { 57, 98, 193, 169 },
		new int[4] { 126, 90, 145, 255 }
	};

	public EntityParticleFish[] FriendFishes;

	public override string Type => "fish";

	public EntityParticleFish(ICoreClientAPI capi, double x, double y, double z, Vec3f size, int colorindex, float maxspeed)
	{
		this.capi = capi;
		Position.Set(x, y, z);
		StartingPosition.Set(x, y, z);
		this.maxspeed = maxspeed;
		Alive = true;
		base.SizeX = size.X;
		base.SizeY = size.Y;
		base.SizeZ = size.Z;
		base.GravityStrength = 0f;
		ColorBlue = (byte)Colors[colorindex][0];
		ColorGreen = (byte)Colors[colorindex][1];
		ColorRed = (byte)Colors[colorindex][2];
		ColorAlpha = (byte)Colors[colorindex][3];
	}

	public override void TickNow(float dt, float physicsdt, ICoreClientAPI api, ParticlePhysics physicsSim)
	{
		base.TickNow(dt, physicsdt, api, physicsSim);
		Velocity.X = GameMath.Clamp(Velocity.X, 0f - maxspeed, maxspeed);
		Velocity.Y = GameMath.Clamp(Velocity.Y, 0f - maxspeed, maxspeed);
		Velocity.Z = GameMath.Clamp(Velocity.Z, 0f - maxspeed, maxspeed);
		wiggle = GameMath.Mod(wiggle + (double)(dt * 30f * Velocity.Length()), 6.2831854820251465);
	}

	protected override void doSlowTick(ParticlePhysics physicsSim, float dt)
	{
		base.doSlowTick(physicsSim, dt);
		Vec3d delta = StartingPosition.SubCopy(Position);
		float dist = (float)delta.Length();
		if (!flee)
		{
			delta.Normalize();
			float velo = GameMath.Clamp((dist - 3f) * 0.1f, 0f, 0.4f);
			Velocity.Add((float)delta.X * velo, (float)delta.Y * velo, (float)delta.Z * velo);
		}
		DoSchool();
		if (rand.NextDouble() < 0.01)
		{
			float dirx = (float)rand.NextDouble() * 0.66f - 0.33f;
			float diry = (float)rand.NextDouble() * 0.2f - 0.1f;
			float dirz = (float)rand.NextDouble() * 0.66f - 0.33f;
			propel(dirx / 3f, diry / 3f, dirz / 3f);
			DoSchool();
			return;
		}
		EntityPlayer npe = capi.World.NearestPlayer(Position.X, Position.Y, Position.Z).Entity;
		double sqdist = 2500.0;
		flee = false;
		if (npe != null && (sqdist = npe.Pos.SquareDistanceTo(Position)) < 25.0 && npe.Player.WorldData.CurrentGameMode != EnumGameMode.Creative && npe.Player.WorldData.CurrentGameMode != EnumGameMode.Spectator)
		{
			Vec3d vec = npe.Pos.XYZ.Sub(Position).Normalize();
			propel((float)(0.0 - vec.X), 0f, (float)(0.0 - vec.Z));
			flee = true;
			DoSchool();
		}
		if (!capi.World.BlockAccessor.GetBlockRaw((int)Position.X, (int)(Position.Y + 0.4000000059604645), (int)Position.Z, 2).IsLiquid())
		{
			Velocity.Y -= 0.1f;
		}
		if (npe == null || sqdist > 1600.0)
		{
			dieAccum += dt;
			if (dieAccum > 15f)
			{
				Alive = false;
			}
		}
		else
		{
			dieAccum = 0f;
		}
	}

	private void DoSchool()
	{
		if (!flee)
		{
			Vec3d center = new Vec3d();
			Vec3f schoolvelocity = new Vec3f();
			EntityParticleFish[] FriendFishes = this.FriendFishes;
			int len = FriendFishes.Length;
			for (int i = 0; i < len; i++)
			{
				Vec3d friendPosition = FriendFishes[i].Position;
				center.Add(friendPosition.X / (double)len, friendPosition.Y / (double)len, friendPosition.Z / (double)len);
				schoolvelocity.Add(FriendFishes[i].Velocity);
				Vec3d delta = Position.SubCopy(center);
				float dist = (float)delta.Length();
				float awayVelocity = GameMath.Clamp((0.05f - dist) / 2f, 0f, 0.03f);
				Velocity.Add((float)Math.Sign(delta.X) * awayVelocity, (float)Math.Sign(delta.Y) * awayVelocity, (float)Math.Sign(delta.Z) * awayVelocity);
			}
			Vec3d cdelta = Position.SubCopy(center);
			float towardsVelocity = GameMath.Clamp(((float)cdelta.Length() - 0.25f) / 1f, 0f, 0.03f);
			Velocity.Add((0f - (float)Math.Sign(cdelta.X)) * towardsVelocity, (0f - (float)Math.Sign(cdelta.Y)) * towardsVelocity, (0f - (float)Math.Sign(cdelta.Z)) * towardsVelocity);
			Velocity.Add(schoolvelocity.X / (float)len / 20f, schoolvelocity.Y / (float)len / 20f, schoolvelocity.Z / (float)len / 20f);
		}
	}

	private void propel(float dirx, float diry, float dirz)
	{
		Velocity.Add(dirx, diry, dirz);
	}

	public override int UpdateAngles(float[] customFloats, int posPosition)
	{
		customFloats[posPosition++] = dirNormalizedX;
		customFloats[posPosition++] = dirNormalizedY;
		customFloats[posPosition++] = dirNormalizedZ;
		customFloats[posPosition++] = GameMath.Sin((float)wiggle) / 5f;
		return posPosition;
	}
}
