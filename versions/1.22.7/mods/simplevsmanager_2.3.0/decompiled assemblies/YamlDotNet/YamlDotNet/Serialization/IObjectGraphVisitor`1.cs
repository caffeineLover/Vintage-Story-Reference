using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	public interface IObjectGraphVisitor<[Nullable(2)] TContext>
	{
		// Token: 0x060000C5 RID: 197
		bool Enter([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, TContext context, ObjectSerializer serializer);

		// Token: 0x060000C6 RID: 198
		bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, TContext context, ObjectSerializer serializer);

		// Token: 0x060000C7 RID: 199
		bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, TContext context, ObjectSerializer serializer);

		// Token: 0x060000C8 RID: 200
		void VisitScalar(IObjectDescriptor scalar, TContext context, ObjectSerializer serializer);

		// Token: 0x060000C9 RID: 201
		void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, TContext context, ObjectSerializer serializer);

		// Token: 0x060000CA RID: 202
		void VisitMappingEnd(IObjectDescriptor mapping, TContext context, ObjectSerializer serializer);

		// Token: 0x060000CB RID: 203
		void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, TContext context, ObjectSerializer serializer);

		// Token: 0x060000CC RID: 204
		void VisitSequenceEnd(IObjectDescriptor sequence, TContext context, ObjectSerializer serializer);
	}
}
