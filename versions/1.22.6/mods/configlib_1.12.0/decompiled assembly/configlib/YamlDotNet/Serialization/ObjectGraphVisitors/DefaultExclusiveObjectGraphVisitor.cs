using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x0200012B RID: 299
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DefaultExclusiveObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x00016081 File Offset: 0x00014281
		public DefaultExclusiveObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000161B0 File Offset: 0x000143B0
		[return: Nullable(2)]
		private static object GetDefault(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000161C2 File Offset: 0x000143C2
		public override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return !object.Equals(value.Value, DefaultExclusiveObjectGraphVisitor.GetDefault(value.Type)) && base.EnterMapping(key, value, context);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000161E8 File Offset: 0x000143E8
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			DefaultValueAttribute customAttribute = key.GetCustomAttribute<DefaultValueAttribute>();
			object obj = (customAttribute != null) ? customAttribute.Value : DefaultExclusiveObjectGraphVisitor.GetDefault(key.Type);
			return !object.Equals(value.Value, obj) && base.EnterMapping(key, value, context);
		}
	}
}
