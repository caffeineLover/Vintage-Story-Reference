using System;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui
{
	// Token: 0x0200001E RID: 30
	[NullableContext(1)]
	[Nullable(0)]
	public static class TimeEditor
	{
		// Token: 0x06000259 RID: 601 RVA: 0x0000CCA0 File Offset: 0x0000AEA0
		public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale)
		{
			float value_f = TimeEditor.Get(title, value, ref timeScale);
			ImGui.DragFloat(MemoryExtensions.AsSpan(title), ref value_f);
			value = TimeEditor.Set(value_f, timeScale);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000CCD7 File Offset: 0x0000AED7
		public static TimeSpan WithScaleSelection(string title, TimeSpan value, ref int timeScale)
		{
			TimeEditor.WithScaleSelection(title, ref value, ref timeScale);
			return value;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		public static void WithScaleSelection(string title, ref TimeSpan value, ref int timeScale, TimeSpan min, TimeSpan max)
		{
			float value_f = TimeEditor.Get(title, value, ref timeScale);
			ImGui.SliderFloat(MemoryExtensions.AsSpan(title), ref value_f, TimeEditor.Get(min, timeScale), TimeEditor.Get(max, timeScale));
			value = TimeEditor.Set(value_f, timeScale);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000CD2C File Offset: 0x0000AF2C
		public static TimeSpan WithScaleSelection(string title, TimeSpan value, ref int timeScale, TimeSpan min, TimeSpan max)
		{
			TimeEditor.WithScaleSelection(title, ref value, ref timeScale, min, max);
			return value;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000CD3B File Offset: 0x0000AF3B
		private static float Get(string title, TimeSpan value, ref int timeScale)
		{
			ImGui.Combo(MemoryExtensions.AsSpan("##selector" + title), ref timeScale, TimeEditor.TimeTypes, TimeEditor.TimeTypes.Length);
			ImGui.SameLine();
			return TimeEditor.Get(value, timeScale);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000CD70 File Offset: 0x0000AF70
		private static float Get(TimeSpan value, int timeScale)
		{
			string text = TimeEditor.TimeTypes[timeScale];
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

		// Token: 0x0600025F RID: 607 RVA: 0x0000CE28 File Offset: 0x0000B028
		private static TimeSpan Set(float value, int timeScale)
		{
			string text = TimeEditor.TimeTypes[timeScale];
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

		// Token: 0x04000046 RID: 70
		public static readonly string[] TimeTypes = new string[]
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
