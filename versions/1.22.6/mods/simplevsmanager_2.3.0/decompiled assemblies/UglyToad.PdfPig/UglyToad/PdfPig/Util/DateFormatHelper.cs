using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000029 RID: 41
	public static class DateFormatHelper
	{
		// Token: 0x06000117 RID: 279 RVA: 0x0000B30C File Offset: 0x0000950C
		public static bool TryParseDateTimeOffset(string s, out DateTimeOffset offset)
		{
			DateFormatHelper.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.s = s;
			offset = DateTimeOffset.MinValue;
			if (CS$<>8__locals1.s == null || CS$<>8__locals1.s.Length < 4)
			{
				return false;
			}
			bool result;
			try
			{
				int num = 0;
				if (CS$<>8__locals1.s.get_Chars(0) == 'D' && CS$<>8__locals1.s.get_Chars(1) == ':')
				{
					num = 2;
				}
				int num2;
				if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 4, ref CS$<>8__locals1))
				{
					result = false;
				}
				else if (!int.TryParse(CS$<>8__locals1.s.Substring(num, 4), ref num2))
				{
					result = false;
				}
				else
				{
					num += 4;
					int num3;
					if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 2, ref CS$<>8__locals1))
					{
						if (!DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
						{
							result = false;
						}
						else
						{
							offset = new DateTimeOffset(num2, 1, 1, 0, 0, 0, TimeSpan.Zero);
							result = true;
						}
					}
					else if (!int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num3) || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num3, 1, 12))
					{
						result = false;
					}
					else
					{
						num += 2;
						int num4;
						if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 2, ref CS$<>8__locals1))
						{
							if (!DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
							{
								result = false;
							}
							else
							{
								offset = new DateTimeOffset(num2, num3, 1, 0, 0, 0, TimeSpan.Zero);
								result = true;
							}
						}
						else if (!int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num4) || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num4, 1, 31))
						{
							result = false;
						}
						else
						{
							num += 2;
							int num5;
							if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 2, ref CS$<>8__locals1))
							{
								if (!DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
								{
									result = false;
								}
								else
								{
									offset = new DateTimeOffset(num2, num3, num4, 0, 0, 0, TimeSpan.Zero);
									result = true;
								}
							}
							else if (!int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num5) || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num5, 0, 23))
							{
								result = false;
							}
							else
							{
								num += 2;
								int num6;
								if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 2, ref CS$<>8__locals1))
								{
									if (!DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
									{
										result = false;
									}
									else
									{
										offset = new DateTimeOffset(num2, num3, num4, num5, 0, 0, TimeSpan.Zero);
										result = true;
									}
								}
								else if (!int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num6) || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num6, 0, 59))
								{
									result = false;
								}
								else
								{
									num += 2;
									int num7;
									if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 2, ref CS$<>8__locals1))
									{
										if (!DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
										{
											result = false;
										}
										else
										{
											offset = new DateTimeOffset(num2, num3, num4, num5, num6, 0, TimeSpan.Zero);
											result = true;
										}
									}
									else if (!int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num7) || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num7, 0, 59))
									{
										result = false;
									}
									else
									{
										num += 2;
										if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 1, ref CS$<>8__locals1))
										{
											if (!DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
											{
												result = false;
											}
											else
											{
												offset = new DateTimeOffset(num2, num3, num4, num5, num6, num7, TimeSpan.Zero);
												result = true;
											}
										}
										else
										{
											char c = CS$<>8__locals1.s.get_Chars(num++);
											if (c != '-' && c != '+' && c != 'Z')
											{
												result = false;
											}
											else
											{
												int num8 = (c == '-') ? -1 : ((c == '+') ? 1 : 0);
												int num9;
												if (DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
												{
													offset = new DateTimeOffset(num2, num3, num4, num5, num6, num7, TimeSpan.Zero);
													result = true;
												}
												else if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 3, ref CS$<>8__locals1) || !int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num9) || CS$<>8__locals1.s.get_Chars(num + 2) != '\'' || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num9, 0, 23))
												{
													result = false;
												}
												else
												{
													num += 3;
													int num10;
													if (DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
													{
														offset = new DateTimeOffset(num2, num3, num4, num5, num6, num7, TimeSpan.FromHours((double)(num9 * num8)));
														result = true;
													}
													else if (!DateFormatHelper.<TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(num, 3, ref CS$<>8__locals1) || !int.TryParse(CS$<>8__locals1.s.Substring(num, 2), ref num10) || CS$<>8__locals1.s.get_Chars(num + 2) != '\'' || !DateFormatHelper.<TryParseDateTimeOffset>g__IsWithinRange|0_2(num10, 0, 59))
													{
														result = false;
													}
													else
													{
														num += 3;
														if (DateFormatHelper.<TryParseDateTimeOffset>g__IsAtEnd|0_1(num, ref CS$<>8__locals1))
														{
															offset = new DateTimeOffset(num2, num3, num4, num5, num6, num7, new TimeSpan(num9 * num8, num10 * num8, 0));
															result = true;
														}
														else
														{
															result = false;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000B780 File Offset: 0x00009980
		[CompilerGenerated]
		internal static bool <TryParseDateTimeOffset>g__HasRemainingCharacters|0_0(int pos, int len, ref DateFormatHelper.<>c__DisplayClass0_0 A_2)
		{
			return pos + len <= A_2.s.Length;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000B795 File Offset: 0x00009995
		[CompilerGenerated]
		internal static bool <TryParseDateTimeOffset>g__IsAtEnd|0_1(int pos, ref DateFormatHelper.<>c__DisplayClass0_0 A_1)
		{
			return pos == A_1.s.Length;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000B7A5 File Offset: 0x000099A5
		[CompilerGenerated]
		internal static bool <TryParseDateTimeOffset>g__IsWithinRange|0_2(int val, int min, int max)
		{
			return val >= min && val <= max;
		}
	}
}
