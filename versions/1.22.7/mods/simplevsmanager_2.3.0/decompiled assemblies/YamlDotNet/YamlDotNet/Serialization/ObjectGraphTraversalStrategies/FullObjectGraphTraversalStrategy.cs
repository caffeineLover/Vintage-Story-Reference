using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	public class FullObjectGraphTraversalStrategy : IObjectGraphTraversalStrategy
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00008764 File Offset: 0x00006964
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

		// Token: 0x0600029D RID: 669 RVA: 0x000087F2 File Offset: 0x000069F2
		void IObjectGraphTraversalStrategy.Traverse<[Nullable(2)] TContext>(IObjectDescriptor graph, IObjectGraphVisitor<TContext> visitor, TContext context, ObjectSerializer serializer)
		{
			this.Traverse<TContext>(null, "<root>", graph, visitor, context, new Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment>(this.maxRecursion), serializer);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00008810 File Offset: 0x00006A10
		protected virtual void Traverse<[Nullable(2)] TContext>([Nullable(2)] IPropertyDescriptor propertyDescriptor, object name, IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path, ObjectSerializer serializer)
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
			if (!visitor.Enter(propertyDescriptor, value, context, serializer))
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
					visitor.VisitScalar(value, context, serializer);
					return;
				}
				if (value.IsDbNull())
				{
					visitor.VisitScalar(new ObjectDescriptor(null, typeof(object), typeof(object)), context, serializer);
				}
				if (value.Value == null || value.Type == typeof(TimeSpan))
				{
					visitor.VisitScalar(value, context, serializer);
				}
				else
				{
					Type underlyingType = Nullable.GetUnderlyingType(value.Type);
					Type type = underlyingType ?? FsharpHelper.GetOptionUnderlyingType(value.Type);
					object obj = (type != null) ? FsharpHelper.GetValue(value) : null;
					if (underlyingType != null)
					{
						this.Traverse<TContext>(propertyDescriptor, "Value", new ObjectDescriptor(value.Value, underlyingType, value.Type, value.ScalarStyle), visitor, context, path, serializer);
					}
					else if (type != null && obj != null)
					{
						this.Traverse<TContext>(propertyDescriptor, "Value", new ObjectDescriptor(FsharpHelper.GetValue(value), type, value.Type, value.ScalarStyle), visitor, context, path, serializer);
					}
					else
					{
						this.TraverseObject<TContext>(propertyDescriptor, value, visitor, context, path, serializer);
					}
				}
			}
			finally
			{
				path.Pop();
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008B80 File Offset: 0x00006D80
		protected virtual void TraverseObject<[Nullable(2)] TContext>([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path, ObjectSerializer serializer)
		{
			if (typeof(IDictionary).IsAssignableFrom(value.Type))
			{
				this.TraverseDictionary<TContext>(propertyDescriptor, value, visitor, typeof(object), typeof(object), context, path, serializer);
				return;
			}
			IDictionary value2;
			Type[] array;
			if (this.objectFactory.GetDictionary(value, out value2, out array))
			{
				this.TraverseDictionary<TContext>(propertyDescriptor, new ObjectDescriptor(value2, value.Type, value.StaticType, value.ScalarStyle), visitor, array[0], array[1], context, path, serializer);
				return;
			}
			if (typeof(IEnumerable).IsAssignableFrom(value.Type))
			{
				this.TraverseList<TContext>(propertyDescriptor, value, visitor, context, path, serializer);
				return;
			}
			this.TraverseProperties<TContext>(value, visitor, context, path, serializer);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008C3C File Offset: 0x00006E3C
		protected virtual void TraverseDictionary<[Nullable(2)] TContext>([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor dictionary, IObjectGraphVisitor<TContext> visitor, Type keyType, Type valueType, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path, ObjectSerializer serializer)
		{
			visitor.VisitMappingStart(dictionary, keyType, valueType, context, serializer);
			bool flag = dictionary.Type.FullName.Equals("System.Dynamic.ExpandoObject");
			foreach (object obj in ((IDictionary)dictionary.NonNullValue()))
			{
				DictionaryEntry value = ((DictionaryEntry?)obj).Value;
				object obj2 = flag ? this.namingConvention.Apply(value.Key.ToString()) : value.Key;
				ObjectDescriptor objectDescriptor = this.GetObjectDescriptor(obj2, keyType);
				ObjectDescriptor objectDescriptor2 = this.GetObjectDescriptor(value.Value, valueType);
				if (visitor.EnterMapping(objectDescriptor, objectDescriptor2, context, serializer))
				{
					this.Traverse<TContext>(propertyDescriptor, obj2, objectDescriptor, visitor, context, path, serializer);
					this.Traverse<TContext>(propertyDescriptor, obj2, objectDescriptor2, visitor, context, path, serializer);
				}
			}
			visitor.VisitMappingEnd(dictionary, context, serializer);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008D4C File Offset: 0x00006F4C
		private void TraverseList<[Nullable(2)] TContext>(IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path, ObjectSerializer serializer)
		{
			Type valueType = this.objectFactory.GetValueType(value.Type);
			visitor.VisitSequenceStart(value, valueType, context, serializer);
			int num = 0;
			foreach (object value2 in ((IEnumerable)value.NonNullValue()))
			{
				this.Traverse<TContext>(propertyDescriptor, num, this.GetObjectDescriptor(value2, valueType), visitor, context, path, serializer);
				num++;
			}
			visitor.VisitSequenceEnd(value, context, serializer);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008DEC File Offset: 0x00006FEC
		protected virtual void TraverseProperties<[Nullable(2)] TContext>(IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path, ObjectSerializer serializer)
		{
			if (context.GetType() != typeof(Nothing))
			{
				this.objectFactory.ExecuteOnSerializing(value.Value);
			}
			visitor.VisitMappingStart(value, typeof(string), typeof(object), context, serializer);
			object obj = value.NonNullValue();
			foreach (IPropertyDescriptor propertyDescriptor in this.typeDescriptor.GetProperties(value.Type, obj))
			{
				IObjectDescriptor objectDescriptor = propertyDescriptor.Read(obj);
				if (visitor.EnterMapping(propertyDescriptor, objectDescriptor, context, serializer))
				{
					this.Traverse<TContext>(null, propertyDescriptor.Name, new ObjectDescriptor(propertyDescriptor.Name, typeof(string), typeof(string), ScalarStyle.Plain), visitor, context, path, serializer);
					this.Traverse<TContext>(propertyDescriptor, propertyDescriptor.Name, objectDescriptor, visitor, context, path, serializer);
				}
			}
			visitor.VisitMappingEnd(value, context, serializer);
			if (context.GetType() != typeof(Nothing))
			{
				this.objectFactory.ExecuteOnSerialized(value.Value);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008F28 File Offset: 0x00007128
		private ObjectDescriptor GetObjectDescriptor([Nullable(2)] object value, Type staticType)
		{
			return new ObjectDescriptor(value, this.typeResolver.Resolve(staticType, value), staticType);
		}

		// Token: 0x040000A2 RID: 162
		private readonly int maxRecursion;

		// Token: 0x040000A3 RID: 163
		private readonly ITypeInspector typeDescriptor;

		// Token: 0x040000A4 RID: 164
		private readonly ITypeResolver typeResolver;

		// Token: 0x040000A5 RID: 165
		private readonly INamingConvention namingConvention;

		// Token: 0x040000A6 RID: 166
		private readonly IObjectFactory objectFactory;

		// Token: 0x02000177 RID: 375
		[Nullable(0)]
		protected readonly struct ObjectPathSegment
		{
			// Token: 0x06000815 RID: 2069 RVA: 0x00019730 File Offset: 0x00017930
			public ObjectPathSegment(object name, IObjectDescriptor value)
			{
				this.Name = name;
				this.Value = value;
			}

			// Token: 0x040002F2 RID: 754
			public readonly object Name;

			// Token: 0x040002F3 RID: 755
			public readonly IObjectDescriptor Value;
		}
	}
}
