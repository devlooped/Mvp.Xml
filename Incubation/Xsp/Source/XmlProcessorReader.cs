using System;
using System.Xml;
using System.Collections.Generic;

namespace Mvp.Xml
{
	/// <summary>
	/// Implements the streaming processing reader. 
	/// </summary>
	/// <remarks>
	/// This reader always parses all elements as having full 
	/// end  elements, so that no empty elements are reported. 
	/// <para>
	/// This behavior does not modify the infoset for 
	/// a document, but allows the important feature of being able to match or 
	/// perform processing when elements end, in a deterministic way.
	/// </para>
	/// </remarks>
	public class XmlProcessorReader : WrappingXmlReader
	{
		private List<XmlProcessor> processors = new List<XmlProcessor>();

		public XmlProcessorReader(XmlReader baseReader)
			: base(new FullEndElementReader(baseReader))
		{
		}

		/// <summary>
		/// Reads the next element from the source.
		/// </summary>
		public override bool Read()
		{
			bool read = base.Read();

			if (read)
			{
				foreach (XmlProcessor processor in processors)
				{
					// TODO: this doesn't work if processors move the reader and 
					// prevent other processors from properly tracking state 
					// via their Process implementation. 
					// For example, if inside the Action a processor 
					// calls read, the other processors would receive the Process 
					// call for the new position when they still haven't received the one 
					// from the previous read (i.e. they "see" an end element before 
					// having seen the start element).
					// See if setting a bookmark reader would solve the issue.
					XmlReader chained = processor.Process(base.InnerReader);

					// Call processors for attributes.
					for (bool go = base.MoveToFirstAttribute(); go; go = base.MoveToNextAttribute())
					{
						chained = processor.Process(chained);
					}

					if (base.HasAttributes) base.MoveToElement();

					base.InnerReader = chained;
				}
			}

			return read;
		}

		/// <summary>
		/// Reads the entire stream, calling the processors as appropriate.
		/// </summary>
		public void ReadToEnd()
		{
			while (Read()) { }
		}

		public List<XmlProcessor> Processors
		{
			get { return processors; }
		}
	}
}