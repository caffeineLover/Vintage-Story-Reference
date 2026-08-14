using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000007 RID: 7
	public struct IndirectReference
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000022DD File Offset: 0x000004DD
		public readonly long ObjectNumber { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000022E5 File Offset: 0x000004E5
		public readonly int Generation { get; }

		// Token: 0x0600001A RID: 26 RVA: 0x000022ED File Offset: 0x000004ED
		[DebuggerStepThrough]
		public IndirectReference(long objectNumber, int generation)
		{
			this.ObjectNumber = objectNumber;
			this.Generation = generation;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002300 File Offset: 0x00000500
		public override bool Equals(object obj)
		{
			if (obj is IndirectReference)
			{
				IndirectReference indirectReference = (IndirectReference)obj;
				return indirectReference.ObjectNumber == this.ObjectNumber && indirectReference.Generation == this.Generation;
			}
			return false;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002340 File Offset: 0x00000540
		public override int GetHashCode()
		{
			return (59 * 97 + this.ObjectNumber.GetHashCode()) * 97 + this.Generation.GetHashCode();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002374 File Offset: 0x00000574
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.ObjectNumber);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Generation);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
