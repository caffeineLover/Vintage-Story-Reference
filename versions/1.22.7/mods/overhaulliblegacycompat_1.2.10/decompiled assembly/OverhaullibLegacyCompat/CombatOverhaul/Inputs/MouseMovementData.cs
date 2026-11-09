using System;

namespace CombatOverhaul.Inputs
{
	// Token: 0x0200006E RID: 110
	public readonly struct MouseMovementData
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001E7D2 File Offset: 0x0001C9D2
		public float Pitch { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001E7DA File Offset: 0x0001C9DA
		public float Yaw { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001E7E2 File Offset: 0x0001C9E2
		public float DeltaPitch { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001E7EA File Offset: 0x0001C9EA
		public float DeltaYaw { get; }

		// Token: 0x06000531 RID: 1329 RVA: 0x0001E7F2 File Offset: 0x0001C9F2
		public MouseMovementData(float pitch, float yaw, float deltaPitch, float deltaYaw)
		{
			this.Pitch = pitch;
			this.Yaw = yaw;
			this.DeltaPitch = deltaPitch;
			this.DeltaYaw = deltaYaw;
		}
	}
}
