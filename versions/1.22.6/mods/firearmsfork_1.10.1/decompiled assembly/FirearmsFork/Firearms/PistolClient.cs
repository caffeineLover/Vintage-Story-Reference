using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000026 RID: 38
	public class PistolClient : MuzzleloaderClient
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x000098F8 File Offset: 0x00007AF8
		[NullableContext(1)]
		public PistolClient(ICoreClientAPI api, Item item) : base(api, item)
		{
		}
	}
}
