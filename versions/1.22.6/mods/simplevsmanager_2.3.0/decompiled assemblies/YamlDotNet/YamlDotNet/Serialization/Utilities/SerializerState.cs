using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x0200004F RID: 79
	public sealed class SerializerState : IDisposable
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00007058 File Offset: 0x00005258
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

		// Token: 0x0600021E RID: 542 RVA: 0x000070A8 File Offset: 0x000052A8
		public void OnDeserialization()
		{
			foreach (IPostDeserializationCallback postDeserializationCallback in Enumerable.OfType<IPostDeserializationCallback>(this.items.Values))
			{
				postDeserializationCallback.OnDeserialization();
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007100 File Offset: 0x00005300
		public void Dispose()
		{
			foreach (IDisposable disposable in Enumerable.OfType<IDisposable>(this.items.Values))
			{
				disposable.Dispose();
			}
		}

		// Token: 0x04000085 RID: 133
		[Nullable(1)]
		private readonly Dictionary<Type, object> items = new Dictionary<Type, object>();
	}
}
