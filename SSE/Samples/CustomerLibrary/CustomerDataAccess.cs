using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;
using Mvp.Xml.Synchronization;

namespace CustomerLibrary
{
	public class CustomerDataAccess
	{
		DbProviderFactory factory;
		string connectionString;

		public CustomerDataAccess(DbProviderFactory providerFactory, string connectionString)
		{
			Guard.ArgumentNotNull(providerFactory, "providerFactory");
			Guard.ArgumentNotNullOrEmptyString(connectionString, "connectionString");

			this.factory = providerFactory;
			this.connectionString = connectionString;
		}

		public int Add(Customer customer)
		{
			Guard.ArgumentNotNull(customer, "customer");

			using (DbConnection cn = GetConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = @"
					INSERT INTO Customer
					(FirstName, LastName, Birthday)
					VALUES
					(@firstName, @lastName, @birthday)";
				AddParameter(cmd, "@firstName", DbType.String, customer.FirstName);
				AddParameter(cmd, "@lastName", DbType.String, customer.LastName);
				AddParameter(cmd, "@birthday", DbType.DateTime, customer.Birthday);

				if (cn.State != ConnectionState.Open) cn.Open();
				cmd.ExecuteNonQuery();

				// Retrieve auto-generated values.
				cmd.CommandText = "SELECT @@IDENTITY";
				int id = Convert.ToInt32(cmd.ExecuteScalar());
				cmd.CommandText = "SELECT LastUpdated FROM Customer WHERE Id=@id";
				cmd.Parameters.Clear();
				AddParameter(cmd, "@id", DbType.Int32, id);
				DateTime updated = Convert.ToDateTime(cmd.ExecuteScalar());

				customer.Id = id;
				customer.Timestamp = updated;

				return id;
			}
		}

		public bool Exists(int id)
		{
			using (DbConnection cn = GetConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = "SELECT Id FROM Customer WHERE Id = @id";
				AddParameter(cmd, "@id", DbType.Int32, id);

				if (cn.State != ConnectionState.Open) cn.Open();
				DbDataReader reader = cmd.ExecuteReader();

				return reader.Read();
			}
		}

		public bool Delete(Customer customer)
		{
			return Delete(customer.Id);
		}

		public bool Delete(int id)
		{
			using (DbConnection cn = GetConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = "DELETE FROM Customer WHERE Id=@id";
				AddParameter(cmd, "@id", DbType.Int32, id);

				if (cn.State != ConnectionState.Open) cn.Open();
				return cmd.ExecuteNonQuery() != 0;
			}
		}

		public Customer GetById(int id)
		{
			using (DbConnection cn = GetConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = "SELECT * FROM Customer WHERE Id = @id";
				AddParameter(cmd, "@id", DbType.Int32, id);

				if (cn.State != ConnectionState.Open) cn.Open();
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

		public IEnumerable<Customer> GetAll()
		{
			using (DbConnection cn = GetConnection())
			{
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = "SELECT * FROM Customer";

				if (cn.State != ConnectionState.Open) cn.Open();
				DbDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					yield return Read(reader);
				}
			}
		}

		public bool Update(Customer customer)
		{
			Guard.ArgumentNotNull(customer, "customer");

			using (DbConnection cn = GetConnection())
			{
				DateTime now = DateTime.Now;
				DbCommand cmd = factory.CreateCommand();
				cmd.Connection = cn;
				cmd.CommandText = @"
					UPDATE Customer 
					SET
						FirstName = @firstName, 
						LastName = @lastName, 
						Birthday = @birthday, 
						LastUpdated = @lastUpdated
					WHERE Id = @id";

				AddParameter(cmd, "@firstName", DbType.String, customer.FirstName);
				AddParameter(cmd, "@lastName", DbType.String, customer.LastName);
				AddParameter(cmd, "@birthday", DbType.DateTime, customer.Birthday);
				AddParameter(cmd, "@lastUpdated", DbType.DateTime, now);
				AddParameter(cmd, "@id", DbType.Int32, customer.Id);

				if (cn.State != ConnectionState.Open) cn.Open();

				bool updated = cmd.ExecuteNonQuery() != 0;

				if (updated)
				{
					customer.Timestamp = now;
				}

				return updated;
			}
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

		private Customer Read(DbDataReader reader)
		{
			Customer c = new Customer();
			c.Id = reader.GetInt32(reader.GetOrdinal("Id"));
			c.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
			c.LastName = reader.GetString(reader.GetOrdinal("LastName"));
			c.Birthday = reader.GetDateTime(reader.GetOrdinal("Birthday"));
			c.Timestamp = reader.GetDateTime(reader.GetOrdinal("LastUpdated"));

			return c;
		}

		private DbConnection GetConnection()
		{
			DbConnection cn = factory.CreateConnection();
			cn.ConnectionString = connectionString;

			// Detect if schema exists.
			DbCommand cmd = cn.CreateCommand();
			cmd.Connection = cn;
			cmd.CommandText = @"
					SELECT COUNT(*) 
					FROM [INFORMATION_SCHEMA].[TABLES] 
					WHERE [TABLE_NAME] = 'Customer'";
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
			cmd.CommandText = @"
CREATE TABLE [Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL CONSTRAINT Customer_PK PRIMARY KEY,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Birthday] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_Customer_LastUpdated]  DEFAULT (getdate())
)";
			cmd.Connection = cn;
			cmd.ExecuteNonQuery();
		}
	}
}
