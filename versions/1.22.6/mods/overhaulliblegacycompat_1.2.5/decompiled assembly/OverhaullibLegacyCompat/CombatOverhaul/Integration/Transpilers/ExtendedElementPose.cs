using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x0200005C RID: 92
	[NullableContext(2)]
	[Nullable(0)]
	public class ExtendedElementPose : ElementPose
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0001C3CD File Offset: 0x0001A5CD
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x0001C3D5 File Offset: 0x0001A5D5
		public EnumAnimatedElement ElementNameEnum { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001C3DE File Offset: 0x0001A5DE
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0001C3E6 File Offset: 0x0001A5E6
		public int ElementNameHash { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001C3EF File Offset: 0x0001A5EF
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x0001C3F7 File Offset: 0x0001A5F7
		public EntityPlayer Player { get; set; }

		// Token: 0x060004CA RID: 1226 RVA: 0x0001C400 File Offset: 0x0001A600
		[NullableContext(1)]
		public void ResolveElementName(ShapeElement element)
		{
			if (element == null || element.Name == null)
			{
				return;
			}
			int hash = element.Name.GetHashCode();
			this.ElementNameHash = hash;
			EnumAnimatedElement enumValue;
			if (ExtendedElementPose._elementNameEnumCache.TryGetValue(element.Name, ref enumValue))
			{
				this.ElementNameEnum = enumValue;
				return;
			}
			this.ElementNameEnum = EnumAnimatedElement.Unknown;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001C450 File Offset: 0x0001A650
		public static EnumAnimatedElement GetElementNameEnum(string name)
		{
			EnumAnimatedElement enumValue;
			if (name != null && ExtendedElementPose._elementNameEnumCache.TryGetValue(name, ref enumValue))
			{
				return enumValue;
			}
			return EnumAnimatedElement.Unknown;
		}

		// Token: 0x040001C4 RID: 452
		[Nullable(1)]
		private static readonly Dictionary<string, EnumAnimatedElement> _elementNameEnumCache = Enumerable.ToDictionary<string, string, EnumAnimatedElement>(Enum.GetNames<EnumAnimatedElement>(), (string name) => name, (string name) => Enum.Parse<EnumAnimatedElement>(name), StringComparer.Ordinal);
	}
}
