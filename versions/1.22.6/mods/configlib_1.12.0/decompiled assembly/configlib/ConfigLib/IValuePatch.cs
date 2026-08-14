using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x02000040 RID: 64
	[NullableContext(1)]
	internal interface IValuePatch
	{
		// Token: 0x060001CB RID: 459
		int Apply(JsonObject asset);

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001CC RID: 460
		string Path { get; }
	}
}
