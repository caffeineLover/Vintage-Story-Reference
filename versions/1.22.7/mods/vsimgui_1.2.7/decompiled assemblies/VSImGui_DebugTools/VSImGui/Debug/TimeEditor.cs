using System;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui.Debug
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TimeEditor
	{
		// Token: 0x06000041 RID: 65 RVA: 0x0000343C File Offset: 0x0000163C
		public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale)
		{
			float value2 = TimeEditor.Get(title, value, ref timeScale);
			ImGui.DragFloat(MemoryExtensions.AsSpan(title), ref value2);
			value = TimeEditor.Set(value2, timeScale);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003474 File Offset: 0x00001674
		public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale, float min, float max)
		{
			float value2 = TimeEditor.Get(title, value, ref timeScale);
			ImGui.SliderFloat(MemoryExtensions.AsSpan(title), ref value2, min, max);
			value = TimeEditor.Set(value2, timeScale);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000034B0 File Offset: 0x000016B0
		public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale, TimeSpan min, TimeSpan max)
		{
			float value2 = TimeEditor.Get(title, value, ref timeScale);
			ImGui.SliderFloat(MemoryExtensions.AsSpan(title), ref value2, TimeEditor.Get(min, timeScale), TimeEditor.Get(max, timeScale));
			value = TimeEditor.Set(value2, timeScale);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000034F8 File Offset: 0x000016F8
		private static float Get(string title, TimeSpan value, ref int timeScale)
		{
			ImGui.Combo(MemoryExtensions.AsSpan("##selector" + title), ref timeScale, TimeEditor.sTimeTypes, TimeEditor.sTimeTypes.Length);
			ImGui.SameLine();
			return TimeEditor.Get(value, timeScale);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000352C File Offset: 0x0000172C
		private static float Get(TimeSpan value, int timeScale)
		{
			string text = TimeEditor.sTimeTypes[timeScale];
			double num;
			if (!(text == "microseconds"))
			{
				if (!(text == "milliseconds"))
				{
					if (!(text == "seconds"))
					{
						if (!(text == "minutes"))
						{
							if (!(text == "hours"))
							{
								if (!(text == "days"))
								{
									num = value.TotalMilliseconds;
								}
								else
								{
									num = (double)((float)value.TotalDays);
								}
							}
							else
							{
								num = (double)((float)value.TotalHours);
							}
						}
						else
						{
							num = (double)((float)value.TotalMinutes);
						}
					}
					else
					{
						num = (double)((float)value.TotalSeconds);
					}
				}
				else
				{
					num = (double)((float)value.TotalMilliseconds);
				}
			}
			else
			{
				num = (double)((float)value.TotalMicroseconds);
			}
			return (float)num;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000035E4 File Offset: 0x000017E4
		private static TimeSpan Set(float value, int timeScale)
		{
			string text = TimeEditor.sTimeTypes[timeScale];
			TimeSpan result;
			if (!(text == "microseconds"))
			{
				if (!(text == "milliseconds"))
				{
					if (!(text == "seconds"))
					{
						if (!(text == "minutes"))
						{
							if (!(text == "hours"))
							{
								if (!(text == "days"))
								{
									result = TimeSpan.FromMilliseconds((double)value);
								}
								else
								{
									result = TimeSpan.FromDays((double)value);
								}
							}
							else
							{
								result = TimeSpan.FromHours((double)value);
							}
						}
						else
						{
							result = TimeSpan.FromMinutes((double)value);
						}
					}
					else
					{
						result = TimeSpan.FromSeconds((double)value);
					}
				}
				else
				{
					result = TimeSpan.FromMilliseconds((double)value);
				}
			}
			else
			{
				result = TimeSpan.FromMicroseconds((double)value);
			}
			return result;
		}

		// Token: 0x04000007 RID: 7
		public static readonly string[] sTimeTypes = new string[]
		{
			"microseconds",
			"milliseconds",
			"seconds",
			"minutes",
			"hours",
			"days"
		};
	}
}
