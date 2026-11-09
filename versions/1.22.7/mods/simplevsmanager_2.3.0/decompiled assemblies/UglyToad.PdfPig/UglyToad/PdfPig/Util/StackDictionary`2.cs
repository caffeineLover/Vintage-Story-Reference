using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000036 RID: 54
	internal class StackDictionary<K, V>
	{
		// Token: 0x1700003E RID: 62
		public V this[K key]
		{
			get
			{
				V result;
				if (this.TryGetValue(key, ref result))
				{
					return result;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No item with key ");
				defaultInterpolatedStringHandler.AppendFormatted<K>(key);
				defaultInterpolatedStringHandler.AppendLiteral(" in stack.");
				throw new KeyNotFoundException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			set
			{
				if (this.values.Count == 0)
				{
					throw new InvalidOperationException("Cannot set item in empty stack, call Push before use.");
				}
				this.values[this.values.Count - 1][key] = value;
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
		public bool TryGetValue(K key, out V result)
		{
			if (this.values.Count == 0)
			{
				throw new InvalidOperationException("Cannot get item from empty stack, call Push before use.");
			}
			for (int i = this.values.Count - 1; i >= 0; i--)
			{
				if (this.values[i].TryGetValue(key, ref result))
				{
					return true;
				}
			}
			result = default(V);
			return false;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000DF10 File Offset: 0x0000C110
		public void Push()
		{
			this.values.Add(new Dictionary<K, V>());
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000DF22 File Offset: 0x0000C122
		public void Pop()
		{
			if (this.values.Count == 0)
			{
				throw new InvalidOperationException("Cannot pop empty stacked dictionary.");
			}
			this.values.RemoveAt(this.values.Count - 1);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000DF54 File Offset: 0x0000C154
		public StackDictionary()
		{
			this.values = new List<Dictionary<K, V>>();
			base..ctor();
		}

		// Token: 0x040000B5 RID: 181
		private readonly List<Dictionary<K, V>> values;
	}
}
