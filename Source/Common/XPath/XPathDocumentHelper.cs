using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Security.Permissions;
using System.Xml.XPath;
using System.Xml;

namespace Mvp.Xml.Common.XPath
{
	/// <summary>
	/// Helper class that allows creation of empty <see cref="XPathDocument"/> 
	/// and retrieval of an <see cref="XmlWriter"/> on top of it.
	/// </summary>
	/// <remarks>
	/// This functionality is required in order to perform fast chained 
	/// transformations, as it avoids re-parsing and the memory consumption 
	/// of intermediate results representation.
	/// <para>
	/// The document cannot be used (no members should be called) until a valid 
	/// document has been created through the <see cref="XmlWriter"/> retrieved 
	/// from it using the <see cref="GetWriter"/> method. Otherwise, 
	/// <see cref="NullReferenceException"/> and others may be thrown.
	/// </para>
	/// <para>
	/// Future versions of <see cref="XPathDocument"/> may offer this 
	/// functionality out of the box.
	/// </para>
	/// <para>
	/// <b>Important: </b> this class requires full trust to run. 
	/// If the Mvp.Xml assembly is installed in the GAC, it will 
	/// run without problems. The Mvp.Xml allows partially trusted 
	/// callers, so only this assembly needs to be GAC'ed. 
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// XPathDocument output = XPathDocumentHelper.CreateDocument();
	/// XmlWriter outputWriter = XPathDocumentHelper.GetWriter(output);
	/// 
	/// xslTransform.Transform(inputReader, outputWriter);
	/// writer.Close();
	/// 
	/// // intermediate output document can be passed to 
	/// // the next XSLT.
	/// </code>
	/// </example>
	public static class XPathDocumentHelper
	{
		static ConstructorInfo nameTableConstructor;
		static ConstructorInfo defaultConstructor;
		static MethodInfo loadWriterMethod;

		static XPathDocumentHelper()
		{
			ReflectionPermission perm = new ReflectionPermission(PermissionState.Unrestricted);
			perm.Flags = ReflectionPermissionFlag.MemberAccess;

			try
			{
				perm.Assert();

				Type t = typeof(XPathDocument);
				nameTableConstructor = t.GetConstructor(
					BindingFlags.NonPublic | BindingFlags.Instance, null,
					new Type[] { typeof(NameTable) },
					new ParameterModifier[0]);
				Debug.Assert(nameTableConstructor != null, ".NET Framework implementation changed");

				defaultConstructor = t.GetConstructor(
					BindingFlags.NonPublic | BindingFlags.Instance, null,
					Type.EmptyTypes,
					new ParameterModifier[0]);
				Debug.Assert(defaultConstructor != null, ".NET Framework implementation changed");

				loadWriterMethod = t.GetMethod("LoadFromWriter", BindingFlags.Instance | BindingFlags.NonPublic);
				Debug.Assert(loadWriterMethod != null, ".NET Framework implementation changed");
			}
			finally
			{
				ReflectionPermission.RevertAssert();
			}
		}

		/// <summary>
		/// Creates an empty <see cref="XPathDocument"/> using the 
		/// <paramref name="nameTable"/> specified.
		/// </summary>
		/// <param name="nameTable">Table to use for tokenized XML names.</param>
		/// <returns>A new instance of an <see cref="XPathDocument"/>.</returns>
		public static XPathDocument CreateDocument(XmlNameTable nameTable)
		{
			return (XPathDocument)nameTableConstructor.Invoke(new object[] { nameTable });
		}

		/// <summary>
		/// Creates an empty <see cref="XPathDocument"/>
		/// </summary>
		/// <returns>A new instance of an <see cref="XPathDocument"/>.</returns>
		public static XPathDocument CreateDocument()
		{
			return (XPathDocument)defaultConstructor.Invoke(new object[0]);
		}

		/// <summary>
		/// Retrieves an <see cref="XmlWriter"/> that can build the 
		/// <see cref="XPathDocument"/> using the writer methods.
		/// </summary>
		/// <param name="document">The document to retrieve the writer from.</param>
		/// <returns>An instance of an <see cref="XmlWriter"/> that populates 
		/// the <paramref name="document"/>.</returns>
		public static XmlWriter GetWriter(XPathDocument document)
		{
			return (XmlWriter)loadWriterMethod.Invoke(document, new object[] { 0, String.Empty });
		}
	}
}
