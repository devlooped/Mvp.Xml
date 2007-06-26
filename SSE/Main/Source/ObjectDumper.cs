// Copyright (c) Microsoft Corporation.  All rights reserved.
#if DEBUG
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ObjectDumper
{
	public static void Write(object o)
	{
		Write(o, int.MaxValue);
	}

	public static void Write(object o, int depth)
	{
		Write(o, depth, Console.Out);
	}

	public static void Write(object o, int depth, TextWriter log)
	{
		ObjectDumper dumper = new ObjectDumper(depth);
		dumper.writer = log;
		dumper.WriteObject(o);
		log.WriteLine();
	}

	TextWriter writer;
	int pos;
	int level;
	int depth;

	private ObjectDumper(int depth)
	{
		this.depth = depth;
	}

	private void Write(string s)
	{
		if (s != null)
		{
			writer.Write(s);
			pos += s.Length;
		}
	}

	private void WriteIndent()
	{
		for (int i = 0; i < level; i++) writer.Write("  ");
	}

	private void WriteLine()
	{
		writer.WriteLine();
		pos = 0;
	}

	private void WriteTab()
	{
		Write("  ");
		while (pos % 8 != 0) Write(" ");
	}

	private void WriteObject(object o)
	{
		if (o == null || o is ValueType || o is string)
		{
			WriteValue(o);
			//WriteLine();
		}
		else if (o is IEnumerable)
		{
			foreach (object element in (IEnumerable)o)
			{
				if (element is IEnumerable && !(element is string))
				{
					WriteIndent();
					Write("...");
					WriteLine();
					if (level < depth)
					{
						level++;
						WriteObject(element);
						level--;
					}
				}
				else
				{
					WriteObject(element);
				}
			}
		}
		else
		{
			MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
			WriteValue(o.GetType().FullName);
			level++;
			bool brackets = false;
			foreach (MemberInfo m in members)
			{
				FieldInfo f = m as FieldInfo;
				PropertyInfo p = m as PropertyInfo;
				if (f != null || p != null)
				{
					if (!brackets)
					{
						WriteLine();
						level--;
						WriteIndent();
						level++;
						Write("{");
						brackets = true;
					}

					WriteLine();
					WriteIndent();
					Type t = f != null ? f.FieldType : p.PropertyType;
					Write("." + m.Name + " = ");
					// Uncomment to show member type before value.
					//Write("(" + t + ")");
					if (t.IsValueType || t == typeof(string))
					{
						WriteValue(f != null ? f.GetValue(o) : p.GetValue(o, null));
					}
					else
					{
						if (typeof(IEnumerable).IsAssignableFrom(t))
						{
							Write("...");
						}
						else
						{
							if (level < depth)
							{
								//WriteLine();
								//WriteIndent();
								WriteObject(f != null ? f.GetValue(o) : p.GetValue(o, null));
							}
						}
					}
				}
			}

			level--;
			if (brackets)
			{
				WriteLine();
				WriteIndent();
				Write("}");
			}
		}
	}

	private void WriteValue(object o)
	{
		if (o == null)
		{
			Write("null");
		}
		else if (o is DateTime)
		{
			Write(((DateTime)o).ToShortDateString());
		}
		else if (o is ValueType || o is string)
		{
			Write(o.ToString());
		}
		else if (o is IEnumerable)
		{
			Write("...");
		}
		else
		{
			Write("{ }");
		}
	}
}
#endif