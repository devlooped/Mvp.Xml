using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml
{
	public enum MatchMode
	{
		/// <summary>
		/// Matches the beginning of an element.
		/// </summary>
		StartElement,
		/// <summary>
		/// Matches the end of the start element.
		/// </summary>
		/// <remarks>
		/// If the element has no attributes, it matches at the 
		/// same time as if it had been a <see cref="StartElement"/> 
		/// match mode. If there are attributes, however, 
		/// it will be matched with the last attribute. 
		/// <para>
		/// Be careful therefore if you depend on matching the attributes 
		/// before matching the start element close, as those matching 
		/// rules need to be added to the <see cref="XmlProcessorReader"/> or 
		/// <see cref="XmlPathReader"/> before adding this type of match.
		/// </para>
		/// On this mode, when the match is performed, the reader will be 
		/// positioned on the last attribute of the element if there are any, 
		/// or on the element if there are no attributes.
		/// </remarks>
		StartElementClosed,
		/// <summary>
		/// Matches the end element.
		/// </summary>
		EndElement,
		/// <summary>
		/// Default is to match the start element.
		/// </summary>
		Default = StartElement,
	}
}
