using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CultureInfoAdapter : CultureInfo
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0000EE38 File Offset: 0x0000D038
		public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider) : base(baseCulture.LCID)
		{
			this.provider = provider;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000EE4D File Offset: 0x0000D04D
		[NullableContext(2)]
		public override object GetFormat(Type formatType)
		{
			return this.provider.GetFormat(formatType);
		}

		// Token: 0x0400015B RID: 347
		private readonly IFormatProvider provider;
	}
}
