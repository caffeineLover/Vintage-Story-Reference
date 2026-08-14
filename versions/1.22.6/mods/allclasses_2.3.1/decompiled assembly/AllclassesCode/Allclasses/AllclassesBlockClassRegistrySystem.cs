using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace Allclasses
{
	// Token: 0x02000002 RID: 2
	public sealed class AllclassesBlockClassRegistrySystem : ModSystem
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[NullableContext(1)]
		public override void Start(ICoreAPI api)
		{
			api.RegisterBlockClass("AllclassesCookingContainer", typeof(AllclassesCookingContainer));
			api.RegisterBlockClass("AllclassesCookedContainer", typeof(AllclassesCookedContainer));
		}
	}
}
