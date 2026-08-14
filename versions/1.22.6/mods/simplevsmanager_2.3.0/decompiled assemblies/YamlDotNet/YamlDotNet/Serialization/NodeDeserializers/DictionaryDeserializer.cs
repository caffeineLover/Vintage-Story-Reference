using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200007B RID: 123
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class DictionaryDeserializer
	{
		// Token: 0x060002DE RID: 734 RVA: 0x00009C24 File Offset: 0x00007E24
		public DictionaryDeserializer(bool duplicateKeyChecking)
		{
			this.duplicateKeyChecking = duplicateKeyChecking;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00009C34 File Offset: 0x00007E34
		private void TryAssign(IDictionary result, object key, object value, MappingStart propertyName)
		{
			if (this.duplicateKeyChecking && result.Contains(key))
			{
				Mark start = propertyName.Start;
				Mark end = propertyName.End;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Encountered duplicate key ");
				defaultInterpolatedStringHandler.AppendFormatted<object>(key);
				throw new YamlException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			result[key] = value;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00009C9C File Offset: 0x00007E9C
		protected virtual void Deserialize(Type tKey, Type tValue, IParser parser, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, IDictionary result, ObjectDeserializer rootDeserializer)
		{
			MappingStart property = parser.Consume<MappingStart>();
			MappingEnd mappingEnd;
			while (!parser.TryConsume(out mappingEnd))
			{
				object key = nestedObjectDeserializer.Invoke(parser, tKey);
				object value = nestedObjectDeserializer.Invoke(parser, tValue);
				IValuePromise valuePromise = value as IValuePromise;
				IValuePromise valuePromise2 = key as IValuePromise;
				if (valuePromise2 != null)
				{
					if (valuePromise == null)
					{
						valuePromise2.ValueAvailable += delegate(object v)
						{
							result[v] = value;
						};
					}
					else
					{
						bool hasFirstPart = false;
						valuePromise2.ValueAvailable += delegate(object v)
						{
							if (hasFirstPart)
							{
								this.TryAssign(result, v, value, property);
								return;
							}
							key = v;
							hasFirstPart = true;
						};
						valuePromise.ValueAvailable += delegate(object v)
						{
							if (hasFirstPart)
							{
								this.TryAssign(result, key, v, property);
								return;
							}
							value = v;
							hasFirstPart = true;
						};
					}
				}
				else
				{
					if (key == null)
					{
						throw new ArgumentException("Empty key names are not supported yet.", "tKey");
					}
					if (valuePromise == null)
					{
						this.TryAssign(result, key, value, property);
					}
					else
					{
						valuePromise.ValueAvailable += delegate(object v)
						{
							result[key] = v;
						};
					}
				}
			}
		}

		// Token: 0x040000B5 RID: 181
		private readonly bool duplicateKeyChecking;
	}
}
