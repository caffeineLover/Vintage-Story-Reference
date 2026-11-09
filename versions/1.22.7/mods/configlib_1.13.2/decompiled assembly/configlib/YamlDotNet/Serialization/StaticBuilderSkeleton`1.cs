using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000CA RID: 202
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticBuilderSkeleton<[Nullable(0)] TBuilder> where TBuilder : StaticBuilderSkeleton<TBuilder>
	{
		// Token: 0x0600047E RID: 1150 RVA: 0x00011AEC File Offset: 0x0000FCEC
		internal StaticBuilderSkeleton(ITypeResolver typeResolver)
		{
			this.namingConvention = NullNamingConvention.Instance;
			this.yamlFormatter = YamlFormatter.Default;
			base..ctor();
			LazyComponentRegistrationList<Nothing, IYamlTypeConverter> lazyComponentRegistrationList = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>();
			Type typeFromHandle = typeof(GuidConverter);
			Func<Nothing, IYamlTypeConverter> func;
			if ((func = StaticBuilderSkeleton<TBuilder>.<>c.<>9__7_0) == null)
			{
				func = (StaticBuilderSkeleton<TBuilder>.<>c.<>9__7_0 = new Func<Nothing, IYamlTypeConverter>(StaticBuilderSkeleton<TBuilder>.<>c.<>9.<.ctor>b__7_0));
			}
			lazyComponentRegistrationList.Add(typeFromHandle, func);
			this.typeConverterFactories = lazyComponentRegistrationList;
			this.typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			this.settings = new Settings();
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600047F RID: 1151
		protected abstract TBuilder Self { get; }

		// Token: 0x06000480 RID: 1152 RVA: 0x00011B7B File Offset: 0x0000FD7B
		public TBuilder WithNamingConvention(INamingConvention namingConvention)
		{
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
			return this.Self;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00011B99 File Offset: 0x0000FD99
		public TBuilder WithTypeResolver(ITypeResolver typeResolver)
		{
			if (typeResolver == null)
			{
				throw new ArgumentNullException("typeResolver");
			}
			this.typeResolver = typeResolver;
			return this.Self;
		}

		// Token: 0x06000482 RID: 1154
		public abstract TBuilder WithTagMapping(TagName tag, Type type);

		// Token: 0x06000483 RID: 1155 RVA: 0x00011BB7 File Offset: 0x0000FDB7
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter)
		{
			Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> action;
			if ((action = StaticBuilderSkeleton<TBuilder>.<>c.<>9__13_0) == null)
			{
				action = (StaticBuilderSkeleton<TBuilder>.<>c.<>9__13_0 = new Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>>(StaticBuilderSkeleton<TBuilder>.<>c.<>9.<WithTypeConverter>b__13_0));
			}
			return this.WithTypeConverter(typeConverter, action);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00011BE0 File Offset: 0x0000FDE0
		public TBuilder WithTypeConverter(IYamlTypeConverter typeConverter, Action<IRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where)
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass14_0 CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass14_0();
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

		// Token: 0x06000485 RID: 1157 RVA: 0x00011C4C File Offset: 0x0000FE4C
		public TBuilder WithTypeConverter<[Nullable(0)] TYamlTypeConverter>(WrapperFactory<IYamlTypeConverter, IYamlTypeConverter> typeConverterFactory, Action<ITrackingRegistrationLocationSelectionSyntax<IYamlTypeConverter>> where) where TYamlTypeConverter : IYamlTypeConverter
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass15_0<TYamlTypeConverter> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass15_0<TYamlTypeConverter>();
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

		// Token: 0x06000486 RID: 1158 RVA: 0x00011CB4 File Offset: 0x0000FEB4
		public TBuilder WithoutTypeConverter<[Nullable(0)] TYamlTypeConverter>() where TYamlTypeConverter : IYamlTypeConverter
		{
			return this.WithoutTypeConverter(typeof(TYamlTypeConverter));
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00011CC6 File Offset: 0x0000FEC6
		public TBuilder WithoutTypeConverter(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this.typeConverterFactories.Remove(converterType);
			return this.Self;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00011CEE File Offset: 0x0000FEEE
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory) where TTypeInspector : ITypeInspector
		{
			Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> action;
			if ((action = StaticBuilderSkeleton<TBuilder>.<>c__18<TTypeInspector>.<>9__18_0) == null)
			{
				action = (StaticBuilderSkeleton<TBuilder>.<>c__18<TTypeInspector>.<>9__18_0 = new Action<IRegistrationLocationSelectionSyntax<ITypeInspector>>(StaticBuilderSkeleton<TBuilder>.<>c__18<TTypeInspector>.<>9.<WithTypeInspector>b__18_0));
			}
			return this.WithTypeInspector<TTypeInspector>(typeInspectorFactory, action);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00011D18 File Offset: 0x0000FF18
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(Func<ITypeInspector, TTypeInspector> typeInspectorFactory, Action<IRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass19_0<TTypeInspector> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass19_0<TTypeInspector>();
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

		// Token: 0x0600048A RID: 1162 RVA: 0x00011D80 File Offset: 0x0000FF80
		public TBuilder WithTypeInspector<[Nullable(0)] TTypeInspector>(WrapperFactory<ITypeInspector, ITypeInspector, TTypeInspector> typeInspectorFactory, Action<ITrackingRegistrationLocationSelectionSyntax<ITypeInspector>> where) where TTypeInspector : ITypeInspector
		{
			StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass20_0<TTypeInspector> CS$<>8__locals1 = new StaticBuilderSkeleton<TBuilder>.<>c__DisplayClass20_0<TTypeInspector>();
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

		// Token: 0x0600048B RID: 1163 RVA: 0x00011DE8 File Offset: 0x0000FFE8
		public TBuilder WithoutTypeInspector<[Nullable(0)] TTypeInspector>() where TTypeInspector : ITypeInspector
		{
			return this.WithoutTypeInspector(typeof(TTypeInspector));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00011DFA File Offset: 0x0000FFFA
		public TBuilder WithoutTypeInspector(Type inspectorType)
		{
			if (inspectorType == null)
			{
				throw new ArgumentNullException("inspectorType");
			}
			this.typeInspectorFactories.Remove(inspectorType);
			return this.Self;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00011E22 File Offset: 0x00010022
		public TBuilder WithYamlFormatter(YamlFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			this.yamlFormatter = formatter;
			return this.Self;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00011E40 File Offset: 0x00010040
		protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
		{
			return this.typeConverterFactories.BuildComponentList<IYamlTypeConverter>();
		}

		// Token: 0x040001F4 RID: 500
		internal INamingConvention namingConvention;

		// Token: 0x040001F5 RID: 501
		internal ITypeResolver typeResolver;

		// Token: 0x040001F6 RID: 502
		internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;

		// Token: 0x040001F7 RID: 503
		internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;

		// Token: 0x040001F8 RID: 504
		internal bool includeNonPublicProperties;

		// Token: 0x040001F9 RID: 505
		internal Settings settings;

		// Token: 0x040001FA RID: 506
		internal YamlFormatter yamlFormatter;
	}
}
