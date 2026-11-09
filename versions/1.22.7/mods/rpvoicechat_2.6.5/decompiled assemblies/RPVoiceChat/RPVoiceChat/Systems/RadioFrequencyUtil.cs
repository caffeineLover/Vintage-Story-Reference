using System;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000034 RID: 52
	public static class RadioFrequencyUtil
	{
		// Token: 0x06000140 RID: 320 RVA: 0x00009248 File Offset: 0x00007448
		public static string Normalize(string frequency)
		{
			if (string.IsNullOrWhiteSpace(frequency))
			{
				return "";
			}
			return frequency.Trim();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00009260 File Offset: 0x00007460
		public static bool Matches(string a, string b)
		{
			string left = RadioFrequencyUtil.Normalize(a);
			string right = RadioFrequencyUtil.Normalize(b);
			return left.Length != 0 && right.Length != 0 && string.Equals(left, right, 5);
		}
	}
}
