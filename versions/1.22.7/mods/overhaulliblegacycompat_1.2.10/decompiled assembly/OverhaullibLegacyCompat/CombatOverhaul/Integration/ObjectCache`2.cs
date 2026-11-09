using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000058 RID: 88
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ObjectCache<TKey, [Nullable(2)] TValue> : IDisposable
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x0001BE90 File Offset: 0x0001A090
		public ObjectCache(ICoreAPI api, string loggedCacheName = "cache", int cleanUpThreshold = 100, int cleanUpPeriod = 600000, bool threadSafe = true)
		{
			this._mapping = new Dictionary<TKey, TValue>();
			this._lastAccess = new Dictionary<TKey, long>();
			this._lock = new ReaderWriterLock();
			base..ctor();
			this._api = api;
			this._cleanUpThreshold = cleanUpThreshold;
			this._cleanUpPeriodMs = cleanUpPeriod;
			this._loggedCacheName = loggedCacheName;
			this._threadSafe = threadSafe;
			this._cleanUpTimer = api.World.RegisterGameTickListener(new Action<float>(this.<.ctor>b__11_0), this._cleanUpPeriodMs, this._cleanUpPeriodMs);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001BF14 File Offset: 0x0001A114
		public void Add(TKey key, TValue value)
		{
			bool threadSafe = this._threadSafe;
			if (threadSafe)
			{
				this._lock.AcquireWriterLock(5000);
			}
			this._addCountBetweenCleanUps += 1L;
			this._mapping[key] = value;
			this._lastAccess[key] = this.CurrentTime();
			if (threadSafe)
			{
				this._lock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001BF78 File Offset: 0x0001A178
		public bool Get(TKey key, [Nullable(2)] [NotNullWhen(true)] out TValue value)
		{
			bool threadSafe = this._threadSafe;
			if (threadSafe)
			{
				this._lock.AcquireWriterLock(5000);
			}
			this._getCountBetweenCleanUps += 1L;
			bool flag = this._mapping.TryGetValue(key, ref value);
			if (flag)
			{
				this._lastAccess[key] = this.CurrentTime();
			}
			if (threadSafe)
			{
				this._lock.ReleaseWriterLock();
			}
			return flag;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001BFE0 File Offset: 0x0001A1E0
		public void Clean()
		{
			long currentTime = this.CurrentTime();
			bool threadSafe = this._threadSafe;
			if (threadSafe)
			{
				this._lock.AcquireWriterLock(5000);
			}
			try
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 5);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(this._loggedCacheName);
				defaultInterpolatedStringHandler.AppendLiteral(") Starting clean up. Current world time: ");
				defaultInterpolatedStringHandler.AppendFormatted<TimeSpan>(TimeSpan.FromMilliseconds(currentTime));
				defaultInterpolatedStringHandler.AppendLiteral("\nSize: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._mapping.Count);
				defaultInterpolatedStringHandler.AppendLiteral("\n'Get' count: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._getCountBetweenCleanUps);
				defaultInterpolatedStringHandler.AppendLiteral("\n'Add' count: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._addCountBetweenCleanUps);
				LoggerUtil.Verbose(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				this._getCountBetweenCleanUps = 0L;
				this._addCountBetweenCleanUps = 0L;
				HashSet<TKey> keysToRemove = new HashSet<TKey>();
				HashSet<TValue> entities = new HashSet<TValue>();
				foreach (KeyValuePair<TKey, long> keyValuePair in this._lastAccess)
				{
					TKey tkey;
					long num;
					keyValuePair.Deconstruct(ref tkey, ref num);
					TKey key = tkey;
					long lastAccess = num;
					if (currentTime - lastAccess > (long)this._cleanUpPeriodMs)
					{
						keysToRemove.Add(key);
						entities.Add(this._mapping[key]);
					}
				}
				foreach (TKey key2 in keysToRemove)
				{
					this._mapping.Remove(key2);
					this._lastAccess.Remove(key2);
				}
				ICoreAPI api2 = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(36, 3);
				defaultInterpolatedStringHandler2.AppendLiteral("(");
				defaultInterpolatedStringHandler2.AppendFormatted(this._loggedCacheName);
				defaultInterpolatedStringHandler2.AppendLiteral(") Cleaned up '");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(keysToRemove.Count);
				defaultInterpolatedStringHandler2.AppendLiteral("' keys for '");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(entities.Count);
				defaultInterpolatedStringHandler2.AppendLiteral("' values.");
				LoggerUtil.Verbose(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			catch (Exception exception)
			{
				ICoreAPI api3 = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(27, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("(");
				defaultInterpolatedStringHandler3.AppendFormatted(this._loggedCacheName);
				defaultInterpolatedStringHandler3.AppendLiteral(") Error on cache cleanup:\n");
				defaultInterpolatedStringHandler3.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api3, this, defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			if (threadSafe)
			{
				this._lock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001C2A0 File Offset: 0x0001A4A0
		public void Clear()
		{
			if (this._threadSafe)
			{
				this._lock.AcquireWriterLock(5000);
			}
			this._mapping.Clear();
			this._lastAccess.Clear();
			if (this._threadSafe)
			{
				this._lock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
		public void Dispose()
		{
			if (this._threadSafe)
			{
				this._lock.AcquireWriterLock(5000);
			}
			this._mapping.Clear();
			this._lastAccess.Clear();
			ICoreAPI api = this._api;
			if (api != null)
			{
				api.World.UnregisterGameTickListener(this._cleanUpTimer);
			}
			this._api = null;
			if (this._threadSafe)
			{
				this._lock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001C361 File Offset: 0x0001A561
		private long CurrentTime()
		{
			ICoreAPI api = this._api;
			if (api == null)
			{
				return 0L;
			}
			return api.World.ElapsedMilliseconds;
		}

		// Token: 0x040001B5 RID: 437
		private readonly Dictionary<TKey, TValue> _mapping;

		// Token: 0x040001B6 RID: 438
		private readonly Dictionary<TKey, long> _lastAccess;

		// Token: 0x040001B7 RID: 439
		private readonly ReaderWriterLock _lock;

		// Token: 0x040001B8 RID: 440
		[Nullable(2)]
		private ICoreAPI _api;

		// Token: 0x040001B9 RID: 441
		private readonly int _cleanUpPeriodMs;

		// Token: 0x040001BA RID: 442
		private readonly long _cleanUpTimer;

		// Token: 0x040001BB RID: 443
		private readonly int _cleanUpThreshold;

		// Token: 0x040001BC RID: 444
		private readonly string _loggedCacheName;

		// Token: 0x040001BD RID: 445
		private readonly bool _threadSafe;

		// Token: 0x040001BE RID: 446
		private long _addCountBetweenCleanUps;

		// Token: 0x040001BF RID: 447
		private long _getCountBetweenCleanUps;
	}
}
