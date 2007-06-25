using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Mobile.TestTools.TestRunner;
using System.IO;

namespace Microsoft.Practices.Mobile.GuiTestRunner
{
	public class TestController
	{
		private TreeNodeCollection testNodes;
		private ListView resultView;
		private ShellForm mainForm;
		private StreamWriter logOutput;

		public TestController(TreeNodeCollection testNodes, ListView resultView, ShellForm form)
		{
			this.testNodes = testNodes;
			this.resultView = resultView;
			this.mainForm = form;
		}

		public void RunTests()
		{
			//mainForm.NumTests = 0;
			mainForm.Passed = 0;
			mainForm.Failed = 0;
			mainForm.NumTests = CountTests(testNodes);

			resultView.Items.Clear();
			resultView.Update();					// Update the screen right away

			string filename = Path.Combine(FileUtility.ExePath, "log.txt");
			logOutput = File.CreateText(filename);

			foreach (TreeNode node in testNodes)
			{
				RunTestMethods(node);
			}

			logOutput.Close();
		}

		private int CountTests(TreeNodeCollection nodes)
		{
			if (nodes == null)
				return 0;

			int count = 0;

			foreach (TreeNode node in nodes)
			{
				if (node.Tag is TestMethodInfo && node.Checked)
					count++;

				count += CountTests(node.Nodes);
			}
			return count;
		}

		private void RunTestMethods(TreeNode classNode)
		{
			TestClassInfo classInfo = (TestClassInfo)classNode.Tag;

			if (classInfo == null)
			{
				foreach (TreeNode node in classNode.Nodes)
				{
					RunTestMethods(node);
				}
			}
			else
			{
				TestClassRunner classRunner = new TestClassRunner(classInfo);

				foreach (TreeNode child in classNode.Nodes)
				{
					if (child.Checked)
					{
						RunTestMethod(classRunner, classInfo, (TestMethodInfo)child.Tag);
					}
				}
			}
		}

		private void RunTestMethod(TestClassRunner classRunner, TestClassInfo classInfo, TestMethodInfo methodInfo)
		{
			string result = classRunner.RunMethod(methodInfo);
			//mainForm.NumTests++;

			string pass;
			if (result == null)
			{
				pass = "-";
				mainForm.Passed++;
			}
			else
			{
				pass = "X";
				mainForm.Failed++;
			}
			string testName = methodInfo.Name;
			string testClass = classInfo.Name;
            string testModule = classInfo.ClassType.Module.Name;

            ListViewItem item = new ListViewItem(new string[] { pass, testName, result, testClass, testModule});
			logOutput.WriteLine(pass + "\t" + testName + "\t" + result + "\t" + testModule);
			logOutput.Flush();						// Make sure we don't lose if unit tester crashes
			resultView.Items.Add(item);
			resultView.Update();					// Show results immediately
		}
	}
}
