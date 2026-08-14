using System;
using System.Collections.Specialized;

namespace ModernWpf.Controls
{
	// Token: 0x0200009D RID: 157
	public class ItemsSourceView : INotifyCollectionChanged
	{
		// Token: 0x060007D9 RID: 2009 RVA: 0x0001C0D3 File Offset: 0x0001A2D3
		public ItemsSourceView(object source)
		{
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001C0E2 File Offset: 0x0001A2E2
		public int Count
		{
			get
			{
				if (this.m_cachedSize == -1)
				{
					this.m_cachedSize = this.GetSizeCore();
				}
				return this.m_cachedSize;
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001C0FF File Offset: 0x0001A2FF
		public object GetAt(int index)
		{
			return this.GetAtCore(index);
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001C108 File Offset: 0x0001A308
		public bool HasKeyIndexMapping
		{
			get
			{
				return this.HasKeyIndexMappingCore();
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001C110 File Offset: 0x0001A310
		public string KeyFromIndex(int index)
		{
			return this.KeyFromIndexCore(index);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001C119 File Offset: 0x0001A319
		public int IndexFromKey(string key)
		{
			return this.IndexFromKeyCore(key);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001C122 File Offset: 0x0001A322
		public int IndexOf(object value)
		{
			return this.IndexOfCore(value);
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060007E0 RID: 2016 RVA: 0x0001C12C File Offset: 0x0001A32C
		// (remove) Token: 0x060007E1 RID: 2017 RVA: 0x0001C164 File Offset: 0x0001A364
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001C199 File Offset: 0x0001A399
		internal void OnItemsSourceChanged(NotifyCollectionChangedEventArgs args)
		{
			this.m_cachedSize = this.GetSizeCore();
			NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
			if (collectionChanged == null)
			{
				return;
			}
			collectionChanged.Invoke(this, args);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001C1B9 File Offset: 0x0001A3B9
		internal virtual int GetSizeCore()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001C1C0 File Offset: 0x0001A3C0
		internal virtual object GetAtCore(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001C1C7 File Offset: 0x0001A3C7
		internal virtual bool HasKeyIndexMappingCore()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001C1CE File Offset: 0x0001A3CE
		internal virtual string KeyFromIndexCore(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001C1D5 File Offset: 0x0001A3D5
		internal virtual int IndexFromKeyCore(string id)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001C1DC File Offset: 0x0001A3DC
		internal virtual int IndexOfCore(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000382 RID: 898
		private int m_cachedSize = -1;

		// Token: 0x02000119 RID: 281
		internal class CollectionChangedRevoker : EventRevoker<ItemsSourceView, NotifyCollectionChangedEventHandler>
		{
			// Token: 0x06000CD9 RID: 3289 RVA: 0x0002B612 File Offset: 0x00029812
			public CollectionChangedRevoker(ItemsSourceView source, NotifyCollectionChangedEventHandler handler) : base(source, handler)
			{
			}

			// Token: 0x06000CDA RID: 3290 RVA: 0x0002B61C File Offset: 0x0002981C
			protected override void AddHandler(ItemsSourceView source, NotifyCollectionChangedEventHandler handler)
			{
				source.CollectionChanged += handler;
			}

			// Token: 0x06000CDB RID: 3291 RVA: 0x0002B625 File Offset: 0x00029825
			protected override void RemoveHandler(ItemsSourceView source, NotifyCollectionChangedEventHandler handler)
			{
				source.CollectionChanged -= handler;
			}
		}
	}
}
