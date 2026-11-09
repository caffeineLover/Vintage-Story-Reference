using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class BuilderSkeleton<[Nullable(0)] TBuilder> where TBuilder : BuilderSkeleton<TBuilder>
	{
		// Token: 0x06000023 RID: 35 RVA: 0x0000269C File Offset: 0x0000089C
		internal BuilderSkeleton(ITypeResolver typeResolver)
		{
			this.namingConvention = NullNamingConvention.Instance;
			this.enumNamingConvention = NullNamingConvention.Instance;
			this.yamlFormatter = YamlFormatter.Default;
			base..ctor();
			this.overrides = new YamlAttributeOverrides();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList2 = lazyComponentRegistrationList;
			Type typeFromHandle = typeof(GuidConverter);
			Func<Nothing, IYamlTypeConverter> func;
			if ((func = BuilderSkeleton<TBuilder>.<>c.<>9__10_0) == null)
			{
				func = (BuilderSkeleton<TBuilder>.<>c.<>9__10_0 = new Func<Nothing, IYamlTypeConverter>(BuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__10_0));
			}
			lazyComponentRegistrationList2.Add(typeFromHandle, func);
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList3 = lazyComponentRegistrationList;
			Type typeFromHandle2 = typeof(SystemTypeConverter);
			Func<Nothing, IYamlTypeConverter> func2;
			if ((func2 = BuilderSkeleton<TBuilder>.<>c.<>9__10_1) == null)
			{
				func2 = (BuilderSkeleton<TBuilder>.<>c.<>9__10_1 = new Func<Nothing, IYamlTypeConverter>(BuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__10_1));
			}
			lazyComponentRegistrationList3.Add(typeFromHandle2, func2);
			this.typeConverterFactories = lazyComponentRegistrationList;
			this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.settings = new Settings();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000024 RID: 36
		protected abstract TBuilder Self { get; }

		// Token: 0x06000025 RID: 37 RVA: 0x00002772 File Offset: 0x00000972
		public TBuilder IgnoreFields()
		{
			this.ignoreFields = true;
			return this.Self;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002781 File Offset: 0x00000981
		public TBuilder IncludeNonPublicProperties()
		{
			this.includeNonPublicProperties = true;
			return this.Self;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002790 File Offset: 0x00000990
		public TBuilder EnablePrivateConstructors()
		{
			this.settings.AllowPrivateConstructors = true;
			return this.Self;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027A4 File Offset: 0x000009A4
		public TBuilder WithNamingConvention(INamingConvention namingConvention)
		{
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			return this.Self;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000027C2 File Offset: 0x000009C2
		public TBuilder WithEnumNamingConvention(INamingConvention enumNamingConvention)
		{
			this.enumNamingConvention = enumNamingConvention;
			return this.Self;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027D1 File Offset: 0x000009D1
		public TBuilder WithTypeResolver(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			return this.Self;
		}

		// Token: 0x0600002B RID: 43
		public abstract TBuilder WithTagMapping(TagName tag, Type type);

		// Token: 0x0600002C RID: 44 RVA: 0x000027EF File Offset: 0x000009EF
		public TBuilder WithAttributeOverride<[Nullable(2)] TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute)
		{
			this.overrides.Add<TClass>(propertyAccessor, attribute);
			return this.Self;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002804 File Offset: 0x00000A04
		public TBuilder WithAttributeOverride(Type type, string member, Attribute attribute)
		{
			this.overrides.Add(type, member, attribute);
			return this.Self;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000281A File Offset: 0x00000A1A
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter)
		{
			Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> action;
			if ((action = BuilderSkeleton<TBuilder>.<>c.<>9__22_0) == null)
			{
				action = (BuilderSkeleton<TBuilder>.<>c.<>9__22_0 = new Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>(BuilderSkeleton<TBuilder>.<>c.<>9.<WithTypeConverter>b__22_0));
			}
			return this.WithTypeConverter(typeConverter, action);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002844 File Offset: 0x00000A44
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter, Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass23_0 CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass23_0();
			CS$<>8__locals1.typeConverter = typeConverter;
			if (CS$<>8__locals1.typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeConverterFactories.CreateRegistrationLocationSelector(CS$<>8__locals1.typeConverter.GetType(), new Func<Nothing, IYamlTypeConverter>(CS$<>8__locals1.<WithTypeConverter>b__0)));
			return this.Self;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028B0 File Offset: 0x00000AB0
		public TBuilder WithTypeConverter<[Nullable(0)] TYamlTypeConverter>(WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where) where TYamlTypeConverter : IYamlTypeConverter
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass24_0<TYamlTypeConverter> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass24_0<TYamlTypeConverter>();
			CS$<>8__locals1.typeConverterFactory = typeConverterFactory;
			if (CS$<>8__locals1.typeConverterFactory == null)
			{
				throw new ArgumentNullException("typeConverterFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeConverterFactories.CreateTrackingRegistrationLocationSelector(typeof(TYamlTypeConverter), new Func<IYamlTypeConverter, Nothing, IYamlTypeConverter>(CS$<>8__locals1.<WithTypeConverter>b__0)));
			return this.Self;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002918 File Offset: 0x00000B18
		public TBuilder WithoutTypeConverter<[Nullable(0)] TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter
		{
			return this.WithoutTypeConverter(typeof(TYamlTypeConverter));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000292A File Offset: 0x00000B2A
		public TBuilder WithoutTypeConverter(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this.typeConverterFactories.Remove(converterType);
			return this.Self;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002952 File Offset: 0x00000B52
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory) where TTypeInspector : ITypeInspector
		{
			Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> action;
			if ((action = BuilderSkeleton<TBuilder>.<>c__27<TTypeInspector>.<>9__27_0) == null)
			{
				action = (BuilderSkeleton<TBuilder>.<>c__27<TTypeInspector>.<>9__27_0 = new Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>(BuilderSkeleton<TBuilder>.<>c__27<TTypeInspector>.<>9.<WithTypeInspector>b__27_0));
			}
			return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, action);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000297C File Offset: 0x00000B7C
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory, Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass28_0<TTypeInspector> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass28_0<TTypeInspector>();
			CS$<>8__locals1.typeInspectorFactory = typeInspectorFactory;
			if (CS$<>8__locals1.typeInspectorFactory == null)
			{
				throw new ArgumentNullException("typeInspectorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeInspectorFactories.CreateRegistrationLocationSelector(typeof(TTypeInspector), new Func<ITypeInspector, ITypeInspector>(CS$<>8__locals1.<WithTypeInspector>b__0)));
			return this.Self;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029E4 File Offset: 0x00000BE4
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass29_0<TTypeInspector> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass29_0<TTypeInspector>();
			CS$<>8__locals1.typeInspectorFactory = typeInspectorFactory;
			if (CS$<>8__locals1.typeInspectorFactory == null)
			{
				throw new ArgumentNullException("typeInspectorFactory");
			}
			if (where == null)
			{
				throw new ArgumentNullException("where");
			}
			where.Invoke(this.typeInspectorFactories.CreateTrackingRegistrationLocationSelector(typeof(TTypeInspector), new Func<ITypeInspector, ITypeInspector, ITypeInspector>(CS$<>8__locals1.<WithTypeInspector>b__0)));
			return this.Self;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A4C File Offset: 0x00000C4C
		public TBuilder WithoutTypeInspector<[Nullable(0)] TTypeInspector>() where TTypeInspector : ITypeInspector
		{
			return this.WithoutTypeInspector(typeof(TTypeInspector));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A5E File Offset: 0x00000C5E
		public TBuilder WithoutTypeInspector(Type inspectorType)
		{
			if (inspectorType == null)
			{
				throw new ArgumentNullException("inspectorType");
			}
			this.typeInspectorFactories.Remove(inspectorType);
			return this.Self;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A86 File Offset: 0x00000C86
		public TBuilder WithYamlFormatter(YamlFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			this.yamlFormatter = formatter;
			return this.Self;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002AA4 File Offset: 0x00000CA4
		protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
		{
			return this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
		}

		// Token: 0x04000005 RID: 5
		internal INamingConvention namingConvention;

		// Token: 0x04000006 RID: 6
		internal INamingConvention enumNamingConvention;

		// Token: 0x04000007 RID: 7
		internal ITypeResolver typeResolver;

		// Token: 0x04000008 RID: 8
		internal readonly YamlAttributeOverrides overrides;

		// Token: 0x04000009 RID: 9
		internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;

		// Token: 0x0400000A RID: 10
		internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

		// Token: 0x0400000B RID: 11
		internal bool ignoreFields;

		// Token: 0x0400000C RID: 12
		internal bool includeNonPublicProperties;

		// Token: 0x0400000D RID: 13
		internal Settings settings;

		// Token: 0x0400000E RID: 14
		internal YamlFormatter yamlFormatter;
	}
}
