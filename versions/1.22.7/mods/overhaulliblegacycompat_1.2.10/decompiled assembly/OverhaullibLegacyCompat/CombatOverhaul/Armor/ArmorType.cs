using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Config;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000135 RID: 309
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ArmorType
	{
		// Token: 0x06000EF1 RID: 3825 RVA: 0x00048D58 File Offset: 0x00046F58
		public ArmorType(ArmorLayers layers, DamageZone slots)
		{
			this.Layers = layers;
			this.Slots = slots;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00048D68 File Offset: 0x00046F68
		public bool Intersect(ArmorLayers layer, DamageZone slot)
		{
			return (this.Layers & layer) != ArmorLayers.None && (this.Slots & slot) > DamageZone.None;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00048D81 File Offset: 0x00046F81
		public bool Intersect(ArmorType type)
		{
			return (this.Layers & type.Layers) != ArmorLayers.None && (this.Slots & type.Slots) > DamageZone.None;
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00048DA4 File Offset: 0x00046FA4
		public static ArmorType Combine(ArmorType first, ArmorType second)
		{
			return new ArmorType(first.Layers | second.Layers, first.Slots | second.Slots);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00048DC5 File Offset: 0x00046FC5
		public static ArmorType Combine(IEnumerable<ArmorType> armorTypes)
		{
			Func<ArmorType, ArmorType, ArmorType> func;
			if ((func = ArmorType.<>O.<0>__Combine) == null)
			{
				func = (ArmorType.<>O.<0>__Combine = new Func<ArmorType, ArmorType, ArmorType>(ArmorType.Combine));
			}
			return Enumerable.Aggregate<ArmorType>(armorTypes, func);
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x00048DE8 File Offset: 0x00046FE8
		public static ArmorType Empty
		{
			get
			{
				return new ArmorType(ArmorLayers.None, DamageZone.None);
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00048DF4 File Offset: 0x00046FF4
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

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00048F40 File Offset: 0x00047140
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

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00048FD8 File Offset: 0x000471D8
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

		// Token: 0x04000734 RID: 1844
		public readonly ArmorLayers Layers;

		// Token: 0x04000735 RID: 1845
		public readonly DamageZone Slots;

		// Token: 0x02000288 RID: 648
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D51 RID: 3409
			[Nullable(0)]
			public static Func<ArmorType, ArmorType, ArmorType> <0>__Combine;
		}
	}
}
