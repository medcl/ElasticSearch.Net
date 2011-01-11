using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace ElasticSearch.Mapping
{
	[JsonObject("_source")]
	public class SourceSetting
	{
		[JsonProperty("enabled")]
		public bool Enabled = true;
	}

	public class TypeSetting
	{
		public TypeSetting(string typeName)
		{
			Type = typeName;
		}

		[JsonIgnore]
		internal string Type;

		[JsonProperty("_source")]
		public SourceSetting SourceSetting;

		public void AddFieldSetting(AbstractFieldSetting fieldSetting)
		{
			_fieldSettings[fieldSetting.Name] = fieldSetting;
		}

		public void AddFieldSetting(string filedName, AbstractFieldSetting fieldSetting)
		{
			_fieldSettings[filedName] = fieldSetting;
		}

		[JsonProperty("properties")]
		Dictionary<string, AbstractFieldSetting> _fieldSettings = new Dictionary<string, AbstractFieldSetting>();

		[JsonProperty("ignore_conflicts")]
		[JsonIgnore]
		public bool IgnoreConflicts;

		#region field mapping operation


		/// <summary>
		/// 
		/// </summary>
		/// <param name="indexName">The name of the field that will be stored in the index. Defaults to the property/field name</param>
		/// <param name="store">Set to yes the store actual field in the index, no to not store it. Defaults to no (note, the JSON document itself is stored, and it can be retrieved from it).
		/// </param>
		/// <param name="index">Set to analyzed for the field to be indexed and searchable after being broken down into token using an analyzer. not_analyzed means that its still searchable, but does not go through any analysis process or broken down into tokens. no means that it won’t be searchable at all. Defaults to analyzed.</param>
		/// <param name="termVector">Possible values are no, yes, with_offsets, with_positions, with_positions_offsets. Defaults to no.</param>
		/// <param name="boost">The boost value. Defaults to 1.0.</param>
		/// <param name="nullValue">When there is a (JSON) null value for the field, use the null_value as the field value. Defaults to not adding the field at all.</param>
		/// <param name="omitNorms">Boolean value if norms should be omitted or not. Defaults to false.</param>
		/// <param name="omitTermFreqAndPositions">Boolean value if term freq and positions should be omitted. Defaults to false.</param>
		/// <param name="analyzer">The analyzer used to analyze the text contents when analyzed during indexing and when searching using a query string. Defaults to the globally configured analyzer.</param>
		/// <param name="indexAnalyzer">The analyzer used to analyze the text contents when analyzed during indexing.</param>
		/// <param name="searchAnalyzer">The analyzer used to analyze the field when part of a query string.</param>
		/// <param name="includeInAll">Should the field be included in the _all field (if enabled). Defaults to true or to the parent object type setting.</param>
		public void CreateStringField(string name, string indexName = null, Store store = Store.no, IndexType index = IndexType.analyzed,
									  TermVector termVector = TermVector.no, double boost = 1.0, string nullValue = null,
									  bool omitNorms = false, bool omitTermFreqAndPositions = false, string analyzer = null,
									  string indexAnalyzer = null, string searchAnalyzer = null, bool includeInAll = true)
		{
			Contract.Ensures(_fieldSettings != null);

			var field = new StringFieldSetting();
			field.Name = name;
			field.IndexName = indexName;
			field.Store = store;
			field.Index = index;
			field.TermVector = termVector;
			field.Boost = boost;
			field.NullValue = nullValue;
			field.OmitNorms = omitNorms;
			field.OmitTermFreqAndPositions = omitTermFreqAndPositions;
			field.Analyzer = analyzer;
			field.IndexAnalyzer = indexAnalyzer;
			field.SearchAnalyzer = searchAnalyzer;
			field.IncludeInAll = includeInAll;

			_fieldSettings[name] = field;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">The type of the number. Can be float, double, integer, long. Required.</param>
		/// <param name="indexName">The name of the field that will be stored in the index. Defaults to the property/field name.</param>
		/// <param name="index">Set to no if the value should not be indexed. In this case, store should be set to yes, since if its not indexed and not stored, there is nothing to do with it.</param>
		/// <param name="store">Set to yes the store actual field in the index, no to not store it. Defaults to no (note, the JSON document itself is stored, and it can be retrieved from it).</param>
		/// <param name="precisionStep">The precision step (number of terms generated for each number value). Defaults to 4.</param>
		/// <param name="boost">The boost value. Defaults to 1.0.</param>
		/// <param name="nullValue">When there is a (JSON) null value for the field, use the null_value as the field value. Defaults to not adding the field at all.</param>
		/// <param name="includeInAll">Should the field be included in the _all field (if enabled). Defaults to true or to the parent object type setting.</param>
		public void CreateNumField(string name, NumType type = NumType.Integer,
								   string indexName = null,
								   IndexType index = IndexType.analyzed,
								   Store store = Store.no,
								   int precisionStep = 4,
								   double boost = 1.0,
								   string nullValue = null,
								   bool includeInAll = true)
		{
			Contract.Ensures(_fieldSettings != null);

			var field = new NumberFieldSetting();
			field.Name = name;

			var numType = "integer";
			switch (type)
			{
				case NumType.Long:
					numType = "long";
					break;
				case NumType.Double:
					numType = "double";
					break;
				case NumType.Float:
					numType = "float";
					break;
			}

			field.Type = numType;
			field.IndexName = indexName;
			field.Store = store;
			field.PrecisionStep = precisionStep;
			field.Boost = boost;
			field.NullValue = nullValue;
			field.IncludeInAll = includeInAll;

			_fieldSettings[name] = field;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="indexName">The name of the field that will be stored in the index. Defaults to the property/field name.</param>
		/// <param name="format">The date format. Defaults to dateOptionalTime.</param>
		/// <param name="store">Set to yes the store actual field in the index, no to not store it. Defaults to no (note, the JSON document itself is stored, and it can be retrieved from it).</param>
		/// <param name="index">Set to no if the value should not be indexed. In this case, store should be set to yes, since if its not indexed and not stored, there is nothing to do with it.</param>
		/// <param name="precisionStep">The precision step (number of terms generated for each number value). Defaults to 4.</param>
		/// <param name="boost">The boost value. Defaults to 1.0.</param>
		/// <param name="nullValue">When there is a (JSON) null value for the field, use the null_value as the field value. Defaults to not adding the field at all.</param>
		/// <param name="includeInAll">Should the field be included in the _all field (if enabled). Defaults to true or to the parent object type setting.</param>
		public void CreateDateField(string name, string indexName = null,
									string format = null,
									Store store = Store.no,
									IndexType index = IndexType.analyzed,
									int precisionStep = 4,
									double boost = 1.0,
									string nullValue = null,
									bool includeInAll = true)
		{
			Contract.Ensures(_fieldSettings != null);

			var field = new DateFieldSetting();
			field.Name = name;
			field.IndexName = indexName;
			field.Format = format;
			field.Store = store;
			field.Index = index;
			field.PrecisionStep = precisionStep;
			field.Boost = boost;
			field.NullValue = nullValue;
			field.IncludeInAll = includeInAll;

			_fieldSettings[name] = field;
		}

		public void CreateBooleanField(string name, string indexName = null,
									string format = null,
									Store store = Store.no,
									IndexType index = IndexType.analyzed,
									int precisionStep = 4,
									double boost = 1.0,
									string nullValue = null,
									bool includeInAll = true)
		{
			Contract.Ensures(_fieldSettings != null);

			var field = new BooleanFieldSetting();
			field.Name = name;
			field.IndexName = indexName;
			field.Store = store;
			field.Index = index;
			field.Boost = boost;
			field.NullValue = nullValue;
			field.IncludeInAll = includeInAll;

			_fieldSettings[name] = field;
		}

		#endregion

	}
}