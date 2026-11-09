using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000127 RID: 295
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultExclusiveObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000628 RID: 1576 RVA: 0x00015739 File Offset: 0x00013939
		public DefaultExclusiveObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
		{
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00015868 File Offset: 0x00013A68
		[return: Nullable(2)]
		private static object GetDefault(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001587A File Offset: 0x00013A7A
		public override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return !object.Equals(value.Value, DefaultExclusiveObjectGraphVisitor.GetDefault(value.Type)) && base.EnterMapping(key, value, context);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000158A0 File Offset: 0x00013AA0
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			DefaultValueAttribute customAttribute = key.GetCustomAttribute<DefaultValueAttribute>();
			object obj = (customAttribute != null) ? customAttribute.Value : DefaultExclusiveObjectGraphVisitor.GetDefault(key.Type);
			return !object.Equals(value.Value, obj) && base.EnterMapping(key, value, context);
		}
	}
}
