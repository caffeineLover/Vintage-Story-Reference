using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F3 RID: 243
	public readonly struct ImpaleKey : IEquatable<ImpaleKey>
	{
		// Token: 0x06000C49 RID: 3145 RVA: 0x0003D822 File Offset: 0x0003BA22
		public ImpaleKey(long attackerEntityId, bool mainHand)
		{
			this.AttackerEntityId = attackerEntityId;
			this.MainHand = mainHand;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003D832 File Offset: 0x0003BA32
		public bool Equals(ImpaleKey other)
		{
			return this.AttackerEntityId == other.AttackerEntityId && this.MainHand == other.MainHand;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0003D854 File Offset: 0x0003BA54
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

		// Token: 0x06000C4C RID: 3148 RVA: 0x0003D879 File Offset: 0x0003BA79
		public override int GetHashCode()
		{
			return HashCode.Combine<long, bool>(this.AttackerEntityId, this.MainHand);
		}

		// Token: 0x040005E5 RID: 1509
		public readonly long AttackerEntityId;

		// Token: 0x040005E6 RID: 1510
		public readonly bool MainHand;
	}
}
