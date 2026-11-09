using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000056 RID: 86
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ClientSettingsWatcher : IDisposable
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x00035654 File Offset: 0x00033854
		public ClientSettingsWatcher(string settingsPath)
		{
			if (string.IsNullOrWhiteSpace(settingsPath))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", "settingsPath");
			}
			this._normalizedSettingsPath = ClientSettingsWatcher.NormalizePath(settingsPath);
			string directoryName = Path.GetDirectoryName(this._normalizedSettingsPath);
			if (directoryName == null)
			{
				throw new ArgumentException("Settings path does not contain a directory.", "settingsPath");
			}
			this._settingsDirectory = directoryName;
			this._settingsFileName = Path.GetFileName(this._normalizedSettingsPath);
			if (string.IsNullOrWhiteSpace(this._settingsFileName))
			{
				throw new ArgumentException("Settings path must include a file name.", "settingsPath");
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000356EC File Offset: 0x000338EC
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

		// Token: 0x060008CA RID: 2250 RVA: 0x000357C4 File Offset: 0x000339C4
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

		// Token: 0x060008CB RID: 2251 RVA: 0x00035814 File Offset: 0x00033A14
		public void SignalPendingChange()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (!this._disposed)
				{
					this._hasPendingChanges = true;
				}
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00035860 File Offset: 0x00033A60
		public void EnsureWatcher()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (!this._disposed && this._watcher == null)
				{
					try
					{
						Directory.CreateDirectory(this._settingsDirectory);
						FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(this._settingsDirectory)
						{
							Filter = this._settingsFileName,
							IncludeSubdirectories = false,
							NotifyFilter = 89
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

		// Token: 0x060008CD RID: 2253 RVA: 0x00035948 File Offset: 0x00033B48
		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			this.HandlePotentialChange(e.FullPath);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00035956 File Offset: 0x00033B56
		private void OnRenamed(object sender, RenamedEventArgs e)
		{
			this.HandlePotentialChange(e.FullPath);
			this.HandlePotentialChange(e.OldFullPath);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00035970 File Offset: 0x00033B70
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
				text = ClientSettingsWatcher.NormalizePath(path);
			}
			catch (Exception)
			{
				return;
			}
			if (!string.Equals(text, this._normalizedSettingsPath, 5))
			{
				return;
			}
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (!this._disposed)
				{
					this._hasPendingChanges = true;
				}
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000359F0 File Offset: 0x00033BF0
		private static string NormalizePath(string value)
		{
			return Path.GetFullPath(value);
		}

		// Token: 0x04000451 RID: 1105
		private readonly string _normalizedSettingsPath;

		// Token: 0x04000452 RID: 1106
		private readonly string _settingsDirectory;

		// Token: 0x04000453 RID: 1107
		private readonly string _settingsFileName;

		// Token: 0x04000454 RID: 1108
		private readonly object _syncRoot = new object();

		// Token: 0x04000455 RID: 1109
		private bool _disposed;

		// Token: 0x04000456 RID: 1110
		private bool _hasPendingChanges;

		// Token: 0x04000457 RID: 1111
		[Nullable(2)]
		private FileSystemWatcher _watcher;
	}
}
