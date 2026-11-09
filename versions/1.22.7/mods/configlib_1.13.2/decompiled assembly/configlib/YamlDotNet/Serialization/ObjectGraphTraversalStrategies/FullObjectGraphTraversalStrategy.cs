using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
	// Token: 0x0200012C RID: 300
	[NullableContext(1)]
	[Nullable(0)]
	public class FullObjectGraphTraversalStrategy : IObjectGraphTraversalStrategy
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x00015B24 File Offset: 0x00013D24
		public FullObjectGraphTraversalStrategy(ITypeInspector typeDescriptor, ITypeResolver typeResolver, int maxRecursion, INamingConvention namingConvention, IObjectFactory objectFactory)
		{
			if (maxRecursion <= 0)
			{
				throw new ArgumentOutOfRangeException("maxRecursion", maxRecursion, "maxRecursion must be greater than 1");
			}
			if (typeDescriptor == null)
			{
				throw new ArgumentNullException("typeDescriptor");
			}
			this.typeDescriptor = typeDescriptor;
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.maxRecursion = maxRecursion;
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00015BB2 File Offset: 0x00013DB2
		void IObjectGraphTraversalStrategy.Traverse<[Nullable(2)] TContext>(IObjectDescriptor graph, IObjectGraphVisitor<TContext> visitor, TContext context)
		{
			this.Traverse<TContext>("<root>", graph, visitor, context, new Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment>(this.maxRecursion));
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00015BD0 File Offset: 0x00013DD0
		protected virtual void Traverse<[Nullable(2)] TContext>(object name, IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path)
		{
			if (path.Count >= this.maxRecursion)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Too much recursion when traversing the object graph.");
				stringBuilder.AppendLine("The path to reach this recursion was:");
				Stack<KeyValuePair<string, string>> stack = new Stack<KeyValuePair<string, string>>(path.Count);
				int num = 0;
				foreach (FullObjectGraphTraversalStrategy.ObjectPathSegment objectPathSegment in path)
				{
					object name2 = objectPathSegment.Name;
					string text = ((name2 != null) ? name2.ToString() : null) ?? string.Empty;
					num = Math.Max(num, text.Length);
					stack.Push(new KeyValuePair<string, string>(text, objectPathSegment.Value.Type.FullName));
				}
				foreach (KeyValuePair<string, string> keyValuePair in stack)
				{
					stringBuilder.Append(" -> ").Append(keyValuePair.Key.PadRight(num)).Append("  [").Append(keyValuePair.Value).AppendLine("]");
				}
				throw new MaximumRecursionLevelReachedException(stringBuilder.ToString());
			}
			if (!visitor.Enter(value, context))
			{
				return;
			}
			path.Push(new FullObjectGraphTraversalStrategy.ObjectPathSegment(name, value));
			try
			{
				TypeCode typeCode = value.Type.GetTypeCode();
				switch (typeCode)
				{
				case 0:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("TypeCode.");
					defaultInterpolatedStringHandler.AppendFormatted<TypeCode>(typeCode);
					defaultInterpolatedStringHandler.AppendLiteral(" is not supported.");
					throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 18:
					visitor.VisitScalar(value, context);
					return;
				}
				if (value.IsDbNull())
				{
					visitor.VisitScalar(new ObjectDescriptor(null, typeof(object), typeof(object)), context);
				}
				if (value.Value == null || value.Type == typeof(TimeSpan))
				{
					visitor.VisitScalar(value, context);
				}
				else
				{
					Type underlyingType = Nullable.GetUnderlyingType(value.Type);
					if (underlyingType != null)
					{
						this.Traverse<TContext>("Value", new ObjectDescriptor(value.Value, underlyingType, value.Type, value.ScalarStyle), visitor, context, path);
					}
					else
					{
						this.TraverseObject<TContext>(value, visitor, context, path);
					}
				}
			}
			finally
			{
				path.Pop();
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00015EC4 File Offset: 0x000140C4
		protected virtual void TraverseObject<[Nullable(2)] TContext>(IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path)
		{
			if (typeof(IDictionary).IsAssignableFrom(value.Type))
			{
				this.TraverseDictionary<TContext>(value, visitor, typeof(object), typeof(object), context, path);
				return;
			}
			IDictionary value2;
			Type[] array;
			if (this.objectFactory.GetDictionary(value, out value2, out array))
			{
				this.TraverseDictionary<TContext>(new ObjectDescriptor(value2, value.Type, value.StaticType, value.ScalarStyle), visitor, array[0], array[1], context, path);
				return;
			}
			if (typeof(IEnumerable).IsAssignableFrom(value.Type))
			{
				this.TraverseList<TContext>(value, visitor, context, path);
				return;
			}
			this.TraverseProperties<TContext>(value, visitor, context, path);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00015F74 File Offset: 0x00014174
		protected virtual void TraverseDictionary<[Nullable(2)] TContext>(IObjectDescriptor dictionary, IObjectGraphVisitor<TContext> visitor, Type keyType, Type valueType, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path)
		{
			visitor.VisitMappingStart(dictionary, keyType, valueType, context);
			bool flag = dictionary.Type.FullName.Equals("System.Dynamic.ExpandoObject");
			foreach (object obj in ((IDictionary)dictionary.NonNullValue()))
			{
				DictionaryEntry value = ((DictionaryEntry?)obj).Value;
				object obj2 = flag ? this.namingConvention.Apply(value.Key.ToString()) : value.Key;
				IObjectDescriptor objectDescriptor = this.GetObjectDescriptor(obj2, keyType);
				IObjectDescriptor objectDescriptor2 = this.GetObjectDescriptor(value.Value, valueType);
				if (visitor.EnterMapping(objectDescriptor, objectDescriptor2, context))
				{
					this.Traverse<TContext>(obj2, objectDescriptor, visitor, context, path);
					this.Traverse<TContext>(obj2, objectDescriptor2, visitor, context, path);
				}
			}
			visitor.VisitMappingEnd(dictionary, context);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00016074 File Offset: 0x00014274
		private void TraverseList<[Nullable(2)] TContext>(IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path)
		{
			Type valueType = this.objectFactory.GetValueType(value.Type);
			visitor.VisitSequenceStart(value, valueType, context);
			int num = 0;
			foreach (object value2 in ((IEnumerable)value.NonNullValue()))
			{
				this.Traverse<TContext>(num, this.GetObjectDescriptor(value2, valueType), visitor, context, path);
				num++;
			}
			visitor.VisitSequenceEnd(value, context);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001610C File Offset: 0x0001430C
		protected virtual void TraverseProperties<[Nullable(2)] TContext>(IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path)
		{
			if (context.GetType() != typeof(Nothing))
			{
				this.objectFactory.ExecuteOnSerializing(value.Value);
			}
			visitor.VisitMappingStart(value, typeof(string), typeof(object), context);
			object obj = value.NonNullValue();
			foreach (IPropertyDescriptor propertyDescriptor in this.typeDescriptor.GetProperties(value.Type, obj))
			{
				IObjectDescriptor objectDescriptor = propertyDescriptor.Read(obj);
				if (visitor.EnterMapping(propertyDescriptor, objectDescriptor, context))
				{
					this.Traverse<TContext>(propertyDescriptor.Name, new ObjectDescriptor(propertyDescriptor.Name, typeof(string), typeof(string), ScalarStyle.Plain), visitor, context, path);
					this.Traverse<TContext>(propertyDescriptor.Name, objectDescriptor, visitor, context, path);
				}
			}
			visitor.VisitMappingEnd(value, context);
			if (context.GetType() != typeof(Nothing))
			{
				this.objectFactory.ExecuteOnSerialized(value.Value);
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001623C File Offset: 0x0001443C
		private IObjectDescriptor GetObjectDescriptor([Nullable(2)] object value, Type staticType)
		{
			return new ObjectDescriptor(value, this.typeResolver.Resolve(staticType, value), staticType);
		}

		// Token: 0x040002BA RID: 698
		private readonly int maxRecursion;

		// Token: 0x040002BB RID: 699
		private readonly ITypeInspector typeDescriptor;

		// Token: 0x040002BC RID: 700
		private readonly ITypeResolver typeResolver;

		// Token: 0x040002BD RID: 701
		private readonly INamingConvention namingConvention;

		// Token: 0x040002BE RID: 702
		private readonly IObjectFactory objectFactory;

		// Token: 0x0200012D RID: 301
		[Nullable(0)]
		protected struct ObjectPathSegment
		{
			// Token: 0x06000653 RID: 1619 RVA: 0x00016252 File Offset: 0x00014452
			public ObjectPathSegment(object name, IObjectDescriptor value)
			{
				this.Name = name;
				this.Value = value;
			}

			// Token: 0x040002BF RID: 703
			public readonly object Name;

			// Token: 0x040002C0 RID: 704
			public readonly IObjectDescriptor Value;
		}
	}
}
