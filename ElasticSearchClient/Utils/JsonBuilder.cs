using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ElasticSearch.Client.Utils
{
	internal sealed class JsonBuilder
	{
		#region escape output json string

		public static readonly string _JSONStrDblQuoteChar = "\"";
		public static readonly string _JSONStrSglQuoteChar = "'";
		public static readonly string[] _JSONEscapeChars = new[] { "\\", "'", "\"", "\r\n", "\n", "\r", "\t", "\f" };
		private static readonly int _JSONEscapeCharsCount = _JSONEscapeChars.Length;

		public static readonly string[] _JSONEscapeCharOutputs = new[] { "\\\\", "\\'", "\\\"", "\\n", "\\n", "\\r", "\\t", "\\f" };

		public static string escapeJSONStr(string srcStr)
		{
			if (srcStr == null)
			{
				return null;
			}
			var sb = new StringBuilder(srcStr);
			for (int i = 0; i < _JSONEscapeCharsCount; i++)
			{
				sb.Replace(_JSONEscapeChars[i], _JSONEscapeCharOutputs[i]);
			}
			return sb.ToString();
		}

		public static string escapeJSONOutputStr(string srcStr, bool useSingleQuote)
		{
			if (srcStr == null)
			{
				return null;
			}
			string quoteChar = useSingleQuote ? _JSONStrSglQuoteChar : _JSONStrDblQuoteChar;
			return quoteChar + escapeJSONStr(srcStr) + quoteChar;
		}

		#endregion

		//default value for string quote char
		public static readonly bool UseSingleQuote;
		//

		private static readonly Type Type_AnyArray = typeof(AnyArray);

		//
		private static readonly Dictionary<Type, ToJSONDelegate> jsonConvertors = new Dictionary<Type, ToJSONDelegate>();
		//register ToJSON convertor Delegate for specific valueType

		//const values
		private static readonly string Comment_Start = "/*";
		private static readonly string Comment_End = "*/";
		private static readonly string Object_Start = "{";
		private static readonly string Object_End = "}";
		private static readonly string Array_Start = "[";
		private static readonly string Array_End = "]";
		private static readonly string KeyValueSepChar = ": ";
		private static readonly string MemberSepChar = ", ";
		private static readonly int EndCharsCount = MemberSepChar.Length;
		public static readonly string NullStr = "null";

		//
		private readonly StringBuilder sb = new StringBuilder();
		private readonly bool useSingleQuote = UseSingleQuote;

		static JsonBuilder()
		{
			//register predefined  ToJSON convertor Delegates
			setJSONConvertor(Type_AnyArray, ToJsonDelegates.fromAnyArray);
			setJSONConvertor(typeof(ArrayList), ToJsonDelegates.fromArrayList);
			setJSONConvertor(typeof(List<object>), ToJsonDelegates.fromListOfObject);
			setJSONConvertor(typeof(Hashtable), ToJsonDelegates.fromHashtable);
			setJSONConvertor(typeof(Dictionary<string, object>), ToJsonDelegates.fromDictionaryOfObject);
			//more ...
		}

		public JsonBuilder(bool useSingleQuote)
		{
			this.useSingleQuote = useSingleQuote;
		}

		public JsonBuilder()
		{
		}

		public static void setJSONConvertor(Type valueType, ToJSONDelegate convertor)
		{
			jsonConvertors[valueType] = convertor;
		}

		// return ToJSON convertor Delegate for specific valueType
		public static ToJSONDelegate getJSONConvertor(Type valueType)
		{
			return jsonConvertors.ContainsKey(valueType) ? jsonConvertors[valueType] : null;
		}

		//remove ending MemberSepChar
		public void trimLastComma()
		{
			int startIndex = sb.Length - EndCharsCount;
			if (startIndex >= 0)
			{
				var endChars = new char[EndCharsCount];
				sb.CopyTo(startIndex, endChars, 0, EndCharsCount);
				if (new string(endChars).Equals(MemberSepChar))
				{
					sb.Remove(startIndex, EndCharsCount);
				}
			}
		}

		public void startObject()
		{
			sb.Append(Object_Start);
		}

		public void endObject()
		{
			trimLastComma();
			sb.Append(Object_End);
		}

		public void startArray()
		{
			sb.Append(Array_Start);
		}

		public void endArray()
		{
			trimLastComma();
			sb.Append(Array_End);
		}

		public void addComment(string comment)
		{
			if (comment == null)
			{
				return;
			}
			string str = comment.Replace(Comment_Start, "/ *");
			str = str.Replace(Comment_Start, "* /");
			sb.Append(Comment_Start).Append(str).Append(Comment_End);
		}

		//core method to convert object to json string
		public static string strValueOf(object value, bool useSingleQuote)
		{
			if (value == null)
			{
				return NullStr;
			}
			Type valueType = value.GetType();
			ToJSONDelegate convertor = getJSONConvertor(valueType);
			if (convertor != null)
			{
				return convertor(value, useSingleQuote);
			}
			if (valueType.IsArray)
			{
				convertor = getJSONConvertor(Type_AnyArray);
				if (convertor != null)
				{
					return convertor(value, useSingleQuote);
				}
			}
			if (valueType == SupportedTypes.Tstring)
			{
				return escapeJSONOutputStr(value as string, useSingleQuote);
			}
			if (valueType == SupportedTypes.Tbool)
			{
				return value.ToString().ToLower();
			}
			if (valueType == SupportedTypes.Tint)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.TDateTime)
			{
				long ticks = ((DateTime)value).Ticks;
				return "new Date(" + ticks + ")";
			}
			if (valueType == SupportedTypes.Tdouble)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tlong)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tfloat)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tchar)
			{
				return escapeJSONOutputStr(value.ToString(), true);
			}
			if (valueType == SupportedTypes.Tshort)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tdecimal)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tushort)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tuint)
			{
				return value.ToString();
			}
			if (valueType == SupportedTypes.Tulong)
			{
				return value.ToString();
			}
			try
			{
				MethodInfo toJsonMethod = valueType.GetMethod("toJSON",
				                                              BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
				if (toJsonMethod != null)
				{
					return toJsonMethod.Invoke(value, null) as string;
				}
			}
			catch (System.Exception exp)
			{
				Debug.WriteLine(exp.Message);
				return NullStr;
			}
			return NullStr;
		}

		public void addKey(string key)
		{
			sb.Append(strValueOf(key, useSingleQuote)).Append(KeyValueSepChar);
		}

		public void addValue(object value)
		{
			sb.Append(strValueOf(value, useSingleQuote)).Append(MemberSepChar);
		}

		public void add(string key, object value)
		{
			addKey(key);
			addValue(value);
		}

		public string toJSON()
		{
			trimLastComma();
			return sb.ToString();
		}

		#region Nested type: AnyArray

		private sealed class AnyArray
		{
			//This is just a flag type for array type that not handled by user
			private AnyArray()
			{
			}
		}

		#endregion

		#region Nested type: SupportedTypes

		private static class SupportedTypes
		{
			public static readonly Type Tstring = typeof(string);
			public static readonly Type Tbool = typeof(bool);
			public static readonly Type Tint = typeof(int);
			public static readonly Type TDateTime = typeof(DateTime);
			public static readonly Type Tdouble = typeof(double);
			public static readonly Type Tlong = typeof(long);
			public static readonly Type Tfloat = typeof(float);
			public static readonly Type Tchar = typeof(char);
			public static readonly Type Tshort = typeof(short);
			public static readonly Type Tdecimal = typeof(decimal);
			public static readonly Type Tushort = typeof(ushort);
			public static readonly Type Tuint = typeof(uint);
			public static readonly Type Tulong = typeof(ulong);
			public static readonly Type TtoJson = typeof(object); //NOT used to compare value's type !!!
		}

		#endregion
	}
}