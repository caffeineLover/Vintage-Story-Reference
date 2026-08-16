using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000E8 RID: 232
	public readonly struct ImpaleKey : IEquatable<ImpaleKey>
	{
		// Token: 0x06000BED RID: 3053 RVA: 0x0003C832 File Offset: 0x0003AA32
		public ImpaleKey(long attackerEntityId, bool mainHand)
		{
			this.AttackerEntityId = attackerEntityId;
			this.MainHand = mainHand;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0003C842 File Offset: 0x0003AA42
		public bool Equals(ImpaleKey other)
		{
			return this.AttackerEntityId == other.AttackerEntityId && this.MainHand == other.MainHand;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0003C864 File Offset: 0x0003AA64
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is ImpaleKey)
			{
				ImpaleKey other = (ImpaleKey)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0003C889 File Offset: 0x0003AA89
		public override int GetHashCode()
		{
			return HashCode.Combine<long, bool>(this.AttackerEntityId, this.MainHand);
		}

		// Token: 0x040005C3 RID: 1475
		public readonly long AttackerEntityId;

		// Token: 0x040005C4 RID: 1476
		public readonly bool MainHand;
	}
}
