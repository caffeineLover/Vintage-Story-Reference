using System;
using System.CodeDom.Compiler;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000007 RID: 7
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.37416")]
	internal static class <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities
	{
		// Token: 0x06000030 RID: 48 RVA: 0x0000257C File Offset: 0x0000077C
		// Note: this type is marked as 'beforefieldinit'.
		static <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities()
		{
			object data = AppContext.GetData("REGEX_DEFAULT_MATCH_TIMEOUT");
			TimeSpan timeSpan;
			if (data is TimeSpan)
			{
				TimeSpan timeout = (TimeSpan)data;
				timeSpan = timeout;
			}
			else
			{
				timeSpan = Regex.InfiniteMatchTimeout;
			}
			<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_defaultTimeout = timeSpan;
			<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_hasTimeout = (<RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__Utilities.s_defaultTimeout != Regex.InfiniteMatchTimeout);
		}

		// Token: 0x04000005 RID: 5
		internal static readonly TimeSpan s_defaultTimeout;

		// Token: 0x04000006 RID: 6
		internal static readonly bool s_hasTimeout;
	}
}
