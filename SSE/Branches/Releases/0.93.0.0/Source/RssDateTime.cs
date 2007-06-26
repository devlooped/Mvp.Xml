#if !PocketPC
using System.Runtime.Serialization;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Serialization;

namespace Mvp.Xml.Synchronization
{
	/// <summary>
	/// RSS/SSE date time, that complies with RFC 822 date format 
	/// but with 4 digit year.
	/// </summary>
	/// <remarks>
	/// See <see cref="http://www.ietf.org/rfc/rfc0822.txt">RFC Specification</see>
	/// </remarks>
	[Serializable]
	public struct RssDateTime : IComparable, IComparable<RssDateTime>,
		IEquatable<RssDateTime>, ICloneable<RssDateTime>
#if !PocketPC
		, ISerializable
#endif
	{
		#region Parsing expressions

		/* From the spec
		 5.  DATE AND TIME SPECIFICATION
		 5.1.  SYNTAX

		 date-time   =  [ day "," ] date time        ; dd mm yy
													 ;  hh:mm:ss zzz

		 day         =  "Mon"  / "Tue" /  "Wed"  / "Thu"
					 /  "Fri"  / "Sat" /  "Sun"

		 date        =  1*2DIGIT month 2DIGIT        ; day month year
													 ;  e.g. 20 Jun 82

		 month       =  "Jan"  /  "Feb" /  "Mar"  /  "Apr"
					 /  "May"  /  "Jun" /  "Jul"  /  "Aug"
					 /  "Sep"  /  "Oct" /  "Nov"  /  "Dec"

		 time        =  hour zone                    ; ANSI and Military

		 hour        =  2DIGIT ":" 2DIGIT [":" 2DIGIT]
													 ; 00:00:00 - 23:59:59

		 zone        =  "UT"  / "GMT"                ; Universal Time
													 ; North American : UT
					 /  "EST" / "EDT"                ;  Eastern:  - 5/ - 4
					 /  "CST" / "CDT"                ;  Central:  - 6/ - 5
					 /  "MST" / "MDT"                ;  Mountain: - 7/ - 6
					 /  "PST" / "PDT"                ;  Pacific:  - 8/ - 7
					 /  1ALPHA                       ; Military: Z = UT;
													 ;  A:-1; (J not used)
													 ;  M:-12; N:+1; Y:+12
					 / ( ("+" / "-") 4DIGIT )        ; Local differential
													 ;  hours+min. (HHMM)
		*/

		// Expressions do not perform full validation of input as part of that 
		// validation is already done by the DateTime we will construct from 
		// the input string. At that time, further validation will be performed.
		// This keeps the expressions simple.
		// We only need to capture the offset value and perform any necessary conversions.
		const string HH = "[0-2][0-9]"; // hours
		const string MS = "[0-5][0-9]"; // minutes or seconds
		const string DayExpression = @"^((Mon|Tue|Wed|Thu|Fri|Sat|Sun),\s)?";
		const string MonthExpression = @"(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s";
		const string DateExpression = @"(\d\d?)\s" + MonthExpression + @"(\d{2,4})\s";
		const string HourExpression = @"(" + HH + "):(" + MS + ")(:(" + MS + @"))?\s";

		const string OffsetExpression = @"(?<offset>[-\+]" + HH + MS + ")";
		const string GmtExpression = @"(?<gmt>UT|GMT|Z)";
		const string ToConvertExpression = @"(?<convert>(E|C|M|P)(ST|DT)|[A-IK-Y])";
		const string ZoneExpression = "(?<zone>" + OffsetExpression + "|" + GmtExpression + "|" + ToConvertExpression + ")$";

		public const string Rfc822Expression = DayExpression + DateExpression + HourExpression + ZoneExpression;
		static readonly Regex ParseExpression = new Regex(Rfc822Expression, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		#endregion

		static readonly Dictionary<string, TimeSpan> convertTable;

		static readonly RssDateTime maxValue = new RssDateTime(DateTime.MaxValue, TimeSpan.Zero);
		static readonly RssDateTime minValue = new RssDateTime(DateTime.MinValue, TimeSpan.Zero);
		static readonly TimeSpan localOffset;

		/// <summary>
		/// Internal value is always kept as UTC.
		/// </summary>
		DateTime value;
		/// <summary>
		/// Offset from <see cref="value"/> which is in UTC.
		/// </summary>
		TimeSpan offset;

		static RssDateTime()
		{
			convertTable = new Dictionary<string, TimeSpan>(32);
			int x = 64;
			int y = 77;
			for (int i = 1; i < 13; i++)
			{
				if ((char)i + x == 'J') x++;
				convertTable.Add(((char)(i + x)).ToString(), TimeSpan.FromHours(i * -1));
				convertTable.Add(((char)(i + y)).ToString(), TimeSpan.FromHours(i));
			}

			convertTable.Add("EST", TimeSpan.FromHours(-5));
			convertTable.Add("EDT", TimeSpan.FromHours(-4));
			convertTable.Add("CST", TimeSpan.FromHours(-6));
			convertTable.Add("CDT", TimeSpan.FromHours(-5));
			convertTable.Add("MST", TimeSpan.FromHours(-7));
			convertTable.Add("MDT", TimeSpan.FromHours(-6));
			convertTable.Add("PST", TimeSpan.FromHours(-8));
			convertTable.Add("PDT", TimeSpan.FromHours(-7));

			localOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
		}

		public static implicit operator RssDateTime(DateTime dateTime)
		{
			return new RssDateTime(dateTime);
		}

		public static implicit operator DateTime(RssDateTime dateTime)
		{
			return dateTime.LocalTime;
		}

		public RssDateTime(DateTime dateTime) : this(dateTime, localOffset) { }

		public RssDateTime(DateTime dateTime, TimeSpan offset)
		{
			// Convert to UTC if necessary.
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				this.value = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).Subtract(offset);
			}
			else
			{
				this.value = dateTime;
			}

			this.offset = offset;
		}

		public static RssDateTime MaxValue { get { return maxValue; } }
		public static RssDateTime MinValue { get { return minValue; } }
		public static TimeSpan LocalOffset { get { return localOffset; } }
		public static RssDateTime Now { get { return new RssDateTime(DateTime.Now); } }

		/// <summary>
		/// Gets the offset with regards to Coordinated Universal Time (UTC) of 
		/// the current <see cref="RssDateTime"/>.
		/// </summary>
		public TimeSpan Offset
		{
			get { return offset; }
		}

		/// <summary>
		/// Gets the value of the current <see cref="RssDateTime"/> as a Coordinated Universal Time (UTC).
		/// </summary>
		public DateTime UniversalTime
		{
			get { return value; }
		}

		/// <summary>
		/// Gets the value of the current <see cref="RssDateTime"/> as a local time.
		/// </summary>
		public DateTime LocalTime
		{
			get { return DateTime.SpecifyKind(value, DateTimeKind.Local).Add(offset); } 
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="RssDateTime"/>.</summary>
		/// <returns><see langword="true"/> if value equals the value of this instance; otherwise, <see langword="false"/>.</returns>
		/// <param name="value">An <see cref="RssDateTime"/> to compare to this instance. </param>
		public bool Equals(RssDateTime other)
		{
			return this == other;
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns><see langword="true"/> if value is an instance of <see cref="RssDateTime"></see> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
		/// <param name="value">An object to compare to this instance. </param>
		public override bool Equals(object value)
		{
			if (value is RssDateTime)
			{
				return Equals((RssDateTime)value);
			}

			return false;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return value.GetHashCode() ^ offset.GetHashCode();
		} 

		/// <summary>
		/// Renders the date as a valid RSS 2.0 date time.
		/// </summary>
		public override string ToString()
		{
			return value.Add(offset).ToString("ddd, dd MMM yyy HH':'mm':'ss ", new CultureInfo("en-US")) + ToOffsetString(offset);
		}

		/// <summary>
		/// Normalizes the date and time for string comparison 
		/// (e.g. "18-10-2006T19:30:10Z" instead of the default "Mie, 18 Oct 06 16:30:10 -0300").
		/// </summary>
		public string ToNormalizedString()
		{
			return value.ToString("dd-MM-yyyyTHH:mm:ssZ");
		}

		private static string ToOffsetString(TimeSpan value)
		{
			string offsetString = value.Hours.ToString("00") + value.Minutes.ToString("00");
			if (offsetString == "0000") offsetString = "-" + offsetString;

			if (!offsetString.StartsWith("-")) offsetString = "+" + offsetString;

			return offsetString;
		}

		#region Parsing

		/// <summary>
		/// Converts the specified string representation of an RSS 2.0 compatible 
		/// date and time to its <see cref="RssDateTime"/> equivalent.
		/// </summary>
		/// <param name="s">A string containing a date and time to convert.</param>
		/// <param name="result">
		/// When this method returns, contains the <see cref="RssDateTime"/> value equivalent 
		/// to the date and time contained in s, if the conversion succeeded, 
		/// or MinValue if the conversion failed. 
		/// The conversion fails if the s parameter is null, or does not contain a valid string 
		/// representation of a date and time. 
		/// This parameter is passed uninitialized. 
		/// </param>
		/// <returns><see langword="true"/> if parameter <paramref name="s"/> was converted successfully; <see langword="false"/> otherwise.</returns>
		/// <seealso cref="RssDateTime"/>
		public static bool TryParse(string s, out RssDateTime result)
		{
			return ParseInternal(s, out result, false);
		}

		/// <summary>
		/// Converts the specified string representation of an RSS 2.0 compatible 
		/// date and time to its <see cref="RssDateTime"/> equivalent.
		/// </summary>
		/// <param name="s">A string containing a date and time to convert.</param>
		/// <returns>An <see cref="RssDateTime"/> equivalent to the date and time contained in <paramref name="s"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null" />.</exception>
		/// <exception cref="FormatException"><paramref name="s"/> does not contain a valid string representation of an RSS date and time.</exception>
		public static RssDateTime Parse(string s)
		{
			RssDateTime result;
			ParseInternal(s, out result, true);

			return result;
		}

		private static bool ParseInternal(string s, out RssDateTime result, bool shouldThrow)
		{
			Guard.ArgumentNotNull(s, "s");

			result = RssDateTime.MinValue;
			Match m = ParseExpression.Match(s);

			ThrowReturnHandler falseOrThrow = delegate(bool throwException)
			{
				if (throwException)
				{
					throw new FormatException(String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.Format_BadDateTime,
						s));
				}
				else
				{
					return false;
				}
			};

			if (!m.Success)
			{
				return falseOrThrow(shouldThrow);
			}

			// Convert offset.
			TimeSpan offsetSpan;
			string offsetValue;
			if (m.Groups[GroupNames.Gmt].Success)
			{
				offsetSpan = TimeSpan.Zero;
				offsetValue = "-0000";
			}
			else if (m.Groups[GroupNames.Offset].Success)
			{
				offsetValue = m.Groups[GroupNames.Offset].Value;
				offsetSpan = offsetValue.StartsWith("+") ? 
					TimeSpan.Parse(offsetValue.Substring(1).Insert(2, ":")) : 
					TimeSpan.Parse(offsetValue.Insert(3, ":"));
			}
			else if (m.Groups[GroupNames.Convert].Success)
			{
				if (!convertTable.TryGetValue(m.Groups[GroupNames.Convert].Value, out offsetSpan))
				{
					return falseOrThrow(shouldThrow);
				}

				offsetValue = ToOffsetString(offsetSpan);
			}
			else
			{
				// Should have been caught by the regex already.
				return falseOrThrow(shouldThrow);
			}

			string dateTimeString = s.Substring(0, m.Groups[GroupNames.Zone].Index) + offsetValue;

			DateTime value;
			if (shouldThrow)
			{
				value = DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
			}
			else
			{
#if PocketPC
				try
				{
					value = DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
				}
				catch (ArgumentException) { return false; }
				catch (FormatException) { return false; }
#else
				bool canParse = DateTime.TryParse(dateTimeString, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out value);
				if (!canParse)
				{
					return falseOrThrow(shouldThrow);
				}
#endif
			}

			result = new RssDateTime(value, offsetSpan);
			return true;
		}

		#endregion

		#region DateTime pass-through

		/// <summary>Adds the value of the specified <see cref="TimeSpan"></see> to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the time interval represented by value.</returns>
		/// <param name="value">A <see cref="TimeSpan"></see> that contains the interval to add. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime Add(TimeSpan value) { return new RssDateTime(UniversalTime.Add(value), Offset); }

		/// <summary>Adds the specified number of days to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the number of days represented by value.</returns>
		/// <param name="value">A number of whole and fractional days. The value parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddDays(double value) { return new RssDateTime(UniversalTime.AddDays(value), Offset); }

		/// <summary>Adds the specified number of hours to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the number of hours represented by value.</returns>
		/// <param name="value">A number of whole and fractional hours. The value parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddHours(double value) { return new RssDateTime(UniversalTime.AddHours(value), Offset); }

		/// <summary>Adds the specified number of milliseconds to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the number of milliseconds represented by value.</returns>
		/// <param name="value">A number of whole and fractional milliseconds. The value parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddMilliseconds(double value) { return new RssDateTime(UniversalTime.AddMilliseconds(value), Offset); }

		/// <summary>Adds the specified number of minutes to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the number of minutes represented by value.</returns>
		/// <param name="value">A number of whole and fractional minutes. The value parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddMinutes(double value) { return new RssDateTime(UniversalTime.AddMinutes(value), Offset); }

		/// <summary>Adds the specified number of months to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and months.</returns>
		/// <param name="months">A number of months. The months parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>.-or- months is less than -120,000 or greater than 120,000. </exception>
		public RssDateTime AddMonths(int months) { return new RssDateTime(UniversalTime.AddMonths(months), Offset); }

		/// <summary>Adds the specified number of seconds to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the number of seconds represented by value.</returns>
		/// <param name="value">A number of whole and fractional seconds. The value parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddSeconds(double value) { return new RssDateTime(UniversalTime.AddSeconds(value), Offset); }

		/// <summary>Adds the specified number of ticks to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the time represented by value.</returns>
		/// <param name="value">A number of 100-nanosecond ticks. The value parameter can be positive or negative. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddTicks(long value) { return new RssDateTime(UniversalTime.AddTicks(value), Offset); }

		/// <summary>Adds the specified number of years to the value of this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the sum of the date and time represented by this instance and the number of years represented by value.</returns>
		/// <param name="value">A number of years. The value parameter can be negative or positive. </param>
		/// <exception cref="ArgumentOutOfRangeException">value or the resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime AddYears(int value) { return new RssDateTime(UniversalTime.AddYears(value), Offset); }

		/// <summary>Subtracts the specified date and time from this instance.</summary>
		/// <returns>A <see cref="TimeSpan"></see> interval equal to the date and time represented by this instance minus the date and time represented by value.</returns>
		/// <param name="value">An instance of <see cref="RssDateTime"></see>. </param>
		/// <exception cref="ArgumentOutOfRangeException">The result is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public TimeSpan Subtract(RssDateTime value) { return this - value; }

		/// <summary>Subtracts the specified duration from this instance.</summary>
		/// <returns>A <see cref="RssDateTime"></see> equal to the date and time represented by this instance minus the time interval represented by value.</returns>
		/// <param name="value">An instance of <see cref="TimeSpan"></see>. </param>
		/// <exception cref="ArgumentOutOfRangeException">The result is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>. </exception>
		public RssDateTime Subtract(TimeSpan value) { return this - value; }

		#endregion

		#region Operators

		/// <summary>Adds a specified time interval to a specified date and time, yielding a new date and time.</summary>
		/// <returns>A <see cref="RssDateTime"></see> that is the sum of the values of d and t.</returns>
		/// <param name="d">A <see cref="RssDateTime"></see>. </param>
		/// <param name="t">A <see cref="TimeSpan"></see>. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>.</exception>
		public static RssDateTime operator +(RssDateTime d, TimeSpan t)
		{
			return new RssDateTime(d.UniversalTime + t, d.Offset);
		}

		/// <summary>Determines whether two specified instances of <see cref="RssDateTime"></see> are equal.</summary>
		/// <returns>true if d1 and d2 represent the same date and time; otherwise, false.</returns>
		/// <param name="d2">A <see cref="RssDateTime"></see>. </param>
		/// <param name="d1">A <see cref="RssDateTime"></see>. </param>
		public static bool operator ==(RssDateTime d1, RssDateTime d2)
		{
			return d1.UniversalTime == d2.UniversalTime;
		}

		/// <summary>Determines whether one specified <see cref="RssDateTime"></see> is greater than another specified <see cref="RssDateTime"></see>.</summary>
		/// <returns>true if t1 is greater than t2; otherwise, false.</returns>
		/// <param name="t2">A <see cref="RssDateTime"></see>. </param>
		/// <param name="t1">A <see cref="RssDateTime"></see>. </param>
		public static bool operator >(RssDateTime t1, RssDateTime t2)
		{
			return t1.UniversalTime > t2.UniversalTime;
		}

		/// <summary>Determines whether one specified <see cref="RssDateTime"></see> is greater than or equal to another specified <see cref="RssDateTime"></see>.</summary>
		/// <returns>true if t1 is greater than or equal to t2; otherwise, false.</returns>
		/// <param name="t2">A <see cref="RssDateTime"></see>. </param>
		/// <param name="t1">A <see cref="RssDateTime"></see>. </param>
		public static bool operator >=(RssDateTime t1, RssDateTime t2)
		{
			return t1.UniversalTime >= t2.UniversalTime;
		}

		/// <summary>Determines whether two specified instances of <see cref="RssDateTime"></see> are not equal.</summary>
		/// <returns>true if d1 and d2 do not represent the same date and time; otherwise, false.</returns>
		/// <param name="d2">A <see cref="RssDateTime"></see>. </param>
		/// <param name="d1">A <see cref="RssDateTime"></see>. </param>
		public static bool operator !=(RssDateTime d1, RssDateTime d2)
		{
			return d1.UniversalTime != d2.UniversalTime;
		}

		/// <summary>Determines whether one specified <see cref="RssDateTime"></see> is less than another specified <see cref="RssDateTime"></see>.</summary>
		/// <returns>true if t1 is less than t2; otherwise, false.</returns>
		/// <param name="t2">A <see cref="RssDateTime"></see>. </param>
		/// <param name="t1">A <see cref="RssDateTime"></see>. </param>
		public static bool operator <(RssDateTime t1, RssDateTime t2)
		{
			return t1.UniversalTime < t2.UniversalTime;
		}

		/// <summary>Determines whether one specified <see cref="RssDateTime"></see> is less than or equal to another specified <see cref="RssDateTime"></see>.</summary>
		/// <returns>true if t1 is less than or equal to t2; otherwise, false.</returns>
		/// <param name="t2">A <see cref="RssDateTime"></see>. </param>
		/// <param name="t1">A <see cref="RssDateTime"></see>. </param>
		public static bool operator <=(RssDateTime t1, RssDateTime t2)
		{
			return t1.UniversalTime <= t2.UniversalTime;
		}

		/// <summary>Subtracts a specified date and time from another specified date and time, yielding a time interval.</summary>
		/// <returns>A <see cref="TimeSpan"></see> that is the time interval between d1 and d2; that is, d1 minus d2.</returns>
		/// <param name="d2">A <see cref="RssDateTime"></see> (the subtrahend). </param>
		/// <param name="d1">A <see cref="RssDateTime"></see> (the minuend). </param>
		public static TimeSpan operator -(RssDateTime d1, RssDateTime d2)
		{
			return d1.UniversalTime - d2.UniversalTime;
		}

		/// <summary>Subtracts a specified time interval from a specified date and time, yielding a new date and time.</summary>
		/// <returns>A <see cref="RssDateTime"></see> whose value is the value of d minus the value of t.</returns>
		/// <param name="d">A <see cref="RssDateTime"></see>. </param>
		/// <param name="t">A <see cref="TimeSpan"></see>. </param>
		/// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="RssDateTime"></see> is less than <see cref="RssDateTime.MinValue"></see> or greater than <see cref="RssDateTime.MaxValue"></see>.</exception>
		public static RssDateTime operator -(RssDateTime d, TimeSpan t)
		{
			return new RssDateTime(d.UniversalTime - t, d.Offset);
		}

		#endregion

		#region ISerializable Members

#if !PocketPC

		private RssDateTime(SerializationInfo info, StreamingContext context)
		{
			value = info.GetDateTime("value");
			offset = (TimeSpan)info.GetValue("offset", typeof(TimeSpan));
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value);
			info.AddValue("offset", offset);
		}

#endif

		#endregion

		#region IComparable<RssDateTime> Members

		public int CompareTo(RssDateTime other)
		{
			return this.UniversalTime.CompareTo(other.UniversalTime);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null) return 1;
			if (!(obj is RssDateTime)) throw new ArgumentException(Properties.Resources.Arg_MustBeRssDateTime);

			return CompareTo((RssDateTime)obj);
		}

		#endregion

		#region ICloneable<RssDateTime> Members

		/// <summary>Creates a new object that is a copy of the current instance.</summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public RssDateTime Clone()
		{
			return new RssDateTime(value, offset);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion

		delegate bool ThrowReturnHandler(bool shouldThrow);

		static class GroupNames
		{
			/// <summary>
			/// Group containing either an <see cref="Offset"/>, 
			/// a <see cref="Gmt"/> or a <see cref="ToConvert"/> match.
			/// Used to truncate the string that will be passed to the 
			/// <see cref="DateTime"/>.
			/// </summary>
			public const string Zone = "zone";
			/// <summary>
			/// If there's a match, an offset with sign and 4 digits was specified.
			/// </summary>
			public const string Offset = "offset";
			/// <summary>
			/// If there's a match, the date is GMT (true for "UT", "GMT" or "Z" in military notation).
			/// </summary>
			public const string Gmt = "gmt";
			/// <summary>
			/// A conversion needs to be performed to calculate the offset from the given notation.
			/// </summary>
			public const string Convert = "convert";
		}
	}
}
