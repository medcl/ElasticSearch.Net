using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// The boosting query can be used to effectively demote results that match a given query. Unlike the ¡°NOT¡± clause in bool query, this still selects documents that contain undesirable terms, but reduces their overall score.
	/// </summary>
	[JsonObject("boosting")]
	[JsonConverter(typeof(BoostingQueryConverter))]
	public class BoostingQuery:IQuery
	{
		public Field Positive;
		public Field Negative;
		public double NegativeBoost;

		public void SetPositive(string filed,object value)
		{
			Positive =new Field(filed,value);
		}
		public void SetNegative(string filed, object value)
		{
			Negative = new Field(filed, value);
		}
		public void SetNegativeBoost(double boost)
		{
			NegativeBoost = boost;
		}
	}
}