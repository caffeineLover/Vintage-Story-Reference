using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace QuestPDF.Skia
{
	// Token: 0x02000015 RID: 21
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class SkPdfTag : IDisposable
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002E0B File Offset: 0x0000100B
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002E13 File Offset: 0x00001013
		public IntPtr Instance { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002E1C File Offset: 0x0000101C
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002E24 File Offset: 0x00001024
		public int NodeId { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002E2D File Offset: 0x0000102D
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002E35 File Offset: 0x00001035
		[Nullable(1)]
		public string Type { [NullableContext(1)] get; [NullableContext(1)] set; } = "";

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002E3E File Offset: 0x0000103E
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002E46 File Offset: 0x00001046
		public string Alt { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002E4F File Offset: 0x0000104F
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002E57 File Offset: 0x00001057
		public string Lang { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002E60 File Offset: 0x00001060
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002E68 File Offset: 0x00001068
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ICollection<SkPdfTag> Children { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x06000080 RID: 128 RVA: 0x00002E71 File Offset: 0x00001071
		private SkPdfTag(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002E96 File Offset: 0x00001096
		[return: Nullable(1)]
		public static SkPdfTag Create(int nodeId, string type, string alt, string lang)
		{
			return new SkPdfTag(SkPdfTag.API.pdf_structure_element_create(nodeId, type, alt, lang))
			{
				NodeId = nodeId,
				Type = (type ?? ""),
				Alt = alt,
				Lang = lang
			};
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002ECC File Offset: 0x000010CC
		[NullableContext(1)]
		public void SetChildren(ICollection<SkPdfTag> children)
		{
			this.Children = children;
			IntPtr[] array = Enumerable.ToArray<IntPtr>(Enumerable.Select<SkPdfTag, IntPtr>(Enumerable.ToArray<SkPdfTag>(children), (SkPdfTag c) => c.Instance));
			IntPtr intPtr = Marshal.AllocHGlobal(IntPtr.Size * array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			SkPdfTag.API.pdf_structure_element_set_children(this.Instance, intPtr, array.Length);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002F40 File Offset: 0x00001140
		[NullableContext(1)]
		public void AddAttribute(string owner, string name, object value)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(owner + "\0");
			byte[] bytes2 = Encoding.ASCII.GetBytes(name + "\0");
			string text = value as string;
			if (text != null)
			{
				byte[] bytes3 = Encoding.ASCII.GetBytes(text + "\0");
				SkPdfTag.API.pdf_structure_element_add_attribute_text(this.Instance, bytes, bytes2, bytes3);
				return;
			}
			if (value is int)
			{
				int value2 = (int)value;
				SkPdfTag.API.pdf_structure_element_add_attribute_integer(this.Instance, bytes, bytes2, value2);
				return;
			}
			if (value is float)
			{
				float value3 = (float)value;
				SkPdfTag.API.pdf_structure_element_add_attribute_float(this.Instance, bytes, bytes2, value3);
				return;
			}
			float[] array = value as float[];
			if (array != null)
			{
				SkPdfTag.API.pdf_structure_element_add_attribute_float_array(this.Instance, bytes, bytes2, array, array.Length);
				return;
			}
			int[] array2 = value as int[];
			if (array2 != null)
			{
				SkPdfTag.API.pdf_structure_element_add_attribute_node_ids(this.Instance, bytes, bytes2, array2, array2.Length);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unsupported attribute value type: ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(value.GetType());
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003058 File Offset: 0x00001258
		~SkPdfTag()
		{
			this.WarnThatFinalizerIsReached<SkPdfTag>();
			this.Dispose();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000308C File Offset: 0x0000128C
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkPdfTag.<Dispose>g__DisposeChildren|29_0(this);
			SkPdfTag.API.pdf_structure_element_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000030C0 File Offset: 0x000012C0
		[NullableContext(1)]
		[CompilerGenerated]
		internal static void <Dispose>g__DisposeChildren|29_0(SkPdfTag parent)
		{
			if (parent.Children == null)
			{
				return;
			}
			foreach (SkPdfTag skPdfTag in parent.Children)
			{
				skPdfTag.Instance = IntPtr.Zero;
				GC.SuppressFinalize(skPdfTag);
				SkPdfTag.<Dispose>g__DisposeChildren|29_0(skPdfTag);
			}
		}

		// Token: 0x0200013B RID: 315
		[NullableContext(1)]
		[Nullable(0)]
		private static class API
		{
			// Token: 0x06000B08 RID: 2824
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr pdf_structure_element_create(int nodeId, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string type, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string alt, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string lang);

			// Token: 0x06000B09 RID: 2825
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_set_children(IntPtr element, IntPtr children, int count);

			// Token: 0x06000B0A RID: 2826
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_add_attribute_text(IntPtr element, byte[] owner, byte[] name, byte[] value);

			// Token: 0x06000B0B RID: 2827
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_add_attribute_integer(IntPtr element, byte[] owner, byte[] name, int value);

			// Token: 0x06000B0C RID: 2828
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_add_attribute_float(IntPtr element, byte[] owner, byte[] name, float value);

			// Token: 0x06000B0D RID: 2829
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_add_attribute_float_array(IntPtr element, byte[] owner, byte[] name, float[] array, int arrayLength);

			// Token: 0x06000B0E RID: 2830
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_add_attribute_node_ids(IntPtr element, byte[] owner, byte[] name, int[] array, int arrayLength);

			// Token: 0x06000B0F RID: 2831
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void pdf_structure_element_delete(IntPtr element);
		}
	}
}
