using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ModernWpf.Controls
{
	// Token: 0x0200009C RID: 156
	internal class InspectingDataSource : ItemsSourceView
	{
		// Token: 0x060007CD RID: 1997 RVA: 0x0001BF14 File Offset: 0x0001A114
		public InspectingDataSource(object source) : base(source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList list = source as IList;
			if (list != null)
			{
				this.m_vector = list;
				this.ListenToCollectionChanges();
			}
			else
			{
				IEnumerable enumerable = source as IEnumerable;
				if (enumerable == null)
				{
					throw new ArgumentException("Argument 'source' is not a supported vector.");
				}
				this.m_vector = this.WrapIterable(enumerable);
			}
			this.m_uniqueIdMaping = (source as IKeyIndexMapping);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001BF80 File Offset: 0x0001A180
		~InspectingDataSource()
		{
			this.UnListenToCollectionChanges();
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001BFAC File Offset: 0x0001A1AC
		internal override int GetSizeCore()
		{
			return this.m_vector.Count;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001BFB9 File Offset: 0x0001A1B9
		internal override object GetAtCore(int index)
		{
			return this.m_vector[index];
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001BFC7 File Offset: 0x0001A1C7
		internal override bool HasKeyIndexMappingCore()
		{
			return this.m_uniqueIdMaping != null;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001BFD2 File Offset: 0x0001A1D2
		internal override string KeyFromIndexCore(int index)
		{
			if (this.m_uniqueIdMaping != null)
			{
				return this.m_uniqueIdMaping.KeyFromIndex(index);
			}
			throw new NotImplementedException();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001BFEE File Offset: 0x0001A1EE
		internal override int IndexFromKeyCore(string id)
		{
			if (this.m_uniqueIdMaping != null)
			{
				return this.m_uniqueIdMaping.IndexFromKey(id);
			}
			throw new NotImplementedException();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001C00C File Offset: 0x0001A20C
		internal override int IndexOfCore(object value)
		{
			int result = -1;
			if (this.m_vector != null)
			{
				int num = this.m_vector.IndexOf(value);
				if (num >= 0)
				{
					result = num;
				}
			}
			return result;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001C038 File Offset: 0x0001A238
		private IList WrapIterable(IEnumerable iterable)
		{
			List<object> list = new List<object>();
			foreach (object obj in iterable)
			{
				list.Add(obj);
			}
			return list;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001C06C File Offset: 0x0001A26C
		private void UnListenToCollectionChanges()
		{
			INotifyCollectionChanged notifyCollectionChanged = this.m_vector as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				CollectionChangedEventManager.RemoveHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001C09C File Offset: 0x0001A29C
		private void ListenToCollectionChanges()
		{
			INotifyCollectionChanged notifyCollectionChanged = this.m_vector as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				CollectionChangedEventManager.AddHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001C0CA File Offset: 0x0001A2CA
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsSourceChanged(e);
		}

		// Token: 0x0400037F RID: 895
		private readonly IList m_vector;

		// Token: 0x04000380 RID: 896
		private readonly IKeyIndexMapping m_uniqueIdMaping;
	}
}
