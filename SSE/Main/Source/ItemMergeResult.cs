using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	public class ItemMergeResult
	{
		public ItemMergeResult(Item original, Item incoming, Item proposed, MergeOperation operation)
		{
			this.original = original;
			this.incoming = incoming;
			this.proposed = proposed;
			this.operation = operation;
		}

		private Item original;

		public Item Original
		{
			get { return original; }
		}

		private Item incoming;

		public Item Incoming
		{
			get { return incoming; }
		}

		private Item proposed;

		public Item Proposed
		{
			get { return proposed; }
		}

		private MergeOperation operation;

		public MergeOperation Operation
		{
			get { return operation; }
		}
	}
}
