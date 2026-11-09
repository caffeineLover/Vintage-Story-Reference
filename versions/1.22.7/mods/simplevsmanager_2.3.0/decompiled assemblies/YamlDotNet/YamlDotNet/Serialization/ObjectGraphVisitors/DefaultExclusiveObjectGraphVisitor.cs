using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000067 RID: 103
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultExclusiveObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x0600027B RID: 635 RVA: 0x0000849B File Offset: 0x0000669B
		public DefaultExclusiveObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
		{
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000084A4 File Offset: 0x000066A4
		[return: Nullable(2)]
		private static object GetDefault(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000084B6 File Offset: 0x000066B6
		public override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return !object.Equals(value.Value, DefaultExclusiveObjectGraphVisitor.GetDefault(value.Type)) && base.EnterMapping(key, value, context, serializer);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000084E0 File Offset: 0x000066E0
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			DefaultValueAttribute customAttribute = key.GetCustomAttribute<DefaultValueAttribute>();
			object obj = (customAttribute != null) ? customAttribute.Value : DefaultExclusiveObjectGraphVisitor.GetDefault(key.Type);
			return !object.Equals(value.Value, obj) && base.EnterMapping(key, value, context, serializer);
		}
	}
}
