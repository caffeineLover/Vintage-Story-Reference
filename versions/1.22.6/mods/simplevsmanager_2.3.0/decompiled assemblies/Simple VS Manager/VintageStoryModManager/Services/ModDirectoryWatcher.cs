using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200006F RID: 111
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModDirectoryWatcher : IDisposable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000A3E RID: 2622 RVA: 0x0003F3BC File Offset: 0x0003D5BC
		// (remove) Token: 0x06000A3F RID: 2623 RVA: 0x0003F3F4 File Offset: 0x0003D5F4
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event EventHandler ChangesDetected;

		// Token: 0x06000A40 RID: 2624 RVA: 0x0003F42C File Offset: 0x0003D62C
		public ModDirectoryWatcher(ModDiscoveryService discoveryService)
		{
			if (discoveryService == null)
			{
				throw new ArgumentNullException("discoveryService");
			}
			this._discoveryService = discoveryService;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0003F480 File Offset: 0x0003D680
		public bool HasPendingChanges
		{
			get
			{
				object syncRoot = this._syncRoot;
				bool result;
				lock (syncRoot)
				{
					result = (this._requiresFullRescan || this._pendingPaths.Count > 0);
				}
				return result;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0003F4D8 File Offset: 0x0003D6D8
		public bool IsWatching
		{
			get
			{
				object syncRoot = this._syncRoot;
				bool isWatching;
				lock (syncRoot)
				{
					isWatching = this._isWatching;
				}
				return isWatching;
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0003F51C File Offset: 0x0003D71C
		public void Dispose()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (!this._disposed)
				{
					foreach (FileSystemWatcher fileSystemWatcher in this._watchers.Values)
					{
						fileSystemWatcher.EnableRaisingEvents = false;
						fileSystemWatcher.Created -= new FileSystemEventHandler(this.OnChanged);
						fileSystemWatcher.Changed -= new FileSystemEventHandler(this.OnChanged);
						fileSystemWatcher.Deleted -= new FileSystemEventHandler(this.OnChanged);
						fileSystemWatcher.Renamed -= new RenamedEventHandler(this.OnRenamed);
						fileSystemWatcher.Dispose();
					}
					this._watchers.Clear();
					this._pendingPaths.Clear();
					this._disposed = true;
					this._isWatching = false;
				}
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0003F61C File Offset: 0x0003D81C
		public void EnsureWatchers()
		{
			IReadOnlyList<string> searchPaths = this._discoveryService.GetSearchPaths();
			bool flag = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._disposed)
				{
					return;
				}
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (string path in searchPaths)
				{
					string text = ModDirectoryWatcher.NormalizePath(path);
					if (text != null)
					{
						hashSet.Add(text);
						if (!this._watchers.ContainsKey(text) && Directory.Exists(text))
						{
							try
							{
								FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(text)
								{
									IncludeSubdirectories = true,
									NotifyFilter = 27
								};
								fileSystemWatcher.Created += new FileSystemEventHandler(this.OnChanged);
								fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnChanged);
								fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnChanged);
								fileSystemWatcher.Renamed += new RenamedEventHandler(this.OnRenamed);
								fileSystemWatcher.EnableRaisingEvents = true;
								this._watchers[text] = fileSystemWatcher;
								this._requiresFullRescan = true;
								flag = true;
							}
							catch (Exception)
							{
							}
						}
					}
				}
				List<string> list = new List<string>();
				foreach (string text2 in this._watchers.Keys)
				{
					if (!hashSet.Contains(text2))
					{
						list.Add(text2);
					}
				}
				if (list.Count > 0)
				{
					foreach (string text3 in list)
					{
						FileSystemWatcher fileSystemWatcher2;
						if (this._watchers.TryGetValue(text3, ref fileSystemWatcher2))
						{
							fileSystemWatcher2.EnableRaisingEvents = false;
							fileSystemWatcher2.Created -= new FileSystemEventHandler(this.OnChanged);
							fileSystemWatcher2.Changed -= new FileSystemEventHandler(this.OnChanged);
							fileSystemWatcher2.Deleted -= new FileSystemEventHandler(this.OnChanged);
							fileSystemWatcher2.Renamed -= new RenamedEventHandler(this.OnRenamed);
							fileSystemWatcher2.Dispose();
						}
						this._watchers.Remove(text3);
					}
					this._requiresFullRescan = true;
					flag = true;
				}
				this._isWatching = (this._watchers.Count > 0);
			}
			if (flag)
			{
				this.NotifyChangeDetected();
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0003F908 File Offset: 0x0003DB08
		public ModDirectoryChangeSet ConsumeChanges()
		{
			object syncRoot = this._syncRoot;
			ModDirectoryChangeSet result;
			lock (syncRoot)
			{
				if (this._pendingPaths.Count == 0 && !this._requiresFullRescan)
				{
					result = ModDirectoryChangeSet.Empty;
				}
				else
				{
					string[] paths = (this._pendingPaths.Count == 0) ? Array.Empty<string>() : new List<string>(this._pendingPaths).ToArray();
					ModDirectoryChangeSet modDirectoryChangeSet = new ModDirectoryChangeSet(this._requiresFullRescan, paths);
					this._pendingPaths.Clear();
					this._requiresFullRescan = false;
					result = modDirectoryChangeSet;
				}
			}
			return result;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0003F9A4 File Offset: 0x0003DBA4
		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			FileSystemWatcher fileSystemWatcher = sender as FileSystemWatcher;
			if (fileSystemWatcher == null || string.IsNullOrWhiteSpace(e.FullPath))
			{
				return;
			}
			string text = ModDirectoryWatcher.TryResolveRootPath(fileSystemWatcher.Path, e.FullPath);
			if (text == null)
			{
				return;
			}
			bool flag = false;
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._pendingPaths.Add(text))
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.NotifyChangeDetected();
			}
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0003FA2C File Offset: 0x0003DC2C
		private void OnRenamed(object sender, RenamedEventArgs e)
		{
			this.OnChanged(sender, e);
			if (!string.IsNullOrWhiteSpace(e.OldFullPath))
			{
				FileSystemWatcher fileSystemWatcher = sender as FileSystemWatcher;
				if (fileSystemWatcher != null)
				{
					string text = ModDirectoryWatcher.TryResolveRootPath(fileSystemWatcher.Path, e.OldFullPath);
					if (text == null)
					{
						return;
					}
					bool flag = false;
					object syncRoot = this._syncRoot;
					lock (syncRoot)
					{
						if (this._pendingPaths.Add(text))
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.NotifyChangeDetected();
					}
					return;
				}
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0003FABC File Offset: 0x0003DCBC
		[return: Nullable(2)]
		private static string TryResolveRootPath(string searchPath, string fullPath)
		{
			string text = ModDirectoryWatcher.NormalizePath(searchPath);
			string text2 = ModDirectoryWatcher.NormalizePath(fullPath);
			if (text == null || text2 == null)
			{
				return text2;
			}
			if (!text2.StartsWith(text, 5))
			{
				return text2;
			}
			string text3 = (text2.Length == text.Length) ? string.Empty : text2.Substring(text.Length).TrimStart(ModDirectoryWatcher.DirectorySeparators);
			if (text3.Length == 0)
			{
				return null;
			}
			int num = text3.IndexOfAny(ModDirectoryWatcher.DirectorySeparators);
			string text4 = (num >= 0) ? text3.Substring(0, num) : text3;
			if (string.IsNullOrWhiteSpace(text4))
			{
				return null;
			}
			return Path.Combine(text, text4);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0003FB54 File Offset: 0x0003DD54
		[NullableContext(2)]
		private static string NormalizePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string result;
			try
			{
				result = Path.GetFullPath(path).TrimEnd(ModDirectoryWatcher.DirectorySeparators);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0003FB98 File Offset: 0x0003DD98
		private void NotifyChangeDetected()
		{
			EventHandler changesDetected = this.ChangesDetected;
			if (changesDetected == null)
			{
				return;
			}
			changesDetected.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x040004BC RID: 1212
		private static readonly char[] DirectorySeparators = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		};

		// Token: 0x040004BD RID: 1213
		private readonly ModDiscoveryService _discoveryService;

		// Token: 0x040004BE RID: 1214
		private readonly HashSet<string> _pendingPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040004BF RID: 1215
		private readonly object _syncRoot = new object();

		// Token: 0x040004C0 RID: 1216
		private readonly Dictionary<string, FileSystemWatcher> _watchers = new Dictionary<string, FileSystemWatcher>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040004C1 RID: 1217
		private bool _disposed;

		// Token: 0x040004C2 RID: 1218
		private bool _isWatching;

		// Token: 0x040004C3 RID: 1219
		private bool _requiresFullRescan;
	}
}
