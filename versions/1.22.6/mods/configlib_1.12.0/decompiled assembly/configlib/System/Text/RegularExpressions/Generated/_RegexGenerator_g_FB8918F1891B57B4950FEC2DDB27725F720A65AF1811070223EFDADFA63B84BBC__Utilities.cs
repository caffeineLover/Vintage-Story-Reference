using System;
using System.CodeDom.Compiler;

namespace System.Text.RegularExpressions.Generated
{
	// Token: 0x02000005 RID: 5
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.13.7005")]
	internal static class <RegexGenerator_g>FB8918F1891B57B4950FEC2DDB27725F720A65AF1811070223EFDADFA63B84BBC__Utilities
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000245C File Offset: 0x0000065C
		// Note: this type is marked as 'beforefieldinit'.
		static <RegexGenerator_g>FB8918F1891B57B4950FEC2DDB27725F720A65AF1811070223EFDADFA63B84BBC__Utilities()
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
			<RegexGenerator_g>FB8918F1891B57B4950FEC2DDB27725F720A65AF1811070223EFDADFA63B84BBC__Utilities.s_defaultTimeout = timeSpan;
			<RegexGenerator_g>FB8918F1891B57B4950FEC2DDB27725F720A65AF1811070223EFDADFA63B84BBC__Utilities.s_hasTimeout = (<RegexGenerator_g>FB8918F1891B57B4950FEC2DDB27725F720A65AF1811070223EFDADFA63B84BBC__Utilities.s_defaultTimeout != Regex.InfiniteMatchTimeout);
		}

		// Token: 0x04000002 RID: 2
		internal static readonly TimeSpan s_defaultTimeout;

		// Token: 0x04000003 RID: 3
		internal static readonly bool s_hasTimeout;
	}
}
