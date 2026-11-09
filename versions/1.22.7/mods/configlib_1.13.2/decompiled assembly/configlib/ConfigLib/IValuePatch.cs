using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	internal interface IValuePatch
	{
		// Token: 0x060001F0 RID: 496
		int Apply(JsonObject asset);

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001F1 RID: 497
		string Path { get; }
	}
}
