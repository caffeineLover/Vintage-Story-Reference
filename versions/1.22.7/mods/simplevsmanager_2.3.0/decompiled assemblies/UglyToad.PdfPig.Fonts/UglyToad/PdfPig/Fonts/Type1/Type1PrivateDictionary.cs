using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;

namespace UglyToad.PdfPig.Fonts.Type1
{
	// Token: 0x0200000A RID: 10
	public class Type1PrivateDictionary : AdobeStylePrivateDictionary
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002AF4 File Offset: 0x00000CF4
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002AFC File Offset: 0x00000CFC
		public int? UniqueId { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002B05 File Offset: 0x00000D05
		public int LenIv { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002B0D File Offset: 0x00000D0D
		public bool? RoundStemUp { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002B15 File Offset: 0x00000D15
		public int Password { get; } = 5839;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002B1D File Offset: 0x00000D1D
		public MinFeature MinFeature { get; } = new MinFeature(16, 16);

		// Token: 0x06000044 RID: 68 RVA: 0x00002B28 File Offset: 0x00000D28
		internal Type1PrivateDictionary(Type1PrivateDictionary.Builder builder) : base(builder)
		{
			if (builder == null)
			{
				throw new ArgumentNullException("builder");
			}
			this.UniqueId = builder.UniqueId;
			this.RoundStemUp = builder.RoundStemUp;
			this.LenIv = builder.LenIv;
			if (builder.Password != null)
			{
				this.Password = builder.Password.Value;
			}
		}

		// Token: 0x020000BA RID: 186
		internal class Builder : AdobeStylePrivateDictionary.BaseBuilder
		{
			// Token: 0x17000221 RID: 545
			// (get) Token: 0x060004CE RID: 1230 RVA: 0x0001F388 File Offset: 0x0001D588
			// (set) Token: 0x060004CF RID: 1231 RVA: 0x0001F390 File Offset: 0x0001D590
			public object Rd { get; set; }

			// Token: 0x17000222 RID: 546
			// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0001F399 File Offset: 0x0001D599
			// (set) Token: 0x060004D1 RID: 1233 RVA: 0x0001F3A1 File Offset: 0x0001D5A1
			public object NoAccessPut { get; set; }

			// Token: 0x17000223 RID: 547
			// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0001F3AA File Offset: 0x0001D5AA
			// (set) Token: 0x060004D3 RID: 1235 RVA: 0x0001F3B2 File Offset: 0x0001D5B2
			public object NoAccessDef { get; set; }

			// Token: 0x17000224 RID: 548
			// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001F3BB File Offset: 0x0001D5BB
			// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0001F3C3 File Offset: 0x0001D5C3
			public IReadOnlyList<Type1CharstringDecryptedBytes> Subroutines { get; set; }

			// Token: 0x17000225 RID: 549
			// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0001F3CC File Offset: 0x0001D5CC
			// (set) Token: 0x060004D7 RID: 1239 RVA: 0x0001F3D4 File Offset: 0x0001D5D4
			public object[] OtherSubroutines { get; set; }

			// Token: 0x17000226 RID: 550
			// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001F3DD File Offset: 0x0001D5DD
			// (set) Token: 0x060004D9 RID: 1241 RVA: 0x0001F3E5 File Offset: 0x0001D5E5
			public int? UniqueId { get; set; }

			// Token: 0x17000227 RID: 551
			// (get) Token: 0x060004DA RID: 1242 RVA: 0x0001F3EE File Offset: 0x0001D5EE
			// (set) Token: 0x060004DB RID: 1243 RVA: 0x0001F3F6 File Offset: 0x0001D5F6
			public int? Password { get; set; }

			// Token: 0x17000228 RID: 552
			// (get) Token: 0x060004DC RID: 1244 RVA: 0x0001F3FF File Offset: 0x0001D5FF
			// (set) Token: 0x060004DD RID: 1245 RVA: 0x0001F407 File Offset: 0x0001D607
			public int LenIv { get; set; }

			// Token: 0x17000229 RID: 553
			// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001F410 File Offset: 0x0001D610
			// (set) Token: 0x060004DF RID: 1247 RVA: 0x0001F418 File Offset: 0x0001D618
			public MinFeature MinFeature { get; set; }

			// Token: 0x1700022A RID: 554
			// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0001F421 File Offset: 0x0001D621
			// (set) Token: 0x060004E1 RID: 1249 RVA: 0x0001F429 File Offset: 0x0001D629
			public bool? RoundStemUp { get; set; }

			// Token: 0x060004E2 RID: 1250 RVA: 0x0001F432 File Offset: 0x0001D632
			public Type1PrivateDictionary Build()
			{
				return new Type1PrivateDictionary(this);
			}
		}
	}
}
