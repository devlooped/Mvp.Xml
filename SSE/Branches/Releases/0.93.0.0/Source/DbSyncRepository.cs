using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public class DbSyncRepository : ISyncRepository
	{
		string repositoryId;
		string connectionString;
		DbProviderFactory factory;

		public DbSyncRepository(DbProviderFactory factory, string repositoryId, string connectionString)
		{
			Guard.ArgumentNotNull(repositoryId, "repositoryId");
			Guard.ArgumentNotNull(connectionString, "connectionString");
			Guard.ArgumentNotNull(factory, "factory");

			this.repositoryId = repositoryId;
			this.connectionString = connectionString;
			this.factory = factory;

			InitializeSchema();
		}

		public Sync Get(string id)
		{
			using (DbConnection cn = OpenConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = GetSql("SELECT * FROM [{0}] WHERE Id = @id");
				AddParameter(cmd, "@id", DbType.String, id);

				DbDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					return Read(reader);
				}
				else
				{
					return null;
				}
			}
		}

		public void Save(Sync sync)
		{
			Guard.ArgumentNotNull(sync.ItemTimestamp, "sync.ItemTimestamp");

			using (DbConnection cn = OpenConnection())
			{
				StringWriter sw = new StringWriter();
				using (XmlWriter xw = XmlWriter.Create(sw))
				{
					new RssFeedWriter(xw).Write(sync);
				}

				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = GetSql(@"
					UPDATE [{0}] 
					SET Sync = @sync, ItemTimestamp = @timestamp
					WHERE Id = @id");
				AddParameter(cmd, "@id", DbType.String, sync.Id);
				AddParameter(cmd, "@sync", DbType.String, sw.ToString());
				AddParameter(cmd, "@timestamp", DbType.DateTime, sync.ItemTimestamp);
				
				int count = cmd.ExecuteNonQuery();
				if (count == 0)
				{
					cmd.CommandText = GetSql(@"
						INSERT INTO [{0}] 
						(Id, Sync, ItemTimestamp)
						VALUES 
						(@id, @sync, @timestamp)");

					cmd.ExecuteNonQuery();
				}
			}
		}

		public DateTime? GetLastSync(string feed)
		{
			using (DbConnection cn = OpenConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = GetSql("SELECT LastSync FROM [{0}_LastSync] WHERE Feed = @feed");
				AddParameter(cmd, "@feed", DbType.String, feed);

				DbDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					return reader.GetDateTime(0);
				}
				else
				{
					return null;
				}
			}
		}

		public void SetLastSync(string feed, DateTime date)
		{
			using (DbConnection cn = OpenConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = GetSql(@"
					UPDATE [{0}_LastSync] 
					SET LastSync = @lastSync
					WHERE Feed = @feed");
				AddParameter(cmd, "@feed", DbType.String, feed);
				AddParameter(cmd, "@lastSync", DbType.DateTime, date);

				int count = cmd.ExecuteNonQuery();
				if (count == 0)
				{
					cmd.CommandText = GetSql(@"
						INSERT INTO [{0}_LastSync] 
						(Feed, LastSync)
						VALUES 
						(@feed, @lastSync)");

					cmd.ExecuteNonQuery();
				}
			}
		}

		public IEnumerable<Sync> GetAll()
		{
			using (DbConnection cn = OpenConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = GetSql("SELECT * FROM [{0}]");

				DbDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					yield return Read(reader);
				}
			}
		}

		public IEnumerable<Sync> GetConflicts()
		{
			// TODO: sub-optimal.
			foreach (Sync sync in GetAll())
			{
				if (sync.Conflicts.Count > 0)
					yield return sync;
			}
		}

		private Sync Read(DbDataReader reader)
		{
			string xml = (string)reader["Sync"];
			// TODO: are we cheating here?
			XmlReader xr = XmlReader.Create(new StringReader(xml));
			xr.MoveToContent();

			Sync sync = new RssFeedReader(xr).ReadSync();
			sync.ItemTimestamp = (DateTime)reader["ItemTimestamp"];

			return sync;
		}

		private void AddParameter(DbCommand cmd, string name, DbType type, object value)
		{
			DbParameter prm = cmd.CreateParameter();
			prm.DbType = type;
			prm.Direction = ParameterDirection.Input;
			prm.ParameterName = name;
			prm.Value = value;

			cmd.Parameters.Add(prm);
		}

		private DbConnection OpenConnection()
		{
			DbConnection cn = factory.CreateConnection();
			cn.ConnectionString = connectionString;
			cn.Open();

			return cn;
		}

		private void InitializeSchema()
		{
			using (DbConnection cn = OpenConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.CommandText = GetSql(@"
					SELECT	COUNT(*) 
					FROM	[INFORMATION_SCHEMA].[TABLES] 
					WHERE	[TABLE_NAME] = '{0}'");
				cmd.Connection = cn;

				int count = Convert.ToInt32(cmd.ExecuteScalar());

				if (count == 0)
				{
					cmd.CommandText = GetSql(@"
						CREATE TABLE [{0}](
							[Id] NVARCHAR(300) NOT NULL PRIMARY KEY,
							[Sync] [NTEXT] NULL, 
							[ItemTimestamp] datetime NOT NULL
						)");
					cmd.ExecuteNonQuery();

					cmd.CommandText = GetSql(@"
						CREATE TABLE [{0}_LastSync](
							[Feed] NVARCHAR(1000) NOT NULL PRIMARY KEY,
							[LastSync] [datetime] NOT NULL
						)");
					cmd.ExecuteNonQuery();
				}
			}
		}

		private string GetSql(string sql)
		{
			return String.Format(sql, "SSE_" + repositoryId);
		}
	}
}
