using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000FA RID: 250
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class MeleeSystemClient : MeleeSystem
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000CAB RID: 3243 RVA: 0x0003EDEC File Offset: 0x0003CFEC
		// (remove) Token: 0x06000CAC RID: 3244 RVA: 0x0003EE24 File Offset: 0x0003D024
		public event MeleeSystemClient.MeleeAttackDelegate OnMeleeAttackStart;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000CAD RID: 3245 RVA: 0x0003EE5C File Offset: 0x0003D05C
		// (remove) Token: 0x06000CAE RID: 3246 RVA: 0x0003EE94 File Offset: 0x0003D094
		public event MeleeSystemClient.MeleeAttackDelegate OnMeleeAttackEnd;

		// Token: 0x06000CAF RID: 3247 RVA: 0x0003EEC9 File Offset: 0x0003D0C9
		[NullableContext(1)]
		public MeleeSystemClient(ICoreClientAPI api)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<MeleeAttackPacket>().RegisterMessageType<MeleePushPacket>().RegisterMessageType<MeleeAttackStatusPacket>();
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0003EEF8 File Offset: 0x0003D0F8
		[NullableContext(1)]
		public void SendPackets(IEnumerable<MeleeDamagePacket> packets)
		{
			this._clientChannel.SendPacket<MeleeAttackPacket>(new MeleeAttackPacket
			{
				MeleeAttackDamagePackets = ((packets as MeleeDamagePacket[]) ?? Enumerable.ToArray<MeleeDamagePacket>(packets))
			});
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003EF30 File Offset: 0x0003D130
		[NullableContext(1)]
		public void SendPackets(IEnumerable<MeleeCollisionPacket> packets)
		{
			this._clientChannel.SendPacket<MeleePushPacket>(new MeleePushPacket
			{
				MeleeAttackDamagePackets = ((packets as MeleeCollisionPacket[]) ?? Enumerable.ToArray<MeleeCollisionPacket>(packets))
			});
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0003EF68 File Offset: 0x0003D168
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

		// Token: 0x0400062F RID: 1583
		[Nullable(1)]
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x02000254 RID: 596
		// (Invoke) Token: 0x0600169B RID: 5787
		[NullableContext(0)]
		public delegate void MeleeAttackDelegate(Entity attacker, [Nullable(2)] ItemSlot slot);
	}
}
