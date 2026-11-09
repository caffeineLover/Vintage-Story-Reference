using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace VSImGui
{
	// Token: 0x02000008 RID: 8
	internal sealed class OffWindowRenderer : IRenderer, IDisposable
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00002D3C File Offset: 0x00000F3C
		[NullableContext(1)]
		public OffWindowRenderer(VSImGuiDialog dialog)
		{
			this._dialog = dialog;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002D4B File Offset: 0x00000F4B
		public double RenderOrder
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002D56 File Offset: 0x00000F56
		public int RenderRange
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D59 File Offset: 0x00000F59
		public void Dispose()
		{
			this._disposed = true;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D64 File Offset: 0x00000F64
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (!this._disposed && stage == 10)
			{
				try
				{
					this._dialog.Update(deltaTime);
					this._dialog.RenderOffWindow(deltaTime);
				}
				catch
				{
				}
			}
		}

		// Token: 0x04000016 RID: 22
		[Nullable(1)]
		private readonly VSImGuiDialog _dialog;

		// Token: 0x04000017 RID: 23
		private bool _disposed;
	}
}
