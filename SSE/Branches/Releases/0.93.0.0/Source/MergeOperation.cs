using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	public enum MergeOperation
	{
		Added,
		Deleted,
		Updated, 
		Conflict,
		None
	}
}
