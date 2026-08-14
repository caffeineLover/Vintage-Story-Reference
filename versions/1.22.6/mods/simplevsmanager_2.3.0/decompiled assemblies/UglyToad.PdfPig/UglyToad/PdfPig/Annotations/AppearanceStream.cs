using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x020001FF RID: 511
	public class AppearanceStream
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0003D9A5 File Offset: 0x0003BBA5
		public bool IsStateless
		{
			get
			{
				return this.statelessAppearanceStream != null;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0003D9B0 File Offset: 0x0003BBB0
		public ICollection<string> GetStates
		{
			get
			{
				if (this.appearanceStreamsByState == null)
				{
					return new string[0];
				}
				return this.appearanceStreamsByState.Keys;
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0003D9D9 File Offset: 0x0003BBD9
		internal AppearanceStream(StreamToken streamToken)
		{
			this.statelessAppearanceStream = streamToken;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
		internal AppearanceStream(IDictionary<string, StreamToken> appearanceStreamsByState)
		{
			this.appearanceStreamsByState = appearanceStreamsByState;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0003D9F8 File Offset: 0x0003BBF8
		public StreamToken Get(string state)
		{
			if (this.appearanceStreamsByState == null)
			{
				throw new Exception("Cannot get appearance by state when this is a stateless appearance stream");
			}
			if (!this.appearanceStreamsByState.ContainsKey(state))
			{
				string text = "state";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(61, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Appearance stream does not have state '");
				defaultInterpolatedStringHandler.AppendFormatted(state);
				defaultInterpolatedStringHandler.AppendLiteral("' (available states: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join(",", this.appearanceStreamsByState.Keys));
				defaultInterpolatedStringHandler.AppendLiteral(")");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.appearanceStreamsByState[state];
		}

		// Token: 0x040006BE RID: 1726
		private readonly IDictionary<string, StreamToken> appearanceStreamsByState;

		// Token: 0x040006BF RID: 1727
		private readonly StreamToken statelessAppearanceStream;
	}
}
