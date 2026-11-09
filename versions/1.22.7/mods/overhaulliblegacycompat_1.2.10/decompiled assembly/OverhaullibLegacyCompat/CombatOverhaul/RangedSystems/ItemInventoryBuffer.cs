using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemInventoryBuffer
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x00033E0B File Offset: 0x0003200B
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x00033E13 File Offset: 0x00032013
		public string Id { get; private set; } = "";

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x00033E1C File Offset: 0x0003201C
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x00033E24 File Offset: 0x00032024
		public List<ItemStack> Items { get; private set; } = new List<ItemStack>();

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00033E2D File Offset: 0x0003202D
		public string Attribute
		{
			get
			{
				return "CombatOverhaul:inventory." + this.Id;
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00033E60 File Offset: 0x00032060
		public void Read(ItemSlot slot, string id)
		{
			this.Clear();
			this.Id = id;
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null) ? itemstack.Attributes : null) == null)
			{
				return;
			}
			if (!slot.Itemstack.Attributes.HasAttribute(this.Attribute))
			{
				return;
			}
			using (MemoryStream input = new MemoryStream(slot.Itemstack.Attributes.GetBytes(this.Attribute, null)))
			{
				using (BinaryReader stream = new BinaryReader(input))
				{
					int size = stream.ReadInt32();
					for (int index = 0; index < size; index++)
					{
						this.Items.Add(new ItemStack(stream));
					}
				}
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00033F24 File Offset: 0x00032124
		public void Write(ItemSlot slot)
		{
			ItemStack itemStack = slot.Itemstack;
			if (itemStack == null)
			{
				return;
			}
			ItemStack itemStack2 = itemStack;
			if (itemStack2.Attributes == null)
			{
				itemStack2.Attributes = new TreeAttribute();
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter stream = new BinaryWriter(memoryStream))
				{
					stream.Write(this.Items.Count);
					for (int index = 0; index < this.Items.Count; index++)
					{
						this.Items[index].ToBytes(stream);
					}
				}
				itemStack.Attributes.RemoveAttribute(this.Attribute);
				itemStack.Attributes.SetBytes(this.Attribute, memoryStream.ToArray());
				slot.MarkDirty();
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00034008 File Offset: 0x00032208
		public void Clear()
		{
			this.Id = "";
			this.Items.Clear();
		}
	}
}
