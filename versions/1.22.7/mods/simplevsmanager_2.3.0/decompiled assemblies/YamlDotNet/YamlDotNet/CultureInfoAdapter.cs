using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000002 RID: 2
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CultureInfoAdapter : CultureInfo
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider) : base(baseCulture.Name)
		{
			this.provider = provider;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002065 File Offset: 0x00000265
		public override object GetFormat([Nullable(2)] Type formatType)
		{
			return this.provider.GetFormat(formatType);
		}

		// Token: 0x04000001 RID: 1
		private readonly IFormatProvider provider;
	}
}
