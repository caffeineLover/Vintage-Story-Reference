using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CultureInfoAdapter : CultureInfo
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000F780 File Offset: 0x0000D980
		public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider) : base(baseCulture.LCID)
		{
			this.provider = provider;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000F795 File Offset: 0x0000D995
		[NullableContext(2)]
		public override object GetFormat(Type formatType)
		{
			return this.provider.GetFormat(formatType);
		}

		// Token: 0x04000189 RID: 393
		private readonly IFormatProvider provider;
	}
}
