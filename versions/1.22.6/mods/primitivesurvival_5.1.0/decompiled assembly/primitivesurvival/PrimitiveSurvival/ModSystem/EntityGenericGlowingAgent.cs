using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000038 RID: 56
	public class EntityGenericGlowingAgent : EntityAgent
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0001C327 File Offset: 0x0001A527
		public override byte[] LightHsv
		{
			get
			{
				return this.lightHsv;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0001C330 File Offset: 0x0001A530
		public override void Initialize(EntityProperties properties, ICoreAPI api, long inChunkIndex3d)
		{
			base.Initialize(properties, api, inChunkIndex3d);
			float d = (float)(api.World.Rand.Next(0, 3) / 10);
			this.offset = base.Pos.X + base.Pos.Y + base.Pos.Z;
			this.strobeFrequency = properties.Attributes["strobeFrequency"].AsFloat(0f);
			this.strobeFrequencyWithOffset = this.strobeFrequency + d;
			this.minLightLevel = properties.Attributes["minLightLevel"].AsFloat(0f);
			this.lightHsv = properties.Attributes["lightHsv"].AsObject<byte[]>(null);
			if (this.lightHsv == null)
			{
				this.lightHsv = new byte[]
				{
					1,
					default(byte),
					4
				};
			}
			this.minLightValue = (float)this.lightHsv[2];
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0001C41C File Offset: 0x0001A61C
		public override string GetInfoText()
		{
			string result = base.GetInfoText();
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				result = result + "\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>\n\n";
			}
			return result;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0001C460 File Offset: 0x0001A660
		public override void OnGameTick(float dt)
		{
			base.OnGameTick(dt);
			if (this.strobeFrequency <= 0f)
			{
				return;
			}
			this.tmp.Set(base.Pos.X, base.Pos.Y + (double)this.SelectionBox.Y1 + (double)(this.SelectionBox.Y2 / 2f), base.Pos.Z);
			Entity[] entities = this.World.GetEntitiesAround(this.tmp, 5f, 5f, null);
			if (entities.Length <= 1 && base.FirstCodePart(0) == "fireflies")
			{
				return;
			}
			if (this.strobeFrequency > 0f)
			{
				float d = 0f;
				if (entities.Length != 0)
				{
					d = (float)(this.Api.World.Rand.Next(0, entities.Length) / 10);
				}
				this.time += dt * this.strobeFrequencyWithOffset - d;
				float timeoff = (float)Math.Abs(Math.Sin((double)this.time + this.offset));
				this.lightHsv[2] = (byte)(timeoff * this.minLightValue + this.minLightLevel);
			}
		}

		// Token: 0x04000108 RID: 264
		private float time;

		// Token: 0x04000109 RID: 265
		private double offset;

		// Token: 0x0400010A RID: 266
		private float strobeFrequency;

		// Token: 0x0400010B RID: 267
		private float minLightLevel;

		// Token: 0x0400010C RID: 268
		private float minLightValue;

		// Token: 0x0400010D RID: 269
		private byte[] lightHsv;

		// Token: 0x0400010E RID: 270
		private float strobeFrequencyWithOffset;

		// Token: 0x0400010F RID: 271
		private Vec3d tmp = new Vec3d();
	}
}
