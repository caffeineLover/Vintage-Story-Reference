using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct SplitMaterialInfo
	{
		// Token: 0x0600030D RID: 781 RVA: 0x0000DF96 File Offset: 0x0000C196
		public SplitMaterialInfo(string bodyMaterial, string bladeMaterial)
		{
			this.BodyMaterial = bodyMaterial;
			this.BladeMaterial = bladeMaterial;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000DFA6 File Offset: 0x0000C1A6
		public string BodyMaterial { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000DFAE File Offset: 0x0000C1AE
		public string BladeMaterial { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000DFB6 File Offset: 0x0000C1B6
		public bool IsMixed
		{
			get
			{
				return !this.BodyMaterial.Equals(this.BladeMaterial, 5);
			}
		}
	}
}
