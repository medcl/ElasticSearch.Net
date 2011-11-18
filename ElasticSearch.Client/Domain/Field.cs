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
}
