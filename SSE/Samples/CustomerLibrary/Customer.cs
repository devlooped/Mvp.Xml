using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Data;

namespace CustomerLibrary
{
	public class Customer
	{
		public Customer()
		{
		}

		public Customer(int id, string firstName, string lastName, DateTime birthday)
		{
			this.firstName = firstName;
			this.lastName = lastName;
			this.Birthday = birthday;
		}

		public Customer(string firstName, string lastName, DateTime birthday)
			: this(-1, firstName, lastName, birthday)
		{
		}

		private int id = -1;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string firstName;

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		private string lastName;

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		private DateTime birthday;

		public DateTime Birthday
		{
			get { return birthday; }
			set { birthday = value; }
		}

		private DateTime timestamp = DateTime.Now;

		public DateTime Timestamp
		{
			get { return timestamp; }
			set { timestamp = value; }
		}
	}
}
