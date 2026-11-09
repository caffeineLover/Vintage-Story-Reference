using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017B RID: 379
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ParticlesFrameJson
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x0005B6B1 File Offset: 0x000598B1
		// (set) Token: 0x060012EC RID: 4844 RVA: 0x0005B6B9 File Offset: 0x000598B9
		public string Code { get; set; } = "";

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x0005B6C2 File Offset: 0x000598C2
		// (set) Token: 0x060012EE RID: 4846 RVA: 0x0005B6CA File Offset: 0x000598CA
		public float DurationFraction { get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0005B6D3 File Offset: 0x000598D3
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x0005B6DB File Offset: 0x000598DB
		public float[] Position { get; set; } = new float[3];

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0005B6E4 File Offset: 0x000598E4
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x0005B6EC File Offset: 0x000598EC
		public float[] Velocity { get; set; } = new float[3];

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0005B6F5 File Offset: 0x000598F5
		// (set) Token: 0x060012F4 RID: 4852 RVA: 0x0005B6FD File Offset: 0x000598FD
		public float Intensity { get; set; }

		// Token: 0x060012F5 RID: 4853 RVA: 0x0005B708 File Offset: 0x00059908
		public ParticlesFrame ToParticlesFrame()
		{
			return new ParticlesFrame(this.Code, this.DurationFraction, new Vector3(this.Position[0], this.Position[1], this.Position[2]), new Vector3(this.Velocity[0], this.Velocity[1], this.Velocity[2]), this.Intensity);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0005B768 File Offset: 0x00059968
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
