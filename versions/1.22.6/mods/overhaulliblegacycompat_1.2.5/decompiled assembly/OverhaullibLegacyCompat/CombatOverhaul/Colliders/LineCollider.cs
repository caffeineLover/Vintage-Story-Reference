using System;
using OpenTK.Mathematics;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000160 RID: 352
	public readonly struct LineCollider
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x0005602C File Offset: 0x0005422C
		public LineCollider(Vector3d tail, Vector3d head)
		{
			this.Tail = tail;
			this.Head = head;
		}

		// Token: 0x04000865 RID: 2149
		public readonly Vector3d Tail;

		// Token: 0x04000866 RID: 2150
		public readonly Vector3d Head;
	}
}
