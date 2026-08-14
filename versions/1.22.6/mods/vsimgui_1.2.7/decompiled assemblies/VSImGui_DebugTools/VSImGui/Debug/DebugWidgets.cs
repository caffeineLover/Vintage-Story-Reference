using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.MathTools;

namespace VSImGui.Debug
{
	// Token: 0x02000003 RID: 3
	[NullableContext(1)]
	[Nullable(0)]
	public static class DebugWidgets
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
		public static void Draw(string domain, string category, int id, Action drawDelegate)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			DebugWindowsManager.DrawEntries[domain][id] = new DebugWindowsManager.DrawEntry(domain, category, drawDelegate);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A2 File Offset: 0x000003A2
		public static void Remove(string domain, int id)
		{
			DebugWindowsManager.DrawEntries[domain].Remove(id);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B8 File Offset: 0x000003B8
		public static int Button(string domain, string category, string label, Action<bool> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Action<bool> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(ImGui.Button(MemoryExtensions.AsSpan(label)));
			});
			return hashCode;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002228 File Offset: 0x00000428
		public static int Button(string domain, string category, string label, Action callback)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				if (ImGui.Button(MemoryExtensions.AsSpan(label)))
				{
					Action callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2.Invoke();
				}
			});
			return hashCode;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002298 File Offset: 0x00000498
		public static int CheckBox(string domain, string category, string label, Func<bool> getter, Action<bool> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<bool> getter2 = getter;
				bool flag = getter2 != null && getter2.Invoke();
				ImGui.Checkbox(MemoryExtensions.AsSpan(label), ref flag);
				Action<bool> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(flag);
			});
			return hashCode;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002310 File Offset: 0x00000510
		public static int IntSlider(string domain, string category, string label, int min, int max, Func<int> getter, Action<int> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<int> getter2 = getter;
				int num = (getter2 != null) ? getter2.Invoke() : 0;
				ImGui.SliderInt(MemoryExtensions.AsSpan(label), ref num, min, max);
				Action<int> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(num);
			});
			return hashCode;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002398 File Offset: 0x00000598
		public static int FloatSlider(string domain, string category, string label, float min, float max, Func<float> getter, Action<float> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<float> getter2 = getter;
				float num = (getter2 != null) ? getter2.Invoke() : 0f;
				ImGui.SliderFloat(MemoryExtensions.AsSpan(label), ref num, min, max);
				Action<float> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(num);
			});
			return hashCode;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002420 File Offset: 0x00000620
		public static int Text(string domain, string category, int id, string text)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			DebugWindowsManager.DrawEntries[domain][id] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				ImGui.Text(MemoryExtensions.AsSpan(text));
			});
			return id;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000247C File Offset: 0x0000067C
		public static int TextInput(string domain, string category, string label, Func<string> getter, Action<string> setter, uint maxLength = 512U, bool controlButtons = false)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<string> getter2 = getter;
				string text = ((getter2 != null) ? getter2.Invoke() : null) ?? "";
				if (controlButtons)
				{
					Controls.CopyPasteSupport(ref text, "Copy##" + label, "Paste##" + label);
					ImGui.SameLine();
				}
				ImGui.InputText(MemoryExtensions.AsSpan(label), ref text, maxLength);
				Action<string> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(text);
			});
			return hashCode;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002504 File Offset: 0x00000704
		public static int TextInputMultiline(string domain, string category, string label, Func<string> getter, Action<string> setter, uint maxLength = 512U, bool controlButtons = false, ImGuiInputTextFlags flags = 0, float width = 0f, float height = 0f)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<string> getter2 = getter;
				string text = ((getter2 != null) ? getter2.Invoke() : null) ?? "";
				if (controlButtons)
				{
					Controls.CopyPasteSupport(ref text, "Copy##" + label, "Paste##" + label);
				}
				ImGui.InputTextMultiline(MemoryExtensions.AsSpan(label), ref text, maxLength, new Vector2(width, height), flags);
				Action<string> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(text);
			});
			return hashCode;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025A4 File Offset: 0x000007A4
		public static int IntDrag(string domain, string category, string label, Func<int> getter, Action<int> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<int> getter2 = getter;
				int num = (getter2 != null) ? getter2.Invoke() : 0;
				ImGui.DragInt(MemoryExtensions.AsSpan(label), ref num);
				Action<int> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(num);
			});
			return hashCode;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000261C File Offset: 0x0000081C
		public static int FloatDrag(string domain, string category, string label, Func<float> getter, Action<float> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<float> getter2 = getter;
				float num = (getter2 != null) ? getter2.Invoke() : 0f;
				ImGui.DragFloat(MemoryExtensions.AsSpan(label), ref num);
				Action<float> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(num);
			});
			return hashCode;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002694 File Offset: 0x00000894
		public static int Float3Drag(string domain, string category, string label, Func<Vec3f> getter, Action<Vec3f> setter)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<Vec3f> getter2 = getter;
				Vec3f vec3f = ((getter2 != null) ? getter2.Invoke() : null) ?? new Vec3f(0f, 0f, 0f);
				Vector3 vector;
				vector..ctor(vec3f.X, vec3f.Y, vec3f.Z);
				ImGui.DragFloat3(MemoryExtensions.AsSpan(label), ref vector);
				vec3f.X = vector.X;
				vec3f.Y = vector.Y;
				vec3f.Z = vector.Z;
				Action<Vec3f> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(vec3f);
			});
			return hashCode;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000270C File Offset: 0x0000090C
		public static int ColorPicker(string domain, string category, string label, Func<Vec4f> getter, Action<Vec4f> setter, ImGuiColorEditFlags flags = 0)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				Func<Vec4f> getter2 = getter;
				Vec4f vec4f = ((getter2 != null) ? getter2.Invoke() : null) ?? new Vec4f(0f, 0f, 0f, 0f);
				Vector4 vector;
				vector..ctor(vec4f.X, vec4f.Y, vec4f.Z, vec4f.W);
				ImGui.ColorEdit4(MemoryExtensions.AsSpan(label), ref vector, flags);
				vec4f.X = vector.X;
				vec4f.Y = vector.Y;
				vec4f.Z = vector.Z;
				vec4f.W = vector.W;
				Action<Vec4f> setter2 = setter;
				if (setter2 == null)
				{
					return;
				}
				setter2.Invoke(vec4f);
			});
			return hashCode;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000278C File Offset: 0x0000098C
		public static int EnumCombo<[Nullable(0)] TEnum>(string domain, string category, string label, [Nullable(new byte[]
		{
			1,
			0
		})] Func<TEnum> getter, [Nullable(new byte[]
		{
			1,
			0
		})] Action<TEnum> setter) where TEnum : struct, Enum
		{
			DebugWidgets.<>c__DisplayClass14_0<TEnum> CS$<>8__locals1 = new DebugWidgets.<>c__DisplayClass14_0<TEnum>();
			CS$<>8__locals1.getter = getter;
			CS$<>8__locals1.label = label;
			CS$<>8__locals1.setter = setter;
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = CS$<>8__locals1.label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, new Action(CS$<>8__locals1.<EnumCombo>b__0));
			return hashCode;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002804 File Offset: 0x00000A04
		public static int EnumList<[Nullable(0)] TEnum>(string domain, string category, string label, [Nullable(new byte[]
		{
			1,
			0
		})] Func<TEnum> getter, [Nullable(new byte[]
		{
			1,
			0
		})] Action<TEnum> setter) where TEnum : struct, Enum
		{
			DebugWidgets.<>c__DisplayClass15_0<TEnum> CS$<>8__locals1 = new DebugWidgets.<>c__DisplayClass15_0<TEnum>();
			CS$<>8__locals1.getter = getter;
			CS$<>8__locals1.label = label;
			CS$<>8__locals1.setter = setter;
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = CS$<>8__locals1.label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, new Action(CS$<>8__locals1.<EnumList>b__0));
			return hashCode;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000287C File Offset: 0x00000A7C
		public static int EnumList<[Nullable(0)] TEnum>(string domain, string category, string label, string filterHolder, [Nullable(new byte[]
		{
			1,
			0
		})] Func<TEnum> getter, [Nullable(new byte[]
		{
			1,
			0
		})] Action<TEnum> setter) where TEnum : struct, Enum
		{
			DebugWidgets.<>c__DisplayClass16_0<TEnum> CS$<>8__locals1 = new DebugWidgets.<>c__DisplayClass16_0<TEnum>();
			CS$<>8__locals1.getter = getter;
			CS$<>8__locals1.label = label;
			CS$<>8__locals1.filterHolder = filterHolder;
			CS$<>8__locals1.setter = setter;
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = CS$<>8__locals1.label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, new Action(CS$<>8__locals1.<EnumList>b__0));
			return hashCode;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028FC File Offset: 0x00000AFC
		public static int StringList(string domain, string category, string label, string filterHolder, string[] list, Func<int> getter, [Nullable(2)] Action<int> indexSetter, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> valueSetter = null)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				int num = getter.Invoke();
				string label2 = label;
				string filterHolder2 = filterHolder;
				IEnumerable<string> list2 = list;
				string text;
				TextEditor.ListWithFilter(label2, filterHolder2, list2, ref num, out text, 256U, "supports wildcards");
				Action<int> indexSetter2 = indexSetter;
				if (indexSetter2 != null)
				{
					indexSetter2.Invoke(num);
				}
				Action<string> valueSetter2 = valueSetter;
				if (valueSetter2 == null)
				{
					return;
				}
				valueSetter2.Invoke(text);
			});
			return hashCode;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000298C File Offset: 0x00000B8C
		public static int StringList(string domain, string category, string label, string filterHolder, string[] list, Func<int> getter, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> valueSetter, [Nullable(2)] Action<int> indexSetter = null)
		{
			if (!DebugWindowsManager.DrawEntries.ContainsKey(domain))
			{
				DebugWindowsManager.DrawEntries[domain] = new Dictionary<int, DebugWindowsManager.DrawEntry>();
			}
			int hashCode = label.GetHashCode();
			DebugWindowsManager.DrawEntries[domain][hashCode] = new DebugWindowsManager.DrawEntry(domain, category, delegate
			{
				int num = getter.Invoke();
				string label2 = label;
				string filterHolder2 = filterHolder;
				IEnumerable<string> list2 = list;
				string text;
				TextEditor.ListWithFilter(label2, filterHolder2, list2, ref num, out text, 256U, "supports wildcards");
				Action<int> indexSetter2 = indexSetter;
				if (indexSetter2 != null)
				{
					indexSetter2.Invoke(num);
				}
				Action<string> valueSetter2 = valueSetter;
				if (valueSetter2 == null)
				{
					return;
				}
				valueSetter2.Invoke(text);
			});
			return hashCode;
		}
	}
}
