using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000017 RID: 23
	internal sealed class SkPictureRecorder : IDisposable
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000031D4 File Offset: 0x000013D4
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000031DC File Offset: 0x000013DC
		public IntPtr Instance { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000031E5 File Offset: 0x000013E5
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000031ED File Offset: 0x000013ED
		private bool IsRecording { get; set; }

		// Token: 0x06000092 RID: 146 RVA: 0x000031F6 File Offset: 0x000013F6
		public SkPictureRecorder()
		{
			this.Instance = SkPictureRecorder.API.picture_recorder_create();
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003214 File Offset: 0x00001414
		[NullableContext(1)]
		public SkCanvas BeginRecording(float width, float height)
		{
			IntPtr instance = SkPictureRecorder.API.picture_recorder_begin_recording(this.Instance, width, height);
			this.IsRecording = true;
			return new SkCanvas(instance, false);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003230 File Offset: 0x00001430
		[NullableContext(1)]
		public SkPicture EndRecording()
		{
			IntPtr instance = SkPictureRecorder.API.picture_recorder_end_recording(this.Instance);
			this.IsRecording = false;
			return new SkPicture(instance);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000324C File Offset: 0x0000144C
		~SkPictureRecorder()
		{
			this.WarnThatFinalizerIsReached<SkPictureRecorder>();
			this.Dispose();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003280 File Offset: 0x00001480
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			if (this.IsRecording)
			{
				try
				{
					this.EndRecording().Dispose();
				}
				catch
				{
				}
			}
			SkPictureRecorder.API.picture_recorder_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0200013E RID: 318
		private static class API
		{
			// Token: 0x06000B16 RID: 2838
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr picture_recorder_create();

			// Token: 0x06000B17 RID: 2839
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr picture_recorder_begin_recording(IntPtr pictureRecorder, float width, float height);

			// Token: 0x06000B18 RID: 2840
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr picture_recorder_end_recording(IntPtr pictureRecorder);

			// Token: 0x06000B19 RID: 2841
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void picture_recorder_delete(IntPtr pictureRecorder);
		}
	}
}
