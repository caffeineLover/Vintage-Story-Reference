using System;
using System.Collections;

namespace ModernWpf.Controls
{
	// Token: 0x02000028 RID: 40
	internal class EmptyEnumerator : IEnumerator
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00006458 File Offset: 0x00004658
		private EmptyEnumerator()
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006460 File Offset: 0x00004660
		public static IEnumerator Instance
		{
			get
			{
				if (EmptyEnumerator._instance == null)
				{
					EmptyEnumerator._instance = new EmptyEnumerator();
				}
				return EmptyEnumerator._instance;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006478 File Offset: 0x00004678
		public void Reset()
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000647A File Offset: 0x0000467A
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000647D File Offset: 0x0000467D
		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04000083 RID: 131
		private static IEnumerator _instance;
	}
}
