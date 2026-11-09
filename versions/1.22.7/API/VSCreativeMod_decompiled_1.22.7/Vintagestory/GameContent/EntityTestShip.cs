using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class EntityTestShip : EntityChunky
{
	public override void OnEntitySpawn()
	{
		base.OnEntitySpawn();
	}

	public static EntityChunky CreateShip(ICoreServerAPI sapi, IMiniDimension dimension)
	{
		EntityChunky obj = (EntityChunky)sapi.World.ClassRegistry.CreateEntity("EntityTestShip");
		obj.Code = new AssetLocation("testship");
		obj.AssociateWithDimension(dimension);
		return obj;
	}

	public override void OnGameTick(float dt)
	{
		if (blocks != null && base.Pos != null)
		{
			base.OnGameTick(dt);
			base.Pos.Motion.X = 0.01;
			base.Pos.Y = (double)(int)base.Pos.Y + 0.5;
			base.Pos.Yaw = (float)(base.Pos.X % 6.3) / 20f;
			base.Pos.Pitch = (float)GameMath.Sin(base.Pos.X % 6.3) / 5f;
			base.Pos.Roll = (float)GameMath.Sin(base.Pos.X % 12.6) / 3f;
		}
	}
}
