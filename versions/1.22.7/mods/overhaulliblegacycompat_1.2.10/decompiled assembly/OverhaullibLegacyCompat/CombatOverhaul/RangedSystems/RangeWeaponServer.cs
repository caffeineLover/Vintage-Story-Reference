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
	// Token: 0x020000DF RID: 223
	[NullableContext(1)]
	[Nullable(0)]
	public class RangeWeaponServer : IServerRangedWeaponLogic
	{
		// Token: 0x06000B5E RID: 2910 RVA: 0x0003AA70 File Offset: 0x00038C70
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

		// Token: 0x06000B5F RID: 2911 RVA: 0x0003AACA File Offset: 0x00038CCA
		public virtual bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet)
		{
			GeneralUtils.MarkItemStack(slot);
			return false;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0003AAD4 File Offset: 0x00038CD4
		public virtual bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			return false;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0003AADE File Offset: 0x00038CDE
		public virtual bool Shoot(ICoreAPI api, IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			return this.Shoot(player, slot, packet, shooter);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003AAF4 File Offset: 0x00038CF4
		protected Vector3d GetDirectionWithDispersion(double[] direction, float[] dispersionMOA)
		{
			Vector3d directionVector;
			directionVector..ctor(direction[0], direction[1], direction[2]);
			Vector2 dispersionVector;
			dispersionVector..ctor(dispersionMOA[0], dispersionMOA[1]);
			return this.GetDirectionWithDispersion(directionVector, dispersionVector);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0003AB28 File Offset: 0x00038D28
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

		// Token: 0x04000549 RID: 1353
		protected ProjectileSystemServer ProjectileSystem;

		// Token: 0x0400054A RID: 1354
		protected readonly Item Item;

		// Token: 0x0400054B RID: 1355
		protected readonly ICoreServerAPI Api;

		// Token: 0x0400054C RID: 1356
		protected readonly NatFloat RandomFloat = new NatFloat(0f, 1f, 2);

		// Token: 0x0400054D RID: 1357
		protected const float MinutesInRadian = 10800f;
	}
}
