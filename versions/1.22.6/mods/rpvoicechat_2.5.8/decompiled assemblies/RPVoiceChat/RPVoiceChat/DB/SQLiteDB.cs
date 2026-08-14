using System;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;

namespace RPVoiceChat.DB
{
	// Token: 0x0200007F RID: 127
	internal sealed class SQLiteDB : SQLiteDBConnection
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0001646D File Offset: 0x0001466D
		public SqliteConnection connection
		{
			get
			{
				return this.sqliteConn;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060003CA RID: 970 RVA: 0x00016478 File Offset: 0x00014678
		// (remove) Token: 0x060003CB RID: 971 RVA: 0x000164B0 File Offset: 0x000146B0
		public event Action<SqliteConnection> OnCreateTables;

		// Token: 0x060003CC RID: 972 RVA: 0x000164E5 File Offset: 0x000146E5
		public SQLiteDB(ILogger logger) : base(logger)
		{
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000164F0 File Offset: 0x000146F0
		public void Open(string filePath)
		{
			string error = null;
			if (!base.OpenOrCreate(filePath, ref error, true, true, false))
			{
				throw new Exception("Failed to create data repository: " + error);
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001651E File Offset: 0x0001471E
		protected override void CreateTablesIfNotExists(SqliteConnection connection)
		{
			Action<SqliteConnection> onCreateTables = this.OnCreateTables;
			if (onCreateTables == null)
			{
				return;
			}
			onCreateTables.Invoke(connection);
		}
	}
}
