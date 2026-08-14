using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000104 RID: 260
	public sealed class SerializerState : IDisposable
	{
		// Token: 0x060005D5 RID: 1493 RVA: 0x00014DD8 File Offset: 0x00012FD8
		[NullableContext(1)]
		public T Get<T>() where T : class, new()
		{
			object obj;
			if (!this.items.TryGetValue(typeof(T), ref obj))
			{
				obj = Activator.CreateInstance<T>();
				this.items.Add(typeof(T), obj);
			}
			return (T)((object)obj);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014E28 File Offset: 0x00013028
		public void OnDeserialization()
		{
			foreach (IPostDeserializationCallback postDeserializationCallback in Enumerable.OfType<IPostDeserializationCallback>(this.items.Values))
			{
				postDeserializationCallback.OnDeserialization();
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00014E7C File Offset: 0x0001307C
		public void Dispose()
		{
			foreach (IDisposable disposable in Enumerable.OfType<IDisposable>(this.items.Values))
			{
				disposable.Dispose();
			}
		}

		// Token: 0x040002A8 RID: 680
		[Nullable(1)]
		private readonly IDictionary<Type, object> items = new Dictionary<Type, object>();
	}
}
