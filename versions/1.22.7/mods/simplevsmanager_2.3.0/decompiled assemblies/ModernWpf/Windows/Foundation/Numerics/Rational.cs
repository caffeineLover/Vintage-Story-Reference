using System;
using System.Runtime.Versioning;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Numerics
{
	// Token: 0x02000057 RID: 87
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[SupportedOSPlatform("Windows10.0.17134.0")]
	[ContractVersion(typeof(UniversalApiContract), 393216U)]
	internal struct Rational : IEquatable<Rational>
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x00006443 File Offset: 0x00004643
		public Rational(uint _Numerator, uint _Denominator)
		{
			this.Numerator = _Numerator;
			this.Denominator = _Denominator;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00006453 File Offset: 0x00004653
		public static bool operator ==(Rational x, Rational y)
		{
			return x.Numerator == y.Numerator && x.Denominator == y.Denominator;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00006473 File Offset: 0x00004673
		public static bool operator !=(Rational x, Rational y)
		{
			return !(x == y);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000647F File Offset: 0x0000467F
		public bool Equals(Rational other)
		{
			return this == other;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00006490 File Offset: 0x00004690
		public override bool Equals(object obj)
		{
			if (obj is Rational)
			{
				Rational y = (Rational)obj;
				return this == y;
			}
			return false;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000064BA File Offset: 0x000046BA
		public override int GetHashCode()
		{
			return this.Numerator.GetHashCode() ^ this.Denominator.GetHashCode();
		}

		// Token: 0x040000E8 RID: 232
		public uint Numerator;

		// Token: 0x040000E9 RID: 233
		public uint Denominator;
	}
}
