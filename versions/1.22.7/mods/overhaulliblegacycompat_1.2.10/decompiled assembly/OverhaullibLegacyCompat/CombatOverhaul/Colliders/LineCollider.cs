using System;
using OpenTK.Mathematics;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200016C RID: 364
	public readonly struct LineCollider
	{
		// Token: 0x0600122E RID: 4654 RVA: 0x0005794C File Offset: 0x00055B4C
		public LineCollider(Vector3d tail, Vector3d head)
		{
			this.Tail = tail;
			this.Head = head;
		}

		// Token: 0x0400088B RID: 2187
		public readonly Vector3d Tail;

		// Token: 0x0400088C RID: 2188
		public readonly Vector3d Head;
	}
}
