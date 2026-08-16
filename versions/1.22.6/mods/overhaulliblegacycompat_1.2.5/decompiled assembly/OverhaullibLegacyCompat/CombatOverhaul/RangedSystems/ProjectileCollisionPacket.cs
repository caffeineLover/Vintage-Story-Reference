using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C5 RID: 197
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ProjectileCollisionPacket
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0003647E File Offset: 0x0003467E
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x00036486 File Offset: 0x00034686
		public Guid Id { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0003648F File Offset: 0x0003468F
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x00036497 File Offset: 0x00034697
		public double[] CollisionPoint { get; set; } = Array.Empty<double>();

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x000364A0 File Offset: 0x000346A0
		// (set) Token: 0x06000A45 RID: 2629 RVA: 0x000364A8 File Offset: 0x000346A8
		public double[] AfterCollisionVelocity { get; set; } = Array.Empty<double>();

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x000364B1 File Offset: 0x000346B1
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x000364B9 File Offset: 0x000346B9
		public double RelativeSpeed { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x000364C2 File Offset: 0x000346C2
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x000364CA File Offset: 0x000346CA
		public string Collider { get; set; } = "";

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x000364D3 File Offset: 0x000346D3
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x000364DB File Offset: 0x000346DB
		public long ReceiverEntity { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x000364E4 File Offset: 0x000346E4
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x000364EC File Offset: 0x000346EC
		public int PacketVersion { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x000364F5 File Offset: 0x000346F5
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x000364FD File Offset: 0x000346FD
		public float PenetrationStrengthLoss { get; set; }
	}
}
