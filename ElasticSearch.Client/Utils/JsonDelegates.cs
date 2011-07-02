using System;
using System.Collections;
using System.Collections.Generic;

namespace ElasticSearch.Client.Utils
{
	internal abstract class ToJsonDelegates
	{
		//@predefined converter
		public static string fromAnyArray(object value, bool useSingleQuote)
		{
			if (value == null)
			{
				return JsonBuilder.NullStr;
			}
			if ((value.GetType()).IsArray)
			{
				var elems = value as Array;
				var jb = new JsonBuilder(useSingleQuote);
				jb.startArray();
				foreach (object elem in elems)
				{
					jb.addValue(elem);
				}
				jb.endArray();
				return jb.toJSON();
			}
			return JsonBuilder.NullStr;
		}

		//@predefined converter
		public static string fromArrayList(object value, bool useSingleQuote)
		{
			if (value == null)
			{
				return JsonBuilder.NullStr;
			}
			if (value.GetType() == typeof(ArrayList))
			{
				var elems = value as ArrayList;
				var jb = new JsonBuilder(useSingleQuote);
				jb.startArray();
				foreach (object elem in elems)
				{
					jb.addValue(elem);
				}
				jb.endArray();
				return jb.toJSON();
			}
			return JsonBuilder.NullStr;
		}

		//@predefined converter
		public static string fromListOfObject(object value, bool useSingleQuote)
		{
			if (value == null)
			{
				return JsonBuilder.NullStr;
			}
			if (value.GetType() == typeof(List<object>))
			{
				var elems = value as List<object>;
				var jb = new JsonBuilder(useSingleQuote);
				jb.startArray();
				foreach (object elem in elems)
				{
					jb.addValue(elem);
				}
				jb.endArray();
				return jb.toJSON();
			}
			return JsonBuilder.NullStr;
		}

		//@predefined converter
		public static string fromHashtable(object value, bool useSingleQuote)
		{
			if (value == null)
			{
				return JsonBuilder.NullStr;
			}
			//
			if (value.GetType() == typeof(Hashtable))
			{
				var elems = value as Hashtable;
				var jb = new JsonBuilder(useSingleQuote);
				jb.startObject();
				foreach (object key in elems.Keys)
				{
					jb.add(key.ToString(), elems[key]);
				}
				jb.endObject();
				return jb.toJSON();
			}
			return JsonBuilder.NullStr;
		}

		//@predefined converter
		public static string fromDictionaryOfObject(object value, bool useSingleQuote)
		{
			if (value == null)
			{
				return JsonBuilder.NullStr;
			}
			//
			if (value.GetType() == typeof(Dictionary<string, object>))
			{
				var elems = value as Dictionary<string, object>;
				var jb = new JsonBuilder(useSingleQuote);
				jb.startObject();
				foreach (string key in elems.Keys)
				{
					jb.add(key, elems[key]);
				}
				jb.endObject();
				return jb.toJSON();
			}
			return JsonBuilder.NullStr;
		}
	}
}