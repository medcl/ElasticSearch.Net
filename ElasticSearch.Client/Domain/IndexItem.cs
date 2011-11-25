using System;
using System.Collections.Generic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Domain
{
	/// <summary>
	/// 
	/// </summary>
	public class IndexItem
	{
		/// <summary>
		/// 
		/// </summary>
		private HashSet<KeyValuePair<string, object>> _fields = new HashSet<KeyValuePair<string, object>>();

		/// <summary>
		/// 
		/// </summary>
		public string IndexKey { get; private set; }

		public string ParentKey { get; protected set; }

		public string IndexType { set; get; }

		/// <summary>
		/// 
		/// </summary>
		public HashSet<KeyValuePair<string, object>> Fields
		{
			get { return _fields; }
			private set { _fields = value; }
		}

		public IndexItem(string indexTypeName, string indexKey)
			: this(indexKey)
		{
			IndexType = indexTypeName;
		}

		public IndexItem(string indexIndexKey)
		{
			IndexKey = indexIndexKey;
			IndexType = "default";
		}

		public IndexItem()
		{
			IndexKey = Guid.NewGuid().ToString();
			IndexType = "default";
		}

		public void Add(string field, string value)
		{
			AddField(field, value);
		}

		public void Add(string field, int value)
		{
			AddField(field, value);
		}

		public void Add(string field, float value)
		{
			AddField(field, value);
		}
		public void Add(string field, double value)
		{
			AddField(field, value);
		}
		public void Add(string field, long value)
		{
			AddField(field, value);
		}

		public void Add(string field, bool value)
		{
			AddField(field, value);
		}

		public void Add(string field, DateTime value)
		{
			AddField(field, value);
		}

		void AddField(string field, object value)
		{
			_fields.Add(new KeyValuePair<string, object>((string)RemoveInvalidChar(field), RemoveInvalidChar(value)));
		}

		static object RemoveInvalidChar(object str)
		{
			if (str is string)
			{
				var str1 = str.ToString();
				if (str1.Contains("\0"))
				{
					str = str1.Replace("\0", string.Empty);
				}
			}
			return str;
		}

		public string FieldsToJson()
		{
            if (Fields == null || Fields.Count == 0)
            {
                throw new ArgumentException("fields cant't be null or empty");
            }

			var jsonBuilder = new JsonBuilder();
			jsonBuilder.startObject();
			foreach (var variable in Fields)
			{
				jsonBuilder.add(variable.Key, variable.Value);
			}
			jsonBuilder.endObject();
			return  jsonBuilder.toJSON();
		}
	}
}