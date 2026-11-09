using System;
using Vintagestory.API.Common;

namespace PetAI
{
	// Token: 0x02000003 RID: 3
	public class BlockPetNest : Block
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002188 File Offset: 0x00000388
		public EnumNestSize nestSize
		{
			get
			{
				EnumNestSize size = EnumNestSize.SMALL;
				Enum.TryParse<EnumNestSize>(this.Variant["size"].ToUpper(), ref size);
				return size;
			}
		}
	}
}
