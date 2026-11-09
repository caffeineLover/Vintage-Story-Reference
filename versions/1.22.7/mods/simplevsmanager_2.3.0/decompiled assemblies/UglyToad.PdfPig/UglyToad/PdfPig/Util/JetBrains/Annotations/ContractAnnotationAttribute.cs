using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000040 RID: 64
	[AttributeUsage(64, AllowMultiple = true)]
	internal sealed class ContractAnnotationAttribute : Attribute
	{
		// Token: 0x0600016E RID: 366 RVA: 0x0000E025 File Offset: 0x0000C225
		public ContractAnnotationAttribute([NotNull] string contract) : this(contract, false)
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000E02F File Offset: 0x0000C22F
		public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
		{
			this.Contract = contract;
			this.ForceFullStates = forceFullStates;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000E045 File Offset: 0x0000C245
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000E04D File Offset: 0x0000C24D
		[NotNull]
		public string Contract { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000E056 File Offset: 0x0000C256
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000E05E File Offset: 0x0000C25E
		public bool ForceFullStates { get; private set; }
	}
}
