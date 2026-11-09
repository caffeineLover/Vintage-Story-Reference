using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000039 RID: 57
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticBuilderSkeleton<[Nullable(0)] TBuilder> where TBuilder : StaticBuilderSkeleton<TBuilder>
	{
		// Token: 0x0600016C RID: 364 RVA: 0x00004C2C File Offset: 0x00002E2C
		internal StaticBuilderSkeleton(ITypeResolver typeResolver)
		{
			this.namingConvention = NullNamingConvention.Instance;
			this.enumNamingConvention = NullNamingConvention.Instance;
			this.yamlFormatter = YamlFormatter.Default;
			base..ctor();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList2 = lazyComponentRegistrationList;
			Type typeFromHandle = typeof(GuidConverter);
			Func<Nothing, IYamlTypeConverter> func;
			if ((func = StaticBuilderSkeleton<TBuilder>.<>c.<>9__8_0) == null)
			{
				func = (StaticBuilderSkeleton<TBuilder>.<>c.<>9__8_0 = new Func<Nothing, IYamlTypeConverter>(StaticBuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__8_0));
			}
			lazyComponentRegistrationList2.Add(typeFromHandle, func);
			this.typeConverterFactories = lazyComponentRegistrationList;
			this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.settings = new Settings();
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600016D RID: 365
		protected abstract TBuilder Self { get; }

		// Token: 0x0600016E RID: 366 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public TBuilder WithNamingConvention(INamingConvention namingConvention)
		{
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			return this.Self;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004CE6 File Offset: 0x00002EE6
		public TBuilder WithEnumNamingConvention(INamingConvention enumNamingConvention)
		{
			if (enumNamingConvention == null)
			{
				throw new ArgumentNullException("enumNamingConvention");
			}
			this.enumNamingConvention = enumNamingConvention;
			return this.Self;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00004D04 File Offset: 0x00002F04
		public TBuilder WithTypeResolver(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			return this.Self;
		}

		// Token: 0x06000171 RID: 369
		public abstract TBuilder WithTagMapping(TagName tag, Type type);

		// Token: 0x06000172 RID: 370 RVA: 0x00004D22 File Offset: 0x00002F22
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter)
		{
			Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> action;
			if ((action = StaticBuilderSkeleton<TBuilder>.<>c.<>9__15_0) == null)
			{
				action = (StaticBuilderSkeleton<TBuilder>.<>c.<>9__15_0 = new Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>(StaticBuilderSkeleton<TBuilder>.<>c.<>9.<WithTypeConverter>b__15_0));
			}
			return this.WithTypeConverter(typeConverter, action);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00004D4C File Offset: 0x00002F4C
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter, Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass16_0 CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass16_0();
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

		// Token: 0x06000174 RID: 372 RVA: 0x00004DB8 File Offset: 0x00002FB8
		public TBuilder WithTypeConverter<[Nullable(0)] TYamlTypeConverter>(WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where) where TYamlTypeConverter : IYamlTypeConverter
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass17_0<TYamlTypeConverter> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass17_0<TYamlTypeConverter>();
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

		// Token: 0x06000175 RID: 373 RVA: 0x00004E20 File Offset: 0x00003020
		public TBuilder WithoutTypeConverter<[Nullable(0)] TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter
		{
			return this.WithoutTypeConverter(typeof(TYamlTypeConverter));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004E32 File Offset: 0x00003032
		public TBuilder WithoutTypeConverter(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this.typeConverterFactories.Remove(converterType);
			return this.Self;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00004E5A File Offset: 0x0000305A
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory) where TTypeInspector : ITypeInspector
		{
			Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> action;
			if ((action = StaticBuilderSkeleton<TBuilder>.<>c__20<TTypeInspector>.<>9__20_0) == null)
			{
				action = (StaticBuilderSkeleton<TBuilder>.<>c__20<TTypeInspector>.<>9__20_0 = new Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>(StaticBuilderSkeleton<TBuilder>.<>c__20<TTypeInspector>.<>9.<WithTypeInspector>b__20_0));
			}
			return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, action);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00004E84 File Offset: 0x00003084
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory, Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass21_0<TTypeInspector> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass21_0<TTypeInspector>();
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

		// Token: 0x06000179 RID: 377 RVA: 0x00004EEC File Offset: 0x000030EC
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass22_0<TTypeInspector> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass22_0<TTypeInspector>();
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

		// Token: 0x0600017A RID: 378 RVA: 0x00004F54 File Offset: 0x00003154
		public TBuilder WithoutTypeInspector<[Nullable(0)] TTypeInspector>() where TTypeInspector : ITypeInspector
		{
			return this.WithoutTypeInspector(typeof(TTypeInspector));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00004F66 File Offset: 0x00003166
		public TBuilder WithoutTypeInspector(Type inspectorType)
		{
			if (inspectorType == null)
			{
				throw new ArgumentNullException("inspectorType");
			}
			this.typeInspectorFactories.Remove(inspectorType);
			return this.Self;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00004F8E File Offset: 0x0000318E
		public TBuilder WithYamlFormatter(YamlFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			this.yamlFormatter = formatter;
			return this.Self;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00004FAC File Offset: 0x000031AC
		protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
		{
			return this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
		}

		// Token: 0x0400004A RID: 74
		internal INamingConvention namingConvention;

		// Token: 0x0400004B RID: 75
		internal INamingConvention enumNamingConvention;

		// Token: 0x0400004C RID: 76
		internal ITypeResolver typeResolver;

		// Token: 0x0400004D RID: 77
		internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;

		// Token: 0x0400004E RID: 78
		internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

		// Token: 0x0400004F RID: 79
		internal bool includeNonPublicProperties;

		// Token: 0x04000050 RID: 80
		internal Settings settings;

		// Token: 0x04000051 RID: 81
		internal YamlFormatter yamlFormatter;
	}
}
