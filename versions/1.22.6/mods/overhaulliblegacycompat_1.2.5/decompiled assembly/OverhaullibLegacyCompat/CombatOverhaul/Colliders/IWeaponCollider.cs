using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000166 RID: 358
	[NullableContext(1)]
	public interface IWeaponCollider : ICollider
	{
		// Token: 0x060011F3 RID: 4595
		bool RoughIntersect(Cuboidf collisionBox);

		// Token: 0x060011F4 RID: 4596
		Vector3d? IntersectCuboids(IEnumerable<Cuboidf> collisionBoxes);

		// Token: 0x060011F5 RID: 4597
		Vector3d? IntersectCuboid(Cuboidf collisionBox, out double parameter);

		// Token: 0x060011F6 RID: 4598
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
