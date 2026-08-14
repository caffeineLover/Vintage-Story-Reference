using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;

namespace RPVoiceChat.DB
{
	// Token: 0x0200007D RID: 125
	public class ClientSettingsRepository : DataRepository
	{
		// Token: 0x060003BC RID: 956 RVA: 0x00016122 File Offset: 0x00014322
		public ClientSettingsRepository(ILogger logger) : base(logger, "client.db")
		{
			this.Load();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00016136 File Offset: 0x00014336
		public float GetPlayerGain(string playerId)
		{
			if (this.cache.ContainsKey(playerId))
			{
				return this.cache[playerId];
			}
			this.SetPlayerGain(playerId, 100);
			return this.cache[playerId];
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016168 File Offset: 0x00014368
		public void SetPlayerGain(string playerId, int value)
		{
			float gain = (float)value / 100f;
			this.cache[playerId] = gain;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001618C File Offset: 0x0001438C
		public void Save()
		{
			using (SqliteTransaction transaction = base.connection.BeginTransaction())
			{
				foreach (KeyValuePair<string, float> entry in this.cache)
				{
					this.setSettingCmd.Transaction = transaction;
					this.setSettingCmd.Parameters["@playerId"].Value = entry.Key;
					this.setSettingCmd.Parameters["@gain"].Value = entry.Value;
					this.setSettingCmd.ExecuteNonQuery();
				}
				transaction.Commit();
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00016264 File Offset: 0x00014464
		protected override void CreateTablesIfNotExists(SqliteConnection connection)
		{
			using (SqliteCommand command = connection.CreateCommand())
			{
				command.CommandText = "CREATE TABLE IF NOT EXISTS settings (playerId TEXT PRIMARY KEY, gain REAL);";
				command.ExecuteNonQuery();
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000162A8 File Offset: 0x000144A8
		protected override void DefineCommands()
		{
			this.getSettingsCmd = base.connection.CreateCommand();
			this.getSettingsCmd.CommandText = "SELECT * FROM settings";
			this.getSettingsCmd.Prepare();
			this.setSettingCmd = base.connection.CreateCommand();
			this.setSettingCmd.CommandText = "INSERT OR REPLACE INTO settings (playerId, gain) VALUES (@playerId, @gain)";
			this.setSettingCmd.Parameters.Add("@playerId", 3, 64);
			this.setSettingCmd.Parameters.Add("@gain", 2);
			this.setSettingCmd.Prepare();
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00016340 File Offset: 0x00014540
		private void Load()
		{
			Dictionary<string, float> entries = new Dictionary<string, float>();
			using (SqliteDataReader datareader = this.getSettingsCmd.ExecuteReader())
			{
				while (datareader.Read())
				{
					string playerId = datareader["playerId"].ToString();
					float gain = datareader.GetFloat(1);
					entries.Add(playerId, gain);
				}
			}
			this.cache = entries;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000163B0 File Offset: 0x000145B0
		public override void Dispose()
		{
			SqliteCommand sqliteCommand = this.getSettingsCmd;
			if (sqliteCommand != null)
			{
				sqliteCommand.Dispose();
			}
			SqliteCommand sqliteCommand2 = this.setSettingCmd;
			if (sqliteCommand2 != null)
			{
				sqliteCommand2.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x040001E9 RID: 489
		private const string dbName = "client.db";

		// Token: 0x040001EA RID: 490
		private const string tableName = "settings";

		// Token: 0x040001EB RID: 491
		private SqliteCommand getSettingsCmd;

		// Token: 0x040001EC RID: 492
		private SqliteCommand setSettingCmd;

		// Token: 0x040001ED RID: 493
		private Dictionary<string, float> cache;
	}
}
