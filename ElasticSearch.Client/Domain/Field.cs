namespace ElasticSearch.Client.QueryDSL
{

	public class Field
	{
		public string Name { get; private set; }
		public object Value { get; private set; }
		public double? Boost { get; private set; }
		
		public Field(string name, double boost) : this(name, boost, null) {}
		public Field(string name, object value) : this(name, null, value) {}
		public Field (string name, double? boost, object value)
		{
			this.Value = value;
			this.Name = name;
			this.Boost = boost;
		}
	}

	public enum SortType
	{
		Asc,
		Desc
	}
	public class SortItem
	{
		public SortItem(string name, SortType type)
		{
			FieldName = name;
			SortType = type;
		}

		public string FieldName { set; get; }
		public SortType SortType { set; get; }

		public override string ToString()
		{
			return FieldName + ":" + SortType.ToString().ToLower();
		}
	}
}
