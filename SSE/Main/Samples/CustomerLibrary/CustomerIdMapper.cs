using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;

namespace CustomerLibrary
{
	public class CustomerIdMapper
	{
		const string RepositoryId = "Customers";
		const string RepositoryMapping = RepositoryId + "IdMapping";
		DbProviderFactory factory;
		private string connectionString;

		public CustomerIdMapper(DbProviderFactory providerFactory, string connectionString)
		{
			this.factory = providerFactory;
			this.connectionString = connectionString;
		}

		public int Map(string id)
		{
			using (DbConnection cn = GetConnection())
			{
				using (DbCommand cmd = factory.CreateCommand())
				{
					cmd.Connection = cn;
					cmd.CommandText = GetSql("SELECT Id FROM {0} WHERE Guid = @guid");
					AddParameter(cmd, "@guid", DbType.String, id);

					if (cn.State != ConnectionState.Open) cn.Open();

					object value = cmd.ExecuteScalar();
					if (value == null)
					{
						return -1;
					}
					else
					{
						return Convert.ToInt32(value);
					}
				}
			}
		}

		public string Map(int id)
		{
			using (DbConnection cn = GetConnection())
			{
				using (DbCommand cmd = factory.CreateCommand())
				{
					cmd.Connection = cn;
					cmd.CommandText = GetSql("SELECT Guid FROM {0} WHERE Id = @id");
					AddParameter(cmd, "@id", DbType.Int32, id);

					if (cn.State != ConnectionState.Open) cn.Open();

					object value = cmd.ExecuteScalar();
					if (value == null)
					{
						// Generate a new ID on-the-fly
						string guid = Guid.NewGuid().ToString();

						cmd.CommandText = GetSql("INSERT INTO {0} (Guid, Id) VALUES (@guid, @id)");
						AddParameter(cmd, "@guid", DbType.String, guid);
						cmd.ExecuteNonQuery();

						return guid;
					}
					else
					{
						return (string)value;
					}
				}
			}
		}

		public void Map(string itemId, int customerId)
		{
			using (DbConnection cn = GetConnection())
			{
				using (DbCommand cmd = factory.CreateCommand())
				{
					cmd.Connection = cn;
					cmd.CommandText = GetSql("SELECT Id FROM {0} WHERE Guid = @guid");
					AddParameter(cmd, "@guid", DbType.String, itemId);

					if (cn.State != ConnectionState.Open) cn.Open();

					object value = cmd.ExecuteScalar();
					if (value == null)
					{
						cmd.CommandText = GetSql("INSERT INTO {0} (Guid, Id) VALUES (@guid, @id)");
						AddParameter(cmd, "@id", DbType.Int32, customerId);
						cmd.ExecuteNonQuery();
					}
					else
					{
						cmd.CommandText = GetSql("UPDATE {0} SET Id = @id WHERE Guid = @guid");
						AddParameter(cmd, "@id", DbType.Int32, customerId);
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

		private void AddParameter(DbCommand cmd, string name, DbType type, object value)
		{
			DbParameter prm = cmd.CreateParameter();
			prm.DbType = type;
			prm.ParameterName = name;
			prm.Value = value;
			prm.Direction = ParameterDirection.Input;

			cmd.Parameters.Add(prm);
		}

		private DbConnection GetConnection()
		{
			DbConnection cn = factory.CreateConnection();
			cn.ConnectionString = connectionString;

			// Detect if schema exists.
			DbCommand cmd = cn.CreateCommand();
			cmd.Connection = cn;
			cmd.CommandText = GetSql(@"
					SELECT COUNT(*) 
					FROM [INFORMATION_SCHEMA].[TABLES] 
					WHERE [TABLE_NAME] = '{0}'");
			if (cn.State != ConnectionState.Open) cn.Open();
			int count = (int)cmd.ExecuteScalar();

			if (count == 0)
			{
				CreateSchema(cn);
			}

			return cn;
		}

		private void CreateSchema(DbConnection cn)
		{
			if (cn.State != ConnectionState.Open) cn.Open();

			DbCommand cmd = factory.CreateCommand();
			cmd.CommandText = GetSql(@"
CREATE TABLE [{0}](
	[Guid] NVARCHAR(300) NOT NULL,
	[Id] [int] NOT NULL,
	CONSTRAINT [PK_{0}] PRIMARY KEY 
	(
		[Guid]
	),
	CONSTRAINT [IX_Foo] UNIQUE 
	(
		[Id]
	)
)");
			cmd.Connection = cn;
			cmd.ExecuteNonQuery();
		}

		private string GetSql(string sql)
		{
			return String.Format(sql, RepositoryMapping);
		}
	}
}
