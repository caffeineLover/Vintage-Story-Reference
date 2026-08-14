using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultValuesObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x0600027F RID: 639 RVA: 0x00008526 File Offset: 0x00006726
		public DefaultValuesObjectGraphVisitor(DefaultValuesHandling handling, IObjectGraphVisitor<IEmitter> nextVisitor, IObjectFactory factory) : base(nextVisitor)
		{
			this.handling = handling;
			this.factory = factory;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000853D File Offset: 0x0000673D
		[return: Nullable(2)]
		private object GetDefault(Type type)
		{
			return this.factory.CreatePrimitive(type);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000854C File Offset: 0x0000674C
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			DefaultValuesHandling defaultValuesHandling = this.handling;
			YamlMemberAttribute customAttribute = key.GetCustomAttribute<YamlMemberAttribute>();
			if (customAttribute != null && customAttribute.IsDefaultValuesHandlingSpecified)
			{
				defaultValuesHandling = customAttribute.DefaultValuesHandling;
			}
			if ((defaultValuesHandling & DefaultValuesHandling.OmitNull) != DefaultValuesHandling.Preserve && value.Value == null)
			{
				return false;
			}
			if ((defaultValuesHandling & DefaultValuesHandling.OmitEmptyCollections) != DefaultValuesHandling.Preserve)
			{
				IEnumerable enumerable = value.Value as IEnumerable;
				if (enumerable != null)
				{
					IEnumerator enumerator = enumerable.GetEnumerator();
					bool flag = enumerator.MoveNext();
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			if ((defaultValuesHandling & DefaultValuesHandling.OmitDefaults) != DefaultValuesHandling.Preserve)
			{
				DefaultValueAttribute customAttribute2 = key.GetCustomAttribute<DefaultValueAttribute>();
				object obj = ((customAttribute2 != null) ? customAttribute2.Value : null) ?? this.GetDefault(key.Type);
				if (object.Equals(value.Value, obj))
				{
					return false;
				}
			}
			return base.EnterMapping(key, value, context, serializer);
		}

		// Token: 0x0400009D RID: 157
		private readonly DefaultValuesHandling handling;

		// Token: 0x0400009E RID: 158
		private readonly IObjectFactory factory;
	}
}
