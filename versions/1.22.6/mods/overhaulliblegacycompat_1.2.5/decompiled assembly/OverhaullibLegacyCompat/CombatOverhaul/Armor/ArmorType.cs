using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Config;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000129 RID: 297
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ArmorType
	{
		// Token: 0x06000E7D RID: 3709 RVA: 0x00047458 File Offset: 0x00045658
		public ArmorType(ArmorLayers layers, DamageZone slots)
		{
			this.Layers = layers;
			this.Slots = slots;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00047468 File Offset: 0x00045668
		public bool Intersect(ArmorLayers layer, DamageZone slot)
		{
			return (this.Layers & layer) != ArmorLayers.None && (this.Slots & slot) > DamageZone.None;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00047481 File Offset: 0x00045681
		public bool Intersect(ArmorType type)
		{
			return (this.Layers & type.Layers) != ArmorLayers.None && (this.Slots & type.Slots) > DamageZone.None;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000474A4 File Offset: 0x000456A4
		public static ArmorType Combine(ArmorType first, ArmorType second)
		{
			return new ArmorType(first.Layers | second.Layers, first.Slots | second.Slots);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000474C5 File Offset: 0x000456C5
		public static ArmorType Combine(IEnumerable<ArmorType> armorTypes)
		{
			Func<ArmorType, ArmorType, ArmorType> func;
			if ((func = ArmorType.<>O.<0>__Combine) == null)
			{
				func = (ArmorType.<>O.<0>__Combine = new Func<ArmorType, ArmorType, ArmorType>(ArmorType.Combine));
			}
			return Enumerable.Aggregate<ArmorType>(armorTypes, func);
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000474E8 File Offset: 0x000456E8
		public static ArmorType Empty
		{
			get
			{
				return new ArmorType(ArmorLayers.None, DamageZone.None);
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x000474F4 File Offset: 0x000456F4
		public override string ToString()
		{
			ArmorLayers layersValue = this.Layers;
			string text;
			if (layersValue != ArmorLayers.None)
			{
				text = Enumerable.Aggregate<string>(Enumerable.Select<ArmorLayers, string>(Enumerable.Where<ArmorLayers>(Enum.GetValues<ArmorLayers>(), (ArmorLayers value) => (value & layersValue) > ArmorLayers.None), (ArmorLayers value) => value.ToString()), (string first, string second) => first + ", " + second);
			}
			else
			{
				text = "None";
			}
			string layers = text;
			DamageZone slotsValue = this.Slots;
			string text2;
			if (slotsValue != DamageZone.None)
			{
				text2 = Enumerable.Aggregate<string>(Enumerable.Select<DamageZone, string>(Enumerable.Where<DamageZone>(Enum.GetValues<DamageZone>(), (DamageZone value) => (value & slotsValue) > DamageZone.None), (DamageZone value) => value.ToString()), (string first, string second) => first + ", " + second);
			}
			else
			{
				text2 = "None";
			}
			string slots = text2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(layers);
			defaultInterpolatedStringHandler.AppendLiteral("|");
			defaultInterpolatedStringHandler.AppendFormatted(slots);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00047640 File Offset: 0x00045840
		public string LayersToTranslatedString()
		{
			ArmorLayers layersValue = this.Layers;
			if (layersValue != ArmorLayers.None)
			{
				return Enumerable.Aggregate<string>(Enumerable.Select<ArmorLayers, string>(Enumerable.Where<ArmorLayers>(Enum.GetValues<ArmorLayers>(), (ArmorLayers value) => (value & layersValue) > ArmorLayers.None), delegate(ArmorLayers value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:armor-layer-");
					defaultInterpolatedStringHandler.AppendFormatted<ArmorLayers>(value);
					return Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
				}), (string first, string second) => first + ", " + second);
			}
			return Lang.Get("combatoverhaul:armor-layer-None", Array.Empty<object>());
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x000476D8 File Offset: 0x000458D8
		public string ZonesToTranslatedString()
		{
			DamageZone slotsValue = this.Slots;
			if (slotsValue != DamageZone.None)
			{
				return Enumerable.Aggregate<string>(Enumerable.Select<DamageZone, string>(Enumerable.Where<DamageZone>(Enum.GetValues<DamageZone>(), (DamageZone value) => (value & slotsValue) > DamageZone.None), delegate(DamageZone value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:damage-zone-");
					defaultInterpolatedStringHandler.AppendFormatted<DamageZone>(value);
					return Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
				}), (string first, string second) => first + ", " + second);
			}
			return Lang.Get("combatoverhaul:damage-zone-None", Array.Empty<object>());
		}

		// Token: 0x0400070E RID: 1806
		public readonly ArmorLayers Layers;

		// Token: 0x0400070F RID: 1807
		public readonly DamageZone Slots;

		// Token: 0x02000275 RID: 629
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D05 RID: 3333
			[Nullable(0)]
			public static Func<ArmorType, ArmorType, ArmorType> <0>__Combine;
		}
	}
}
