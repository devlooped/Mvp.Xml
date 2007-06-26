using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlServerCe;
using System.Threading;
using System.Security.Principal;

namespace CustomerSample
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

			// Initialize SyncDB
			SqlCeConnection cn = new SqlCeConnection(Properties.Settings.Default.SyncDB);
			if (!File.Exists(cn.Database))
				new SqlCeEngine(Properties.Settings.Default.SyncDB).CreateDatabase();
			cn = new SqlCeConnection(Properties.Settings.Default.DB);
			if (!File.Exists(cn.Database))
				new SqlCeEngine(Properties.Settings.Default.DB).CreateDatabase();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}