using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat.DB
{
	// Token: 0x0200007E RID: 126
	public abstract class DataRepository : IDisposable
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000163DA File Offset: 0x000145DA
		protected SqliteConnection connection { get; }

		// Token: 0x060003C5 RID: 965 RVA: 0x000163E4 File Offset: 0x000145E4
		public DataRepository(ILogger logger, string dbName)
		{
			string text = Path.Combine(GamePaths.DataPath, "ModData", RPVoiceChatMod.modID);
			Directory.CreateDirectory(text);
			string filePath = Path.Combine(text, dbName);
			this._db = new SQLiteDB(logger);
			this._db.OnCreateTables += new Action<SqliteConnection>(this.CreateTablesIfNotExists);
			this._db.Open(filePath);
			this.connection = this._db.connection;
			this.DefineCommands();
		}

		// Token: 0x060003C6 RID: 966
		protected abstract void CreateTablesIfNotExists(SqliteConnection connection);

		// Token: 0x060003C7 RID: 967
		protected abstract void DefineCommands();

		// Token: 0x060003C8 RID: 968 RVA: 0x00016460 File Offset: 0x00014660
		public virtual void Dispose()
		{
			this._db.Dispose();
		}

		// Token: 0x040001EF RID: 495
		private SQLiteDB _db;
	}
}
