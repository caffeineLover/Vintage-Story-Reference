using System;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001CD RID: 461
	internal class CryptDictionary
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00037334 File Offset: 0x00035534
		public static CryptDictionary Identity { get; } = new CryptDictionary();

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0003733B File Offset: 0x0003553B
		public CryptDictionary.Method Name { get; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00037343 File Offset: 0x00035543
		public CryptDictionary.TriggerEvent Event { get; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0003734B File Offset: 0x0003554B
		public int Length { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x00037353 File Offset: 0x00035553
		public bool IsIdentity { get; }

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003735B File Offset: 0x0003555B
		public CryptDictionary(CryptDictionary.Method name, CryptDictionary.TriggerEvent @event, int length)
		{
			this.Name = name;
			this.Event = @event;
			this.Length = length;
			this.IsIdentity = 0;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0003737F File Offset: 0x0003557F
		private CryptDictionary()
		{
			this.Name = 0;
			this.IsIdentity = 1;
		}

		// Token: 0x020002B4 RID: 692
		public enum Method
		{
			// Token: 0x040008D7 RID: 2263
			None,
			// Token: 0x040008D8 RID: 2264
			V2,
			// Token: 0x040008D9 RID: 2265
			AesV2,
			// Token: 0x040008DA RID: 2266
			AesV3
		}

		// Token: 0x020002B5 RID: 693
		public enum TriggerEvent
		{
			// Token: 0x040008DC RID: 2268
			DocumentOpen,
			// Token: 0x040008DD RID: 2269
			EmbeddedFileOpen
		}
	}
}
