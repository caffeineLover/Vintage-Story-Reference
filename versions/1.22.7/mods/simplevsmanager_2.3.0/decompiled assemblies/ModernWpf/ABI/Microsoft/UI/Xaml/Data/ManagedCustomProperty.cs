using System;
using System.Reflection;
using Microsoft.UI.Xaml.Data;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x0200016E RID: 366
	internal sealed class ManagedCustomProperty : Microsoft.UI.Xaml.Data.ICustomProperty
	{
		// Token: 0x06000BCE RID: 3022 RVA: 0x00020C3F File Offset: 0x0001EE3F
		public ManagedCustomProperty(PropertyInfo property)
		{
			this._property = property;
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00020C4E File Offset: 0x0001EE4E
		public bool CanRead
		{
			get
			{
				return this._property.CanRead;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00020C5B File Offset: 0x0001EE5B
		public bool CanWrite
		{
			get
			{
				return this._property.CanWrite;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x00020C68 File Offset: 0x0001EE68
		public string Name
		{
			get
			{
				return this._property.Name;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00020C75 File Offset: 0x0001EE75
		public Type Type
		{
			get
			{
				return this._property.PropertyType;
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00020C82 File Offset: 0x0001EE82
		public object GetIndexedValue(object target, object index)
		{
			return this._property.GetValue(target, new object[]
			{
				index
			});
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00020C9A File Offset: 0x0001EE9A
		public object GetValue(object target)
		{
			return this._property.GetValue(target);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00020CA8 File Offset: 0x0001EEA8
		public void SetIndexedValue(object target, object value, object index)
		{
			this._property.SetValue(target, value, new object[]
			{
				index
			});
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00020CC1 File Offset: 0x0001EEC1
		public void SetValue(object target, object value)
		{
			this._property.SetValue(target, value);
		}

		// Token: 0x040001FB RID: 507
		private readonly PropertyInfo _property;
	}
}
