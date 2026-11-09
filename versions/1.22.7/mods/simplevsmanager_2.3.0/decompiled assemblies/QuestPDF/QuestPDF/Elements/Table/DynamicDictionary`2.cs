using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuestPDF.Elements.Table
{
	// Token: 0x02000104 RID: 260
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DynamicDictionary<[Nullable(2)] TKey, [Nullable(2)] TValue>
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00015AB6 File Offset: 0x00013CB6
		private TValue Default
		{
			[CompilerGenerated]
			get
			{
				return this.<Default>k__BackingField;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00015ABE File Offset: 0x00013CBE
		private IDictionary<TKey, TValue> Dictionary
		{
			[CompilerGenerated]
			get
			{
				return this.<Dictionary>k__BackingField;
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00015AC6 File Offset: 0x00013CC6
		public DynamicDictionary()
		{
			this.<Dictionary>k__BackingField = new Dictionary<TKey, TValue>();
			base..ctor();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00015AD9 File Offset: 0x00013CD9
		public DynamicDictionary(TValue defaultValue)
		{
			this.<Dictionary>k__BackingField = new Dictionary<TKey, TValue>();
			base..ctor();
			this.<Default>k__BackingField = defaultValue;
		}

		// Token: 0x1700021E RID: 542
		public TValue this[TKey key]
		{
			get
			{
				TValue result;
				if (!this.Dictionary.TryGetValue(key, ref result))
				{
					return this.Default;
				}
				return result;
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00015B28 File Offset: 0x00013D28
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public List<KeyValuePair<TKey, TValue>> Items
		{
			[return: Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})]
			get
			{
				return Enumerable.ToList<KeyValuePair<TKey, TValue>>(this.Dictionary);
			}
		}
	}
}
