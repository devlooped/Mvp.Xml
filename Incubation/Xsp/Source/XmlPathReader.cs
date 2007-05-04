using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml
{
	/// <summary>
	/// Specialized <see cref="XmlProcessorReader"/> that 
	/// provides usability methods for adding XML path-based 
	/// matching rules.
	/// </summary>
	/// <remarks>
	/// Format for the path expression is the one supported 
	/// by the <see cref="PathExpressionParser"/>.
	/// </remarks>
	public class XmlPathReader : XmlProcessorReader
	{
		/// <summary>
		/// Initializes the reader without reporting full end elements.
		/// </summary>
		public XmlPathReader(XmlReader baseReader)
			: this(baseReader, false)
		{
		}

		/// <summary>
		/// Initializes the reader specifying whether to 
		/// report full end elements.
		/// </summary>
		public XmlPathReader(XmlReader baseReader, bool fullEndElements)
			: base(baseReader)
		{
			if (fullEndElements)
			{
				base.InnerReader = new FullEndElementReader(baseReader);
			}
		}

		/// <summary>
		/// Adds a action that will be executed with the given 
		/// <see cref="pathExpression"/> is matched by the reader.
		/// </summary>
		/// <param name="pathExpression">The path of the item to match, 
		/// with the format specified by <see cref="PathExpressionParser"/>.
		/// </param>
		/// <param name="action">The action to invoke when the current 
		/// node matches the expression.</param>
		public void AddPathAction(string pathExpression, Action<XmlReader> action)
		{
			AddPathAction(pathExpression, action,
				new XmlNamespaceManager(this.NameTable));
		}

		/// <summary>
		/// Adds a action that will be executed with the given 
		/// <see cref="pathExpression"/> is matched by the reader, specifying 
		/// whether the expression should match an end element.
		/// </summary>
		/// <param name="pathExpression">The path of the item to match, 
		/// with the format specified by <see cref="PathExpressionParser"/>.
		/// </param>
		/// <param name="matchMode">Kind of matching to perform against elements.
		/// </param>
		/// <param name="action">The action to invoke when the current 
		/// node matches the expression.</param>
		public void AddPathAction(string pathExpression, MatchMode matchMode,
			Action<XmlReader> action)
		{
			AddPathAction(pathExpression, matchMode, action,
				new XmlNamespaceManager(this.NameTable));
		}

		/// <summary>
		/// Adds a action that will be executed with the given 
		/// <see cref="pathExpression"/> is matched by the reader, using 
		/// the given namespace manager to resolve prefixes in the 
		/// expression.
		/// </summary>
		/// <param name="pathExpression">The path of the item to match, 
		/// with the format specified by <see cref="PathExpressionParser"/>.
		/// </param>
		/// <param name="action">The action to invoke when the current 
		/// node matches the expression.</param>
		/// <param name="nsManager">The <see cref="XmlNamespaceManager"/> to 
		/// use to resolve prefixes in the <paramref name="pathExpression"/>.</param>
		public void AddPathAction(string pathExpression, Action<XmlReader> action,
			XmlNamespaceManager nsManager)
		{
			base.Processors.Add(
				new XmlPathProcessor(pathExpression, action, nsManager));
		}

		/// <summary>
		/// Adds a action that will be executed with the given 
		/// <see cref="pathExpression"/> is matched by the reader, specifying 
		/// whether the expression should match an end element and the 
		/// namespace manager to resolve prefixes in the expression.
		/// </summary>
		/// <param name="pathExpression">The path of the item to match, 
		/// with the format specified by <see cref="PathExpressionParser"/>.
		/// </param>
		/// <param name="matchMode">Kind of matching to perform against elements.
		/// </param>
		/// <param name="action">The action to invoke when the current 
		/// node matches the expression.</param>
		/// <param name="nsManager">The <see cref="XmlNamespaceManager"/> to 
		/// use to resolve prefixes in the <paramref name="pathExpression"/>.</param>
		public void AddPathAction(string pathExpression, MatchMode matchMode,
			Action<XmlReader> action, XmlNamespaceManager nsManager)
		{
			base.Processors.Add(new XmlPathProcessor(
				pathExpression, matchMode, action, nsManager));
		}
	}
}
