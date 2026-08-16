using System;

namespace CombatOverhaul.Inputs
{
	// Token: 0x0200006E RID: 110
	public readonly struct MouseMovementData
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001E79A File Offset: 0x0001C99A
		public float Pitch { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001E7A2 File Offset: 0x0001C9A2
		public float Yaw { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001E7AA File Offset: 0x0001C9AA
		public float DeltaPitch { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001E7B2 File Offset: 0x0001C9B2
		public float DeltaYaw { get; }

		// Token: 0x06000531 RID: 1329 RVA: 0x0001E7BA File Offset: 0x0001C9BA
		public MouseMovementData(float pitch, float yaw, float deltaPitch, float deltaYaw)
		{
			this.Pitch = pitch;
			this.Yaw = yaw;
			this.DeltaPitch = deltaPitch;
			this.DeltaYaw = deltaYaw;
		}
	}
}
