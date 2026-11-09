using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.Common.Database;

public class SQLiteDbConnectionv1 : IGameDbConnection, IDisposable
{
	private SqliteConnection sqliteConn;

	private string databaseFileName;

	public ILogger logger = new NullLogger();

	private static int MapChunkYCoord = 99998;

	private static int MapRegionYCoord = 99999;

	private static ulong pow20minus1 = 1048575uL;

	public bool IsReadOnly => false;

	public SQLiteDbConnectionv1(ILogger logger)
	{
		this.logger = logger;
	}

	public bool OpenOrCreate(string filename, ref string errorMessage, bool requireWriteAccess, bool corruptionProtection, bool doIntegrityCheck)
	{
		try
		{
			databaseFileName = filename;
			bool newdatabase = !File.Exists(databaseFileName);
			DbConnectionStringBuilder conf = new DbConnectionStringBuilder
			{
				{ "Data Source", databaseFileName },
				{ "Pooling", "false" }
			};
			sqliteConn = new SqliteConnection(conf.ToString());
			sqliteConn.Open();
			using (SqliteCommand cmd = sqliteConn.CreateCommand())
			{
				cmd.CommandText = "PRAGMA journal_mode=Off;";
				cmd.ExecuteNonQuery();
			}
			if (newdatabase)
			{
				CreateTables(sqliteConn);
			}
			if (doIntegrityCheck && !integrityCheck(sqliteConn))
			{
				logger.Error("Database is possibly corrupted.");
			}
		}
		catch (Exception e)
		{
			logger.Error(errorMessage = "Failed opening savegame.");
			logger.Error(e);
			return false;
		}
		return true;
	}

	public void Close()
	{
		sqliteConn.Close();
		sqliteConn.Dispose();
	}

	public void Dispose()
	{
		Close();
	}

	private void CreateTables(SqliteConnection sqliteConn)
	{
		using SqliteCommand sqlite_cmd = sqliteConn.CreateCommand();
		sqlite_cmd.CommandText = "CREATE TABLE chunks (position integer PRIMARY KEY, data BLOB);";
		sqlite_cmd.ExecuteNonQuery();
	}

	public void CreateBackup(string backupFilename)
	{
		if (databaseFileName == backupFilename)
		{
			logger.Warning("Cannot overwrite current running database. Chose another destination.");
			return;
		}
		if (File.Exists(backupFilename))
		{
			logger.Notification("File " + backupFilename + " exists. Overwriting file.");
		}
		SqliteConnection sqliteBckConn = new SqliteConnection(new DbConnectionStringBuilder
		{
			{
				"Data Source",
				Path.Combine(GamePaths.Backups, backupFilename)
			},
			{ "Pooling", "false" }
		}.ToString());
		sqliteBckConn.Open();
		using (SqliteCommand configCmd = sqliteBckConn.CreateCommand())
		{
			configCmd.CommandText = "PRAGMA journal_mode=Off;";
			configCmd.ExecuteNonQuery();
		}
		sqliteConn.BackupDatabase(sqliteBckConn, sqliteBckConn.Database, sqliteConn.Database);
		sqliteBckConn.Close();
		sqliteBckConn.Dispose();
	}

	public void BackupPartial(string backupFilename, Vec2i start, Vec2i end, int mapSizeYinChunks)
	{
		logger.Warning("BackupPartial is not supported for database version 1");
	}

	private bool integrityCheck(SqliteConnection sqliteConn)
	{
		bool okay = false;
		using SqliteCommand command = sqliteConn.CreateCommand();
		command.CommandText = "PRAGMA integrity_check";
		using SqliteDataReader sqlite_datareader = command.ExecuteReader();
		logger.Notification($"Database: {sqliteConn.DataSource}. Running SQLite integrity check...");
		while (sqlite_datareader.Read())
		{
			logger.Notification("Integrity check " + sqlite_datareader[0].ToString());
			if (sqlite_datareader[0].ToString() == "ok")
			{
				okay = true;
				break;
			}
		}
		return okay;
	}

	public int QuantityChunks()
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT count(*) FROM chunks";
		return System.Convert.ToInt32(cmd.ExecuteScalar());
	}

	public IEnumerable<byte[]> GetChunks(IEnumerable<Vec3i> chunkpositions)
	{
		using SqliteTransaction transaction = sqliteConn.BeginTransaction();
		foreach (Vec3i xyz in chunkpositions)
		{
			ulong pos = ToMapPos(xyz.X, xyz.Y, xyz.Z);
			yield return GetChunk(pos);
		}
		transaction.Commit();
	}

	public IEnumerable<DbChunk> GetAllChunks()
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT position, data FROM chunks";
		using SqliteDataReader reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			object value = reader["position"];
			object data = reader["data"];
			ulong posUlong = System.Convert.ToUInt64(value);
			Vec3i pos = FromMapPos(posUlong);
			if (pos.Y != MapChunkYCoord && pos.Y != MapRegionYCoord && posUlong != long.MaxValue)
			{
				yield return new DbChunk
				{
					Position = new ChunkPos(pos),
					Data = (data as byte[])
				};
			}
		}
	}

	public IEnumerable<DbChunk> GetAllMapChunks()
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT position, data FROM chunks";
		using SqliteDataReader sqlite_datareader = cmd.ExecuteReader();
		while (sqlite_datareader.Read())
		{
			object value = sqlite_datareader["position"];
			object data = sqlite_datareader["data"];
			ulong posUlong = System.Convert.ToUInt64(value);
			Vec3i pos = FromMapPos(posUlong);
			if (pos.Y == MapChunkYCoord && posUlong != long.MaxValue)
			{
				yield return new DbChunk
				{
					Position = new ChunkPos(pos),
					Data = (data as byte[])
				};
			}
		}
	}

	public IEnumerable<DbChunk> GetAllMapRegions()
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT position, data FROM chunks";
		using SqliteDataReader sqlite_datareader = cmd.ExecuteReader();
		while (sqlite_datareader.Read())
		{
			object value = sqlite_datareader["position"];
			object data = sqlite_datareader["data"];
			ulong posUlong = System.Convert.ToUInt64(value);
			Vec3i pos = FromMapPos(posUlong);
			if (pos.Y == MapRegionYCoord && posUlong != long.MaxValue)
			{
				yield return new DbChunk
				{
					Position = new ChunkPos(pos),
					Data = (data as byte[])
				};
			}
		}
	}

	public void ForAllChunks(Action<DbChunk> action)
	{
		throw new NotImplementedException();
	}

	public byte[] GetMapChunk(ulong position)
	{
		Vec3i vec = FromMapPos(position);
		position = ToMapPos(vec.X, MapChunkYCoord, vec.Z);
		return GetChunk(position);
	}

	public byte[] GetMapRegion(ulong position)
	{
		Vec3i vec = FromMapPos(position);
		position = ToMapPos(vec.X, MapRegionYCoord, vec.Z);
		return GetChunk(position);
	}

	public byte[] GetChunk(ulong position)
	{
		using SqliteCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "SELECT data FROM chunks WHERE position=@position";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, position, cmd));
		using SqliteDataReader dataReader = cmd.ExecuteReader();
		if (dataReader.Read())
		{
			return dataReader["data"] as byte[];
		}
		return null;
	}

	public void DeleteMapChunks(IEnumerable<ChunkPos> coords)
	{
		using SqliteTransaction transaction = sqliteConn.BeginTransaction();
		foreach (ChunkPos vec in coords)
		{
			DeleteChunk(ToMapPos(vec.X, MapChunkYCoord, vec.Z));
		}
		transaction.Commit();
	}

	public void DeleteMapRegions(IEnumerable<ChunkPos> coords)
	{
		using SqliteTransaction transaction = sqliteConn.BeginTransaction();
		foreach (ChunkPos vec in coords)
		{
			DeleteChunk(ToMapPos(vec.X, MapRegionYCoord, vec.Z));
		}
		transaction.Commit();
	}

	public void DeleteChunks(IEnumerable<ChunkPos> coords)
	{
		using SqliteTransaction transaction = sqliteConn.BeginTransaction();
		foreach (ChunkPos vec in coords)
		{
			DeleteChunk(ToMapPos(vec.X, vec.Y, vec.Z));
		}
		transaction.Commit();
	}

	public void DeleteChunk(ulong position)
	{
		using DbCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "DELETE FROM chunks WHERE position=@position";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, position, cmd));
		cmd.ExecuteNonQuery();
	}

	public void SetChunks(IEnumerable<DbChunk> chunks)
	{
		using SqliteTransaction transaction = sqliteConn.BeginTransaction();
		foreach (DbChunk c in chunks)
		{
			ulong pos = ToMapPos(c.Position.X, c.Position.Y, c.Position.Z);
			InsertChunk(pos, c.Data);
		}
		transaction.Commit();
	}

	public void SetMapChunks(IEnumerable<DbChunk> mapchunks)
	{
		foreach (DbChunk mapchunk in mapchunks)
		{
			mapchunk.Position.Y = MapChunkYCoord;
		}
		SetChunks(mapchunks);
	}

	public void SetMapRegions(IEnumerable<DbChunk> mapregions)
	{
		foreach (DbChunk mapregion in mapregions)
		{
			mapregion.Position.Y = MapRegionYCoord;
		}
		SetChunks(mapregions);
	}

	private void InsertChunk(ulong position, byte[] data)
	{
		using DbCommand cmd = sqliteConn.CreateCommand();
		cmd.CommandText = "INSERT OR REPLACE INTO chunks (position, data) VALUES (@position,@data)";
		cmd.Parameters.Add(CreateParameter("position", DbType.UInt64, position, cmd));
		cmd.Parameters.Add(CreateParameter("data", DbType.Object, data, cmd));
		cmd.ExecuteNonQuery();
	}

	private DbParameter CreateParameter(string parameterName, DbType dbType, object value, DbCommand command)
	{
		DbParameter dbParameter = command.CreateParameter();
		dbParameter.ParameterName = parameterName;
		dbParameter.DbType = dbType;
		dbParameter.Value = value;
		return dbParameter;
	}

	public byte[] GetGameData()
	{
		try
		{
			return GetChunk(9223372036854775807uL);
		}
		catch (Exception ex)
		{
			logger.Warning("Exception thrown on GetGlobalData: " + ex.Message);
			return null;
		}
	}

	public void StoreGameData(byte[] data)
	{
		using SqliteTransaction transaction = sqliteConn.BeginTransaction();
		InsertChunk(9223372036854775807uL, data);
		transaction.Commit();
	}

	public bool QuickCorrectSaveGameVersionTest()
	{
		return true;
	}

	public static Vec3i FromMapPos(ulong v)
	{
		uint z = (uint)(v & pow20minus1);
		v >>= 20;
		uint y = (uint)(v & pow20minus1);
		v >>= 20;
		return new Vec3i((int)(v & pow20minus1), (int)y, (int)z);
	}

	public static ulong ToMapPos(int x, int y, int z)
	{
		return (ulong)(((long)x << 40) | ((long)y << 20) | (uint)z);
	}

	public byte[] GetPlayerData(string playeruid)
	{
		throw new NotImplementedException();
	}

	public void SetPlayerData(string playeruid, byte[] data)
	{
		throw new NotImplementedException();
	}

	public void UpgradeToWriteAccess()
	{
	}

	public bool IntegrityCheck()
	{
		throw new NotImplementedException();
	}

	public bool ChunkExists(ulong position)
	{
		throw new NotImplementedException();
	}

	public bool MapChunkExists(ulong position)
	{
		throw new NotImplementedException();
	}

	public bool MapRegionExists(ulong position)
	{
		throw new NotImplementedException();
	}

	public void Vacuum()
	{
		using SqliteCommand command = sqliteConn.CreateCommand();
		command.CommandText = "VACUUM;";
		command.ExecuteNonQuery();
	}
}
