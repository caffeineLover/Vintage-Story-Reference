using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D4 RID: 212
	[NullableContext(1)]
	[Nullable(0)]
	public class RangeWeaponServer : IServerRangedWeaponLogic
	{
		// Token: 0x06000B02 RID: 2818 RVA: 0x00039A80 File Offset: 0x00037C80
		public RangeWeaponServer(ICoreServerAPI api, Item item)
		{
			CombatOverhaulSystem system = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			ProjectileSystemServer serverProjectileSystem = system.ServerProjectileSystem;
			if (serverProjectileSystem == null)
			{
				throw new Exception();
			}
			this.ProjectileSystem = serverProjectileSystem;
			this.Item = item;
			this.Api = api;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00039ADA File Offset: 0x00037CDA
		public virtual bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet)
		{
			GeneralUtils.MarkItemStack(slot);
			return false;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00039AE4 File Offset: 0x00037CE4
		public virtual bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			return false;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00039AEE File Offset: 0x00037CEE
		public virtual bool Shoot(ICoreAPI api, IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			return this.Shoot(player, slot, packet, shooter);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00039B04 File Offset: 0x00037D04
		protected Vector3d GetDirectionWithDispersion(double[] direction, float[] dispersionMOA)
		{
			Vector3d directionVector;
			directionVector..ctor(direction[0], direction[1], direction[2]);
			Vector2 dispersionVector;
			dispersionVector..ctor(dispersionMOA[0], dispersionMOA[1]);
			return this.GetDirectionWithDispersion(directionVector, dispersionVector);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00039B38 File Offset: 0x00037D38
		protected Vector3d GetDirectionWithDispersion(Vector3d direction, Vector2 dispersionMOA)
		{
			float randomPitch = this.RandomFloat.nextFloat() * dispersionMOA.Y * 3.1415927f / 10800f;
			float randomYaw = this.RandomFloat.nextFloat() * dispersionMOA.X * 3.1415927f / 10800f;
			Vector3d forwardAxis = Vector3d.Normalize(direction);
			Vector3d verticalAxis;
			verticalAxis..ctor(0.0, 0.0, 1.0);
			if (Math.Abs(Vector3d.Dot(forwardAxis, verticalAxis)) > 0.999999)
			{
				verticalAxis..ctor(0.0, 1.0, 0.0);
			}
			Vector3d vector3d = Vector3d.Normalize(Vector3d.Cross(forwardAxis, verticalAxis));
			Vector3d pitchAxis = Vector3d.Normalize(Vector3d.Cross(vector3d, forwardAxis));
			Vector3d yawComponent = vector3d * Math.Tan((double)randomYaw);
			Vector3d pitchComponent = pitchAxis * Math.Tan((double)randomPitch);
			return Vector3d.Normalize(forwardAxis + yawComponent + pitchComponent);
		}

		// Token: 0x04000527 RID: 1319
		protected ProjectileSystemServer ProjectileSystem;

		// Token: 0x04000528 RID: 1320
		protected readonly Item Item;

		// Token: 0x04000529 RID: 1321
		protected readonly ICoreServerAPI Api;

		// Token: 0x0400052A RID: 1322
		protected readonly NatFloat RandomFloat = new NatFloat(0f, 1f, 2);

		// Token: 0x0400052B RID: 1323
		protected const float MinutesInRadian = 10800f;
	}
}
