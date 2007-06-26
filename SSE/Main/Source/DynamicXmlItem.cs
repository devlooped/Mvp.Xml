using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public class DynamicXmlItem : XmlItem
	{
		static readonly Regex ReplacementExpression = new Regex("{([^}]+)}", RegexOptions.Compiled);
		
		public DynamicXmlItem(string id, string titleExpression, 
			string descriptionExpression, DateTime timestamp, 
			XmlElement payload, object data) 
			: base(id, Evaluate(titleExpression, data), Evaluate(descriptionExpression, data),
				timestamp, payload)
		{
		}

		private static string Evaluate(string expression, object data)
		{
			Guard.ArgumentNotNullOrEmptyString(expression, "expression");
			Guard.ArgumentNotNull(data, "data");

			// Resolve context references for the expression.
			Match referencematch = ReplacementExpression.Match(expression);

			// If we don't match, there was nothing to resolve. Return the input expression.
			if (!referencematch.Success)
			{
				return expression;
			}

			StringBuilder sb = new StringBuilder();
			string original = expression;
			int start = 0;

			for (; referencematch.Success; referencematch = referencematch.NextMatch())
			{
				// Append the unmatched text before the current match.
				sb.Append(original, start, referencematch.Index - start);

				// Append the replaced parameter, automatically converted to string.
				object refvalue = EvaluateReference(referencematch.Groups[1].Value, data);
				sb.Append(refvalue);

				// Move the start position to the end of the last matched string.
				start = referencematch.Index + referencematch.Length;
			}

			// Append any remaining text.
			sb.Append(original, start, original.Length - start);

			string value = sb.ToString();

			return sb.ToString();
		}

		private static object EvaluateReference(string memberName, object data)
		{
			MemberInfo[] members = data.GetType().GetMember(memberName);

			if (members.Length == 0)
				throw new ArgumentException(String.Format(
					"Member named {0} not found in type {1}.",
					memberName, data.GetType().FullName));

			MemberInfo member = null;

			// If more than one, it's because they're methods
			if (members.Length > 1)
			{
				foreach (MethodBase overload in members)
				{
					if (overload.GetParameters().Length == 0)
					{
						member = overload;
						break;
					}
				}
				// we didn't find a parameterless one.
				if (member == null)
					throw new ArgumentException(String.Format(
						"Method named {0} in type {1} does not provide a parameterless overload.",
						memberName, data.GetType().FullName));
			}
			else
			{
				member = members[0];
			}
			
			FieldInfo field = member as FieldInfo;
			if (field != null) return field.GetValue(data);

			PropertyInfo property = member as PropertyInfo;
			if (property != null && property.CanRead) return property.GetValue(data, null);

			MethodBase method = member as MethodBase;
			if (method != null)
			{
				// we didn't find a parameterless one.
				if (((MethodBase)members[0]).GetParameters().Length > 0)
					throw new ArgumentException(String.Format(
						"Method named {0} in type {1} does not provide a parameterless overload.",
						memberName, data.GetType().FullName));
			
				return method.Invoke(data, null);
			}

			return null;
		}
	}
}
