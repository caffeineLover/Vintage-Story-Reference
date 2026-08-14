using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200002C RID: 44
	public static class ImGui
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00004504 File Offset: 0x00002704
		public unsafe static ImGuiPayloadPtr AcceptDragDropPayload(ReadOnlySpan<char> type)
		{
			int num = 0;
			byte* ptr;
			if (type != null)
			{
				num = Encoding.UTF8.GetByteCount(type);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(type, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiDragDropFlags flags = ImGuiDragDropFlags.None;
			ImGuiPayload* nativePtr = ImGuiNative.igAcceptDragDropPayload(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImGuiPayloadPtr(nativePtr);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00004578 File Offset: 0x00002778
		public unsafe static ImGuiPayloadPtr AcceptDragDropPayload(ReadOnlySpan<char> type, ImGuiDragDropFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (type != null)
			{
				num = Encoding.UTF8.GetByteCount(type);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(type, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPayload* nativePtr = ImGuiNative.igAcceptDragDropPayload(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return new ImGuiPayloadPtr(nativePtr);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000045E8 File Offset: 0x000027E8
		public static void AlignTextToFramePadding()
		{
			ImGuiNative.igAlignTextToFramePadding();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000045F0 File Offset: 0x000027F0
		public unsafe static bool ArrowButton(ReadOnlySpan<char> str_id, ImGuiDir dir)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igArrowButton(ptr, dir);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00004660 File Offset: 0x00002860
		public unsafe static bool Begin(ReadOnlySpan<char> name)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* p_open = null;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBegin(ptr, p_open, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000046D8 File Offset: 0x000028D8
		public unsafe static bool Begin(ReadOnlySpan<char> name, ref bool p_open)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_open > false) ? 1 : 0;
			byte* p_open2 = &b;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBegin(ptr, p_open2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_open = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00004760 File Offset: 0x00002960
		public unsafe static bool Begin(ReadOnlySpan<char> name, ref bool p_open, ImGuiWindowFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_open > false) ? 1 : 0;
			byte* p_open2 = &b;
			int num2 = (int)ImGuiNative.igBegin(ptr, p_open2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_open = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000047E4 File Offset: 0x000029E4
		public unsafe static bool BeginChild(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			Vector2 size = default(Vector2);
			byte border = 0;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBeginChild_Str(ptr, size, border, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00004864 File Offset: 0x00002A64
		public unsafe static bool BeginChild(ReadOnlySpan<char> str_id, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte border = 0;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBeginChild_Str(ptr, size, border, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000048DC File Offset: 0x00002ADC
		public unsafe static bool BeginChild(ReadOnlySpan<char> str_id, Vector2 size, bool border)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte border2 = (border > false) ? 1 : 0;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBeginChild_Str(ptr, size, border2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00004958 File Offset: 0x00002B58
		public unsafe static bool BeginChild(ReadOnlySpan<char> str_id, Vector2 size, bool border, ImGuiWindowFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte border2 = (border > false) ? 1 : 0;
			int num2 = (int)ImGuiNative.igBeginChild_Str(ptr, size, border2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000049D0 File Offset: 0x00002BD0
		public static bool BeginChild(uint id)
		{
			Vector2 size = default(Vector2);
			byte border = 0;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			return ImGuiNative.igBeginChild_ID(id, size, border, flags) > 0;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000049F8 File Offset: 0x00002BF8
		public static bool BeginChild(uint id, Vector2 size)
		{
			byte border = 0;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			return ImGuiNative.igBeginChild_ID(id, size, border, flags) > 0;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00004A18 File Offset: 0x00002C18
		public static bool BeginChild(uint id, Vector2 size, bool border)
		{
			byte border2 = (border > false) ? 1 : 0;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			return ImGuiNative.igBeginChild_ID(id, size, border2, flags) > 0;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00004A3C File Offset: 0x00002C3C
		public static bool BeginChild(uint id, Vector2 size, bool border, ImGuiWindowFlags flags)
		{
			byte border2 = (border > false) ? 1 : 0;
			return ImGuiNative.igBeginChild_ID(id, size, border2, flags) > 0;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00004A5C File Offset: 0x00002C5C
		public static bool BeginChildFrame(uint id, Vector2 size)
		{
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			return ImGuiNative.igBeginChildFrame(id, size, flags) > 0;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00004A76 File Offset: 0x00002C76
		public static bool BeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags)
		{
			return ImGuiNative.igBeginChildFrame(id, size, flags) > 0;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00004A84 File Offset: 0x00002C84
		public unsafe static bool BeginCombo(ReadOnlySpan<char> label, ReadOnlySpan<char> preview_value)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (preview_value != null)
			{
				num2 = Encoding.UTF8.GetByteCount(preview_value);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(preview_value, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiComboFlags flags = ImGuiComboFlags.None;
			int num3 = (int)ImGuiNative.igBeginCombo(ptr, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00004B54 File Offset: 0x00002D54
		public unsafe static bool BeginCombo(ReadOnlySpan<char> label, ReadOnlySpan<char> preview_value, ImGuiComboFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (preview_value != null)
			{
				num2 = Encoding.UTF8.GetByteCount(preview_value);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(preview_value, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igBeginCombo(ptr, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00004C1E File Offset: 0x00002E1E
		public static void BeginDisabled()
		{
			ImGuiNative.igBeginDisabled(1);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00004C26 File Offset: 0x00002E26
		public static void BeginDisabled(bool disabled)
		{
			ImGuiNative.igBeginDisabled((disabled > false) ? 1 : 0);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00004C32 File Offset: 0x00002E32
		public static bool BeginDragDropSource()
		{
			return ImGuiNative.igBeginDragDropSource(ImGuiDragDropFlags.None) > 0;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00004C3D File Offset: 0x00002E3D
		public static bool BeginDragDropSource(ImGuiDragDropFlags flags)
		{
			return ImGuiNative.igBeginDragDropSource(flags) > 0;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00004C48 File Offset: 0x00002E48
		public static bool BeginDragDropTarget()
		{
			return ImGuiNative.igBeginDragDropTarget() > 0;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00004C52 File Offset: 0x00002E52
		public static void BeginGroup()
		{
			ImGuiNative.igBeginGroup();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00004C59 File Offset: 0x00002E59
		public static bool BeginItemTooltip()
		{
			return ImGuiNative.igBeginItemTooltip() > 0;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00004C64 File Offset: 0x00002E64
		public unsafe static bool BeginListBox(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginListBox(ptr, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00004CDC File Offset: 0x00002EDC
		public unsafe static bool BeginListBox(ReadOnlySpan<char> label, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginListBox(ptr, size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00004D4A File Offset: 0x00002F4A
		public static bool BeginMainMenuBar()
		{
			return ImGuiNative.igBeginMainMenuBar() > 0;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00004D54 File Offset: 0x00002F54
		public unsafe static bool BeginMenu(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte enabled = 1;
			int num2 = (int)ImGuiNative.igBeginMenu(ptr, enabled);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public unsafe static bool BeginMenu(ReadOnlySpan<char> label, bool enabled)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte enabled2 = (enabled > false) ? 1 : 0;
			int num2 = (int)ImGuiNative.igBeginMenu(ptr, enabled2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00004E38 File Offset: 0x00003038
		public static bool BeginMenuBar()
		{
			return ImGuiNative.igBeginMenuBar() > 0;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00004E44 File Offset: 0x00003044
		public unsafe static bool BeginPopup(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBeginPopup(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00004EB4 File Offset: 0x000030B4
		public unsafe static bool BeginPopup(ReadOnlySpan<char> str_id, ImGuiWindowFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginPopup(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00004F24 File Offset: 0x00003124
		public unsafe static bool BeginPopupContextItem()
		{
			byte* str_id = null;
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			return ImGuiNative.igBeginPopupContextItem(str_id, popup_flags) > 0;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00004F40 File Offset: 0x00003140
		public unsafe static bool BeginPopupContextItem(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			int num2 = (int)ImGuiNative.igBeginPopupContextItem(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00004FB0 File Offset: 0x000031B0
		public unsafe static bool BeginPopupContextItem(ReadOnlySpan<char> str_id, ImGuiPopupFlags popup_flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginPopupContextItem(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00005020 File Offset: 0x00003220
		public unsafe static bool BeginPopupContextVoid()
		{
			byte* str_id = null;
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			return ImGuiNative.igBeginPopupContextVoid(str_id, popup_flags) > 0;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000503C File Offset: 0x0000323C
		public unsafe static bool BeginPopupContextVoid(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			int num2 = (int)ImGuiNative.igBeginPopupContextVoid(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000050AC File Offset: 0x000032AC
		public unsafe static bool BeginPopupContextVoid(ReadOnlySpan<char> str_id, ImGuiPopupFlags popup_flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginPopupContextVoid(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000511C File Offset: 0x0000331C
		public unsafe static bool BeginPopupContextWindow()
		{
			byte* str_id = null;
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			return ImGuiNative.igBeginPopupContextWindow(str_id, popup_flags) > 0;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00005138 File Offset: 0x00003338
		public unsafe static bool BeginPopupContextWindow(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			int num2 = (int)ImGuiNative.igBeginPopupContextWindow(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000051A8 File Offset: 0x000033A8
		public unsafe static bool BeginPopupContextWindow(ReadOnlySpan<char> str_id, ImGuiPopupFlags popup_flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginPopupContextWindow(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00005218 File Offset: 0x00003418
		public unsafe static bool BeginPopupModal(ReadOnlySpan<char> name)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* p_open = null;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBeginPopupModal(ptr, p_open, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00005290 File Offset: 0x00003490
		public unsafe static bool BeginPopupModal(ReadOnlySpan<char> name, ref bool p_open)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_open > false) ? 1 : 0;
			byte* p_open2 = &b;
			ImGuiWindowFlags flags = ImGuiWindowFlags.None;
			int num2 = (int)ImGuiNative.igBeginPopupModal(ptr, p_open2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_open = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00005318 File Offset: 0x00003518
		public unsafe static bool BeginPopupModal(ReadOnlySpan<char> name, ref bool p_open, ImGuiWindowFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_open > false) ? 1 : 0;
			byte* p_open2 = &b;
			int num2 = (int)ImGuiNative.igBeginPopupModal(ptr, p_open2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_open = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000539C File Offset: 0x0000359C
		public unsafe static bool BeginTabBar(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiTabBarFlags flags = ImGuiTabBarFlags.None;
			int num2 = (int)ImGuiNative.igBeginTabBar(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000540C File Offset: 0x0000360C
		public unsafe static bool BeginTabBar(ReadOnlySpan<char> str_id, ImGuiTabBarFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginTabBar(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000547C File Offset: 0x0000367C
		public unsafe static bool BeginTabItem(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* p_open = null;
			ImGuiTabItemFlags flags = ImGuiTabItemFlags.None;
			int num2 = (int)ImGuiNative.igBeginTabItem(ptr, p_open, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000054F4 File Offset: 0x000036F4
		public unsafe static bool BeginTabItem(ReadOnlySpan<char> label, ref bool p_open)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_open > false) ? 1 : 0;
			byte* p_open2 = &b;
			ImGuiTabItemFlags flags = ImGuiTabItemFlags.None;
			int num2 = (int)ImGuiNative.igBeginTabItem(ptr, p_open2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_open = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000557C File Offset: 0x0000377C
		public unsafe static bool BeginTabItem(ReadOnlySpan<char> label, ref bool p_open, ImGuiTabItemFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_open > false) ? 1 : 0;
			byte* p_open2 = &b;
			int num2 = (int)ImGuiNative.igBeginTabItem(ptr, p_open2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_open = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00005600 File Offset: 0x00003800
		public unsafe static bool BeginTable(ReadOnlySpan<char> str_id, int column)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiTableFlags flags = ImGuiTableFlags.None;
			Vector2 outer_size = default(Vector2);
			float inner_width = 0f;
			int num2 = (int)ImGuiNative.igBeginTable(ptr, column, flags, outer_size, inner_width);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00005688 File Offset: 0x00003888
		public unsafe static bool BeginTable(ReadOnlySpan<char> str_id, int column, ImGuiTableFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			Vector2 outer_size = default(Vector2);
			float inner_width = 0f;
			int num2 = (int)ImGuiNative.igBeginTable(ptr, column, flags, outer_size, inner_width);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000570C File Offset: 0x0000390C
		public unsafe static bool BeginTable(ReadOnlySpan<char> str_id, int column, ImGuiTableFlags flags, Vector2 outer_size)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float inner_width = 0f;
			int num2 = (int)ImGuiNative.igBeginTable(ptr, column, flags, outer_size, inner_width);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00005784 File Offset: 0x00003984
		public unsafe static bool BeginTable(ReadOnlySpan<char> str_id, int column, ImGuiTableFlags flags, Vector2 outer_size, float inner_width)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igBeginTable(ptr, column, flags, outer_size, inner_width);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000057F6 File Offset: 0x000039F6
		public static bool BeginTooltip()
		{
			return ImGuiNative.igBeginTooltip() > 0;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00005800 File Offset: 0x00003A00
		public static void Bullet()
		{
			ImGuiNative.igBullet();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00005808 File Offset: 0x00003A08
		public unsafe static void BulletText(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igBulletText(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00005874 File Offset: 0x00003A74
		public unsafe static bool Button(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igButton(ptr, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000058EC File Offset: 0x00003AEC
		public unsafe static bool Button(ReadOnlySpan<char> label, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igButton(ptr, size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000595A File Offset: 0x00003B5A
		public static float CalcItemWidth()
		{
			return ImGuiNative.igCalcItemWidth();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00005964 File Offset: 0x00003B64
		public unsafe static bool Checkbox(ReadOnlySpan<char> label, ref bool v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (v > false) ? 1 : 0;
			byte* v2 = &b;
			int num2 = (int)ImGuiNative.igCheckbox(ptr, v2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			v = (b > 0);
			return num2 != 0;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000059E8 File Offset: 0x00003BE8
		public unsafe static bool CheckboxFlags(ReadOnlySpan<char> label, ref int flags, int flags_value)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (int* ptr2 = &flags)
			{
				int* flags2 = ptr2;
				int num2 = (int)ImGuiNative.igCheckboxFlags_IntPtr(ptr, flags2, flags_value);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00005A60 File Offset: 0x00003C60
		public unsafe static bool CheckboxFlags(ReadOnlySpan<char> label, ref uint flags, uint flags_value)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (uint* ptr2 = &flags)
			{
				uint* flags2 = ptr2;
				int num2 = (int)ImGuiNative.igCheckboxFlags_UintPtr(ptr, flags2, flags_value);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00005AD6 File Offset: 0x00003CD6
		public static void CloseCurrentPopup()
		{
			ImGuiNative.igCloseCurrentPopup();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00005AE0 File Offset: 0x00003CE0
		public unsafe static bool CollapsingHeader(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None;
			int num2 = (int)ImGuiNative.igCollapsingHeader_TreeNodeFlags(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00005B50 File Offset: 0x00003D50
		public unsafe static bool CollapsingHeader(ReadOnlySpan<char> label, ImGuiTreeNodeFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igCollapsingHeader_TreeNodeFlags(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00005BC0 File Offset: 0x00003DC0
		public unsafe static bool CollapsingHeader(ReadOnlySpan<char> label, ref bool p_visible)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_visible > false) ? 1 : 0;
			byte* p_visible2 = &b;
			ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None;
			int num2 = (int)ImGuiNative.igCollapsingHeader_BoolPtr(ptr, p_visible2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_visible = (b > 0);
			return num2 != 0;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00005C48 File Offset: 0x00003E48
		public unsafe static bool CollapsingHeader(ReadOnlySpan<char> label, ref bool p_visible, ImGuiTreeNodeFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_visible > false) ? 1 : 0;
			byte* p_visible2 = &b;
			int num2 = (int)ImGuiNative.igCollapsingHeader_BoolPtr(ptr, p_visible2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_visible = (b > 0);
			return num2 != 0;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00005CCC File Offset: 0x00003ECC
		public unsafe static bool ColorButton(ReadOnlySpan<char> desc_id, Vector4 col)
		{
			int num = 0;
			byte* ptr;
			if (desc_id != null)
			{
				num = Encoding.UTF8.GetByteCount(desc_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(desc_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiColorEditFlags flags = ImGuiColorEditFlags.None;
			int num2 = (int)ImGuiNative.igColorButton(ptr, col, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00005D48 File Offset: 0x00003F48
		public unsafe static bool ColorButton(ReadOnlySpan<char> desc_id, Vector4 col, ImGuiColorEditFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (desc_id != null)
			{
				num = Encoding.UTF8.GetByteCount(desc_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(desc_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igColorButton(ptr, col, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public unsafe static bool ColorButton(ReadOnlySpan<char> desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (desc_id != null)
			{
				num = Encoding.UTF8.GetByteCount(desc_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(desc_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igColorButton(ptr, col, flags, size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00005E30 File Offset: 0x00004030
		public static uint ColorConvertFloat4ToU32(Vector4 @in)
		{
			return ImGuiNative.igColorConvertFloat4ToU32(@in);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00005E38 File Offset: 0x00004038
		public unsafe static void ColorConvertHSVtoRGB(float h, float s, float v, out float out_r, out float out_g, out float out_b)
		{
			fixed (float* ptr = &out_r)
			{
				float* out_r2 = ptr;
				fixed (float* ptr2 = &out_g)
				{
					float* out_g2 = ptr2;
					fixed (float* ptr3 = &out_b)
					{
						float* out_b2 = ptr3;
						ImGuiNative.igColorConvertHSVtoRGB(h, s, v, out_r2, out_g2, out_b2);
					}
				}
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00005E70 File Offset: 0x00004070
		public unsafe static void ColorConvertRGBtoHSV(float r, float g, float b, out float out_h, out float out_s, out float out_v)
		{
			fixed (float* ptr = &out_h)
			{
				float* out_h2 = ptr;
				fixed (float* ptr2 = &out_s)
				{
					float* out_s2 = ptr2;
					fixed (float* ptr3 = &out_v)
					{
						float* out_v2 = ptr3;
						ImGuiNative.igColorConvertRGBtoHSV(r, g, b, out_h2, out_s2, out_v2);
					}
				}
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00005EA8 File Offset: 0x000040A8
		public unsafe static Vector4 ColorConvertU32ToFloat4(uint @in)
		{
			Vector4 result;
			ImGuiNative.igColorConvertU32ToFloat4(&result, @in);
			return result;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00005EC0 File Offset: 0x000040C0
		public unsafe static bool ColorEdit3(ReadOnlySpan<char> label, ref Vector3 col)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiColorEditFlags flags = ImGuiColorEditFlags.None;
			fixed (Vector3* ptr2 = &col)
			{
				Vector3* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorEdit3(ptr, col2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00005F3C File Offset: 0x0000413C
		public unsafe static bool ColorEdit3(ReadOnlySpan<char> label, ref Vector3 col, ImGuiColorEditFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (Vector3* ptr2 = &col)
			{
				Vector3* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorEdit3(ptr, col2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00005FB4 File Offset: 0x000041B4
		public unsafe static bool ColorEdit4(ReadOnlySpan<char> label, ref Vector4 col)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiColorEditFlags flags = ImGuiColorEditFlags.None;
			fixed (Vector4* ptr2 = &col)
			{
				Vector4* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorEdit4(ptr, col2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00006030 File Offset: 0x00004230
		public unsafe static bool ColorEdit4(ReadOnlySpan<char> label, ref Vector4 col, ImGuiColorEditFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (Vector4* ptr2 = &col)
			{
				Vector4* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorEdit4(ptr, col2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000060A8 File Offset: 0x000042A8
		public unsafe static bool ColorPicker3(ReadOnlySpan<char> label, ref Vector3 col)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiColorEditFlags flags = ImGuiColorEditFlags.None;
			fixed (Vector3* ptr2 = &col)
			{
				Vector3* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorPicker3(ptr, col2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00006124 File Offset: 0x00004324
		public unsafe static bool ColorPicker3(ReadOnlySpan<char> label, ref Vector3 col, ImGuiColorEditFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (Vector3* ptr2 = &col)
			{
				Vector3* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorPicker3(ptr, col2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000619C File Offset: 0x0000439C
		public unsafe static bool ColorPicker4(ReadOnlySpan<char> label, ref Vector4 col)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiColorEditFlags flags = ImGuiColorEditFlags.None;
			float* ref_col = null;
			fixed (Vector4* ptr2 = &col)
			{
				Vector4* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorPicker4(ptr, col2, flags, ref_col);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000621C File Offset: 0x0000441C
		public unsafe static bool ColorPicker4(ReadOnlySpan<char> label, ref Vector4 col, ImGuiColorEditFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float* ref_col = null;
			fixed (Vector4* ptr2 = &col)
			{
				Vector4* col2 = ptr2;
				int num2 = (int)ImGuiNative.igColorPicker4(ptr, col2, flags, ref_col);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00006298 File Offset: 0x00004498
		public unsafe static bool ColorPicker4(ReadOnlySpan<char> label, ref Vector4 col, ImGuiColorEditFlags flags, ref float ref_col)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (Vector4* ptr2 = &col)
			{
				Vector4* col2 = ptr2;
				fixed (float* ptr3 = &ref_col)
				{
					float* ref_col2 = ptr3;
					int num2 = (int)ImGuiNative.igColorPicker4(ptr, col2, flags, ref_col2);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00006318 File Offset: 0x00004518
		public unsafe static void Columns()
		{
			int count = 1;
			byte* id = null;
			byte border = 1;
			ImGuiNative.igColumns(count, id, border);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00006334 File Offset: 0x00004534
		public unsafe static void Columns(int count)
		{
			byte* id = null;
			byte border = 1;
			ImGuiNative.igColumns(count, id, border);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00006350 File Offset: 0x00004550
		public unsafe static void Columns(int count, ReadOnlySpan<char> id)
		{
			int num = 0;
			byte* ptr;
			if (id != null)
			{
				num = Encoding.UTF8.GetByteCount(id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte border = 1;
			ImGuiNative.igColumns(count, ptr, border);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000063C0 File Offset: 0x000045C0
		public unsafe static void Columns(int count, ReadOnlySpan<char> id, bool border)
		{
			int num = 0;
			byte* ptr;
			if (id != null)
			{
				num = Encoding.UTF8.GetByteCount(id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte border2 = (border > false) ? 1 : 0;
			ImGuiNative.igColumns(count, ptr, border2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00006434 File Offset: 0x00004634
		public unsafe static bool Combo(ReadOnlySpan<char> label, ref int current_item, string[] items, int items_count)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int* ptr2 = stackalloc int[checked(unchecked((UIntPtr)items.Length) * 4)];
			int num2 = 0;
			for (int i = 0; i < items.Length; i++)
			{
				string text = items[i];
				ptr2[i] = Encoding.UTF8.GetByteCount(text);
				num2 += ptr2[i] + 1;
			}
			byte* ptr3 = stackalloc byte[(UIntPtr)num2];
			int num3 = 0;
			for (int j = 0; j < items.Length; j++)
			{
				string text2 = items[j];
				char* ptr4;
				if (text2 == null)
				{
					ptr4 = null;
				}
				else
				{
					fixed (char* ptr5 = text2.GetPinnableReference())
					{
						ptr4 = ptr5;
					}
				}
				char* ptr6 = ptr4;
				num3 += Encoding.UTF8.GetBytes(ptr6, text2.Length, ptr3 + num3, ptr2[j]);
				ptr3[num3] = 0;
				num3++;
				char* ptr5 = null;
			}
			byte** ptr7 = stackalloc byte*[checked(unchecked((UIntPtr)items.Length) * (UIntPtr)sizeof(byte*))];
			num3 = 0;
			for (int k = 0; k < items.Length; k++)
			{
				*(IntPtr*)(ptr7 + (IntPtr)k * (IntPtr)sizeof(byte*) / (IntPtr)sizeof(byte*)) = ptr3 + num3;
				num3 += ptr2[k] + 1;
			}
			int popup_max_height_in_items = -1;
			fixed (int* ptr8 = &current_item)
			{
				int* current_item2 = ptr8;
				int num4 = (int)ImGuiNative.igCombo_Str_arr(ptr, current_item2, ptr7, items_count, popup_max_height_in_items);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num4 != 0;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000065B0 File Offset: 0x000047B0
		public unsafe static bool Combo(ReadOnlySpan<char> label, ref int current_item, string[] items, int items_count, int popup_max_height_in_items)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int* ptr2 = stackalloc int[checked(unchecked((UIntPtr)items.Length) * 4)];
			int num2 = 0;
			for (int i = 0; i < items.Length; i++)
			{
				string text = items[i];
				ptr2[i] = Encoding.UTF8.GetByteCount(text);
				num2 += ptr2[i] + 1;
			}
			byte* ptr3 = stackalloc byte[(UIntPtr)num2];
			int num3 = 0;
			for (int j = 0; j < items.Length; j++)
			{
				string text2 = items[j];
				char* ptr4;
				if (text2 == null)
				{
					ptr4 = null;
				}
				else
				{
					fixed (char* ptr5 = text2.GetPinnableReference())
					{
						ptr4 = ptr5;
					}
				}
				char* ptr6 = ptr4;
				num3 += Encoding.UTF8.GetBytes(ptr6, text2.Length, ptr3 + num3, ptr2[j]);
				ptr3[num3] = 0;
				num3++;
				char* ptr5 = null;
			}
			byte** ptr7 = stackalloc byte*[checked(unchecked((UIntPtr)items.Length) * (UIntPtr)sizeof(byte*))];
			num3 = 0;
			for (int k = 0; k < items.Length; k++)
			{
				*(IntPtr*)(ptr7 + (IntPtr)k * (IntPtr)sizeof(byte*) / (IntPtr)sizeof(byte*)) = ptr3 + num3;
				num3 += ptr2[k] + 1;
			}
			fixed (int* ptr8 = &current_item)
			{
				int* current_item2 = ptr8;
				int num4 = (int)ImGuiNative.igCombo_Str_arr(ptr, current_item2, ptr7, items_count, popup_max_height_in_items);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num4 != 0;
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00006728 File Offset: 0x00004928
		public unsafe static bool Combo(ReadOnlySpan<char> label, ref int current_item, ReadOnlySpan<char> items_separated_by_zeros)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (items_separated_by_zeros != null)
			{
				num2 = Encoding.UTF8.GetByteCount(items_separated_by_zeros);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(items_separated_by_zeros, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int popup_max_height_in_items = -1;
			fixed (int* ptr3 = &current_item)
			{
				int* current_item2 = ptr3;
				int num3 = (int)ImGuiNative.igCombo_Str(ptr, current_item2, ptr2, popup_max_height_in_items);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006800 File Offset: 0x00004A00
		public unsafe static bool Combo(ReadOnlySpan<char> label, ref int current_item, ReadOnlySpan<char> items_separated_by_zeros, int popup_max_height_in_items)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (items_separated_by_zeros != null)
			{
				num2 = Encoding.UTF8.GetByteCount(items_separated_by_zeros);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(items_separated_by_zeros, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &current_item)
			{
				int* current_item2 = ptr3;
				int num3 = (int)ImGuiNative.igCombo_Str(ptr, current_item2, ptr2, popup_max_height_in_items);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000068D4 File Offset: 0x00004AD4
		public unsafe static IntPtr CreateContext()
		{
			ImFontAtlas* shared_font_atlas = null;
			return ImGuiNative.igCreateContext(shared_font_atlas);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000068EA File Offset: 0x00004AEA
		public static IntPtr CreateContext(ImFontAtlasPtr shared_font_atlas)
		{
			return ImGuiNative.igCreateContext(shared_font_atlas.NativePtr);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000068F8 File Offset: 0x00004AF8
		public unsafe static bool DebugCheckVersionAndDataLayout(ReadOnlySpan<char> version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert, uint sz_drawidx)
		{
			int num = 0;
			byte* ptr;
			if (version_str != null)
			{
				num = Encoding.UTF8.GetByteCount(version_str);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(version_str, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igDebugCheckVersionAndDataLayout(ptr, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert, sz_drawidx);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00006970 File Offset: 0x00004B70
		public unsafe static void DebugTextEncoding(ReadOnlySpan<char> text)
		{
			int num = 0;
			byte* ptr;
			if (text != null)
			{
				num = Encoding.UTF8.GetByteCount(text);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(text, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igDebugTextEncoding(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000069DA File Offset: 0x00004BDA
		public static void DestroyContext()
		{
			ImGuiNative.igDestroyContext(IntPtr.Zero);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000069E6 File Offset: 0x00004BE6
		public static void DestroyContext(IntPtr ctx)
		{
			ImGuiNative.igDestroyContext(ctx);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000069EE File Offset: 0x00004BEE
		public static void DestroyPlatformWindows()
		{
			ImGuiNative.igDestroyPlatformWindows();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000069F8 File Offset: 0x00004BF8
		public unsafe static uint DockSpace(uint id)
		{
			Vector2 size = default(Vector2);
			ImGuiDockNodeFlags flags = ImGuiDockNodeFlags.None;
			ImGuiWindowClass* window_class = null;
			return ImGuiNative.igDockSpace(id, size, flags, window_class);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00006A1C File Offset: 0x00004C1C
		public unsafe static uint DockSpace(uint id, Vector2 size)
		{
			ImGuiDockNodeFlags flags = ImGuiDockNodeFlags.None;
			ImGuiWindowClass* window_class = null;
			return ImGuiNative.igDockSpace(id, size, flags, window_class);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00006A38 File Offset: 0x00004C38
		public unsafe static uint DockSpace(uint id, Vector2 size, ImGuiDockNodeFlags flags)
		{
			ImGuiWindowClass* window_class = null;
			return ImGuiNative.igDockSpace(id, size, flags, window_class);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00006A54 File Offset: 0x00004C54
		public unsafe static uint DockSpace(uint id, Vector2 size, ImGuiDockNodeFlags flags, ImGuiWindowClassPtr window_class)
		{
			ImGuiWindowClass* nativePtr = window_class.NativePtr;
			return ImGuiNative.igDockSpace(id, size, flags, nativePtr);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00006A74 File Offset: 0x00004C74
		public unsafe static uint DockSpaceOverViewport()
		{
			ImGuiViewport* viewport = null;
			ImGuiDockNodeFlags flags = ImGuiDockNodeFlags.None;
			ImGuiWindowClass* window_class = null;
			return ImGuiNative.igDockSpaceOverViewport(viewport, flags, window_class);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00006A94 File Offset: 0x00004C94
		public unsafe static uint DockSpaceOverViewport(ImGuiViewportPtr viewport)
		{
			ImGuiViewport* nativePtr = viewport.NativePtr;
			ImGuiDockNodeFlags flags = ImGuiDockNodeFlags.None;
			ImGuiWindowClass* window_class = null;
			return ImGuiNative.igDockSpaceOverViewport(nativePtr, flags, window_class);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public unsafe static uint DockSpaceOverViewport(ImGuiViewportPtr viewport, ImGuiDockNodeFlags flags)
		{
			ImGuiViewport* nativePtr = viewport.NativePtr;
			ImGuiWindowClass* window_class = null;
			return ImGuiNative.igDockSpaceOverViewport(nativePtr, flags, window_class);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public unsafe static uint DockSpaceOverViewport(ImGuiViewportPtr viewport, ImGuiDockNodeFlags flags, ImGuiWindowClassPtr window_class)
		{
			ImGuiViewport* nativePtr = viewport.NativePtr;
			ImGuiWindowClass* nativePtr2 = window_class.NativePtr;
			return ImGuiNative.igDockSpaceOverViewport(nativePtr, flags, nativePtr2);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00006AF8 File Offset: 0x00004CF8
		public unsafe static bool DragFloat(ReadOnlySpan<char> label, ref float v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00006BEC File Offset: 0x00004DEC
		public unsafe static bool DragFloat(ReadOnlySpan<char> label, ref float v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00006CD8 File Offset: 0x00004ED8
		public unsafe static bool DragFloat(ReadOnlySpan<char> label, ref float v, float v_speed, float v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public unsafe static bool DragFloat(ReadOnlySpan<char> label, ref float v, float v_speed, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00006E8C File Offset: 0x0000508C
		public unsafe static bool DragFloat(ReadOnlySpan<char> label, ref float v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00006F6C File Offset: 0x0000516C
		public unsafe static bool DragFloat(ReadOnlySpan<char> label, ref float v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00007048 File Offset: 0x00005248
		public unsafe static bool DragFloat2(ReadOnlySpan<char> label, ref Vector2 v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000713C File Offset: 0x0000533C
		public unsafe static bool DragFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007228 File Offset: 0x00005428
		public unsafe static bool DragFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_speed, float v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00007308 File Offset: 0x00005508
		public unsafe static bool DragFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_speed, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000073DC File Offset: 0x000055DC
		public unsafe static bool DragFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000074BC File Offset: 0x000056BC
		public unsafe static bool DragFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00007598 File Offset: 0x00005798
		public unsafe static bool DragFloat3(ReadOnlySpan<char> label, ref Vector3 v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000768C File Offset: 0x0000588C
		public unsafe static bool DragFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00007778 File Offset: 0x00005978
		public unsafe static bool DragFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_speed, float v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00007858 File Offset: 0x00005A58
		public unsafe static bool DragFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_speed, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000792C File Offset: 0x00005B2C
		public unsafe static bool DragFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00007A0C File Offset: 0x00005C0C
		public unsafe static bool DragFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00007AE8 File Offset: 0x00005CE8
		public unsafe static bool DragFloat4(ReadOnlySpan<char> label, ref Vector4 v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00007BDC File Offset: 0x00005DDC
		public unsafe static bool DragFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public unsafe static bool DragFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_speed, float v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00007DA8 File Offset: 0x00005FA8
		public unsafe static bool DragFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_speed, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragFloat4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00007E7C File Offset: 0x0000607C
		public unsafe static bool DragFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00007F5C File Offset: 0x0000615C
		public unsafe static bool DragFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragFloat4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00008038 File Offset: 0x00006238
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_current_min)
			{
				float* v_current_min2 = ptr3;
				fixed (float* ptr4 = &v_current_max)
				{
					float* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000813C File Offset: 0x0000633C
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_min = 0f;
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_current_min)
			{
				float* v_current_min2 = ptr3;
				fixed (float* ptr4 = &v_current_max)
				{
					float* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00008238 File Offset: 0x00006438
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_max = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_current_min)
			{
				float* v_current_min2 = ptr3;
				fixed (float* ptr4 = &v_current_max)
				{
					float* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008328 File Offset: 0x00006528
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_current_min)
			{
				float* v_current_min2 = ptr3;
				fixed (float* ptr4 = &v_current_max)
				{
					float* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000840C File Offset: 0x0000660C
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_current_min)
			{
				float* v_current_min2 = ptr3;
				fixed (float* ptr4 = &v_current_max)
				{
					float* v_current_max2 = ptr4;
					int num3 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (num2 > 2048)
					{
						Util.Free(ptr2);
					}
					return num3 != 0;
				}
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000084FC File Offset: 0x000066FC
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format, ReadOnlySpan<char> format_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = 0;
			byte* ptr3;
			if (format_max != null)
			{
				num3 = Encoding.UTF8.GetByteCount(format_max);
				if (num3 > 2048)
				{
					ptr3 = Util.Allocate(num3 + 1);
				}
				else
				{
					ptr3 = stackalloc byte[(UIntPtr)(num3 + 1)];
				}
				int utf3 = Util.GetUtf8(format_max, ptr3, num3);
				ptr3[utf3] = 0;
			}
			else
			{
				ptr3 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr4 = &v_current_min)
			{
				float* v_current_min2 = ptr4;
				fixed (float* ptr5 = &v_current_max)
				{
					float* v_current_max2 = ptr5;
					int num4 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, ptr3, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (num2 > 2048)
					{
						Util.Free(ptr2);
					}
					if (num3 > 2048)
					{
						Util.Free(ptr3);
					}
					return num4 != 0;
				}
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00008654 File Offset: 0x00006854
		public unsafe static bool DragFloatRange2(ReadOnlySpan<char> label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max, ReadOnlySpan<char> format, ReadOnlySpan<char> format_max, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = 0;
			byte* ptr3;
			if (format_max != null)
			{
				num3 = Encoding.UTF8.GetByteCount(format_max);
				if (num3 > 2048)
				{
					ptr3 = Util.Allocate(num3 + 1);
				}
				else
				{
					ptr3 = stackalloc byte[(UIntPtr)(num3 + 1)];
				}
				int utf3 = Util.GetUtf8(format_max, ptr3, num3);
				ptr3[utf3] = 0;
			}
			else
			{
				ptr3 = null;
			}
			fixed (float* ptr4 = &v_current_min)
			{
				float* v_current_min2 = ptr4;
				fixed (float* ptr5 = &v_current_max)
				{
					float* v_current_max2 = ptr5;
					int num4 = (int)ImGuiNative.igDragFloatRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, ptr3, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (num2 > 2048)
					{
						Util.Free(ptr2);
					}
					if (num3 > 2048)
					{
						Util.Free(ptr3);
					}
					return num4 != 0;
				}
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000087A8 File Offset: 0x000069A8
		public unsafe static bool DragInt(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00008894 File Offset: 0x00006A94
		public unsafe static bool DragInt(ReadOnlySpan<char> label, ref int v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008978 File Offset: 0x00006B78
		public unsafe static bool DragInt(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00008A54 File Offset: 0x00006C54
		public unsafe static bool DragInt(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008B28 File Offset: 0x00006D28
		public unsafe static bool DragInt(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00008C08 File Offset: 0x00006E08
		public unsafe static bool DragInt(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00008CE4 File Offset: 0x00006EE4
		public unsafe static bool DragInt2(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public unsafe static bool DragInt2(ReadOnlySpan<char> label, ref int v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00008EB4 File Offset: 0x000070B4
		public unsafe static bool DragInt2(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00008F90 File Offset: 0x00007190
		public unsafe static bool DragInt2(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009064 File Offset: 0x00007264
		public unsafe static bool DragInt2(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009144 File Offset: 0x00007344
		public unsafe static bool DragInt2(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt2(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009220 File Offset: 0x00007420
		public unsafe static bool DragInt3(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000930C File Offset: 0x0000750C
		public unsafe static bool DragInt3(ReadOnlySpan<char> label, ref int v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000093F0 File Offset: 0x000075F0
		public unsafe static bool DragInt3(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000094CC File Offset: 0x000076CC
		public unsafe static bool DragInt3(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000095A0 File Offset: 0x000077A0
		public unsafe static bool DragInt3(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00009680 File Offset: 0x00007880
		public unsafe static bool DragInt3(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt3(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000975C File Offset: 0x0000795C
		public unsafe static bool DragInt4(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009848 File Offset: 0x00007A48
		public unsafe static bool DragInt4(ReadOnlySpan<char> label, ref int v, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000992C File Offset: 0x00007B2C
		public unsafe static bool DragInt4(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009A08 File Offset: 0x00007C08
		public unsafe static bool DragInt4(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igDragInt4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009ADC File Offset: 0x00007CDC
		public unsafe static bool DragInt4(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009BBC File Offset: 0x00007DBC
		public unsafe static bool DragInt4(ReadOnlySpan<char> label, ref int v, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igDragInt4(ptr, v2, v_speed, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009C98 File Offset: 0x00007E98
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_speed = 1f;
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v_current_min)
			{
				int* v_current_min2 = ptr3;
				fixed (int* ptr4 = &v_current_max)
				{
					int* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009D94 File Offset: 0x00007F94
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_min = 0;
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v_current_min)
			{
				int* v_current_min2 = ptr3;
				fixed (int* ptr4 = &v_current_max)
				{
					int* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009E88 File Offset: 0x00008088
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int v_max = 0;
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v_current_min)
			{
				int* v_current_min2 = ptr3;
				fixed (int* ptr4 = &v_current_max)
				{
					int* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009F74 File Offset: 0x00008174
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v_current_min)
			{
				int* v_current_min2 = ptr3;
				fixed (int* ptr4 = &v_current_max)
				{
					int* v_current_max2 = ptr4;
					int num2 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (byteCount > 2048)
					{
						Util.Free(ptr2);
					}
					return num2 != 0;
				}
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000A058 File Offset: 0x00008258
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte* format_max = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v_current_min)
			{
				int* v_current_min2 = ptr3;
				fixed (int* ptr4 = &v_current_max)
				{
					int* v_current_max2 = ptr4;
					int num3 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, format_max, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (num2 > 2048)
					{
						Util.Free(ptr2);
					}
					return num3 != 0;
				}
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000A148 File Offset: 0x00008348
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format, ReadOnlySpan<char> format_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = 0;
			byte* ptr3;
			if (format_max != null)
			{
				num3 = Encoding.UTF8.GetByteCount(format_max);
				if (num3 > 2048)
				{
					ptr3 = Util.Allocate(num3 + 1);
				}
				else
				{
					ptr3 = stackalloc byte[(UIntPtr)(num3 + 1)];
				}
				int utf3 = Util.GetUtf8(format_max, ptr3, num3);
				ptr3[utf3] = 0;
			}
			else
			{
				ptr3 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr4 = &v_current_min)
			{
				int* v_current_min2 = ptr4;
				fixed (int* ptr5 = &v_current_max)
				{
					int* v_current_max2 = ptr5;
					int num4 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, ptr3, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (num2 > 2048)
					{
						Util.Free(ptr2);
					}
					if (num3 > 2048)
					{
						Util.Free(ptr3);
					}
					return num4 != 0;
				}
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000A2A0 File Offset: 0x000084A0
		public unsafe static bool DragIntRange2(ReadOnlySpan<char> label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max, ReadOnlySpan<char> format, ReadOnlySpan<char> format_max, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = 0;
			byte* ptr3;
			if (format_max != null)
			{
				num3 = Encoding.UTF8.GetByteCount(format_max);
				if (num3 > 2048)
				{
					ptr3 = Util.Allocate(num3 + 1);
				}
				else
				{
					ptr3 = stackalloc byte[(UIntPtr)(num3 + 1)];
				}
				int utf3 = Util.GetUtf8(format_max, ptr3, num3);
				ptr3[utf3] = 0;
			}
			else
			{
				ptr3 = null;
			}
			fixed (int* ptr4 = &v_current_min)
			{
				int* v_current_min2 = ptr4;
				fixed (int* ptr5 = &v_current_max)
				{
					int* v_current_max2 = ptr5;
					int num4 = (int)ImGuiNative.igDragIntRange2(ptr, v_current_min2, v_current_max2, v_speed, v_min, v_max, ptr2, ptr3, flags);
					if (num > 2048)
					{
						Util.Free(ptr);
					}
					if (num2 > 2048)
					{
						Util.Free(ptr2);
					}
					if (num3 > 2048)
					{
						Util.Free(ptr3);
					}
					return num4 != 0;
				}
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000A3F4 File Offset: 0x000085F4
		public unsafe static bool DragScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			float v_speed = 1f;
			void* p_min = null;
			void* p_max = null;
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalar(ptr, data_type, p_data2, v_speed, p_min, p_max, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A48C File Offset: 0x0000868C
		public unsafe static bool DragScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min = null;
			void* p_max = null;
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalar(ptr, data_type, p_data2, v_speed, p_min, p_max, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A51C File Offset: 0x0000871C
		public unsafe static bool DragScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, float v_speed, IntPtr p_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max = null;
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalar(ptr, data_type, p_data2, v_speed, p_min2, p_max, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A5B0 File Offset: 0x000087B0
		public unsafe static bool DragScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, float v_speed, IntPtr p_min, IntPtr p_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalar(ptr, data_type, p_data2, v_speed, p_min2, p_max2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A64C File Offset: 0x0000884C
		public unsafe static bool DragScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, float v_speed, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num3 = (int)ImGuiNative.igDragScalar(ptr, data_type, p_data2, v_speed, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000A74C File Offset: 0x0000894C
		public unsafe static bool DragScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, float v_speed, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igDragScalar(ptr, data_type, p_data2, v_speed, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000A848 File Offset: 0x00008A48
		public unsafe static bool DragScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			float v_speed = 1f;
			void* p_min = null;
			void* p_max = null;
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalarN(ptr, data_type, p_data2, components, v_speed, p_min, p_max, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		public unsafe static bool DragScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, float v_speed)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min = null;
			void* p_max = null;
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalarN(ptr, data_type, p_data2, components, v_speed, p_min, p_max, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A974 File Offset: 0x00008B74
		public unsafe static bool DragScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, float v_speed, IntPtr p_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max = null;
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalarN(ptr, data_type, p_data2, components, v_speed, p_min2, p_max, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000AA0C File Offset: 0x00008C0C
		public unsafe static bool DragScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, float v_speed, IntPtr p_min, IntPtr p_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igDragScalarN(ptr, data_type, p_data2, components, v_speed, p_min2, p_max2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		public unsafe static bool DragScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, float v_speed, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num3 = (int)ImGuiNative.igDragScalarN(ptr, data_type, p_data2, components, v_speed, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000ABA8 File Offset: 0x00008DA8
		public unsafe static bool DragScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, float v_speed, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igDragScalarN(ptr, data_type, p_data2, components, v_speed, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000ACA5 File Offset: 0x00008EA5
		public static void Dummy(Vector2 size)
		{
			ImGuiNative.igDummy(size);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000ACAD File Offset: 0x00008EAD
		public static void End()
		{
			ImGuiNative.igEnd();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000ACB4 File Offset: 0x00008EB4
		public static void EndChild()
		{
			ImGuiNative.igEndChild();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000ACBB File Offset: 0x00008EBB
		public static void EndChildFrame()
		{
			ImGuiNative.igEndChildFrame();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000ACC2 File Offset: 0x00008EC2
		public static void EndCombo()
		{
			ImGuiNative.igEndCombo();
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000ACC9 File Offset: 0x00008EC9
		public static void EndDisabled()
		{
			ImGuiNative.igEndDisabled();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		public static void EndDragDropSource()
		{
			ImGuiNative.igEndDragDropSource();
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000ACD7 File Offset: 0x00008ED7
		public static void EndDragDropTarget()
		{
			ImGuiNative.igEndDragDropTarget();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000ACDE File Offset: 0x00008EDE
		public static void EndFrame()
		{
			ImGuiNative.igEndFrame();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000ACE5 File Offset: 0x00008EE5
		public static void EndGroup()
		{
			ImGuiNative.igEndGroup();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000ACEC File Offset: 0x00008EEC
		public static void EndListBox()
		{
			ImGuiNative.igEndListBox();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000ACF3 File Offset: 0x00008EF3
		public static void EndMainMenuBar()
		{
			ImGuiNative.igEndMainMenuBar();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000ACFA File Offset: 0x00008EFA
		public static void EndMenu()
		{
			ImGuiNative.igEndMenu();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000AD01 File Offset: 0x00008F01
		public static void EndMenuBar()
		{
			ImGuiNative.igEndMenuBar();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000AD08 File Offset: 0x00008F08
		public static void EndPopup()
		{
			ImGuiNative.igEndPopup();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000AD0F File Offset: 0x00008F0F
		public static void EndTabBar()
		{
			ImGuiNative.igEndTabBar();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000AD16 File Offset: 0x00008F16
		public static void EndTabItem()
		{
			ImGuiNative.igEndTabItem();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000AD1D File Offset: 0x00008F1D
		public static void EndTable()
		{
			ImGuiNative.igEndTable();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000AD24 File Offset: 0x00008F24
		public static void EndTooltip()
		{
			ImGuiNative.igEndTooltip();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000AD2B File Offset: 0x00008F2B
		public static ImGuiViewportPtr FindViewportByID(uint id)
		{
			return new ImGuiViewportPtr(ImGuiNative.igFindViewportByID(id));
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000AD38 File Offset: 0x00008F38
		public static ImGuiViewportPtr FindViewportByPlatformHandle(IntPtr platform_handle)
		{
			return new ImGuiViewportPtr(ImGuiNative.igFindViewportByPlatformHandle(platform_handle.ToPointer()));
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000AD4C File Offset: 0x00008F4C
		public unsafe static void GetAllocatorFunctions(ref IntPtr p_alloc_func, ref IntPtr p_free_func, ref void* p_user_data)
		{
			fixed (IntPtr* ptr = &p_alloc_func)
			{
				IntPtr* p_alloc_func2 = ptr;
				fixed (IntPtr* ptr2 = &p_free_func)
				{
					IntPtr* p_free_func2 = ptr2;
					fixed (void** ptr3 = &p_user_data)
					{
						void** p_user_data2 = ptr3;
						ImGuiNative.igGetAllocatorFunctions(p_alloc_func2, p_free_func2, p_user_data2);
					}
				}
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000AD7A File Offset: 0x00008F7A
		public static ImDrawListPtr GetBackgroundDrawList()
		{
			return new ImDrawListPtr(ImGuiNative.igGetBackgroundDrawList_Nil());
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000AD86 File Offset: 0x00008F86
		public static ImDrawListPtr GetBackgroundDrawList(ImGuiViewportPtr viewport)
		{
			return new ImDrawListPtr(ImGuiNative.igGetBackgroundDrawList_ViewportPtr(viewport.NativePtr));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000AD99 File Offset: 0x00008F99
		public static string GetClipboardText()
		{
			return Util.StringFromPtr(ImGuiNative.igGetClipboardText());
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		public static uint GetColorU32(ImGuiCol idx)
		{
			float alpha_mul = 1f;
			return ImGuiNative.igGetColorU32_Col(idx, alpha_mul);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000ADC2 File Offset: 0x00008FC2
		public static uint GetColorU32(ImGuiCol idx, float alpha_mul)
		{
			return ImGuiNative.igGetColorU32_Col(idx, alpha_mul);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000ADCB File Offset: 0x00008FCB
		public static uint GetColorU32(Vector4 col)
		{
			return ImGuiNative.igGetColorU32_Vec4(col);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000ADD3 File Offset: 0x00008FD3
		public static uint GetColorU32(uint col)
		{
			return ImGuiNative.igGetColorU32_U32(col);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000ADDB File Offset: 0x00008FDB
		public static int GetColumnIndex()
		{
			return ImGuiNative.igGetColumnIndex();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000ADE2 File Offset: 0x00008FE2
		public static float GetColumnOffset()
		{
			return ImGuiNative.igGetColumnOffset(-1);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000ADEA File Offset: 0x00008FEA
		public static float GetColumnOffset(int column_index)
		{
			return ImGuiNative.igGetColumnOffset(column_index);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000ADF2 File Offset: 0x00008FF2
		public static int GetColumnsCount()
		{
			return ImGuiNative.igGetColumnsCount();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000ADF9 File Offset: 0x00008FF9
		public static float GetColumnWidth()
		{
			return ImGuiNative.igGetColumnWidth(-1);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000AE01 File Offset: 0x00009001
		public static float GetColumnWidth(int column_index)
		{
			return ImGuiNative.igGetColumnWidth(column_index);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000AE0C File Offset: 0x0000900C
		public unsafe static Vector2 GetContentRegionAvail()
		{
			Vector2 result;
			ImGuiNative.igGetContentRegionAvail(&result);
			return result;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000AE24 File Offset: 0x00009024
		public unsafe static Vector2 GetContentRegionMax()
		{
			Vector2 result;
			ImGuiNative.igGetContentRegionMax(&result);
			return result;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000AE3A File Offset: 0x0000903A
		public static IntPtr GetCurrentContext()
		{
			return ImGuiNative.igGetCurrentContext();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000AE44 File Offset: 0x00009044
		public unsafe static Vector2 GetCursorPos()
		{
			Vector2 result;
			ImGuiNative.igGetCursorPos(&result);
			return result;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000AE5A File Offset: 0x0000905A
		public static float GetCursorPosX()
		{
			return ImGuiNative.igGetCursorPosX();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000AE61 File Offset: 0x00009061
		public static float GetCursorPosY()
		{
			return ImGuiNative.igGetCursorPosY();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000AE68 File Offset: 0x00009068
		public unsafe static Vector2 GetCursorScreenPos()
		{
			Vector2 result;
			ImGuiNative.igGetCursorScreenPos(&result);
			return result;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000AE80 File Offset: 0x00009080
		public unsafe static Vector2 GetCursorStartPos()
		{
			Vector2 result;
			ImGuiNative.igGetCursorStartPos(&result);
			return result;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AE96 File Offset: 0x00009096
		public static ImGuiPayloadPtr GetDragDropPayload()
		{
			return new ImGuiPayloadPtr(ImGuiNative.igGetDragDropPayload());
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AEA2 File Offset: 0x000090A2
		public static ImDrawDataPtr GetDrawData()
		{
			return new ImDrawDataPtr(ImGuiNative.igGetDrawData());
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000AEAE File Offset: 0x000090AE
		public static IntPtr GetDrawListSharedData()
		{
			return ImGuiNative.igGetDrawListSharedData();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000AEB5 File Offset: 0x000090B5
		public static ImFontPtr GetFont()
		{
			return new ImFontPtr(ImGuiNative.igGetFont());
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000AEC1 File Offset: 0x000090C1
		public static float GetFontSize()
		{
			return ImGuiNative.igGetFontSize();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000AEC8 File Offset: 0x000090C8
		public unsafe static Vector2 GetFontTexUvWhitePixel()
		{
			Vector2 result;
			ImGuiNative.igGetFontTexUvWhitePixel(&result);
			return result;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000AEDE File Offset: 0x000090DE
		public static ImDrawListPtr GetForegroundDrawList()
		{
			return new ImDrawListPtr(ImGuiNative.igGetForegroundDrawList_Nil());
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000AEEA File Offset: 0x000090EA
		public static ImDrawListPtr GetForegroundDrawList(ImGuiViewportPtr viewport)
		{
			return new ImDrawListPtr(ImGuiNative.igGetForegroundDrawList_ViewportPtr(viewport.NativePtr));
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000AEFD File Offset: 0x000090FD
		public static int GetFrameCount()
		{
			return ImGuiNative.igGetFrameCount();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000AF04 File Offset: 0x00009104
		public static float GetFrameHeight()
		{
			return ImGuiNative.igGetFrameHeight();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AF0B File Offset: 0x0000910B
		public static float GetFrameHeightWithSpacing()
		{
			return ImGuiNative.igGetFrameHeightWithSpacing();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000AF14 File Offset: 0x00009114
		public unsafe static uint GetID(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			uint result = ImGuiNative.igGetID_Str(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return result;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000AF7E File Offset: 0x0000917E
		public static uint GetID(IntPtr ptr_id)
		{
			return ImGuiNative.igGetID_Ptr(ptr_id.ToPointer());
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000AF8C File Offset: 0x0000918C
		public static ImGuiIOPtr GetIO()
		{
			return new ImGuiIOPtr(ImGuiNative.igGetIO());
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000AF98 File Offset: 0x00009198
		public static uint GetItemID()
		{
			return ImGuiNative.igGetItemID();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public unsafe static Vector2 GetItemRectMax()
		{
			Vector2 result;
			ImGuiNative.igGetItemRectMax(&result);
			return result;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000AFB8 File Offset: 0x000091B8
		public unsafe static Vector2 GetItemRectMin()
		{
			Vector2 result;
			ImGuiNative.igGetItemRectMin(&result);
			return result;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public unsafe static Vector2 GetItemRectSize()
		{
			Vector2 result;
			ImGuiNative.igGetItemRectSize(&result);
			return result;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000AFE6 File Offset: 0x000091E6
		public static ImGuiKey GetKeyIndex(ImGuiKey key)
		{
			return ImGuiNative.igGetKeyIndex(key);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000AFEE File Offset: 0x000091EE
		public static string GetKeyName(ImGuiKey key)
		{
			return Util.StringFromPtr(ImGuiNative.igGetKeyName(key));
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000AFFB File Offset: 0x000091FB
		public static int GetKeyPressedAmount(ImGuiKey key, float repeat_delay, float rate)
		{
			return ImGuiNative.igGetKeyPressedAmount(key, repeat_delay, rate);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000B005 File Offset: 0x00009205
		public static ImGuiViewportPtr GetMainViewport()
		{
			return new ImGuiViewportPtr(ImGuiNative.igGetMainViewport());
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B011 File Offset: 0x00009211
		public static int GetMouseClickedCount(ImGuiMouseButton button)
		{
			return ImGuiNative.igGetMouseClickedCount(button);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000B019 File Offset: 0x00009219
		public static ImGuiMouseCursor GetMouseCursor()
		{
			return ImGuiNative.igGetMouseCursor();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000B020 File Offset: 0x00009220
		public unsafe static Vector2 GetMouseDragDelta()
		{
			ImGuiMouseButton button = ImGuiMouseButton.Left;
			float lock_threshold = -1f;
			Vector2 result;
			ImGuiNative.igGetMouseDragDelta(&result, button, lock_threshold);
			return result;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000B040 File Offset: 0x00009240
		public unsafe static Vector2 GetMouseDragDelta(ImGuiMouseButton button)
		{
			float lock_threshold = -1f;
			Vector2 result;
			ImGuiNative.igGetMouseDragDelta(&result, button, lock_threshold);
			return result;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000B060 File Offset: 0x00009260
		public unsafe static Vector2 GetMouseDragDelta(ImGuiMouseButton button, float lock_threshold)
		{
			Vector2 result;
			ImGuiNative.igGetMouseDragDelta(&result, button, lock_threshold);
			return result;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000B078 File Offset: 0x00009278
		public unsafe static Vector2 GetMousePos()
		{
			Vector2 result;
			ImGuiNative.igGetMousePos(&result);
			return result;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000B090 File Offset: 0x00009290
		public unsafe static Vector2 GetMousePosOnOpeningCurrentPopup()
		{
			Vector2 result;
			ImGuiNative.igGetMousePosOnOpeningCurrentPopup(&result);
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B0A6 File Offset: 0x000092A6
		public static ImGuiPlatformIOPtr GetPlatformIO()
		{
			return new ImGuiPlatformIOPtr(ImGuiNative.igGetPlatformIO());
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000B0B2 File Offset: 0x000092B2
		public static float GetScrollMaxX()
		{
			return ImGuiNative.igGetScrollMaxX();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000B0B9 File Offset: 0x000092B9
		public static float GetScrollMaxY()
		{
			return ImGuiNative.igGetScrollMaxY();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000B0C0 File Offset: 0x000092C0
		public static float GetScrollX()
		{
			return ImGuiNative.igGetScrollX();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000B0C7 File Offset: 0x000092C7
		public static float GetScrollY()
		{
			return ImGuiNative.igGetScrollY();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000B0CE File Offset: 0x000092CE
		public static ImGuiStoragePtr GetStateStorage()
		{
			return new ImGuiStoragePtr(ImGuiNative.igGetStateStorage());
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000B0DA File Offset: 0x000092DA
		public static ImGuiStylePtr GetStyle()
		{
			return new ImGuiStylePtr(ImGuiNative.igGetStyle());
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000B0E6 File Offset: 0x000092E6
		public static string GetStyleColorName(ImGuiCol idx)
		{
			return Util.StringFromPtr(ImGuiNative.igGetStyleColorName(idx));
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000B0F3 File Offset: 0x000092F3
		public unsafe static Vector4* GetStyleColorVec4(ImGuiCol idx)
		{
			return ImGuiNative.igGetStyleColorVec4(idx);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B0FB File Offset: 0x000092FB
		public static float GetTextLineHeight()
		{
			return ImGuiNative.igGetTextLineHeight();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000B102 File Offset: 0x00009302
		public static float GetTextLineHeightWithSpacing()
		{
			return ImGuiNative.igGetTextLineHeightWithSpacing();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000B109 File Offset: 0x00009309
		public static double GetTime()
		{
			return ImGuiNative.igGetTime();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000B110 File Offset: 0x00009310
		public static float GetTreeNodeToLabelSpacing()
		{
			return ImGuiNative.igGetTreeNodeToLabelSpacing();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B117 File Offset: 0x00009317
		public static string GetVersion()
		{
			return Util.StringFromPtr(ImGuiNative.igGetVersion());
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B124 File Offset: 0x00009324
		public unsafe static Vector2 GetWindowContentRegionMax()
		{
			Vector2 result;
			ImGuiNative.igGetWindowContentRegionMax(&result);
			return result;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000B13C File Offset: 0x0000933C
		public unsafe static Vector2 GetWindowContentRegionMin()
		{
			Vector2 result;
			ImGuiNative.igGetWindowContentRegionMin(&result);
			return result;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000B152 File Offset: 0x00009352
		public static uint GetWindowDockID()
		{
			return ImGuiNative.igGetWindowDockID();
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000B159 File Offset: 0x00009359
		public static float GetWindowDpiScale()
		{
			return ImGuiNative.igGetWindowDpiScale();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000B160 File Offset: 0x00009360
		public static ImDrawListPtr GetWindowDrawList()
		{
			return new ImDrawListPtr(ImGuiNative.igGetWindowDrawList());
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000B16C File Offset: 0x0000936C
		public static float GetWindowHeight()
		{
			return ImGuiNative.igGetWindowHeight();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000B174 File Offset: 0x00009374
		public unsafe static Vector2 GetWindowPos()
		{
			Vector2 result;
			ImGuiNative.igGetWindowPos(&result);
			return result;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000B18C File Offset: 0x0000938C
		public unsafe static Vector2 GetWindowSize()
		{
			Vector2 result;
			ImGuiNative.igGetWindowSize(&result);
			return result;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B1A2 File Offset: 0x000093A2
		public static ImGuiViewportPtr GetWindowViewport()
		{
			return new ImGuiViewportPtr(ImGuiNative.igGetWindowViewport());
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B1AE File Offset: 0x000093AE
		public static float GetWindowWidth()
		{
			return ImGuiNative.igGetWindowWidth();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000B1B8 File Offset: 0x000093B8
		public static void Image(IntPtr user_texture_id, Vector2 size)
		{
			Vector2 uv = default(Vector2);
			Vector2 uv2;
			uv2..ctor(1f, 1f);
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			Vector4 border_col = default(Vector4);
			ImGuiNative.igImage(user_texture_id, size, uv, uv2, tint_col, border_col);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000B20C File Offset: 0x0000940C
		public static void Image(IntPtr user_texture_id, Vector2 size, Vector2 uv0)
		{
			Vector2 uv;
			uv..ctor(1f, 1f);
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			Vector4 border_col = default(Vector4);
			ImGuiNative.igImage(user_texture_id, size, uv0, uv, tint_col, border_col);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B258 File Offset: 0x00009458
		public static void Image(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1)
		{
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			Vector4 border_col = default(Vector4);
			ImGuiNative.igImage(user_texture_id, size, uv0, uv1, tint_col, border_col);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000B294 File Offset: 0x00009494
		public static void Image(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col)
		{
			ImGuiNative.igImage(user_texture_id, size, uv0, uv1, tint_col, default(Vector4));
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000B2B5 File Offset: 0x000094B5
		public static void Image(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col)
		{
			ImGuiNative.igImage(user_texture_id, size, uv0, uv1, tint_col, border_col);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public unsafe static bool ImageButton(ReadOnlySpan<char> str_id, IntPtr user_texture_id, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			Vector2 uv = default(Vector2);
			Vector2 uv2;
			uv2..ctor(1f, 1f);
			Vector4 bg_col = default(Vector4);
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			int num2 = (int)ImGuiNative.igImageButton(ptr, user_texture_id, size, uv, uv2, bg_col, tint_col);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B378 File Offset: 0x00009578
		public unsafe static bool ImageButton(ReadOnlySpan<char> str_id, IntPtr user_texture_id, Vector2 size, Vector2 uv0)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			Vector2 uv;
			uv..ctor(1f, 1f);
			Vector4 bg_col = default(Vector4);
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			int num2 = (int)ImGuiNative.igImageButton(ptr, user_texture_id, size, uv0, uv, bg_col, tint_col);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B424 File Offset: 0x00009624
		public unsafe static bool ImageButton(ReadOnlySpan<char> str_id, IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			Vector4 bg_col = default(Vector4);
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			int num2 = (int)ImGuiNative.igImageButton(ptr, user_texture_id, size, uv0, uv1, bg_col, tint_col);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B4C0 File Offset: 0x000096C0
		public unsafe static bool ImageButton(ReadOnlySpan<char> str_id, IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 bg_col)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			Vector4 tint_col;
			tint_col..ctor(1f, 1f, 1f, 1f);
			int num2 = (int)ImGuiNative.igImageButton(ptr, user_texture_id, size, uv0, uv1, bg_col, tint_col);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000B550 File Offset: 0x00009750
		public unsafe static bool ImageButton(ReadOnlySpan<char> str_id, IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igImageButton(ptr, user_texture_id, size, uv0, uv1, bg_col, tint_col);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000B5C6 File Offset: 0x000097C6
		public static void Indent()
		{
			ImGuiNative.igIndent(0f);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000B5D2 File Offset: 0x000097D2
		public static void Indent(float indent_w)
		{
			ImGuiNative.igIndent(indent_w);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000B5DC File Offset: 0x000097DC
		public unsafe static bool InputDouble(ReadOnlySpan<char> label, ref double v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			double step = 0.0;
			double step_fast = 0.0;
			int byteCount = Encoding.UTF8.GetByteCount("%.6f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.6f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (double* ptr3 = &v)
			{
				double* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputDouble(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B6CC File Offset: 0x000098CC
		public unsafe static bool InputDouble(ReadOnlySpan<char> label, ref double v, double step)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			double step_fast = 0.0;
			int byteCount = Encoding.UTF8.GetByteCount("%.6f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.6f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (double* ptr3 = &v)
			{
				double* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputDouble(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B7AC File Offset: 0x000099AC
		public unsafe static bool InputDouble(ReadOnlySpan<char> label, ref double v, double step, double step_fast)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.6f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.6f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (double* ptr3 = &v)
			{
				double* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputDouble(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000B87C File Offset: 0x00009A7C
		public unsafe static bool InputDouble(ReadOnlySpan<char> label, ref double v, double step, double step_fast, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (double* ptr3 = &v)
			{
				double* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputDouble(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000B95C File Offset: 0x00009B5C
		public unsafe static bool InputDouble(ReadOnlySpan<char> label, ref double v, double step, double step_fast, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (double* ptr3 = &v)
			{
				double* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputDouble(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000BA38 File Offset: 0x00009C38
		public unsafe static bool InputFloat(ReadOnlySpan<char> label, ref float v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float step = 0f;
			float step_fast = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputFloat(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BB20 File Offset: 0x00009D20
		public unsafe static bool InputFloat(ReadOnlySpan<char> label, ref float v, float step)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float step_fast = 0f;
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputFloat(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public unsafe static bool InputFloat(ReadOnlySpan<char> label, ref float v, float step, float step_fast)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputFloat(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000BCCC File Offset: 0x00009ECC
		public unsafe static bool InputFloat(ReadOnlySpan<char> label, ref float v, float step, float step_fast, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000BDAC File Offset: 0x00009FAC
		public unsafe static bool InputFloat(ReadOnlySpan<char> label, ref float v, float step, float step_fast, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat(ptr, v2, step, step_fast, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000BE88 File Offset: 0x0000A088
		public unsafe static bool InputFloat2(ReadOnlySpan<char> label, ref Vector2 v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputFloat2(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000BF58 File Offset: 0x0000A158
		public unsafe static bool InputFloat2(ReadOnlySpan<char> label, ref Vector2 v, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat2(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000C030 File Offset: 0x0000A230
		public unsafe static bool InputFloat2(ReadOnlySpan<char> label, ref Vector2 v, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat2(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000C104 File Offset: 0x0000A304
		public unsafe static bool InputFloat3(ReadOnlySpan<char> label, ref Vector3 v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputFloat3(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000C1D4 File Offset: 0x0000A3D4
		public unsafe static bool InputFloat3(ReadOnlySpan<char> label, ref Vector3 v, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat3(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		public unsafe static bool InputFloat3(ReadOnlySpan<char> label, ref Vector3 v, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat3(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000C380 File Offset: 0x0000A580
		public unsafe static bool InputFloat4(ReadOnlySpan<char> label, ref Vector4 v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num2 = (int)ImGuiNative.igInputFloat4(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000C450 File Offset: 0x0000A650
		public unsafe static bool InputFloat4(ReadOnlySpan<char> label, ref Vector4 v, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat4(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000C528 File Offset: 0x0000A728
		public unsafe static bool InputFloat4(ReadOnlySpan<char> label, ref Vector4 v, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num3 = (int)ImGuiNative.igInputFloat4(ptr, v2, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		public unsafe static bool InputInt(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int step = 1;
			int step_fast = 100;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt(ptr, v2, step, step_fast, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000C684 File Offset: 0x0000A884
		public unsafe static bool InputInt(ReadOnlySpan<char> label, ref int v, int step)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int step_fast = 100;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt(ptr, v2, step, step_fast, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000C708 File Offset: 0x0000A908
		public unsafe static bool InputInt(ReadOnlySpan<char> label, ref int v, int step, int step_fast)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt(ptr, v2, step, step_fast, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000C784 File Offset: 0x0000A984
		public unsafe static bool InputInt(ReadOnlySpan<char> label, ref int v, int step, int step_fast, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt(ptr, v2, step, step_fast, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000C800 File Offset: 0x0000AA00
		public unsafe static bool InputInt2(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt2(ptr, v2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000C87C File Offset: 0x0000AA7C
		public unsafe static bool InputInt2(ReadOnlySpan<char> label, ref int v, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt2(ptr, v2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		public unsafe static bool InputInt3(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt3(ptr, v2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000C970 File Offset: 0x0000AB70
		public unsafe static bool InputInt3(ReadOnlySpan<char> label, ref int v, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt3(ptr, v2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000C9E8 File Offset: 0x0000ABE8
		public unsafe static bool InputInt4(ReadOnlySpan<char> label, ref int v)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt4(ptr, v2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000CA64 File Offset: 0x0000AC64
		public unsafe static bool InputInt4(ReadOnlySpan<char> label, ref int v, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igInputInt4(ptr, v2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000CADC File Offset: 0x0000ACDC
		public unsafe static bool InputScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step = null;
			void* p_step_fast = null;
			byte* format = null;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num2 = (int)ImGuiNative.igInputScalar(ptr, data_type, p_data2, p_step, p_step_fast, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000CB6C File Offset: 0x0000AD6C
		public unsafe static bool InputScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_step)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast = null;
			byte* format = null;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num2 = (int)ImGuiNative.igInputScalar(ptr, data_type, p_data2, p_step2, p_step_fast, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000CC00 File Offset: 0x0000AE00
		public unsafe static bool InputScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_step, IntPtr p_step_fast)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast2 = p_step_fast.ToPointer();
			byte* format = null;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num2 = (int)ImGuiNative.igInputScalar(ptr, data_type, p_data2, p_step2, p_step_fast2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000CC98 File Offset: 0x0000AE98
		public unsafe static bool InputScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_step, IntPtr p_step_fast, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast2 = p_step_fast.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num3 = (int)ImGuiNative.igInputScalar(ptr, data_type, p_data2, p_step2, p_step_fast2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000CD98 File Offset: 0x0000AF98
		public unsafe static bool InputScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_step, IntPtr p_step_fast, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast2 = p_step_fast.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igInputScalar(ptr, data_type, p_data2, p_step2, p_step_fast2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000CE94 File Offset: 0x0000B094
		public unsafe static bool InputScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step = null;
			void* p_step_fast = null;
			byte* format = null;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num2 = (int)ImGuiNative.igInputScalarN(ptr, data_type, p_data2, components, p_step, p_step_fast, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000CF24 File Offset: 0x0000B124
		public unsafe static bool InputScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_step)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast = null;
			byte* format = null;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num2 = (int)ImGuiNative.igInputScalarN(ptr, data_type, p_data2, components, p_step2, p_step_fast, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		public unsafe static bool InputScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_step, IntPtr p_step_fast)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast2 = p_step_fast.ToPointer();
			byte* format = null;
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num2 = (int)ImGuiNative.igInputScalarN(ptr, data_type, p_data2, components, p_step2, p_step_fast2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000D054 File Offset: 0x0000B254
		public unsafe static bool InputScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_step, IntPtr p_step_fast, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast2 = p_step_fast.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiInputTextFlags flags = ImGuiInputTextFlags.None;
			int num3 = (int)ImGuiNative.igInputScalarN(ptr, data_type, p_data2, components, p_step2, p_step_fast2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000D154 File Offset: 0x0000B354
		public unsafe static bool InputScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_step, IntPtr p_step_fast, ReadOnlySpan<char> format, ImGuiInputTextFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_step2 = p_step.ToPointer();
			void* p_step_fast2 = p_step_fast.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igInputScalarN(ptr, data_type, p_data2, components, p_step2, p_step_fast2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000D250 File Offset: 0x0000B450
		public unsafe static bool InvisibleButton(ReadOnlySpan<char> str_id, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiButtonFlags flags = ImGuiButtonFlags.None;
			int num2 = (int)ImGuiNative.igInvisibleButton(ptr, size, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		public unsafe static bool InvisibleButton(ReadOnlySpan<char> str_id, Vector2 size, ImGuiButtonFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igInvisibleButton(ptr, size, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000D333 File Offset: 0x0000B533
		public static bool IsAnyItemActive()
		{
			return ImGuiNative.igIsAnyItemActive() > 0;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000D33D File Offset: 0x0000B53D
		public static bool IsAnyItemFocused()
		{
			return ImGuiNative.igIsAnyItemFocused() > 0;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D347 File Offset: 0x0000B547
		public static bool IsAnyItemHovered()
		{
			return ImGuiNative.igIsAnyItemHovered() > 0;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000D351 File Offset: 0x0000B551
		public static bool IsAnyMouseDown()
		{
			return ImGuiNative.igIsAnyMouseDown() > 0;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000D35B File Offset: 0x0000B55B
		public static bool IsItemActivated()
		{
			return ImGuiNative.igIsItemActivated() > 0;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000D365 File Offset: 0x0000B565
		public static bool IsItemActive()
		{
			return ImGuiNative.igIsItemActive() > 0;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000D36F File Offset: 0x0000B56F
		public static bool IsItemClicked()
		{
			return ImGuiNative.igIsItemClicked(ImGuiMouseButton.Left) > 0;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000D37A File Offset: 0x0000B57A
		public static bool IsItemClicked(ImGuiMouseButton mouse_button)
		{
			return ImGuiNative.igIsItemClicked(mouse_button) > 0;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000D385 File Offset: 0x0000B585
		public static bool IsItemDeactivated()
		{
			return ImGuiNative.igIsItemDeactivated() > 0;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000D38F File Offset: 0x0000B58F
		public static bool IsItemDeactivatedAfterEdit()
		{
			return ImGuiNative.igIsItemDeactivatedAfterEdit() > 0;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000D399 File Offset: 0x0000B599
		public static bool IsItemEdited()
		{
			return ImGuiNative.igIsItemEdited() > 0;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000D3A3 File Offset: 0x0000B5A3
		public static bool IsItemFocused()
		{
			return ImGuiNative.igIsItemFocused() > 0;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000D3AD File Offset: 0x0000B5AD
		public static bool IsItemHovered()
		{
			return ImGuiNative.igIsItemHovered(ImGuiHoveredFlags.None) > 0;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		public static bool IsItemHovered(ImGuiHoveredFlags flags)
		{
			return ImGuiNative.igIsItemHovered(flags) > 0;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000D3C3 File Offset: 0x0000B5C3
		public static bool IsItemToggledOpen()
		{
			return ImGuiNative.igIsItemToggledOpen() > 0;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000D3CD File Offset: 0x0000B5CD
		public static bool IsItemVisible()
		{
			return ImGuiNative.igIsItemVisible() > 0;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000D3D7 File Offset: 0x0000B5D7
		public static bool IsKeyDown(ImGuiKey key)
		{
			return ImGuiNative.igIsKeyDown_Nil(key) > 0;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		public static bool IsKeyPressed(ImGuiKey key)
		{
			byte repeat = 1;
			return ImGuiNative.igIsKeyPressed_Bool(key, repeat) > 0;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000D400 File Offset: 0x0000B600
		public static bool IsKeyPressed(ImGuiKey key, bool repeat)
		{
			byte repeat2 = (repeat > false) ? 1 : 0;
			return ImGuiNative.igIsKeyPressed_Bool(key, repeat2) > 0;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000D41D File Offset: 0x0000B61D
		public static bool IsKeyReleased(ImGuiKey key)
		{
			return ImGuiNative.igIsKeyReleased_Nil(key) > 0;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000D428 File Offset: 0x0000B628
		public static bool IsMouseClicked(ImGuiMouseButton button)
		{
			byte repeat = 0;
			return ImGuiNative.igIsMouseClicked_Bool(button, repeat) > 0;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000D444 File Offset: 0x0000B644
		public static bool IsMouseClicked(ImGuiMouseButton button, bool repeat)
		{
			byte repeat2 = (repeat > false) ? 1 : 0;
			return ImGuiNative.igIsMouseClicked_Bool(button, repeat2) > 0;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000D461 File Offset: 0x0000B661
		public static bool IsMouseDoubleClicked(ImGuiMouseButton button)
		{
			return ImGuiNative.igIsMouseDoubleClicked(button) > 0;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000D46C File Offset: 0x0000B66C
		public static bool IsMouseDown(ImGuiMouseButton button)
		{
			return ImGuiNative.igIsMouseDown_Nil(button) > 0;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000D478 File Offset: 0x0000B678
		public static bool IsMouseDragging(ImGuiMouseButton button)
		{
			float lock_threshold = -1f;
			return ImGuiNative.igIsMouseDragging(button, lock_threshold) > 0;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000D495 File Offset: 0x0000B695
		public static bool IsMouseDragging(ImGuiMouseButton button, float lock_threshold)
		{
			return ImGuiNative.igIsMouseDragging(button, lock_threshold) > 0;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000D4A4 File Offset: 0x0000B6A4
		public static bool IsMouseHoveringRect(Vector2 r_min, Vector2 r_max)
		{
			byte clip = 1;
			return ImGuiNative.igIsMouseHoveringRect(r_min, r_max, clip) > 0;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public static bool IsMouseHoveringRect(Vector2 r_min, Vector2 r_max, bool clip)
		{
			byte clip2 = (clip > false) ? 1 : 0;
			return ImGuiNative.igIsMouseHoveringRect(r_min, r_max, clip2) > 0;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		public unsafe static bool IsMousePosValid()
		{
			Vector2* mouse_pos = null;
			return ImGuiNative.igIsMousePosValid(mouse_pos) > 0;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000D4FC File Offset: 0x0000B6FC
		public unsafe static bool IsMousePosValid(ref Vector2 mouse_pos)
		{
			fixed (Vector2* ptr = &mouse_pos)
			{
				return ImGuiNative.igIsMousePosValid(ptr) > 0;
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000D515 File Offset: 0x0000B715
		public static bool IsMouseReleased(ImGuiMouseButton button)
		{
			return ImGuiNative.igIsMouseReleased_Nil(button) > 0;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000D520 File Offset: 0x0000B720
		public unsafe static bool IsPopupOpen(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPopupFlags flags = ImGuiPopupFlags.None;
			int num2 = (int)ImGuiNative.igIsPopupOpen_Str(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000D590 File Offset: 0x0000B790
		public unsafe static bool IsPopupOpen(ReadOnlySpan<char> str_id, ImGuiPopupFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igIsPopupOpen_Str(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000D5FE File Offset: 0x0000B7FE
		public static bool IsRectVisible(Vector2 size)
		{
			return ImGuiNative.igIsRectVisible_Nil(size) > 0;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000D609 File Offset: 0x0000B809
		public static bool IsRectVisible(Vector2 rect_min, Vector2 rect_max)
		{
			return ImGuiNative.igIsRectVisible_Vec2(rect_min, rect_max) > 0;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D615 File Offset: 0x0000B815
		public static bool IsWindowAppearing()
		{
			return ImGuiNative.igIsWindowAppearing() > 0;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000D61F File Offset: 0x0000B81F
		public static bool IsWindowCollapsed()
		{
			return ImGuiNative.igIsWindowCollapsed() > 0;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D629 File Offset: 0x0000B829
		public static bool IsWindowDocked()
		{
			return ImGuiNative.igIsWindowDocked() > 0;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D633 File Offset: 0x0000B833
		public static bool IsWindowFocused()
		{
			return ImGuiNative.igIsWindowFocused(ImGuiFocusedFlags.None) > 0;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000D63E File Offset: 0x0000B83E
		public static bool IsWindowFocused(ImGuiFocusedFlags flags)
		{
			return ImGuiNative.igIsWindowFocused(flags) > 0;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000D649 File Offset: 0x0000B849
		public static bool IsWindowHovered()
		{
			return ImGuiNative.igIsWindowHovered(ImGuiHoveredFlags.None) > 0;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000D654 File Offset: 0x0000B854
		public static bool IsWindowHovered(ImGuiHoveredFlags flags)
		{
			return ImGuiNative.igIsWindowHovered(flags) > 0;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000D660 File Offset: 0x0000B860
		public unsafe static void LabelText(ReadOnlySpan<char> label, ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (fmt != null)
			{
				num2 = Encoding.UTF8.GetByteCount(fmt);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(fmt, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiNative.igLabelText(ptr, ptr2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000D728 File Offset: 0x0000B928
		public unsafe static bool ListBox(ReadOnlySpan<char> label, ref int current_item, string[] items, int items_count)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int* ptr2 = stackalloc int[checked(unchecked((UIntPtr)items.Length) * 4)];
			int num2 = 0;
			for (int i = 0; i < items.Length; i++)
			{
				string text = items[i];
				ptr2[i] = Encoding.UTF8.GetByteCount(text);
				num2 += ptr2[i] + 1;
			}
			byte* ptr3 = stackalloc byte[(UIntPtr)num2];
			int num3 = 0;
			for (int j = 0; j < items.Length; j++)
			{
				string text2 = items[j];
				char* ptr4;
				if (text2 == null)
				{
					ptr4 = null;
				}
				else
				{
					fixed (char* ptr5 = text2.GetPinnableReference())
					{
						ptr4 = ptr5;
					}
				}
				char* ptr6 = ptr4;
				num3 += Encoding.UTF8.GetBytes(ptr6, text2.Length, ptr3 + num3, ptr2[j]);
				ptr3[num3] = 0;
				num3++;
				char* ptr5 = null;
			}
			byte** ptr7 = stackalloc byte*[checked(unchecked((UIntPtr)items.Length) * (UIntPtr)sizeof(byte*))];
			num3 = 0;
			for (int k = 0; k < items.Length; k++)
			{
				*(IntPtr*)(ptr7 + (IntPtr)k * (IntPtr)sizeof(byte*) / (IntPtr)sizeof(byte*)) = ptr3 + num3;
				num3 += ptr2[k] + 1;
			}
			int height_in_items = -1;
			fixed (int* ptr8 = &current_item)
			{
				int* current_item2 = ptr8;
				int num4 = (int)ImGuiNative.igListBox_Str_arr(ptr, current_item2, ptr7, items_count, height_in_items);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num4 != 0;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		public unsafe static bool ListBox(ReadOnlySpan<char> label, ref int current_item, string[] items, int items_count, int height_in_items)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int* ptr2 = stackalloc int[checked(unchecked((UIntPtr)items.Length) * 4)];
			int num2 = 0;
			for (int i = 0; i < items.Length; i++)
			{
				string text = items[i];
				ptr2[i] = Encoding.UTF8.GetByteCount(text);
				num2 += ptr2[i] + 1;
			}
			byte* ptr3 = stackalloc byte[(UIntPtr)num2];
			int num3 = 0;
			for (int j = 0; j < items.Length; j++)
			{
				string text2 = items[j];
				char* ptr4;
				if (text2 == null)
				{
					ptr4 = null;
				}
				else
				{
					fixed (char* ptr5 = text2.GetPinnableReference())
					{
						ptr4 = ptr5;
					}
				}
				char* ptr6 = ptr4;
				num3 += Encoding.UTF8.GetBytes(ptr6, text2.Length, ptr3 + num3, ptr2[j]);
				ptr3[num3] = 0;
				num3++;
				char* ptr5 = null;
			}
			byte** ptr7 = stackalloc byte*[checked(unchecked((UIntPtr)items.Length) * (UIntPtr)sizeof(byte*))];
			num3 = 0;
			for (int k = 0; k < items.Length; k++)
			{
				*(IntPtr*)(ptr7 + (IntPtr)k * (IntPtr)sizeof(byte*) / (IntPtr)sizeof(byte*)) = ptr3 + num3;
				num3 += ptr2[k] + 1;
			}
			fixed (int* ptr8 = &current_item)
			{
				int* current_item2 = ptr8;
				int num4 = (int)ImGuiNative.igListBox_Str_arr(ptr, current_item2, ptr7, items_count, height_in_items);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num4 != 0;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000DA1C File Offset: 0x0000BC1C
		public unsafe static void LoadIniSettingsFromDisk(ReadOnlySpan<char> ini_filename)
		{
			int num = 0;
			byte* ptr;
			if (ini_filename != null)
			{
				num = Encoding.UTF8.GetByteCount(ini_filename);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(ini_filename, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igLoadIniSettingsFromDisk(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000DA88 File Offset: 0x0000BC88
		public unsafe static void LoadIniSettingsFromMemory(ReadOnlySpan<char> ini_data)
		{
			int num = 0;
			byte* ptr;
			if (ini_data != null)
			{
				num = Encoding.UTF8.GetByteCount(ini_data);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(ini_data, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			uint ini_size = 0U;
			ImGuiNative.igLoadIniSettingsFromMemory(ptr, ini_size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		public unsafe static void LoadIniSettingsFromMemory(ReadOnlySpan<char> ini_data, uint ini_size)
		{
			int num = 0;
			byte* ptr;
			if (ini_data != null)
			{
				num = Encoding.UTF8.GetByteCount(ini_data);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(ini_data, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igLoadIniSettingsFromMemory(ptr, ini_size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000DB63 File Offset: 0x0000BD63
		public static void LogButtons()
		{
			ImGuiNative.igLogButtons();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000DB6A File Offset: 0x0000BD6A
		public static void LogFinish()
		{
			ImGuiNative.igLogFinish();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000DB74 File Offset: 0x0000BD74
		public unsafe static void LogText(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igLogText(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000DBDE File Offset: 0x0000BDDE
		public static void LogToClipboard()
		{
			ImGuiNative.igLogToClipboard(-1);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000DBE6 File Offset: 0x0000BDE6
		public static void LogToClipboard(int auto_open_depth)
		{
			ImGuiNative.igLogToClipboard(auto_open_depth);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000DBF0 File Offset: 0x0000BDF0
		public unsafe static void LogToFile()
		{
			int auto_open_depth = -1;
			byte* filename = null;
			ImGuiNative.igLogToFile(auto_open_depth, filename);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000DC08 File Offset: 0x0000BE08
		public unsafe static void LogToFile(int auto_open_depth)
		{
			byte* filename = null;
			ImGuiNative.igLogToFile(auto_open_depth, filename);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000DC20 File Offset: 0x0000BE20
		public unsafe static void LogToFile(int auto_open_depth, ReadOnlySpan<char> filename)
		{
			int num = 0;
			byte* ptr;
			if (filename != null)
			{
				num = Encoding.UTF8.GetByteCount(filename);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(filename, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igLogToFile(auto_open_depth, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000DC8B File Offset: 0x0000BE8B
		public static void LogToTTY()
		{
			ImGuiNative.igLogToTTY(-1);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000DC93 File Offset: 0x0000BE93
		public static void LogToTTY(int auto_open_depth)
		{
			ImGuiNative.igLogToTTY(auto_open_depth);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000DC9B File Offset: 0x0000BE9B
		public static IntPtr MemAlloc(uint size)
		{
			return (IntPtr)ImGuiNative.igMemAlloc(size);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000DCA8 File Offset: 0x0000BEA8
		public static void MemFree(IntPtr ptr)
		{
			ImGuiNative.igMemFree(ptr.ToPointer());
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		public unsafe static bool MenuItem(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* shortcut = null;
			byte selected = 0;
			byte enabled = 1;
			int num2 = (int)ImGuiNative.igMenuItem_Bool(ptr, shortcut, selected, enabled);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000DD34 File Offset: 0x0000BF34
		public unsafe static bool MenuItem(ReadOnlySpan<char> label, ReadOnlySpan<char> shortcut)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (shortcut != null)
			{
				num2 = Encoding.UTF8.GetByteCount(shortcut);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(shortcut, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte selected = 0;
			byte enabled = 1;
			int num3 = (int)ImGuiNative.igMenuItem_Bool(ptr, ptr2, selected, enabled);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000DE08 File Offset: 0x0000C008
		public unsafe static bool MenuItem(ReadOnlySpan<char> label, ReadOnlySpan<char> shortcut, bool selected)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (shortcut != null)
			{
				num2 = Encoding.UTF8.GetByteCount(shortcut);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(shortcut, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte selected2 = (selected > false) ? 1 : 0;
			byte enabled = 1;
			int num3 = (int)ImGuiNative.igMenuItem_Bool(ptr, ptr2, selected2, enabled);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		public unsafe static bool MenuItem(ReadOnlySpan<char> label, ReadOnlySpan<char> shortcut, bool selected, bool enabled)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (shortcut != null)
			{
				num2 = Encoding.UTF8.GetByteCount(shortcut);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(shortcut, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte selected2 = (selected > false) ? 1 : 0;
			byte enabled2 = (enabled > false) ? 1 : 0;
			int num3 = (int)ImGuiNative.igMenuItem_Bool(ptr, ptr2, selected2, enabled2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000DFBC File Offset: 0x0000C1BC
		public unsafe static bool MenuItem(ReadOnlySpan<char> label, ReadOnlySpan<char> shortcut, ref bool p_selected)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (shortcut != null)
			{
				num2 = Encoding.UTF8.GetByteCount(shortcut);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(shortcut, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte b = (p_selected > false) ? 1 : 0;
			byte* p_selected2 = &b;
			byte enabled = 1;
			int num3 = (int)ImGuiNative.igMenuItem_BoolPtr(ptr, ptr2, p_selected2, enabled);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			p_selected = (b > 0);
			return num3 != 0;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
		public unsafe static bool MenuItem(ReadOnlySpan<char> label, ReadOnlySpan<char> shortcut, ref bool p_selected, bool enabled)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (shortcut != null)
			{
				num2 = Encoding.UTF8.GetByteCount(shortcut);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(shortcut, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			byte b = (p_selected > false) ? 1 : 0;
			byte* p_selected2 = &b;
			byte enabled2 = (enabled > false) ? 1 : 0;
			int num3 = (int)ImGuiNative.igMenuItem_BoolPtr(ptr, ptr2, p_selected2, enabled2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			p_selected = (b > 0);
			return num3 != 0;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000E188 File Offset: 0x0000C388
		public static void NewFrame()
		{
			ImGuiNative.igNewFrame();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000E18F File Offset: 0x0000C38F
		public static void NewLine()
		{
			ImGuiNative.igNewLine();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000E196 File Offset: 0x0000C396
		public static void NextColumn()
		{
			ImGuiNative.igNextColumn();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		public unsafe static void OpenPopup(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.None;
			ImGuiNative.igOpenPopup_Str(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E210 File Offset: 0x0000C410
		public unsafe static void OpenPopup(ReadOnlySpan<char> str_id, ImGuiPopupFlags popup_flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igOpenPopup_Str(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E27C File Offset: 0x0000C47C
		public static void OpenPopup(uint id)
		{
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.None;
			ImGuiNative.igOpenPopup_ID(id, popup_flags);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000E292 File Offset: 0x0000C492
		public static void OpenPopup(uint id, ImGuiPopupFlags popup_flags)
		{
			ImGuiNative.igOpenPopup_ID(id, popup_flags);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000E29C File Offset: 0x0000C49C
		public unsafe static void OpenPopupOnItemClick()
		{
			byte* str_id = null;
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			ImGuiNative.igOpenPopupOnItemClick(str_id, popup_flags);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		public unsafe static void OpenPopupOnItemClick(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiPopupFlags popup_flags = ImGuiPopupFlags.MouseButtonRight;
			ImGuiNative.igOpenPopupOnItemClick(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000E328 File Offset: 0x0000C528
		public unsafe static void OpenPopupOnItemClick(ReadOnlySpan<char> str_id, ImGuiPopupFlags popup_flags)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igOpenPopupOnItemClick(ptr, popup_flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000E394 File Offset: 0x0000C594
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int values_offset = 0;
			byte* overlay_text = null;
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr2 = &values)
			{
				float* values2 = ptr2;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, overlay_text, maxValue, maxValue2, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000E438 File Offset: 0x0000C638
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* overlay_text = null;
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr2 = &values)
			{
				float* values2 = ptr2;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, overlay_text, maxValue, maxValue2, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, ptr2, maxValue, maxValue2, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000E5D4 File Offset: 0x0000C7D4
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			float maxValue = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, maxValue, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min, float scale_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, scale_max, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000E7B4 File Offset: 0x0000C9B4
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min, float scale_max, Vector2 graph_size)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, scale_max, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000E898 File Offset: 0x0000CA98
		public unsafe static void PlotHistogram(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotHistogram_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, scale_max, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000E97C File Offset: 0x0000CB7C
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int values_offset = 0;
			byte* overlay_text = null;
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr2 = &values)
			{
				float* values2 = ptr2;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, overlay_text, maxValue, maxValue2, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EA20 File Offset: 0x0000CC20
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* overlay_text = null;
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr2 = &values)
			{
				float* values2 = ptr2;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, overlay_text, maxValue, maxValue2, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			float maxValue = float.MaxValue;
			float maxValue2 = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, ptr2, maxValue, maxValue2, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000EBBC File Offset: 0x0000CDBC
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			float maxValue = float.MaxValue;
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, maxValue, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000ECB0 File Offset: 0x0000CEB0
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min, float scale_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			Vector2 graph_size = default(Vector2);
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, scale_max, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000ED9C File Offset: 0x0000CF9C
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min, float scale_max, Vector2 graph_size)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int stride = 4;
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, scale_max, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000EE80 File Offset: 0x0000D080
		public unsafe static void PlotLines(ReadOnlySpan<char> label, ref float values, int values_count, int values_offset, ReadOnlySpan<char> overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (overlay_text != null)
			{
				num2 = Encoding.UTF8.GetByteCount(overlay_text);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(overlay_text, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &values)
			{
				float* values2 = ptr3;
				ImGuiNative.igPlotLines_FloatPtr(ptr, values2, values_count, values_offset, ptr2, scale_min, scale_max, graph_size, stride);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000EF61 File Offset: 0x0000D161
		public static void PopButtonRepeat()
		{
			ImGuiNative.igPopButtonRepeat();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000EF68 File Offset: 0x0000D168
		public static void PopClipRect()
		{
			ImGuiNative.igPopClipRect();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000EF6F File Offset: 0x0000D16F
		public static void PopFont()
		{
			ImGuiNative.igPopFont();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000EF76 File Offset: 0x0000D176
		public static void PopID()
		{
			ImGuiNative.igPopID();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000EF7D File Offset: 0x0000D17D
		public static void PopItemWidth()
		{
			ImGuiNative.igPopItemWidth();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000EF84 File Offset: 0x0000D184
		public static void PopStyleColor()
		{
			ImGuiNative.igPopStyleColor(1);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public static void PopStyleColor(int count)
		{
			ImGuiNative.igPopStyleColor(count);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000EF94 File Offset: 0x0000D194
		public static void PopStyleVar()
		{
			ImGuiNative.igPopStyleVar(1);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000EF9C File Offset: 0x0000D19C
		public static void PopStyleVar(int count)
		{
			ImGuiNative.igPopStyleVar(count);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		public static void PopTabStop()
		{
			ImGuiNative.igPopTabStop();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000EFAB File Offset: 0x0000D1AB
		public static void PopTextWrapPos()
		{
			ImGuiNative.igPopTextWrapPos();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000EFB4 File Offset: 0x0000D1B4
		public unsafe static void ProgressBar(float fraction)
		{
			Vector2 size_arg;
			size_arg..ctor(float.MaxValue, 0f);
			byte* overlay = null;
			ImGuiNative.igProgressBar(fraction, size_arg, overlay);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000EFE0 File Offset: 0x0000D1E0
		public unsafe static void ProgressBar(float fraction, Vector2 size_arg)
		{
			byte* overlay = null;
			ImGuiNative.igProgressBar(fraction, size_arg, overlay);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		public unsafe static void ProgressBar(float fraction, Vector2 size_arg, ReadOnlySpan<char> overlay)
		{
			int num = 0;
			byte* ptr;
			if (overlay != null)
			{
				num = Encoding.UTF8.GetByteCount(overlay);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(overlay, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igProgressBar(fraction, size_arg, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000F064 File Offset: 0x0000D264
		public static void PushButtonRepeat(bool repeat)
		{
			ImGuiNative.igPushButtonRepeat((repeat > false) ? 1 : 0);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000F070 File Offset: 0x0000D270
		public static void PushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect)
		{
			byte intersect_with_current_clip_rect2 = (intersect_with_current_clip_rect > false) ? 1 : 0;
			ImGuiNative.igPushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect2);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000F08B File Offset: 0x0000D28B
		public static void PushFont(ImFontPtr font)
		{
			ImGuiNative.igPushFont(font.NativePtr);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000F09C File Offset: 0x0000D29C
		public unsafe static void PushID(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igPushID_Str(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000F106 File Offset: 0x0000D306
		public static void PushID(IntPtr ptr_id)
		{
			ImGuiNative.igPushID_Ptr(ptr_id.ToPointer());
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000F114 File Offset: 0x0000D314
		public static void PushID(int int_id)
		{
			ImGuiNative.igPushID_Int(int_id);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000F11C File Offset: 0x0000D31C
		public static void PushItemWidth(float item_width)
		{
			ImGuiNative.igPushItemWidth(item_width);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000F124 File Offset: 0x0000D324
		public static void PushStyleColor(ImGuiCol idx, uint col)
		{
			ImGuiNative.igPushStyleColor_U32(idx, col);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000F12D File Offset: 0x0000D32D
		public static void PushStyleColor(ImGuiCol idx, Vector4 col)
		{
			ImGuiNative.igPushStyleColor_Vec4(idx, col);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000F136 File Offset: 0x0000D336
		public static void PushStyleVar(ImGuiStyleVar idx, float val)
		{
			ImGuiNative.igPushStyleVar_Float(idx, val);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F13F File Offset: 0x0000D33F
		public static void PushStyleVar(ImGuiStyleVar idx, Vector2 val)
		{
			ImGuiNative.igPushStyleVar_Vec2(idx, val);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F148 File Offset: 0x0000D348
		public static void PushTabStop(bool tab_stop)
		{
			ImGuiNative.igPushTabStop((tab_stop > false) ? 1 : 0);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000F154 File Offset: 0x0000D354
		public static void PushTextWrapPos()
		{
			ImGuiNative.igPushTextWrapPos(0f);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000F160 File Offset: 0x0000D360
		public static void PushTextWrapPos(float wrap_local_pos_x)
		{
			ImGuiNative.igPushTextWrapPos(wrap_local_pos_x);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000F168 File Offset: 0x0000D368
		public unsafe static bool RadioButton(ReadOnlySpan<char> label, bool active)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte active2 = (active > false) ? 1 : 0;
			int num2 = (int)ImGuiNative.igRadioButton_Bool(ptr, active2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000F1DC File Offset: 0x0000D3DC
		public unsafe static bool RadioButton(ReadOnlySpan<char> label, ref int v, int v_button)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			fixed (int* ptr2 = &v)
			{
				int* v2 = ptr2;
				int num2 = (int)ImGuiNative.igRadioButton_IntPtr(ptr, v2, v_button);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000F252 File Offset: 0x0000D452
		public static void Render()
		{
			ImGuiNative.igRender();
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000F25C File Offset: 0x0000D45C
		public unsafe static void RenderPlatformWindowsDefault()
		{
			void* platform_render_arg = null;
			void* renderer_render_arg = null;
			ImGuiNative.igRenderPlatformWindowsDefault(platform_render_arg, renderer_render_arg);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000F278 File Offset: 0x0000D478
		public unsafe static void RenderPlatformWindowsDefault(IntPtr platform_render_arg)
		{
			void* platform_render_arg2 = platform_render_arg.ToPointer();
			void* renderer_render_arg = null;
			ImGuiNative.igRenderPlatformWindowsDefault(platform_render_arg2, renderer_render_arg);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000F298 File Offset: 0x0000D498
		public unsafe static void RenderPlatformWindowsDefault(IntPtr platform_render_arg, IntPtr renderer_render_arg)
		{
			void* platform_render_arg2 = platform_render_arg.ToPointer();
			void* renderer_render_arg2 = renderer_render_arg.ToPointer();
			ImGuiNative.igRenderPlatformWindowsDefault(platform_render_arg2, renderer_render_arg2);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F2BA File Offset: 0x0000D4BA
		public static void ResetMouseDragDelta()
		{
			ImGuiNative.igResetMouseDragDelta(ImGuiMouseButton.Left);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000F2C2 File Offset: 0x0000D4C2
		public static void ResetMouseDragDelta(ImGuiMouseButton button)
		{
			ImGuiNative.igResetMouseDragDelta(button);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		public static void SameLine()
		{
			float offset_from_start_x = 0f;
			float spacing = -1f;
			ImGuiNative.igSameLine(offset_from_start_x, spacing);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		public static void SameLine(float offset_from_start_x)
		{
			float spacing = -1f;
			ImGuiNative.igSameLine(offset_from_start_x, spacing);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F306 File Offset: 0x0000D506
		public static void SameLine(float offset_from_start_x, float spacing)
		{
			ImGuiNative.igSameLine(offset_from_start_x, spacing);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F310 File Offset: 0x0000D510
		public unsafe static void SaveIniSettingsToDisk(ReadOnlySpan<char> ini_filename)
		{
			int num = 0;
			byte* ptr;
			if (ini_filename != null)
			{
				num = Encoding.UTF8.GetByteCount(ini_filename);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(ini_filename, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSaveIniSettingsToDisk(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F37C File Offset: 0x0000D57C
		public unsafe static string SaveIniSettingsToMemory()
		{
			uint* out_ini_size = null;
			return Util.StringFromPtr(ImGuiNative.igSaveIniSettingsToMemory(out_ini_size));
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000F398 File Offset: 0x0000D598
		public unsafe static string SaveIniSettingsToMemory(out uint out_ini_size)
		{
			fixed (uint* ptr = &out_ini_size)
			{
				return Util.StringFromPtr(ImGuiNative.igSaveIniSettingsToMemory(ptr));
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000F3B4 File Offset: 0x0000D5B4
		public unsafe static bool Selectable(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte selected = 0;
			ImGuiSelectableFlags flags = ImGuiSelectableFlags.None;
			int num2 = (int)ImGuiNative.igSelectable_Bool(ptr, selected, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000F434 File Offset: 0x0000D634
		public unsafe static bool Selectable(ReadOnlySpan<char> label, bool selected)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte selected2 = (selected > false) ? 1 : 0;
			ImGuiSelectableFlags flags = ImGuiSelectableFlags.None;
			int num2 = (int)ImGuiNative.igSelectable_Bool(ptr, selected2, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		public unsafe static bool Selectable(ReadOnlySpan<char> label, bool selected, ImGuiSelectableFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte selected2 = (selected > false) ? 1 : 0;
			int num2 = (int)ImGuiNative.igSelectable_Bool(ptr, selected2, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000F538 File Offset: 0x0000D738
		public unsafe static bool Selectable(ReadOnlySpan<char> label, bool selected, ImGuiSelectableFlags flags, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte selected2 = (selected > false) ? 1 : 0;
			int num2 = (int)ImGuiNative.igSelectable_Bool(ptr, selected2, flags, size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
		public unsafe static bool Selectable(ReadOnlySpan<char> label, ref bool p_selected)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_selected > false) ? 1 : 0;
			byte* p_selected2 = &b;
			ImGuiSelectableFlags flags = ImGuiSelectableFlags.None;
			int num2 = (int)ImGuiNative.igSelectable_BoolPtr(ptr, p_selected2, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_selected = (b > 0);
			return num2 != 0;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000F640 File Offset: 0x0000D840
		public unsafe static bool Selectable(ReadOnlySpan<char> label, ref bool p_selected, ImGuiSelectableFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_selected > false) ? 1 : 0;
			byte* p_selected2 = &b;
			int num2 = (int)ImGuiNative.igSelectable_BoolPtr(ptr, p_selected2, flags, default(Vector2));
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_selected = (b > 0);
			return num2 != 0;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		public unsafe static bool Selectable(ReadOnlySpan<char> label, ref bool p_selected, ImGuiSelectableFlags flags, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b = (p_selected > false) ? 1 : 0;
			byte* p_selected2 = &b;
			int num2 = (int)ImGuiNative.igSelectable_BoolPtr(ptr, p_selected2, flags, size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			p_selected = (b > 0);
			return num2 != 0;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000F74F File Offset: 0x0000D94F
		public static void Separator()
		{
			ImGuiNative.igSeparator();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000F758 File Offset: 0x0000D958
		public unsafe static void SeparatorText(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSeparatorText(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000F7C4 File Offset: 0x0000D9C4
		public unsafe static void SetAllocatorFunctions(IntPtr alloc_func, IntPtr free_func)
		{
			void* user_data = null;
			ImGuiNative.igSetAllocatorFunctions(alloc_func, free_func, user_data);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		public unsafe static void SetAllocatorFunctions(IntPtr alloc_func, IntPtr free_func, IntPtr user_data)
		{
			void* user_data2 = user_data.ToPointer();
			ImGuiNative.igSetAllocatorFunctions(alloc_func, free_func, user_data2);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		public unsafe static void SetClipboardText(ReadOnlySpan<char> text)
		{
			int num = 0;
			byte* ptr;
			if (text != null)
			{
				num = Encoding.UTF8.GetByteCount(text);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(text, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetClipboardText(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000F866 File Offset: 0x0000DA66
		public static void SetColorEditOptions(ImGuiColorEditFlags flags)
		{
			ImGuiNative.igSetColorEditOptions(flags);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000F86E File Offset: 0x0000DA6E
		public static void SetColumnOffset(int column_index, float offset_x)
		{
			ImGuiNative.igSetColumnOffset(column_index, offset_x);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000F877 File Offset: 0x0000DA77
		public static void SetColumnWidth(int column_index, float width)
		{
			ImGuiNative.igSetColumnWidth(column_index, width);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000F880 File Offset: 0x0000DA80
		public static void SetCurrentContext(IntPtr ctx)
		{
			ImGuiNative.igSetCurrentContext(ctx);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000F888 File Offset: 0x0000DA88
		public static void SetCursorPos(Vector2 local_pos)
		{
			ImGuiNative.igSetCursorPos(local_pos);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000F890 File Offset: 0x0000DA90
		public static void SetCursorPosX(float local_x)
		{
			ImGuiNative.igSetCursorPosX(local_x);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000F898 File Offset: 0x0000DA98
		public static void SetCursorPosY(float local_y)
		{
			ImGuiNative.igSetCursorPosY(local_y);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		public static void SetCursorScreenPos(Vector2 pos)
		{
			ImGuiNative.igSetCursorScreenPos(pos);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public unsafe static bool SetDragDropPayload(ReadOnlySpan<char> type, IntPtr data, uint sz)
		{
			int num = 0;
			byte* ptr;
			if (type != null)
			{
				num = Encoding.UTF8.GetByteCount(type);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(type, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* data2 = data.ToPointer();
			ImGuiCond cond = ImGuiCond.None;
			int num2 = (int)ImGuiNative.igSetDragDropPayload(ptr, data2, sz, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000F924 File Offset: 0x0000DB24
		public unsafe static bool SetDragDropPayload(ReadOnlySpan<char> type, IntPtr data, uint sz, ImGuiCond cond)
		{
			int num = 0;
			byte* ptr;
			if (type != null)
			{
				num = Encoding.UTF8.GetByteCount(type);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(type, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* data2 = data.ToPointer();
			int num2 = (int)ImGuiNative.igSetDragDropPayload(ptr, data2, sz, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000F99C File Offset: 0x0000DB9C
		public static void SetItemDefaultFocus()
		{
			ImGuiNative.igSetItemDefaultFocus();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		public unsafe static void SetItemTooltip(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetItemTooltip(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000FA0E File Offset: 0x0000DC0E
		public static void SetKeyboardFocusHere()
		{
			ImGuiNative.igSetKeyboardFocusHere(0);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000FA16 File Offset: 0x0000DC16
		public static void SetKeyboardFocusHere(int offset)
		{
			ImGuiNative.igSetKeyboardFocusHere(offset);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000FA1E File Offset: 0x0000DC1E
		public static void SetMouseCursor(ImGuiMouseCursor cursor_type)
		{
			ImGuiNative.igSetMouseCursor(cursor_type);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000FA26 File Offset: 0x0000DC26
		public static void SetNextFrameWantCaptureKeyboard(bool want_capture_keyboard)
		{
			ImGuiNative.igSetNextFrameWantCaptureKeyboard((want_capture_keyboard > false) ? 1 : 0);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000FA32 File Offset: 0x0000DC32
		public static void SetNextFrameWantCaptureMouse(bool want_capture_mouse)
		{
			ImGuiNative.igSetNextFrameWantCaptureMouse((want_capture_mouse > false) ? 1 : 0);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FA3E File Offset: 0x0000DC3E
		public static void SetNextItemAllowOverlap()
		{
			ImGuiNative.igSetNextItemAllowOverlap();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public static void SetNextItemOpen(bool is_open)
		{
			byte is_open2 = (is_open > false) ? 1 : 0;
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetNextItemOpen(is_open2, cond);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000FA62 File Offset: 0x0000DC62
		public static void SetNextItemOpen(bool is_open, ImGuiCond cond)
		{
			ImGuiNative.igSetNextItemOpen((is_open > false) ? 1 : 0, cond);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FA6F File Offset: 0x0000DC6F
		public static void SetNextItemWidth(float item_width)
		{
			ImGuiNative.igSetNextItemWidth(item_width);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000FA77 File Offset: 0x0000DC77
		public static void SetNextWindowBgAlpha(float alpha)
		{
			ImGuiNative.igSetNextWindowBgAlpha(alpha);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000FA7F File Offset: 0x0000DC7F
		public static void SetNextWindowClass(ImGuiWindowClassPtr window_class)
		{
			ImGuiNative.igSetNextWindowClass(window_class.NativePtr);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000FA90 File Offset: 0x0000DC90
		public static void SetNextWindowCollapsed(bool collapsed)
		{
			byte collapsed2 = (collapsed > false) ? 1 : 0;
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetNextWindowCollapsed(collapsed2, cond);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000FAAA File Offset: 0x0000DCAA
		public static void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond)
		{
			ImGuiNative.igSetNextWindowCollapsed((collapsed > false) ? 1 : 0, cond);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000FAB7 File Offset: 0x0000DCB7
		public static void SetNextWindowContentSize(Vector2 size)
		{
			ImGuiNative.igSetNextWindowContentSize(size);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000FAC0 File Offset: 0x0000DCC0
		public static void SetNextWindowDockID(uint dock_id)
		{
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetNextWindowDockID(dock_id, cond);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000FAD6 File Offset: 0x0000DCD6
		public static void SetNextWindowDockID(uint dock_id, ImGuiCond cond)
		{
			ImGuiNative.igSetNextWindowDockID(dock_id, cond);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000FADF File Offset: 0x0000DCDF
		public static void SetNextWindowFocus()
		{
			ImGuiNative.igSetNextWindowFocus();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		public static void SetNextWindowPos(Vector2 pos)
		{
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetNextWindowPos(pos, cond, default(Vector2));
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000FB08 File Offset: 0x0000DD08
		public static void SetNextWindowPos(Vector2 pos, ImGuiCond cond)
		{
			ImGuiNative.igSetNextWindowPos(pos, cond, default(Vector2));
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000FB25 File Offset: 0x0000DD25
		public static void SetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot)
		{
			ImGuiNative.igSetNextWindowPos(pos, cond, pivot);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		public static void SetNextWindowScroll(Vector2 scroll)
		{
			ImGuiNative.igSetNextWindowScroll(scroll);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000FB38 File Offset: 0x0000DD38
		public static void SetNextWindowSize(Vector2 size)
		{
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetNextWindowSize(size, cond);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000FB4E File Offset: 0x0000DD4E
		public static void SetNextWindowSize(Vector2 size, ImGuiCond cond)
		{
			ImGuiNative.igSetNextWindowSize(size, cond);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000FB58 File Offset: 0x0000DD58
		public unsafe static void SetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max)
		{
			ImGuiSizeCallback custom_callback = null;
			void* custom_callback_data = null;
			ImGuiNative.igSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000FB74 File Offset: 0x0000DD74
		public unsafe static void SetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback)
		{
			void* custom_callback_data = null;
			ImGuiNative.igSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000FB90 File Offset: 0x0000DD90
		public unsafe static void SetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, IntPtr custom_callback_data)
		{
			void* custom_callback_data2 = custom_callback_data.ToPointer();
			ImGuiNative.igSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data2);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000FBAE File Offset: 0x0000DDAE
		public static void SetNextWindowViewport(uint viewport_id)
		{
			ImGuiNative.igSetNextWindowViewport(viewport_id);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000FBB8 File Offset: 0x0000DDB8
		public static void SetScrollFromPosX(float local_x)
		{
			float center_x_ratio = 0.5f;
			ImGuiNative.igSetScrollFromPosX_Float(local_x, center_x_ratio);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000FBD2 File Offset: 0x0000DDD2
		public static void SetScrollFromPosX(float local_x, float center_x_ratio)
		{
			ImGuiNative.igSetScrollFromPosX_Float(local_x, center_x_ratio);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000FBDC File Offset: 0x0000DDDC
		public static void SetScrollFromPosY(float local_y)
		{
			float center_y_ratio = 0.5f;
			ImGuiNative.igSetScrollFromPosY_Float(local_y, center_y_ratio);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000FBF6 File Offset: 0x0000DDF6
		public static void SetScrollFromPosY(float local_y, float center_y_ratio)
		{
			ImGuiNative.igSetScrollFromPosY_Float(local_y, center_y_ratio);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000FBFF File Offset: 0x0000DDFF
		public static void SetScrollHereX()
		{
			ImGuiNative.igSetScrollHereX(0.5f);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000FC0B File Offset: 0x0000DE0B
		public static void SetScrollHereX(float center_x_ratio)
		{
			ImGuiNative.igSetScrollHereX(center_x_ratio);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000FC13 File Offset: 0x0000DE13
		public static void SetScrollHereY()
		{
			ImGuiNative.igSetScrollHereY(0.5f);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000FC1F File Offset: 0x0000DE1F
		public static void SetScrollHereY(float center_y_ratio)
		{
			ImGuiNative.igSetScrollHereY(center_y_ratio);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000FC27 File Offset: 0x0000DE27
		public static void SetScrollX(float scroll_x)
		{
			ImGuiNative.igSetScrollX_Float(scroll_x);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000FC2F File Offset: 0x0000DE2F
		public static void SetScrollY(float scroll_y)
		{
			ImGuiNative.igSetScrollY_Float(scroll_y);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000FC37 File Offset: 0x0000DE37
		public static void SetStateStorage(ImGuiStoragePtr storage)
		{
			ImGuiNative.igSetStateStorage(storage.NativePtr);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000FC48 File Offset: 0x0000DE48
		public unsafe static void SetTabItemClosed(ReadOnlySpan<char> tab_or_docked_window_label)
		{
			int num = 0;
			byte* ptr;
			if (tab_or_docked_window_label != null)
			{
				num = Encoding.UTF8.GetByteCount(tab_or_docked_window_label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(tab_or_docked_window_label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetTabItemClosed(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		public unsafe static void SetTooltip(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetTooltip(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000FD20 File Offset: 0x0000DF20
		public static void SetWindowCollapsed(bool collapsed)
		{
			byte collapsed2 = (collapsed > false) ? 1 : 0;
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetWindowCollapsed_Bool(collapsed2, cond);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000FD3A File Offset: 0x0000DF3A
		public static void SetWindowCollapsed(bool collapsed, ImGuiCond cond)
		{
			ImGuiNative.igSetWindowCollapsed_Bool((collapsed > false) ? 1 : 0, cond);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000FD48 File Offset: 0x0000DF48
		public unsafe static void SetWindowCollapsed(ReadOnlySpan<char> name, bool collapsed)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte collapsed2 = (collapsed > false) ? 1 : 0;
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetWindowCollapsed_Str(ptr, collapsed2, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		public unsafe static void SetWindowCollapsed(ReadOnlySpan<char> name, bool collapsed, ImGuiCond cond)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte collapsed2 = (collapsed > false) ? 1 : 0;
			ImGuiNative.igSetWindowCollapsed_Str(ptr, collapsed2, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000FE32 File Offset: 0x0000E032
		public static void SetWindowFocus()
		{
			ImGuiNative.igSetWindowFocus_Nil();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000FE3C File Offset: 0x0000E03C
		public unsafe static void SetWindowFocus(ReadOnlySpan<char> name)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetWindowFocus_Str(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000FEA6 File Offset: 0x0000E0A6
		public static void SetWindowFontScale(float scale)
		{
			ImGuiNative.igSetWindowFontScale(scale);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000FEB0 File Offset: 0x0000E0B0
		public static void SetWindowPos(Vector2 pos)
		{
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetWindowPos_Vec2(pos, cond);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000FEC6 File Offset: 0x0000E0C6
		public static void SetWindowPos(Vector2 pos, ImGuiCond cond)
		{
			ImGuiNative.igSetWindowPos_Vec2(pos, cond);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		public unsafe static void SetWindowPos(ReadOnlySpan<char> name, Vector2 pos)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetWindowPos_Str(ptr, pos, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000FF40 File Offset: 0x0000E140
		public unsafe static void SetWindowPos(ReadOnlySpan<char> name, Vector2 pos, ImGuiCond cond)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetWindowPos_Str(ptr, pos, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000FFAC File Offset: 0x0000E1AC
		public static void SetWindowSize(Vector2 size)
		{
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetWindowSize_Vec2(size, cond);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000FFC2 File Offset: 0x0000E1C2
		public static void SetWindowSize(Vector2 size, ImGuiCond cond)
		{
			ImGuiNative.igSetWindowSize_Vec2(size, cond);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000FFCC File Offset: 0x0000E1CC
		public unsafe static void SetWindowSize(ReadOnlySpan<char> name, Vector2 size)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiCond cond = ImGuiCond.None;
			ImGuiNative.igSetWindowSize_Str(ptr, size, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001003C File Offset: 0x0000E23C
		public unsafe static void SetWindowSize(ReadOnlySpan<char> name, Vector2 size, ImGuiCond cond)
		{
			int num = 0;
			byte* ptr;
			if (name != null)
			{
				num = Encoding.UTF8.GetByteCount(name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igSetWindowSize_Str(ptr, size, cond);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public unsafe static void ShowAboutWindow()
		{
			byte* p_open = null;
			ImGuiNative.igShowAboutWindow(p_open);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000100C0 File Offset: 0x0000E2C0
		public unsafe static void ShowAboutWindow(ref bool p_open)
		{
			byte b = (p_open > false) ? 1 : 0;
			ImGuiNative.igShowAboutWindow(&b);
			p_open = (b > 0);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000100E4 File Offset: 0x0000E2E4
		public unsafe static void ShowDebugLogWindow()
		{
			byte* p_open = null;
			ImGuiNative.igShowDebugLogWindow(p_open);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000100FC File Offset: 0x0000E2FC
		public unsafe static void ShowDebugLogWindow(ref bool p_open)
		{
			byte b = (p_open > false) ? 1 : 0;
			ImGuiNative.igShowDebugLogWindow(&b);
			p_open = (b > 0);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010120 File Offset: 0x0000E320
		public unsafe static void ShowDemoWindow()
		{
			byte* p_open = null;
			ImGuiNative.igShowDemoWindow(p_open);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00010138 File Offset: 0x0000E338
		public unsafe static void ShowDemoWindow(ref bool p_open)
		{
			byte b = (p_open > false) ? 1 : 0;
			ImGuiNative.igShowDemoWindow(&b);
			p_open = (b > 0);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001015C File Offset: 0x0000E35C
		public unsafe static void ShowFontSelector(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igShowFontSelector(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000101C8 File Offset: 0x0000E3C8
		public unsafe static void ShowMetricsWindow()
		{
			byte* p_open = null;
			ImGuiNative.igShowMetricsWindow(p_open);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000101E0 File Offset: 0x0000E3E0
		public unsafe static void ShowMetricsWindow(ref bool p_open)
		{
			byte b = (p_open > false) ? 1 : 0;
			ImGuiNative.igShowMetricsWindow(&b);
			p_open = (b > 0);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00010204 File Offset: 0x0000E404
		public unsafe static void ShowStackToolWindow()
		{
			byte* p_open = null;
			ImGuiNative.igShowStackToolWindow(p_open);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001021C File Offset: 0x0000E41C
		public unsafe static void ShowStackToolWindow(ref bool p_open)
		{
			byte b = (p_open > false) ? 1 : 0;
			ImGuiNative.igShowStackToolWindow(&b);
			p_open = (b > 0);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00010240 File Offset: 0x0000E440
		public unsafe static void ShowStyleEditor()
		{
			ImGuiStyle* @ref = null;
			ImGuiNative.igShowStyleEditor(@ref);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00010256 File Offset: 0x0000E456
		public static void ShowStyleEditor(ImGuiStylePtr @ref)
		{
			ImGuiNative.igShowStyleEditor(@ref.NativePtr);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00010264 File Offset: 0x0000E464
		public unsafe static bool ShowStyleSelector(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igShowStyleSelector(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000102D1 File Offset: 0x0000E4D1
		public static void ShowUserGuide()
		{
			ImGuiNative.igShowUserGuide();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000102D8 File Offset: 0x0000E4D8
		public unsafe static bool SliderAngle(ReadOnlySpan<char> label, ref float v_rad)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_degrees_min = -360f;
			float v_degrees_max = 360f;
			int byteCount = Encoding.UTF8.GetByteCount("%.0f deg");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.0f deg", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_rad)
			{
				float* v_rad2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderAngle(ptr, v_rad2, v_degrees_min, v_degrees_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000103C0 File Offset: 0x0000E5C0
		public unsafe static bool SliderAngle(ReadOnlySpan<char> label, ref float v_rad, float v_degrees_min)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float v_degrees_max = 360f;
			int byteCount = Encoding.UTF8.GetByteCount("%.0f deg");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.0f deg", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_rad)
			{
				float* v_rad2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderAngle(ptr, v_rad2, v_degrees_min, v_degrees_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001049C File Offset: 0x0000E69C
		public unsafe static bool SliderAngle(ReadOnlySpan<char> label, ref float v_rad, float v_degrees_min, float v_degrees_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.0f deg");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.0f deg", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_rad)
			{
				float* v_rad2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderAngle(ptr, v_rad2, v_degrees_min, v_degrees_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001056C File Offset: 0x0000E76C
		public unsafe static bool SliderAngle(ReadOnlySpan<char> label, ref float v_rad, float v_degrees_min, float v_degrees_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v_rad)
			{
				float* v_rad2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderAngle(ptr, v_rad2, v_degrees_min, v_degrees_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001064C File Offset: 0x0000E84C
		public unsafe static bool SliderAngle(ReadOnlySpan<char> label, ref float v_rad, float v_degrees_min, float v_degrees_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &v_rad)
			{
				float* v_rad2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderAngle(ptr, v_rad2, v_degrees_min, v_degrees_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00010728 File Offset: 0x0000E928
		public unsafe static bool SliderFloat(ReadOnlySpan<char> label, ref float v, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderFloat(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000107F8 File Offset: 0x0000E9F8
		public unsafe static bool SliderFloat(ReadOnlySpan<char> label, ref float v, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000108D8 File Offset: 0x0000EAD8
		public unsafe static bool SliderFloat(ReadOnlySpan<char> label, ref float v, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000109B4 File Offset: 0x0000EBB4
		public unsafe static bool SliderFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderFloat2(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00010A84 File Offset: 0x0000EC84
		public unsafe static bool SliderFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat2(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00010B64 File Offset: 0x0000ED64
		public unsafe static bool SliderFloat2(ReadOnlySpan<char> label, ref Vector2 v, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector2* ptr3 = &v)
			{
				Vector2* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat2(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00010C40 File Offset: 0x0000EE40
		public unsafe static bool SliderFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderFloat3(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00010D10 File Offset: 0x0000EF10
		public unsafe static bool SliderFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat3(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010DF0 File Offset: 0x0000EFF0
		public unsafe static bool SliderFloat3(ReadOnlySpan<char> label, ref Vector3 v, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector3* ptr3 = &v)
			{
				Vector3* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat3(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00010ECC File Offset: 0x0000F0CC
		public unsafe static bool SliderFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderFloat4(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00010F9C File Offset: 0x0000F19C
		public unsafe static bool SliderFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat4(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001107C File Offset: 0x0000F27C
		public unsafe static bool SliderFloat4(ReadOnlySpan<char> label, ref Vector4 v, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (Vector4* ptr3 = &v)
			{
				Vector4* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderFloat4(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00011158 File Offset: 0x0000F358
		public unsafe static bool SliderInt(ReadOnlySpan<char> label, ref int v, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderInt(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00011228 File Offset: 0x0000F428
		public unsafe static bool SliderInt(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00011308 File Offset: 0x0000F508
		public unsafe static bool SliderInt(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000113E4 File Offset: 0x0000F5E4
		public unsafe static bool SliderInt2(ReadOnlySpan<char> label, ref int v, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderInt2(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000114B4 File Offset: 0x0000F6B4
		public unsafe static bool SliderInt2(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt2(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00011594 File Offset: 0x0000F794
		public unsafe static bool SliderInt2(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt2(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00011670 File Offset: 0x0000F870
		public unsafe static bool SliderInt3(ReadOnlySpan<char> label, ref int v, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderInt3(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00011740 File Offset: 0x0000F940
		public unsafe static bool SliderInt3(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt3(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00011820 File Offset: 0x0000FA20
		public unsafe static bool SliderInt3(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt3(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000118FC File Offset: 0x0000FAFC
		public unsafe static bool SliderInt4(ReadOnlySpan<char> label, ref int v, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igSliderInt4(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000119CC File Offset: 0x0000FBCC
		public unsafe static bool SliderInt4(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt4(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00011AAC File Offset: 0x0000FCAC
		public unsafe static bool SliderInt4(ReadOnlySpan<char> label, ref int v, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igSliderInt4(ptr, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00011B88 File Offset: 0x0000FD88
		public unsafe static bool SliderScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_min, IntPtr p_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igSliderScalar(ptr, data_type, p_data2, p_min2, p_max2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00011C20 File Offset: 0x0000FE20
		public unsafe static bool SliderScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num3 = (int)ImGuiNative.igSliderScalar(ptr, data_type, p_data2, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00011D20 File Offset: 0x0000FF20
		public unsafe static bool SliderScalar(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igSliderScalar(ptr, data_type, p_data2, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00011E1C File Offset: 0x0001001C
		public unsafe static bool SliderScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_min, IntPtr p_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igSliderScalarN(ptr, data_type, p_data2, components, p_min2, p_max2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00011EB8 File Offset: 0x000100B8
		public unsafe static bool SliderScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num3 = (int)ImGuiNative.igSliderScalarN(ptr, data_type, p_data2, components, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00011FB8 File Offset: 0x000101B8
		public unsafe static bool SliderScalarN(ReadOnlySpan<char> label, ImGuiDataType data_type, IntPtr p_data, int components, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igSliderScalarN(ptr, data_type, p_data2, components, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000120B4 File Offset: 0x000102B4
		public unsafe static bool SmallButton(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igSmallButton(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00012121 File Offset: 0x00010321
		public static void Spacing()
		{
			ImGuiNative.igSpacing();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00012128 File Offset: 0x00010328
		public unsafe static void StyleColorsClassic()
		{
			ImGuiStyle* dst = null;
			ImGuiNative.igStyleColorsClassic(dst);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001213E File Offset: 0x0001033E
		public static void StyleColorsClassic(ImGuiStylePtr dst)
		{
			ImGuiNative.igStyleColorsClassic(dst.NativePtr);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001214C File Offset: 0x0001034C
		public unsafe static void StyleColorsDark()
		{
			ImGuiStyle* dst = null;
			ImGuiNative.igStyleColorsDark(dst);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00012162 File Offset: 0x00010362
		public static void StyleColorsDark(ImGuiStylePtr dst)
		{
			ImGuiNative.igStyleColorsDark(dst.NativePtr);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00012170 File Offset: 0x00010370
		public unsafe static void StyleColorsLight()
		{
			ImGuiStyle* dst = null;
			ImGuiNative.igStyleColorsLight(dst);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00012186 File Offset: 0x00010386
		public static void StyleColorsLight(ImGuiStylePtr dst)
		{
			ImGuiNative.igStyleColorsLight(dst.NativePtr);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00012194 File Offset: 0x00010394
		public unsafe static bool TabItemButton(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiTabItemFlags flags = ImGuiTabItemFlags.None;
			int num2 = (int)ImGuiNative.igTabItemButton(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00012204 File Offset: 0x00010404
		public unsafe static bool TabItemButton(ReadOnlySpan<char> label, ImGuiTabItemFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igTabItemButton(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00012272 File Offset: 0x00010472
		public static int TableGetColumnCount()
		{
			return ImGuiNative.igTableGetColumnCount();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00012279 File Offset: 0x00010479
		public static ImGuiTableColumnFlags TableGetColumnFlags()
		{
			return ImGuiNative.igTableGetColumnFlags(-1);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00012281 File Offset: 0x00010481
		public static ImGuiTableColumnFlags TableGetColumnFlags(int column_n)
		{
			return ImGuiNative.igTableGetColumnFlags(column_n);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00012289 File Offset: 0x00010489
		public static int TableGetColumnIndex()
		{
			return ImGuiNative.igTableGetColumnIndex();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00012290 File Offset: 0x00010490
		public static string TableGetColumnName()
		{
			return Util.StringFromPtr(ImGuiNative.igTableGetColumnName_Int(-1));
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001229D File Offset: 0x0001049D
		public static string TableGetColumnName(int column_n)
		{
			return Util.StringFromPtr(ImGuiNative.igTableGetColumnName_Int(column_n));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000122AA File Offset: 0x000104AA
		public static int TableGetRowIndex()
		{
			return ImGuiNative.igTableGetRowIndex();
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000122B1 File Offset: 0x000104B1
		public static ImGuiTableSortSpecsPtr TableGetSortSpecs()
		{
			return new ImGuiTableSortSpecsPtr(ImGuiNative.igTableGetSortSpecs());
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000122C0 File Offset: 0x000104C0
		public unsafe static void TableHeader(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igTableHeader(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001232A File Offset: 0x0001052A
		public static void TableHeadersRow()
		{
			ImGuiNative.igTableHeadersRow();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00012331 File Offset: 0x00010531
		public static bool TableNextColumn()
		{
			return ImGuiNative.igTableNextColumn() > 0;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001233C File Offset: 0x0001053C
		public static void TableNextRow()
		{
			ImGuiTableRowFlags row_flags = ImGuiTableRowFlags.None;
			float min_row_height = 0f;
			ImGuiNative.igTableNextRow(row_flags, min_row_height);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00012358 File Offset: 0x00010558
		public static void TableNextRow(ImGuiTableRowFlags row_flags)
		{
			float min_row_height = 0f;
			ImGuiNative.igTableNextRow(row_flags, min_row_height);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00012372 File Offset: 0x00010572
		public static void TableNextRow(ImGuiTableRowFlags row_flags, float min_row_height)
		{
			ImGuiNative.igTableNextRow(row_flags, min_row_height);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001237C File Offset: 0x0001057C
		public static void TableSetBgColor(ImGuiTableBgTarget target, uint color)
		{
			int column_n = -1;
			ImGuiNative.igTableSetBgColor(target, color, column_n);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00012393 File Offset: 0x00010593
		public static void TableSetBgColor(ImGuiTableBgTarget target, uint color, int column_n)
		{
			ImGuiNative.igTableSetBgColor(target, color, column_n);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000123A0 File Offset: 0x000105A0
		public static void TableSetColumnEnabled(int column_n, bool v)
		{
			byte v2 = (v > false) ? 1 : 0;
			ImGuiNative.igTableSetColumnEnabled(column_n, v2);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000123BA File Offset: 0x000105BA
		public static bool TableSetColumnIndex(int column_n)
		{
			return ImGuiNative.igTableSetColumnIndex(column_n) > 0;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000123C8 File Offset: 0x000105C8
		public unsafe static void TableSetupColumn(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiTableColumnFlags flags = ImGuiTableColumnFlags.None;
			float init_width_or_weight = 0f;
			uint user_id = 0U;
			ImGuiNative.igTableSetupColumn(ptr, flags, init_width_or_weight, user_id);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00012444 File Offset: 0x00010644
		public unsafe static void TableSetupColumn(ReadOnlySpan<char> label, ImGuiTableColumnFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			float init_width_or_weight = 0f;
			uint user_id = 0U;
			ImGuiNative.igTableSetupColumn(ptr, flags, init_width_or_weight, user_id);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000124BC File Offset: 0x000106BC
		public unsafe static void TableSetupColumn(ReadOnlySpan<char> label, ImGuiTableColumnFlags flags, float init_width_or_weight)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			uint user_id = 0U;
			ImGuiNative.igTableSetupColumn(ptr, flags, init_width_or_weight, user_id);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001252C File Offset: 0x0001072C
		public unsafe static void TableSetupColumn(ReadOnlySpan<char> label, ImGuiTableColumnFlags flags, float init_width_or_weight, uint user_id)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igTableSetupColumn(ptr, flags, init_width_or_weight, user_id);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00012599 File Offset: 0x00010799
		public static void TableSetupScrollFreeze(int cols, int rows)
		{
			ImGuiNative.igTableSetupScrollFreeze(cols, rows);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000125A4 File Offset: 0x000107A4
		public unsafe static void Text(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igText(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00012610 File Offset: 0x00010810
		public unsafe static void TextColored(Vector4 col, ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igTextColored(col, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001267C File Offset: 0x0001087C
		public unsafe static void TextDisabled(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igTextDisabled(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000126E8 File Offset: 0x000108E8
		public unsafe static void TextUnformatted(ReadOnlySpan<char> text)
		{
			int num = 0;
			byte* ptr;
			if (text != null)
			{
				num = Encoding.UTF8.GetByteCount(text);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(text, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* text_end = null;
			ImGuiNative.igTextUnformatted(ptr, text_end);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00012758 File Offset: 0x00010958
		public unsafe static void TextWrapped(ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igTextWrapped(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000127C4 File Offset: 0x000109C4
		public unsafe static bool TreeNode(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igTreeNode_Str(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00012834 File Offset: 0x00010A34
		public unsafe static bool TreeNode(ReadOnlySpan<char> str_id, ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (fmt != null)
			{
				num2 = Encoding.UTF8.GetByteCount(fmt);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(fmt, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igTreeNode_StrStr(ptr, ptr2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00012900 File Offset: 0x00010B00
		public unsafe static bool TreeNode(IntPtr ptr_id, ReadOnlySpan<char> fmt)
		{
			void* ptr_id2 = ptr_id.ToPointer();
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igTreeNode_Ptr(ptr_id2, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00012978 File Offset: 0x00010B78
		public unsafe static bool TreeNodeEx(ReadOnlySpan<char> label)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None;
			int num2 = (int)ImGuiNative.igTreeNodeEx_Str(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000129E8 File Offset: 0x00010BE8
		public unsafe static bool TreeNodeEx(ReadOnlySpan<char> label, ImGuiTreeNodeFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igTreeNodeEx_Str(ptr, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00012A58 File Offset: 0x00010C58
		public unsafe static bool TreeNodeEx(ReadOnlySpan<char> str_id, ImGuiTreeNodeFlags flags, ReadOnlySpan<char> fmt)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (fmt != null)
			{
				num2 = Encoding.UTF8.GetByteCount(fmt);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(fmt, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igTreeNodeEx_StrStr(ptr, flags, ptr2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00012B24 File Offset: 0x00010D24
		public unsafe static bool TreeNodeEx(IntPtr ptr_id, ImGuiTreeNodeFlags flags, ReadOnlySpan<char> fmt)
		{
			void* ptr_id2 = ptr_id.ToPointer();
			int num = 0;
			byte* ptr;
			if (fmt != null)
			{
				num = Encoding.UTF8.GetByteCount(fmt);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(fmt, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.igTreeNodeEx_Ptr(ptr_id2, flags, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00012B9B File Offset: 0x00010D9B
		public static void TreePop()
		{
			ImGuiNative.igTreePop();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00012BA4 File Offset: 0x00010DA4
		public unsafe static void TreePush(ReadOnlySpan<char> str_id)
		{
			int num = 0;
			byte* ptr;
			if (str_id != null)
			{
				num = Encoding.UTF8.GetByteCount(str_id);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(str_id, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igTreePush_Str(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00012C0E File Offset: 0x00010E0E
		public static void TreePush(IntPtr ptr_id)
		{
			ImGuiNative.igTreePush_Ptr(ptr_id.ToPointer());
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00012C1C File Offset: 0x00010E1C
		public static void Unindent()
		{
			ImGuiNative.igUnindent(0f);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00012C28 File Offset: 0x00010E28
		public static void Unindent(float indent_w)
		{
			ImGuiNative.igUnindent(indent_w);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00012C30 File Offset: 0x00010E30
		public static void UpdatePlatformWindows()
		{
			ImGuiNative.igUpdatePlatformWindows();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00012C38 File Offset: 0x00010E38
		public unsafe static void Value(ReadOnlySpan<char> prefix, bool b)
		{
			int num = 0;
			byte* ptr;
			if (prefix != null)
			{
				num = Encoding.UTF8.GetByteCount(prefix);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(prefix, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte b2 = (b > false) ? 1 : 0;
			ImGuiNative.igValue_Bool(ptr, b2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00012CAC File Offset: 0x00010EAC
		public unsafe static void Value(ReadOnlySpan<char> prefix, int v)
		{
			int num = 0;
			byte* ptr;
			if (prefix != null)
			{
				num = Encoding.UTF8.GetByteCount(prefix);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(prefix, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igValue_Int(ptr, v);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00012D18 File Offset: 0x00010F18
		public unsafe static void Value(ReadOnlySpan<char> prefix, uint v)
		{
			int num = 0;
			byte* ptr;
			if (prefix != null)
			{
				num = Encoding.UTF8.GetByteCount(prefix);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(prefix, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			ImGuiNative.igValue_Uint(ptr, v);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00012D84 File Offset: 0x00010F84
		public unsafe static void Value(ReadOnlySpan<char> prefix, float v)
		{
			int num = 0;
			byte* ptr;
			if (prefix != null)
			{
				num = Encoding.UTF8.GetByteCount(prefix);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(prefix, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			byte* float_format = null;
			ImGuiNative.igValue_Float(ptr, v, float_format);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00012DF4 File Offset: 0x00010FF4
		public unsafe static void Value(ReadOnlySpan<char> prefix, float v, ReadOnlySpan<char> float_format)
		{
			int num = 0;
			byte* ptr;
			if (prefix != null)
			{
				num = Encoding.UTF8.GetByteCount(prefix);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(prefix, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (float_format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(float_format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(float_format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiNative.igValue_Float(ptr, v, ptr2);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00012EBC File Offset: 0x000110BC
		public unsafe static bool VSliderFloat(ReadOnlySpan<char> label, Vector2 size, ref float v, float v_min, float v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%.3f");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%.3f", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num2 = (int)ImGuiNative.igVSliderFloat(ptr, size, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00012F90 File Offset: 0x00011190
		public unsafe static bool VSliderFloat(ReadOnlySpan<char> label, Vector2 size, ref float v, float v_min, float v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igVSliderFloat(ptr, size, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00013070 File Offset: 0x00011270
		public unsafe static bool VSliderFloat(ReadOnlySpan<char> label, Vector2 size, ref float v, float v_min, float v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (float* ptr3 = &v)
			{
				float* v2 = ptr3;
				int num3 = (int)ImGuiNative.igVSliderFloat(ptr, size, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001314C File Offset: 0x0001134C
		public unsafe static bool VSliderInt(ReadOnlySpan<char> label, Vector2 size, ref int v, int v_min, int v_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int byteCount = Encoding.UTF8.GetByteCount("%d");
			byte* ptr2;
			if (byteCount > 2048)
			{
				ptr2 = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			int utf2 = Util.GetUtf8("%d", ptr2, byteCount);
			ptr2[utf2] = 0;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num2 = (int)ImGuiNative.igVSliderInt(ptr, size, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (byteCount > 2048)
				{
					Util.Free(ptr2);
				}
				return num2 != 0;
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00013220 File Offset: 0x00011420
		public unsafe static bool VSliderInt(ReadOnlySpan<char> label, Vector2 size, ref int v, int v_min, int v_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igVSliderInt(ptr, size, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00013300 File Offset: 0x00011500
		public unsafe static bool VSliderInt(ReadOnlySpan<char> label, Vector2 size, ref int v, int v_min, int v_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			fixed (int* ptr3 = &v)
			{
				int* v2 = ptr3;
				int num3 = (int)ImGuiNative.igVSliderInt(ptr, size, v2, v_min, v_max, ptr2, flags);
				if (num > 2048)
				{
					Util.Free(ptr);
				}
				if (num2 > 2048)
				{
					Util.Free(ptr2);
				}
				return num3 != 0;
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000133DC File Offset: 0x000115DC
		public unsafe static bool VSliderScalar(ReadOnlySpan<char> label, Vector2 size, ImGuiDataType data_type, IntPtr p_data, IntPtr p_min, IntPtr p_max)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			byte* format = null;
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num2 = (int)ImGuiNative.igVSliderScalar(ptr, size, data_type, p_data2, p_min2, p_max2, format, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00013478 File Offset: 0x00011678
		public unsafe static bool VSliderScalar(ReadOnlySpan<char> label, Vector2 size, ImGuiDataType data_type, IntPtr p_data, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			ImGuiSliderFlags flags = ImGuiSliderFlags.None;
			int num3 = (int)ImGuiNative.igVSliderScalar(ptr, size, data_type, p_data2, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00013578 File Offset: 0x00011778
		public unsafe static bool VSliderScalar(ReadOnlySpan<char> label, Vector2 size, ImGuiDataType data_type, IntPtr p_data, IntPtr p_min, IntPtr p_max, ReadOnlySpan<char> format, ImGuiSliderFlags flags)
		{
			int num = 0;
			byte* ptr;
			if (label != null)
			{
				num = Encoding.UTF8.GetByteCount(label);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(label, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			void* p_data2 = p_data.ToPointer();
			void* p_min2 = p_min.ToPointer();
			void* p_max2 = p_max.ToPointer();
			int num2 = 0;
			byte* ptr2;
			if (format != null)
			{
				num2 = Encoding.UTF8.GetByteCount(format);
				if (num2 > 2048)
				{
					ptr2 = Util.Allocate(num2 + 1);
				}
				else
				{
					ptr2 = stackalloc byte[(UIntPtr)(num2 + 1)];
				}
				int utf2 = Util.GetUtf8(format, ptr2, num2);
				ptr2[utf2] = 0;
			}
			else
			{
				ptr2 = null;
			}
			int num3 = (int)ImGuiNative.igVSliderScalar(ptr, size, data_type, p_data2, p_min2, p_max2, ptr2, flags);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			if (num2 > 2048)
			{
				Util.Free(ptr2);
			}
			return num3 != 0;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00013673 File Offset: 0x00011873
		public static bool InputText(ReadOnlySpan<char> label, byte[] buf, uint buf_size)
		{
			return ImGui.InputText(label, buf, buf_size, ImGuiInputTextFlags.None, null, IntPtr.Zero);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00013684 File Offset: 0x00011884
		public static bool InputText(ReadOnlySpan<char> label, byte[] buf, uint buf_size, ImGuiInputTextFlags flags)
		{
			return ImGui.InputText(label, buf, buf_size, flags, null, IntPtr.Zero);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00013695 File Offset: 0x00011895
		public static bool InputText(ReadOnlySpan<char> label, byte[] buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
		{
			return ImGui.InputText(label, buf, buf_size, flags, callback, IntPtr.Zero);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000136A8 File Offset: 0x000118A8
		public unsafe static bool InputText(ReadOnlySpan<char> label, byte[] buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
		{
			int byteCount = Encoding.UTF8.GetByteCount(label);
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			Util.GetUtf8(label, ptr, byteCount);
			bool result;
			fixed (byte[] array = buf)
			{
				byte* buf2;
				if (buf == null || array.Length == 0)
				{
					buf2 = null;
				}
				else
				{
					buf2 = &array[0];
				}
				result = (ImGuiNative.igInputText(ptr, buf2, buf_size, flags, callback, user_data.ToPointer()) > 0);
			}
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			return result;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00013721 File Offset: 0x00011921
		public static bool InputText(ReadOnlySpan<char> label, ref string input, uint maxLength)
		{
			return ImGui.InputText(label, ref input, maxLength, ImGuiInputTextFlags.None, null, IntPtr.Zero);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00013732 File Offset: 0x00011932
		public static bool InputText(ReadOnlySpan<char> label, ref string input, uint maxLength, ImGuiInputTextFlags flags)
		{
			return ImGui.InputText(label, ref input, maxLength, flags, null, IntPtr.Zero);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00013743 File Offset: 0x00011943
		public static bool InputText(ReadOnlySpan<char> label, ref string input, uint maxLength, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
		{
			return ImGui.InputText(label, ref input, maxLength, flags, callback, IntPtr.Zero);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00013758 File Offset: 0x00011958
		public unsafe static bool InputText(ReadOnlySpan<char> label, ref string input, uint maxLength, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
		{
			int byteCount = Encoding.UTF8.GetByteCount(label);
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			Util.GetUtf8(label, ptr, byteCount);
			int byteCount2 = Encoding.UTF8.GetByteCount(input);
			int num = Math.Max((int)(maxLength + 1U), byteCount2 + 1);
			byte* ptr2;
			byte* ptr3;
			if (num > 2048)
			{
				ptr2 = Util.Allocate(num);
				ptr3 = Util.Allocate(num);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)num];
				ptr3 = stackalloc byte[(UIntPtr)num];
			}
			Util.GetUtf8(input, ptr2, num);
			uint num2 = (uint)(num - byteCount2);
			Unsafe.InitBlockUnaligned((void*)(ptr2 + byteCount2), 0, num2);
			Unsafe.CopyBlock((void*)ptr3, (void*)ptr2, (uint)num);
			byte b = ImGuiNative.igInputText(ptr, ptr2, (uint)num, flags, callback, user_data.ToPointer());
			if (!Util.AreStringsEqual(ptr3, num, ptr2))
			{
				input = Util.StringFromPtr(ptr2);
			}
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			if (num > 2048)
			{
				Util.Free(ptr2);
				Util.Free(ptr3);
			}
			return b > 0;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001384C File Offset: 0x00011A4C
		public static bool InputTextMultiline(ReadOnlySpan<char> label, ref string input, uint maxLength, Vector2 size)
		{
			return ImGui.InputTextMultiline(label, ref input, maxLength, size, ImGuiInputTextFlags.None, null, IntPtr.Zero);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001385E File Offset: 0x00011A5E
		public static bool InputTextMultiline(ReadOnlySpan<char> label, ref string input, uint maxLength, Vector2 size, ImGuiInputTextFlags flags)
		{
			return ImGui.InputTextMultiline(label, ref input, maxLength, size, flags, null, IntPtr.Zero);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00013871 File Offset: 0x00011A71
		public static bool InputTextMultiline(ReadOnlySpan<char> label, ref string input, uint maxLength, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
		{
			return ImGui.InputTextMultiline(label, ref input, maxLength, size, flags, callback, IntPtr.Zero);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00013888 File Offset: 0x00011A88
		public unsafe static bool InputTextMultiline(ReadOnlySpan<char> label, ref string input, uint maxLength, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
		{
			int byteCount = Encoding.UTF8.GetByteCount(label);
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			Util.GetUtf8(label, ptr, byteCount);
			int byteCount2 = Encoding.UTF8.GetByteCount(input);
			int num = Math.Max((int)(maxLength + 1U), byteCount2 + 1);
			byte* ptr2;
			byte* ptr3;
			if (num > 2048)
			{
				ptr2 = Util.Allocate(num);
				ptr3 = Util.Allocate(num);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)num];
				ptr3 = stackalloc byte[(UIntPtr)num];
			}
			Util.GetUtf8(input, ptr2, num);
			uint num2 = (uint)(num - byteCount2);
			Unsafe.InitBlockUnaligned((void*)(ptr2 + byteCount2), 0, num2);
			Unsafe.CopyBlock((void*)ptr3, (void*)ptr2, (uint)num);
			byte b = ImGuiNative.igInputTextMultiline(ptr, ptr2, (uint)num, size, flags, callback, user_data.ToPointer());
			if (!Util.AreStringsEqual(ptr3, num, ptr2))
			{
				input = Util.StringFromPtr(ptr2);
			}
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			if (num > 2048)
			{
				Util.Free(ptr2);
				Util.Free(ptr3);
			}
			return b > 0;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001397E File Offset: 0x00011B7E
		public static bool InputTextWithHint(ReadOnlySpan<char> label, ReadOnlySpan<char> hint, ref string input, uint maxLength)
		{
			return ImGui.InputTextWithHint(label, hint, ref input, maxLength, ImGuiInputTextFlags.None, null, IntPtr.Zero);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00013990 File Offset: 0x00011B90
		public static bool InputTextWithHint(ReadOnlySpan<char> label, ReadOnlySpan<char> hint, ref string input, uint maxLength, ImGuiInputTextFlags flags)
		{
			return ImGui.InputTextWithHint(label, hint, ref input, maxLength, flags, null, IntPtr.Zero);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000139A3 File Offset: 0x00011BA3
		public static bool InputTextWithHint(ReadOnlySpan<char> label, ReadOnlySpan<char> hint, ref string input, uint maxLength, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
		{
			return ImGui.InputTextWithHint(label, hint, ref input, maxLength, flags, callback, IntPtr.Zero);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000139B8 File Offset: 0x00011BB8
		public unsafe static bool InputTextWithHint(ReadOnlySpan<char> label, ReadOnlySpan<char> hint, ref string input, uint maxLength, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
		{
			int byteCount = Encoding.UTF8.GetByteCount(label);
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			Util.GetUtf8(label, ptr, byteCount);
			int byteCount2 = Encoding.UTF8.GetByteCount(hint);
			byte* ptr2;
			if (byteCount2 > 2048)
			{
				ptr2 = Util.Allocate(byteCount2 + 1);
			}
			else
			{
				ptr2 = stackalloc byte[(UIntPtr)(byteCount2 + 1)];
			}
			Util.GetUtf8(hint, ptr2, byteCount2);
			int byteCount3 = Encoding.UTF8.GetByteCount(input);
			int num = Math.Max((int)(maxLength + 1U), byteCount3 + 1);
			byte* ptr3;
			byte* ptr4;
			if (num > 2048)
			{
				ptr3 = Util.Allocate(num);
				ptr4 = Util.Allocate(num);
			}
			else
			{
				ptr3 = stackalloc byte[(UIntPtr)num];
				ptr4 = stackalloc byte[(UIntPtr)num];
			}
			Util.GetUtf8(input, ptr3, num);
			uint num2 = (uint)(num - byteCount3);
			Unsafe.InitBlockUnaligned((void*)(ptr3 + byteCount3), 0, num2);
			Unsafe.CopyBlock((void*)ptr4, (void*)ptr3, (uint)num);
			byte b = ImGuiNative.igInputTextWithHint(ptr, ptr2, ptr3, (uint)num, flags, callback, user_data.ToPointer());
			if (!Util.AreStringsEqual(ptr4, num, ptr3))
			{
				input = Util.StringFromPtr(ptr3);
			}
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			if (byteCount2 > 2048)
			{
				Util.Free(ptr2);
			}
			if (num > 2048)
			{
				Util.Free(ptr3);
				Util.Free(ptr4);
			}
			return b > 0;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00013AFC File Offset: 0x00011CFC
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text)
		{
			return ImGui.CalcTextSizeImpl(text, 0, default(int?), false, -1f);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00013B20 File Offset: 0x00011D20
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start)
		{
			return ImGui.CalcTextSizeImpl(text, start, default(int?), false, -1f);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00013B44 File Offset: 0x00011D44
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, float wrapWidth)
		{
			return ImGui.CalcTextSizeImpl(text, 0, default(int?), false, wrapWidth);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00013B68 File Offset: 0x00011D68
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, bool hideTextAfterDoubleHash)
		{
			return ImGui.CalcTextSizeImpl(text, 0, default(int?), hideTextAfterDoubleHash, -1f);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00013B8D File Offset: 0x00011D8D
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start, int length)
		{
			return ImGui.CalcTextSizeImpl(text, start, new int?(length), false, -1f);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00013BA4 File Offset: 0x00011DA4
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start, bool hideTextAfterDoubleHash)
		{
			return ImGui.CalcTextSizeImpl(text, start, default(int?), hideTextAfterDoubleHash, -1f);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00013BCC File Offset: 0x00011DCC
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start, float wrapWidth)
		{
			return ImGui.CalcTextSizeImpl(text, start, default(int?), false, wrapWidth);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00013BF0 File Offset: 0x00011DF0
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, bool hideTextAfterDoubleHash, float wrapWidth)
		{
			return ImGui.CalcTextSizeImpl(text, 0, default(int?), hideTextAfterDoubleHash, wrapWidth);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00013C13 File Offset: 0x00011E13
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start, int length, bool hideTextAfterDoubleHash)
		{
			return ImGui.CalcTextSizeImpl(text, start, new int?(length), hideTextAfterDoubleHash, -1f);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00013C28 File Offset: 0x00011E28
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start, int length, float wrapWidth)
		{
			return ImGui.CalcTextSizeImpl(text, start, new int?(length), false, wrapWidth);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00013C39 File Offset: 0x00011E39
		public static Vector2 CalcTextSize(ReadOnlySpan<char> text, int start, int length, bool hideTextAfterDoubleHash, float wrapWidth)
		{
			return ImGui.CalcTextSizeImpl(text, start, new int?(length), hideTextAfterDoubleHash, wrapWidth);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00013C4C File Offset: 0x00011E4C
		private unsafe static Vector2 CalcTextSizeImpl(ReadOnlySpan<char> text, int start = 0, int? length = null, bool hideTextAfterDoubleHash = false, float wrapWidth = -1f)
		{
			byte* ptr = null;
			byte* text_end = null;
			int num = 0;
			if (text != null)
			{
				int length2 = (length != null) ? length.Value : text.Length;
				num = Util.CalcSizeInUtf8(text, start, length2);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(text, start, length2, ptr, num);
				ptr[utf] = 0;
				text_end = ptr + utf;
			}
			Vector2 result;
			ImGuiNative.igCalcTextSize(&result, ptr, text_end, (*(&hideTextAfterDoubleHash)) ? 1 : 0, wrapWidth);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return result;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00013CE5 File Offset: 0x00011EE5
		public static bool InputText(ReadOnlySpan<char> label, IntPtr buf, uint buf_size)
		{
			return ImGui.InputText(label, buf, buf_size, ImGuiInputTextFlags.None, null, IntPtr.Zero);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00013CF6 File Offset: 0x00011EF6
		public static bool InputText(ReadOnlySpan<char> label, IntPtr buf, uint buf_size, ImGuiInputTextFlags flags)
		{
			return ImGui.InputText(label, buf, buf_size, flags, null, IntPtr.Zero);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00013D07 File Offset: 0x00011F07
		public static bool InputText(ReadOnlySpan<char> label, IntPtr buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
		{
			return ImGui.InputText(label, buf, buf_size, flags, callback, IntPtr.Zero);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00013D1C File Offset: 0x00011F1C
		public unsafe static bool InputText(ReadOnlySpan<char> label, IntPtr buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, IntPtr user_data)
		{
			int byteCount = Encoding.UTF8.GetByteCount(label);
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			Util.GetUtf8(label, ptr, byteCount);
			bool result = ImGuiNative.igInputText(ptr, (byte*)buf.ToPointer(), buf_size, flags, callback, user_data.ToPointer()) > 0;
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			return result;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00013D84 File Offset: 0x00011F84
		public unsafe static bool Begin(ReadOnlySpan<char> name, ImGuiWindowFlags flags)
		{
			int byteCount = Encoding.UTF8.GetByteCount(name);
			byte* ptr;
			if (byteCount > 2048)
			{
				ptr = Util.Allocate(byteCount + 1);
			}
			else
			{
				ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			}
			Util.GetUtf8(name, ptr, byteCount);
			byte* p_open = null;
			int num = (int)ImGuiNative.igBegin(ptr, p_open, flags);
			if (byteCount > 2048)
			{
				Util.Free(ptr);
			}
			return num != 0;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00013DDC File Offset: 0x00011FDC
		public static bool MenuItem(ReadOnlySpan<char> label, bool enabled)
		{
			return ImGui.MenuItem(label, string.Empty, false, enabled);
		}
	}
}
