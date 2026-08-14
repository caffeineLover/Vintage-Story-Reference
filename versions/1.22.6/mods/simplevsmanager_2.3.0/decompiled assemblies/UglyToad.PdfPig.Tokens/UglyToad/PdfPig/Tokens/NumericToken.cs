using System;
using System.Globalization;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x0200000E RID: 14
	public class NumericToken : IDataToken<decimal>, IToken, IEquatable<IToken>
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00004B9D File Offset: 0x00002D9D
		public decimal Data { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004BA5 File Offset: 0x00002DA5
		public bool HasDecimalPlaces
		{
			get
			{
				return decimal.Floor(this.Data) != this.Data;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00004BBD File Offset: 0x00002DBD
		public int Int
		{
			get
			{
				return (int)this.Data;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00004BCA File Offset: 0x00002DCA
		public long Long
		{
			get
			{
				return (long)this.Data;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004BD7 File Offset: 0x00002DD7
		public double Double
		{
			get
			{
				return (double)this.Data;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004BE5 File Offset: 0x00002DE5
		public NumericToken(decimal value)
		{
			this.Data = value;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004BF4 File Offset: 0x00002DF4
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			NumericToken numericToken = obj as NumericToken;
			return numericToken != null && this.Data == numericToken.Data;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004C24 File Offset: 0x00002E24
		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004C40 File Offset: 0x00002E40
		public override string ToString()
		{
			return this.Data.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0400022E RID: 558
		public static readonly NumericToken MinusOne = new NumericToken(-1m);

		// Token: 0x0400022F RID: 559
		public static readonly NumericToken Zero = new NumericToken(0m);

		// Token: 0x04000230 RID: 560
		public static readonly NumericToken One = new NumericToken(1m);

		// Token: 0x04000231 RID: 561
		public static readonly NumericToken Two = new NumericToken(2m);

		// Token: 0x04000232 RID: 562
		public static readonly NumericToken Three = new NumericToken(3m);

		// Token: 0x04000233 RID: 563
		public static readonly NumericToken Four = new NumericToken(4m);

		// Token: 0x04000234 RID: 564
		public static readonly NumericToken Five = new NumericToken(5m);

		// Token: 0x04000235 RID: 565
		public static readonly NumericToken Six = new NumericToken(6m);

		// Token: 0x04000236 RID: 566
		public static readonly NumericToken Seven = new NumericToken(7m);

		// Token: 0x04000237 RID: 567
		public static readonly NumericToken Eight = new NumericToken(8m);

		// Token: 0x04000238 RID: 568
		public static readonly NumericToken Nine = new NumericToken(9m);

		// Token: 0x04000239 RID: 569
		public static readonly NumericToken Ten = new NumericToken(10m);

		// Token: 0x0400023A RID: 570
		public static readonly NumericToken Eleven = new NumericToken(11m);

		// Token: 0x0400023B RID: 571
		public static readonly NumericToken Twelve = new NumericToken(12m);

		// Token: 0x0400023C RID: 572
		public static readonly NumericToken Thirteen = new NumericToken(13m);

		// Token: 0x0400023D RID: 573
		public static readonly NumericToken Fourteen = new NumericToken(14m);

		// Token: 0x0400023E RID: 574
		public static readonly NumericToken Fifteen = new NumericToken(15m);

		// Token: 0x0400023F RID: 575
		public static readonly NumericToken Sixteen = new NumericToken(16m);

		// Token: 0x04000240 RID: 576
		public static readonly NumericToken Seventeen = new NumericToken(17m);

		// Token: 0x04000241 RID: 577
		public static readonly NumericToken Eighteen = new NumericToken(18m);

		// Token: 0x04000242 RID: 578
		public static readonly NumericToken Nineteen = new NumericToken(19m);

		// Token: 0x04000243 RID: 579
		public static readonly NumericToken Twenty = new NumericToken(20m);

		// Token: 0x04000244 RID: 580
		public static readonly NumericToken OneHundred = new NumericToken(100m);

		// Token: 0x04000245 RID: 581
		public static readonly NumericToken FiveHundred = new NumericToken(500m);

		// Token: 0x04000246 RID: 582
		public static readonly NumericToken OneThousand = new NumericToken(1000m);
	}
}
