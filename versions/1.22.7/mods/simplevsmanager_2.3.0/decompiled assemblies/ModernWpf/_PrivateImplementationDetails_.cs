using System;
using System.Runtime.CompilerServices;

// Token: 0x020002B2 RID: 690
[CompilerGenerated]
internal sealed class <PrivateImplementationDetails>
{
	// Token: 0x06001544 RID: 5444 RVA: 0x0003F3F8 File Offset: 0x0003D5F8
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
