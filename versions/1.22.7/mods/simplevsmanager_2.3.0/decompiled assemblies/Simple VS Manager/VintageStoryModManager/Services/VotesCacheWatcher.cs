using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VotesCacheWatcher : IDisposable
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000D33 RID: 3379 RVA: 0x00050748 File Offset: 0x0004E948
		// (remove) Token: 0x06000D34 RID: 3380 RVA: 0x00050780 File Offset: 0x0004E980
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event EventHandler CacheChanged;

		// Token: 0x06000D35 RID: 3381 RVA: 0x000507B8 File Offset: 0x0004E9B8
		public VotesCacheWatcher(string cachePath)
		{
			if (string.IsNullOrWhiteSpace(cachePath))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", "cachePath");
			}
			this._normalizedCachePath = VotesCacheWatcher.NormalizePath(cachePath);
			string directoryName = Path.GetDirectoryName(this._normalizedCachePath);
			if (directoryName == null)
			{
				throw new ArgumentException("Cache path does not contain a directory.", "cachePath");
			}
			this._cacheDirectory = directoryName;
			this._cacheFileName = Path.GetFileName(this._normalizedCachePath);
			if (string.IsNullOrWhiteSpace(this._cacheFileName))
			{
				throw new ArgumentException("Cache path must include a file name.", "cachePath");
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00050850 File Offset: 0x0004EA50
		public void Dispose()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (!this._disposed)
				{
					this._disposed = true;
					if (this._watcher != null)
					{
						this._watcher.EnableRaisingEvents = false;
						this._watcher.Changed -= new FileSystemEventHandler(this.OnChanged);
						this._watcher.Created -= new FileSystemEventHandler(this.OnChanged);
						this._watcher.Deleted -= new FileSystemEventHandler(this.OnChanged);
						this._watcher.Renamed -= new RenamedEventHandler(this.OnRenamed);
						this._watcher.Dispose();
						this._watcher = null;
					}
					this._hasPendingChanges = false;
				}
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00050928 File Offset: 0x0004EB28
		public bool TryConsumePendingChanges()
		{
			object syncRoot = this._syncRoot;
			bool result;
			lock (syncRoot)
			{
				if (!this._hasPendingChanges)
				{
					result = false;
				}
				else
				{
					this._hasPendingChanges = false;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00050978 File Offset: 0x0004EB78
		public void EnsureWatcher()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (!this._disposed && this._watcher == null)
				{
					try
					{
						Directory.CreateDirectory(this._cacheDirectory);
						FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(this._cacheDirectory)
						{
							Filter = this._cacheFileName,
							IncludeSubdirectories = false,
							NotifyFilter = 17
						};
						fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnChanged);
						fileSystemWatcher.Created += new FileSystemEventHandler(this.OnChanged);
						fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnChanged);
						fileSystemWatcher.Renamed += new RenamedEventHandler(this.OnRenamed);
						fileSystemWatcher.EnableRaisingEvents = true;
						this._watcher = fileSystemWatcher;
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00050A60 File Offset: 0x0004EC60
		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			this.HandlePotentialChange(e.FullPath);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00050A6E File Offset: 0x0004EC6E
		private void OnRenamed(object sender, RenamedEventArgs e)
		{
			this.HandlePotentialChange(e.FullPath);
			this.HandlePotentialChange(e.OldFullPath);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00050A88 File Offset: 0x0004EC88
		[NullableContext(2)]
		private void HandlePotentialChange(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			string text;
			try
			{
				text = VotesCacheWatcher.NormalizePath(path);
			}
			catch (Exception)
			{
				return;
			}
			if (!string.Equals(text, this._normalizedCachePath, 5))
			{
				return;
			}
			object syncRoot = this._syncRoot;
			EventHandler cacheChanged;
			lock (syncRoot)
			{
				if (this._disposed)
				{
					return;
				}
				bool flag2 = !this._hasPendingChanges;
				this._hasPendingChanges = true;
				if (!flag2)
				{
					return;
				}
				cacheChanged = this.CacheChanged;
			}
			if (cacheChanged != null)
			{
				cacheChanged.Invoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00050B28 File Offset: 0x0004ED28
		private static string NormalizePath(string value)
		{
			return Path.GetFullPath(value);
		}

		// Token: 0x040005D1 RID: 1489
		private readonly string _normalizedCachePath;

		// Token: 0x040005D2 RID: 1490
		private readonly string _cacheDirectory;

		// Token: 0x040005D3 RID: 1491
		private readonly string _cacheFileName;

		// Token: 0x040005D4 RID: 1492
		private readonly object _syncRoot = new object();

		// Token: 0x040005D5 RID: 1493
		private bool _disposed;

		// Token: 0x040005D6 RID: 1494
		private bool _hasPendingChanges;

		// Token: 0x040005D7 RID: 1495
		[Nullable(2)]
		private FileSystemWatcher _watcher;
	}
}
