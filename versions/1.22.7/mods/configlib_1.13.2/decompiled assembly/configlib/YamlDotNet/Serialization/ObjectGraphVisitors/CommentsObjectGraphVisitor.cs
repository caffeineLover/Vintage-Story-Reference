using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000124 RID: 292
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CommentsObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x00015739 File Offset: 0x00013939
		public CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00015744 File Offset: 0x00013944
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			YamlMemberAttribute customAttribute = key.GetCustomAttribute<YamlMemberAttribute>();
			if (customAttribute != null && customAttribute.Description != null)
			{
				context.Emit(new Comment(customAttribute.Description, false));
			}
			return base.EnterMapping(key, value, context);
		}
	}
}
