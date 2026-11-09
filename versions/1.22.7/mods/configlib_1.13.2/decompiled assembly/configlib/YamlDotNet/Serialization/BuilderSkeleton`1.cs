using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class BuilderSkeleton<[Nullable(0)] TBuilder> where TBuilder : BuilderSkeleton<TBuilder>
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0000F08C File Offset: 0x0000D28C
		internal BuilderSkeleton(ITypeResolver typeResolver)
		{
			this.namingConvention = NullNamingConvention.Instance;
			this.yamlFormatter = YamlFormatter.Default;
			base..ctor();
			this.overrides = new YamlAttributeOverrides();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
			Type typeFromHandle = typeof(GuidConverter);
			Func<Nothing, IYamlTypeConverter> func;
			if ((func = BuilderSkeleton<TBuilder>.<>c.<>9__9_0) == null)
			{
				func = (BuilderSkeleton<TBuilder>.<>c.<>9__9_0 = new Func<Nothing, IYamlTypeConverter>(BuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__9_0));
			}
			lazyComponentRegistrationList.Add(typeFromHandle, func);
			Type typeFromHandle2 = typeof(SystemTypeConverter);
			Func<Nothing, IYamlTypeConverter> func2;
			if ((func2 = BuilderSkeleton<TBuilder>.<>c.<>9__9_1) == null)
			{
				func2 = (BuilderSkeleton<TBuilder>.<>c.<>9__9_1 = new Func<Nothing, IYamlTypeConverter>(BuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__9_1));
			}
			lazyComponentRegistrationList.Add(typeFromHandle2, func2);
			this.typeConverterFactories = lazyComponentRegistrationList;
			this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.settings = new Settings();
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002D9 RID: 729
		protected abstract TBuilder Self { get; }

		// Token: 0x060002DA RID: 730 RVA: 0x0000F155 File Offset: 0x0000D355
		public TBuilder IgnoreFields()
		{
			this.ignoreFields = true;
			return this.Self;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F164 File Offset: 0x0000D364
		public TBuilder IncludeNonPublicProperties()
		{
			this.includeNonPublicProperties = true;
			return this.Self;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F173 File Offset: 0x0000D373
		public TBuilder EnablePrivateConstructors()
		{
			this.settings.AllowPrivateConstructors = true;
			return this.Self;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F187 File Offset: 0x0000D387
		public TBuilder WithNamingConvention(INamingConvention namingConvention)
		{
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			return this.Self;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F1A5 File Offset: 0x0000D3A5
		public TBuilder WithTypeResolver(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			return this.Self;
		}

		// Token: 0x060002DF RID: 735
		public abstract TBuilder WithTagMapping(TagName tag, Type type);

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F1C3 File Offset: 0x0000D3C3
		public TBuilder WithAttributeOverride<[Nullable(2)] TClass>(Expression<Func<TClass, object>> propertyAccessor, Attribute attribute)
		{
			this.overrides.Add<TClass>(propertyAccessor, attribute);
			return this.Self;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		public TBuilder WithAttributeOverride(Type type, string member, Attribute attribute)
		{
			this.overrides.Add(type, member, attribute);
			return this.Self;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F1EE File Offset: 0x0000D3EE
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter)
		{
			Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> action;
			if ((action = BuilderSkeleton<TBuilder>.<>c.<>9__20_0) == null)
			{
				action = (BuilderSkeleton<TBuilder>.<>c.<>9__20_0 = new Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>(BuilderSkeleton<TBuilder>.<>c.<>9.<WithTypeConverter>b__20_0));
			}
			return this.WithTypeConverter(typeConverter, action);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F218 File Offset: 0x0000D418
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter, Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass21_0 CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass21_0();
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

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F284 File Offset: 0x0000D484
		public TBuilder WithTypeConverter<[Nullable(0)] TYamlTypeConverter>(WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where) where TYamlTypeConverter : IYamlTypeConverter
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass22_0<TYamlTypeConverter> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass22_0<TYamlTypeConverter>();
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

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		public TBuilder WithoutTypeConverter<[Nullable(0)] TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter
		{
			return this.WithoutTypeConverter(typeof(TYamlTypeConverter));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F2FE File Offset: 0x0000D4FE
		public TBuilder WithoutTypeConverter(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this.typeConverterFactories.Remove(converterType);
			return this.Self;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F326 File Offset: 0x0000D526
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory) where TTypeInspector : ITypeInspector
		{
			Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> action;
			if ((action = BuilderSkeleton<TBuilder>.<>c__25<TTypeInspector>.<>9__25_0) == null)
			{
				action = (BuilderSkeleton<TBuilder>.<>c__25<TTypeInspector>.<>9__25_0 = new Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>(BuilderSkeleton<TBuilder>.<>c__25<TTypeInspector>.<>9.<WithTypeInspector>b__25_0));
			}
			return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, action);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F350 File Offset: 0x0000D550
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory, Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass26_0<TTypeInspector> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass26_0<TTypeInspector>();
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

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F3B8 File Offset: 0x0000D5B8
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			BuilderSkeleton<TBuilder>.<>c__DisplayClass27_0<TTypeInspector> CS$<>8__locals1 = new BuilderSkeleton<TBuilder>.<>c__DisplayClass27_0<TTypeInspector>();
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

		// Token: 0x060002EA RID: 746 RVA: 0x0000F420 File Offset: 0x0000D620
		public TBuilder WithoutTypeInspector<[Nullable(0)] TTypeInspector>() where TTypeInspector : ITypeInspector
		{
			return this.WithoutTypeInspector(typeof(TTypeInspector));
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F432 File Offset: 0x0000D632
		public TBuilder WithoutTypeInspector(Type inspectorType)
		{
			if (inspectorType == null)
			{
				throw new ArgumentNullException("inspectorType");
			}
			this.typeInspectorFactories.Remove(inspectorType);
			return this.Self;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F45A File Offset: 0x0000D65A
		public TBuilder WithYamlFormatter(YamlFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			this.yamlFormatter = formatter;
			return this.Self;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000F478 File Offset: 0x0000D678
		protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
		{
			return this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
		}

		// Token: 0x0400015F RID: 351
		internal INamingConvention namingConvention;

		// Token: 0x04000160 RID: 352
		internal ITypeResolver typeResolver;

		// Token: 0x04000161 RID: 353
		internal readonly YamlAttributeOverrides overrides;

		// Token: 0x04000162 RID: 354
		internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;

		// Token: 0x04000163 RID: 355
		internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

		// Token: 0x04000164 RID: 356
		internal bool ignoreFields;

		// Token: 0x04000165 RID: 357
		internal bool includeNonPublicProperties;

		// Token: 0x04000166 RID: 358
		internal Settings settings;

		// Token: 0x04000167 RID: 359
		internal YamlFormatter yamlFormatter;
	}
}
