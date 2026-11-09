using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000B5 RID: 181
	[NullableContext(1)]
	[Nullable(0)]
	public static class ExpressionExtensions
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		public static PropertyInfo AsProperty(this LambdaExpression propertyAccessor)
		{
			PropertyInfo propertyInfo = ExpressionExtensions.TryGetMemberExpression<PropertyInfo>(propertyAccessor);
			if (propertyInfo == null)
			{
				throw new ArgumentException("Expected a lambda expression in the form: x => x.SomeProperty", "propertyAccessor");
			}
			return propertyInfo;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000E52C File Offset: 0x0000C72C
		[return: MaybeNull]
		private static TMemberInfo TryGetMemberExpression<[Nullable(0)] TMemberInfo>(LambdaExpression lambdaExpression) where TMemberInfo : MemberInfo
		{
			if (lambdaExpression.Parameters.Count != 1)
			{
				return default(TMemberInfo);
			}
			Expression expression = lambdaExpression.Body;
			UnaryExpression unaryExpression = expression as UnaryExpression;
			if (unaryExpression != null)
			{
				if (unaryExpression.NodeType != 10)
				{
					return default(TMemberInfo);
				}
				expression = unaryExpression.Operand;
			}
			MemberExpression memberExpression = expression as MemberExpression;
			if (memberExpression == null)
			{
				return default(TMemberInfo);
			}
			if (memberExpression.Expression != lambdaExpression.Parameters[0])
			{
				return default(TMemberInfo);
			}
			return memberExpression.Member as TMemberInfo;
		}
	}
}
