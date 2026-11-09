using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Ioc : IServiceProvider
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000056C3 File Offset: 0x000038C3
		public static Ioc Default { get; } = new Ioc();

		// Token: 0x06000116 RID: 278 RVA: 0x000056CA File Offset: 0x000038CA
		[return: Nullable(2)]
		public object GetService(Type serviceType)
		{
			ArgumentNullException.ThrowIfNull(serviceType, "serviceType");
			IServiceProvider serviceProvider = this.serviceProvider;
			if (serviceProvider == null)
			{
				Ioc.ThrowInvalidOperationExceptionForMissingInitialization();
			}
			return serviceProvider.GetService(serviceType);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000056ED File Offset: 0x000038ED
		[return: Nullable(2)]
		public T GetService<T>() where T : class
		{
			IServiceProvider serviceProvider = this.serviceProvider;
			if (serviceProvider == null)
			{
				Ioc.ThrowInvalidOperationExceptionForMissingInitialization();
			}
			return (T)((object)serviceProvider.GetService(typeof(T)));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005713 File Offset: 0x00003913
		public T GetRequiredService<T>() where T : class
		{
			IServiceProvider serviceProvider = this.serviceProvider;
			if (serviceProvider == null)
			{
				Ioc.ThrowInvalidOperationExceptionForMissingInitialization();
			}
			T t = (T)((object)serviceProvider.GetService(typeof(T)));
			if (t == null)
			{
				Ioc.ThrowInvalidOperationExceptionForUnregisteredType();
			}
			return t;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005746 File Offset: 0x00003946
		public void ConfigureServices(IServiceProvider serviceProvider)
		{
			ArgumentNullException.ThrowIfNull(serviceProvider, "serviceProvider");
			if (Interlocked.CompareExchange<IServiceProvider>(ref this.serviceProvider, serviceProvider, null) != null)
			{
				Ioc.ThrowInvalidOperationExceptionForRepeatedConfiguration();
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005767 File Offset: 0x00003967
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForMissingInitialization()
		{
			throw new InvalidOperationException("The service provider has not been configured yet.");
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005773 File Offset: 0x00003973
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForUnregisteredType()
		{
			throw new InvalidOperationException("The requested service type was not registered.");
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000577F File Offset: 0x0000397F
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForRepeatedConfiguration()
		{
			throw new InvalidOperationException("The default service provider has already been configured.");
		}

		// Token: 0x04000060 RID: 96
		[Nullable(2)]
		private volatile IServiceProvider serviceProvider;
	}
}
