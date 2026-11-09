using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000038 RID: 56
	internal sealed class CriticalHitFeedbackRenderer : IRenderer, IDisposable
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00011503 File Offset: 0x0000F703
		public double RenderOrder
		{
			get
			{
				return 1.02;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001150E File Offset: 0x0000F70E
		public int RenderRange
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00011515 File Offset: 0x0000F715
		[NullableContext(1)]
		public CriticalHitFeedbackRenderer(ICoreClientAPI api)
		{
			this._api = api;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001152F File Offset: 0x0000F72F
		public void Pulse(float intensity)
		{
			this._elapsedSeconds = 0f;
			this._intensity = intensity;
			this._active = true;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001154C File Offset: 0x0000F74C
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (!this._active)
			{
				return;
			}
			this._elapsedSeconds += deltaTime;
			if (this._elapsedSeconds >= 0.4f)
			{
				this._active = false;
				return;
			}
			float progress = Math.Clamp(this._elapsedSeconds / 0.4f, 0f, 1f);
			float fade = 1f - progress;
			float pulse = MathF.Sin(progress * 3.1415927f);
			float scale = RuntimeEnv.GUIScale * (1f + 0.25f * pulse * this._intensity);
			float centerX = (float)this._api.Render.FrameWidth / 2f;
			float centerY = (float)this._api.Render.FrameHeight / 2f;
			float halfSize = 14f * scale;
			float thickness = MathF.Max(3f, 4.5f * scale);
			int glowColor = ColorUtil.ColorFromRgba(255, 136, 0, (int)(115f * fade));
			int goldColor = ColorUtil.ColorFromRgba(255, 220, 58, (int)(255f * fade));
			int highlightColor = ColorUtil.ColorFromRgba(255, 248, 170, (int)(180f * fade));
			this.DrawX(centerX, centerY, halfSize + 3f * scale, thickness + 4f * scale, 10049f, glowColor);
			this.DrawX(centerX, centerY, halfSize, thickness, 10050f, goldColor);
			this.DrawX(centerX, centerY, halfSize * 0.58f, MathF.Max(2f, thickness * 0.45f), 10051f, highlightColor);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000116D8 File Offset: 0x0000F8D8
		private void DrawX(float centerX, float centerY, float halfSize, float thickness, float z, int color)
		{
			int steps = Math.Max(7, (int)MathF.Ceiling(halfSize * 2f / MathF.Max(1f, thickness * 0.75f)));
			float spacing = halfSize * 2f / (float)(steps - 1);
			for (int index = 0; index < steps; index++)
			{
				float offset = -halfSize + spacing * (float)index;
				this.DrawSquare(centerX + offset, centerY + offset, thickness, z, color);
				this.DrawSquare(centerX + offset, centerY - offset, thickness, z, color);
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00011757 File Offset: 0x0000F957
		private void DrawSquare(float centerX, float centerY, float size, float z, int color)
		{
			this._api.Render.RenderRectangle(centerX - size / 2f, centerY - size / 2f, z, size, size, color);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00011781 File Offset: 0x0000F981
		public void Dispose()
		{
		}

		// Token: 0x04000155 RID: 341
		private const float PulseDurationSeconds = 0.4f;

		// Token: 0x04000156 RID: 342
		[Nullable(1)]
		private readonly ICoreClientAPI _api;

		// Token: 0x04000157 RID: 343
		private float _elapsedSeconds;

		// Token: 0x04000158 RID: 344
		private float _intensity = 1f;

		// Token: 0x04000159 RID: 345
		private bool _active;
	}
}
