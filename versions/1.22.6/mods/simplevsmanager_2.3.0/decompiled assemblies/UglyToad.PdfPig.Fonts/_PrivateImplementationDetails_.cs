using System;
using System.Runtime.CompilerServices;

// Token: 0x020000B6 RID: 182
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x060004A8 RID: 1192 RVA: 0x0001F200 File Offset: 0x0001D400
	internal static uint ComputeStringHash(string s)
	{
		uint num;
		if (s != null)
		{
			num = 2166136261U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((uint)s.get_Chars(i) ^ num) * 16777619U;
			}
		}
		return num;
	}
}
