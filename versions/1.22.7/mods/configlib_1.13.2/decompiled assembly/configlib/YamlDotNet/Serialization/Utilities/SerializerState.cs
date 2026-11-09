using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000100 RID: 256
	public sealed class SerializerState : IDisposable
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x00014490 File Offset: 0x00012690
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

		// Token: 0x0600059A RID: 1434 RVA: 0x000144E0 File Offset: 0x000126E0
		public void OnDeserialization()
		{
			foreach (IPostDeserializationCallback postDeserializationCallback in Enumerable.OfType<IPostDeserializationCallback>(this.items.Values))
			{
				postDeserializationCallback.OnDeserialization();
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00014534 File Offset: 0x00012734
		public void Dispose()
		{
			foreach (IDisposable disposable in Enumerable.OfType<IDisposable>(this.items.Values))
			{
				disposable.Dispose();
			}
		}

		// Token: 0x0400027A RID: 634
		[Nullable(1)]
		private readonly IDictionary<Type, object> items = new Dictionary<Type, object>();
	}
}
