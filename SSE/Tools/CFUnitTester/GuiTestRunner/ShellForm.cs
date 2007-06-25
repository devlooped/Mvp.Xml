using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Mobile.TestTools.TestRunner;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Practices.Mobile.GuiTestRunner
{
	public partial class ShellForm : Form
	{
		private TestController testController;
        private const string argumentFile = "argument.txt";
		private int passedCount;
		private int failedCount;
		private int testCount;

        public ShellForm()
		{
            InitializeComponent();

			List<string> assemblies = new List<string>(Directory.GetFiles(FileUtility.ExePath, "*.dll"));
			//string[] args = this.ReadArguments();

            LoadAssemblies(assemblies.ToArray());
			testController = new TestController(treTests.Nodes, lstResults, this);

			lblPassed.Text = String.Empty;
			lblTests.Text = String.Empty;
			lblFailed.Text = String.Empty;

			Debug.Listeners.Add(new TextBoxTraceListener(this.txtTrace));
		}

		public int NumTests
		{
			set
			{
				testCount = value;
				lblTests.Text = value.ToString(CultureInfo.CurrentCulture);
				lblTests.Update();
			}
			get { return testCount; }
		}

		public int Passed
		{
			set
			{
				passedCount = value;
				lblPassed.Text = value.ToString(CultureInfo.CurrentCulture);
				lblPassed.Update();
			}
			get { return passedCount; }
		}

		public int Failed
		{
			set
			{
				failedCount = value;
				lblFailed.Text = value.ToString(CultureInfo.CurrentCulture);
				lblFailed.Update();
			}
			get { return failedCount; }
		}

        private static string[] ReadArguments()
        {
			string argumentFilePath = Path.Combine(FileUtility.ExePath, argumentFile);
            string[] args = null;

            if (File.Exists(argumentFilePath))
            {
                using (StreamReader sr = new StreamReader(argumentFilePath))
                {
                    string line = sr.ReadToEnd();
                    args = line.Trim().Split(';');
                    if (args[0].Trim().Length == 0)
                    {
                        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, 
							Properties.Resources.ExceptionMissingDll, argumentFilePath));
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ExceptionMissingFile, argumentFilePath));
            }
            return args;
        }

        private void LoadAssemblies(string[] fileNames)
        {
			Array.Sort(fileNames);
            treTests.Nodes.Clear();
            foreach (string fileName in fileNames)
            {
                LoadAssembly(fileName);
            }
        }

		private void LoadAssembly(string fileName)
		{
            //v-sunep added because the framework treats Assembly.Load differently on the framework and the compact framework
			//v-jsocha: moved code to a common class called FileUtility.
            if (Path.GetFileName(fileName) == fileName)
            {
				fileName = Path.Combine(FileUtility.ExePath, fileName);
            }            

			TestAssemblyInfo assemblyInfo = new TestAssemblyInfo(fileName);
			TestClassInfo[] classes = assemblyInfo.GetTestClasses();
			if (classes.Length > 0)
			{
				// Make our assemblies shorter.
				TreeNode assemblyNode = new TreeNode(
					Path.GetFileNameWithoutExtension(fileName)
					.Replace("Microsoft.Practices.Mobile.", "")
					.Replace("microsoft.practices.mobile.", ""));

				foreach (TestClassInfo info in classes)
				{
					LoadClassInfo(assemblyNode, info);
				}

				treTests.Nodes.Add(assemblyNode);
			}
		}

		private static void LoadClassInfo(TreeNode assemblyNode, TestClassInfo classInfo)
		{
			TreeNode classNode = new TreeNode(classInfo.Name);
			classNode.Tag = classInfo;
			assemblyNode.Nodes.Add(classNode);

			TestMethodInfo[] methods = classInfo.TestMethods;
			foreach (TestMethodInfo method in methods)
			{
				TreeNode child = new TreeNode(method.Name);
				child.Tag = method;
				classNode.Nodes.Add(child);
			}
		}

		private void miRunSelected_Click(object sender, EventArgs e)
		{
            Cursor.Current = Cursors.WaitCursor;
			tabControl1.SelectedIndex = tabControl1.TabPages.IndexOf(tabResults);
			tabControl1.Update();					// Redraw page before we consume all CPU time
			txtTrace.Text = String.Empty;
            testController.RunTests();
            Cursor.Current = Cursors.Default;
		}

		private void miSelectAll_Click(object sender, EventArgs e)
		{
			CheckTreeNodes(treTests.Nodes, true);
		}

		private void miUnselectAll_Click(object sender, EventArgs e)
		{
			CheckTreeNodes(treTests.Nodes, false);
		}

		private void CheckTreeNodes(TreeNodeCollection nodes, bool checkedValue)
		{
			foreach (TreeNode childNode in nodes)
			{
				childNode.Checked = checkedValue;
				CheckTreeNodes(childNode.Nodes, checkedValue);
			}
		}

		//
		// v-jsocha: Added this code to check/uncheck all the children when you check/uncheck
		//			 the parent node.
		//
		private void treTests_AfterCheck(object sender, TreeViewEventArgs e)
		{
			CheckTreeNodes(e.Node.Nodes, e.Node.Checked);
		}
	}
}
