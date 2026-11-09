using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000016 RID: 22
	internal sealed class SkPicture : IDisposable
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003124 File Offset: 0x00001324
		// (set) Token: 0x06000088 RID: 136 RVA: 0x0000312C File Offset: 0x0000132C
		public IntPtr Instance { get; private set; }

		// Token: 0x06000089 RID: 137 RVA: 0x00003135 File Offset: 0x00001335
		public SkPicture(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000314F File Offset: 0x0000134F
		[NullableContext(1)]
		public static SkPicture Deserialize(SkData data)
		{
			return new SkPicture(SkPicture.API.picture_deserialize(data.Instance));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003161 File Offset: 0x00001361
		[NullableContext(1)]
		public SkData Serialize()
		{
			return new SkData(SkPicture.API.picture_serialize(this.Instance));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003174 File Offset: 0x00001374
		~SkPicture()
		{
			this.WarnThatFinalizerIsReached<SkPicture>();
			this.Dispose();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000031A8 File Offset: 0x000013A8
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkPicture.API.picture_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0200013D RID: 317
		private static class API
		{
			// Token: 0x06000B13 RID: 2835
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void picture_unref(IntPtr picture);

			// Token: 0x06000B14 RID: 2836
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr picture_serialize(IntPtr picture);

			// Token: 0x06000B15 RID: 2837
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr picture_deserialize(IntPtr data);
		}
	}
}
