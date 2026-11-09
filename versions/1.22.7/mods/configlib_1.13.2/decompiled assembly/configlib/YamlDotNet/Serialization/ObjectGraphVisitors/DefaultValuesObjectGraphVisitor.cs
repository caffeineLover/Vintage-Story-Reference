using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000128 RID: 296
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultValuesObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x000158E4 File Offset: 0x00013AE4
		public DefaultValuesObjectGraphVisitor(DefaultValuesHandling handling, IObjectGraphVisitor<IEmitter> nextVisitor, IObjectFactory factory) : base(nextVisitor)
		{
			this.handling = handling;
			this.factory = factory;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000158FB File Offset: 0x00013AFB
		[return: Nullable(2)]
		private object GetDefault(Type type)
		{
			return this.factory.CreatePrimitive(type);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001590C File Offset: 0x00013B0C
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
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
			return base.EnterMapping(key, value, context);
		}

		// Token: 0x040002B5 RID: 693
		private readonly DefaultValuesHandling handling;

		// Token: 0x040002B6 RID: 694
		private readonly IObjectFactory factory;
	}
}
