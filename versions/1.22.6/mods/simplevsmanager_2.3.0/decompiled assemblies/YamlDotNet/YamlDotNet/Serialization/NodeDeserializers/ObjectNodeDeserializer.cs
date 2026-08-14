using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000A0B0 File Offset: 0x000082B0
		public ObjectNodeDeserializer(IObjectFactory objectFactory, ITypeInspector typeInspector, bool ignoreUnmatched, bool duplicateKeyChecking, ITypeConverter typeConverter, INamingConvention enumNamingConvention, bool enforceNullability, bool caseInsensitivePropertyMatching, bool enforceRequiredProperties, IEnumerable<IYamlTypeConverter> typeConverters)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
			if (typeInspector == null)
			{
				throw new ArgumentNullException("typeInspector");
			}
			this.typeInspector = typeInspector;
			this.ignoreUnmatched = ignoreUnmatched;
			this.duplicateKeyChecking = duplicateKeyChecking;
			if (typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			this.typeConverter = typeConverter;
			if (enumNamingConvention == null)
			{
				throw new ArgumentNullException("enumNamingConvention");
			}
			this.enumNamingConvention = enumNamingConvention;
			this.enforceNullability = enforceNullability;
			this.caseInsensitivePropertyMatching = caseInsensitivePropertyMatching;
			this.enforceRequiredProperties = enforceRequiredProperties;
			this.typeConverters = new TypeConverterCache(typeConverters);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000A154 File Offset: 0x00008354
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			MappingStart mappingStart;
			if (!parser.TryConsume(out mappingStart))
			{
				value = null;
				return false;
			}
			Type type;
			if ((type = Nullable.GetUnderlyingType(expectedType)) == null)
			{
				type = (FsharpHelper.GetOptionUnderlyingType(expectedType) ?? expectedType);
			}
			Type type2 = type;
			value = this.objectFactory.Create(type2);
			this.objectFactory.ExecuteOnDeserializing(value);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.Ordinal);
			Mark mark = Mark.Empty;
			MappingEnd mappingEnd;
			while (!parser.TryConsume(out mappingEnd))
			{
				Scalar propertyName = parser.Consume<Scalar>();
				if (this.duplicateKeyChecking && !hashSet.Add(propertyName.Value))
				{
					Mark start = propertyName.Start;
					Mark end = propertyName.End;
					throw new YamlException(ref start, ref end, "Encountered duplicate key " + propertyName.Value);
				}
				try
				{
					IPropertyDescriptor property = this.typeInspector.GetProperty(type2, null, propertyName.Value, this.ignoreUnmatched, this.caseInsensitivePropertyMatching);
					if (property == null)
					{
						parser.SkipThisAndNestedEvents();
						continue;
					}
					hashSet2.Add(property.Name);
					object obj;
					if (property.ConverterType != null)
					{
						IYamlTypeConverter converterByType = this.typeConverters.GetConverterByType(property.ConverterType);
						obj = converterByType.ReadYaml(parser, property.Type, rootDeserializer);
					}
					else
					{
						obj = nestedObjectDeserializer.Invoke(parser, property.Type);
					}
					IValuePromise valuePromise = obj as IValuePromise;
					if (valuePromise != null)
					{
						object valueRef = value;
						valuePromise.ValueAvailable += delegate(object v)
						{
							object value3 = this.typeConverter.ChangeType(v, property.Type, this.enumNamingConvention, this.typeInspector);
							this.NullCheck(value3, property, propertyName);
							property.Write(valueRef, value3);
						};
					}
					else
					{
						object value2 = this.typeConverter.ChangeType(obj, property.Type, this.enumNamingConvention, this.typeInspector);
						this.NullCheck(value2, property, propertyName);
						property.Write(value, value2);
					}
				}
				catch (SerializationException ex)
				{
					Mark start = propertyName.Start;
					Mark end = propertyName.End;
					throw new YamlException(ref start, ref end, ex.Message);
				}
				catch (YamlException)
				{
					throw;
				}
				catch (Exception innerException)
				{
					Mark start = propertyName.Start;
					Mark end = propertyName.End;
					throw new YamlException(ref start, ref end, "Exception during deserialization", innerException);
				}
				mark = propertyName.End;
			}
			if (this.enforceRequiredProperties)
			{
				IEnumerable<IPropertyDescriptor> properties = this.typeInspector.GetProperties(type2, value);
				List<string> list = new List<string>();
				foreach (IPropertyDescriptor propertyDescriptor in properties)
				{
					if (propertyDescriptor.Required && !hashSet2.Contains(propertyDescriptor.Name))
					{
						list.Add(propertyDescriptor.Name);
					}
				}
				if (list.Count > 0)
				{
					string text = string.Join(",", list);
					throw new YamlException(ref mark, ref mark, "Missing properties, '" + text + "' in source yaml.");
				}
			}
			this.objectFactory.ExecuteOnDeserialized(value);
			return true;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000A500 File Offset: 0x00008700
		public void NullCheck(object value, IPropertyDescriptor property, Scalar propertyName)
		{
			if (this.enforceNullability && value == null && !property.AllowNulls)
			{
				Mark start = propertyName.Start;
				Mark end = propertyName.End;
				throw new YamlException(ref start, ref end, "Strict nullability enforcement error.", new NullReferenceException("Yaml value is null when target property requires non null values."));
			}
		}

		// Token: 0x040000B9 RID: 185
		private readonly IObjectFactory objectFactory;

		// Token: 0x040000BA RID: 186
		private readonly ITypeInspector typeInspector;

		// Token: 0x040000BB RID: 187
		private readonly bool ignoreUnmatched;

		// Token: 0x040000BC RID: 188
		private readonly bool duplicateKeyChecking;

		// Token: 0x040000BD RID: 189
		private readonly ITypeConverter typeConverter;

		// Token: 0x040000BE RID: 190
		private readonly INamingConvention enumNamingConvention;

		// Token: 0x040000BF RID: 191
		private readonly bool enforceNullability;

		// Token: 0x040000C0 RID: 192
		private readonly bool caseInsensitivePropertyMatching;

		// Token: 0x040000C1 RID: 193
		private readonly bool enforceRequiredProperties;

		// Token: 0x040000C2 RID: 194
		private readonly TypeConverterCache typeConverters;
	}
}
