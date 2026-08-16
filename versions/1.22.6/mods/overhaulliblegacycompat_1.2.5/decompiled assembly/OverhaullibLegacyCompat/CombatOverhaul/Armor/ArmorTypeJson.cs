using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000128 RID: 296
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArmorTypeJson
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00047372 File Offset: 0x00045572
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x0004737A File Offset: 0x0004557A
		public string[] Layers { get; set; } = Array.Empty<string>();

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00047383 File Offset: 0x00045583
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x0004738B File Offset: 0x0004558B
		public string[] Slots { get; set; } = Array.Empty<string>();

		// Token: 0x06000E7B RID: 3707 RVA: 0x00047394 File Offset: 0x00045594
		public ArmorType ToArmorType()
		{
			IEnumerable<string> layers = this.Layers;
			Func<string, ArmorLayers> func;
			if ((func = ArmorTypeJson.<>O.<0>__Parse) == null)
			{
				func = (ArmorTypeJson.<>O.<0>__Parse = new Func<string, ArmorLayers>(Enum.Parse<ArmorLayers>));
			}
			ArmorLayers layers2 = Enumerable.Aggregate<ArmorLayers>(Enumerable.Select<string, ArmorLayers>(layers, func), (ArmorLayers first, ArmorLayers second) => first | second);
			IEnumerable<string> slots = this.Slots;
			Func<string, DamageZone> func2;
			if ((func2 = ArmorTypeJson.<>O.<1>__Parse) == null)
			{
				func2 = (ArmorTypeJson.<>O.<1>__Parse = new Func<string, DamageZone>(Enum.Parse<DamageZone>));
			}
			return new ArmorType(layers2, Enumerable.Aggregate<DamageZone>(Enumerable.Select<string, DamageZone>(slots, func2), (DamageZone first, DamageZone second) => first | second));
		}

		// Token: 0x02000273 RID: 627
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D00 RID: 3328
			[Nullable(0)]
			public static Func<string, ArmorLayers> <0>__Parse;

			// Token: 0x04000D01 RID: 3329
			[Nullable(0)]
			public static Func<string, DamageZone> <1>__Parse;
		}
	}
}
