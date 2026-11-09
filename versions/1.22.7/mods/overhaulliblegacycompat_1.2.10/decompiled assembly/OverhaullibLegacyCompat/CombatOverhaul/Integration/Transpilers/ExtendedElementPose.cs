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
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0001C405 File Offset: 0x0001A605
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x0001C40D File Offset: 0x0001A60D
		public EnumAnimatedElement ElementNameEnum { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001C416 File Offset: 0x0001A616
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0001C41E File Offset: 0x0001A61E
		public int ElementNameHash { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001C427 File Offset: 0x0001A627
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x0001C42F File Offset: 0x0001A62F
		public EntityPlayer Player { get; set; }

		// Token: 0x060004CA RID: 1226 RVA: 0x0001C438 File Offset: 0x0001A638
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

		// Token: 0x060004CB RID: 1227 RVA: 0x0001C488 File Offset: 0x0001A688
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
