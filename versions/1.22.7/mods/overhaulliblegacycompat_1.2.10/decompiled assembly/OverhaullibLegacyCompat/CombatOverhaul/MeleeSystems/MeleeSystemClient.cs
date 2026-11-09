using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x02000105 RID: 261
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MeleeSystemClient : MeleeSystem
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000D07 RID: 3335 RVA: 0x0003FDDC File Offset: 0x0003DFDC
		// (remove) Token: 0x06000D08 RID: 3336 RVA: 0x0003FE14 File Offset: 0x0003E014
		public event MeleeSystemClient.MeleeAttackDelegate OnMeleeAttackStart;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000D09 RID: 3337 RVA: 0x0003FE4C File Offset: 0x0003E04C
		// (remove) Token: 0x06000D0A RID: 3338 RVA: 0x0003FE84 File Offset: 0x0003E084
		public event MeleeSystemClient.MeleeAttackDelegate OnMeleeAttackEnd;

		// Token: 0x06000D0B RID: 3339 RVA: 0x0003FEB9 File Offset: 0x0003E0B9
		[NullableContext(1)]
		public MeleeSystemClient(ICoreClientAPI api)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<MeleeAttackPacket>().RegisterMessageType<MeleePushPacket>().RegisterMessageType<MeleeAttackStatusPacket>();
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0003FEE8 File Offset: 0x0003E0E8
		[NullableContext(1)]
		public void SendPackets(IEnumerable<MeleeDamagePacket> packets)
		{
			this._clientChannel.SendPacket<MeleeAttackPacket>(new MeleeAttackPacket
			{
				MeleeAttackDamagePackets = ((packets as MeleeDamagePacket[]) ?? Enumerable.ToArray<MeleeDamagePacket>(packets))
			});
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0003FF20 File Offset: 0x0003E120
		[NullableContext(1)]
		public void SendPackets(IEnumerable<MeleeCollisionPacket> packets)
		{
			this._clientChannel.SendPacket<MeleePushPacket>(new MeleePushPacket
			{
				MeleeAttackDamagePackets = ((packets as MeleeCollisionPacket[]) ?? Enumerable.ToArray<MeleeCollisionPacket>(packets))
			});
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0003FF58 File Offset: 0x0003E158
		[NullableContext(1)]
		public void UpdateAttackStatus(EntityPlayer attacker, MeleeAttackStatus status, bool mainHand)
		{
			this._clientChannel.SendPacket<MeleeAttackStatusPacket>(new MeleeAttackStatusPacket
			{
				Status = status,
				MainHand = mainHand
			});
			ItemSlot weaponSlot = mainHand ? attacker.ActiveHandItemSlot : attacker.LeftHandItemSlot;
			if (status != MeleeAttackStatus.Start)
			{
				if (status != MeleeAttackStatus.End)
				{
					return;
				}
				MeleeSystemClient.MeleeAttackDelegate onMeleeAttackEnd = this.OnMeleeAttackEnd;
				if (onMeleeAttackEnd == null)
				{
					return;
				}
				onMeleeAttackEnd(attacker, weaponSlot);
				return;
			}
			else
			{
				MeleeSystemClient.MeleeAttackDelegate onMeleeAttackStart = this.OnMeleeAttackStart;
				if (onMeleeAttackStart == null)
				{
					return;
				}
				onMeleeAttackStart(attacker, weaponSlot);
				return;
			}
		}

		// Token: 0x04000651 RID: 1617
		[Nullable(1)]
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x02000263 RID: 611
		// (Invoke) Token: 0x06001716 RID: 5910
		[NullableContext(0)]
		public delegate void MeleeAttackDelegate(Entity attacker, [Nullable(2)] ItemSlot slot);
	}
}
