using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000172 RID: 370
	[NullableContext(1)]
	public interface IWeaponCollider : ICollider
	{
		// Token: 0x06001267 RID: 4711
		bool RoughIntersect(Cuboidf collisionBox);

		// Token: 0x06001268 RID: 4712
		Vector3d? IntersectCuboids(IEnumerable<Cuboidf> collisionBoxes);

		// Token: 0x06001269 RID: 4713
		Vector3d? IntersectCuboid(Cuboidf collisionBox, out double parameter);

		// Token: 0x0600126A RID: 4714
		[return: TupleElementNames(new string[]
		{
			"block",
			"position",
			"parameter"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		ValueTuple<Block, Vector3d, double>? IntersectTerrain(ICoreClientAPI api);
	}
}
