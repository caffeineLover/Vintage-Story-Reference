using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200016F RID: 367
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ParticlesFrameJson
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x00059D91 File Offset: 0x00057F91
		// (set) Token: 0x06001278 RID: 4728 RVA: 0x00059D99 File Offset: 0x00057F99
		public string Code { get; set; } = "";

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x00059DA2 File Offset: 0x00057FA2
		// (set) Token: 0x0600127A RID: 4730 RVA: 0x00059DAA File Offset: 0x00057FAA
		public float DurationFraction { get; set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00059DB3 File Offset: 0x00057FB3
		// (set) Token: 0x0600127C RID: 4732 RVA: 0x00059DBB File Offset: 0x00057FBB
		public float[] Position { get; set; } = new float[3];

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00059DC4 File Offset: 0x00057FC4
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x00059DCC File Offset: 0x00057FCC
		public float[] Velocity { get; set; } = new float[3];

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00059DD5 File Offset: 0x00057FD5
		// (set) Token: 0x06001280 RID: 4736 RVA: 0x00059DDD File Offset: 0x00057FDD
		public float Intensity { get; set; }

		// Token: 0x06001281 RID: 4737 RVA: 0x00059DE8 File Offset: 0x00057FE8
		public ParticlesFrame ToParticlesFrame()
		{
			return new ParticlesFrame(this.Code, this.DurationFraction, new Vector3(this.Position[0], this.Position[1], this.Position[2]), new Vector3(this.Velocity[0], this.Velocity[1], this.Velocity[2]), this.Intensity);
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00059E48 File Offset: 0x00058048
		public static ParticlesFrameJson FromParticlesFrame(ParticlesFrame frame)
		{
			return new ParticlesFrameJson
			{
				Code = frame.Code,
				DurationFraction = frame.DurationFraction,
				Position = new float[]
				{
					frame.Position.X,
					frame.Position.Y,
					frame.Position.Z
				},
				Velocity = new float[]
				{
					frame.Velocity.X,
					frame.Velocity.Y,
					frame.Velocity.Z
				},
				Intensity = frame.Intensity
			};
		}
	}
}
