using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000134 RID: 308
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArmorTypeJson
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00048C72 File Offset: 0x00046E72
		// (set) Token: 0x06000EEC RID: 3820 RVA: 0x00048C7A File Offset: 0x00046E7A
		public string[] Layers { get; set; } = Array.Empty<string>();

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x00048C83 File Offset: 0x00046E83
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x00048C8B File Offset: 0x00046E8B
		public string[] Slots { get; set; } = Array.Empty<string>();

		// Token: 0x06000EEF RID: 3823 RVA: 0x00048C94 File Offset: 0x00046E94
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

		// Token: 0x02000286 RID: 646
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D4C RID: 3404
			[Nullable(0)]
			public static Func<string, ArmorLayers> <0>__Parse;

			// Token: 0x04000D4D RID: 3405
			[Nullable(0)]
			public static Func<string, DamageZone> <1>__Parse;
		}
	}
}
