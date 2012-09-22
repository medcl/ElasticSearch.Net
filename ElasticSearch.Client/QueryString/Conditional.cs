using System;
using System.Text;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace ElasticSearch.Client.QueryString
{
	/// <summary>
	/// 表示一个条件语句，由条件表达式组成
	/// </summary>
	public class Conditional
	{
		private static readonly string[] EscapeChars = new[]
		                                               	{
		                                               		"+", "-", "&&", "||", "!", "(", ")", "{", "}", "[", "]", "^", "\"",
		                                               		"~", "*", "?", ":", "\\"
		                                               	};

		private static readonly int EscapeCharsCount = EscapeChars.Length;

		private static readonly string[] EscapeCharOutputs = new[]
		                                                     	{
		                                                     		"\\+", "\\-", "\\&&", "\\||", "\\!", "\\(", "\\)", "\\{", "\\}"
		                                                     		, "\\[", "\\]", "\\^", "\\\"", "\\~", "\\*", "\\?", "\\:",
		                                                     		"\\\\"
		                                                     	};

		private BooleanQuery _queryExpression = new BooleanQuery();

		private Conditional()
		{
		}

		public string Query
		{
			get { return _queryExpression.ToString(); }
		}

		public override string ToString()
		{
			return _queryExpression.ToString();
		}

		/// <summary>
		/// 构造一个条件语句
		/// </summary>
		/// <param name="exp">条件表达式</param>
		/// <returns>条件语句</returns>
		public static Conditional Get(ExpressionEx exp)
		{
			Query bq = Convert(exp);
			return Get(bq);
		}

		/// <summary>
		/// 通过Lucene Query 构造Conditional
		/// </summary>
		/// <param name="query">Lucene查询</param>
		/// <returns></returns>
		public static Conditional Get(Query query)
		{
			var conditional = new Conditional();
			if (query is BooleanQuery)
			{
				foreach (object variable in ((BooleanQuery)query).Clauses())
				{
					conditional._queryExpression.Clauses().Add(variable);
				}
			}
			else
			{
				conditional._queryExpression.Add(query, BooleanClause.Occur.MUST);
			}
			return conditional;
		}

		/// <summary>
		/// 构造一个And条件语句
		/// </summary>
		/// <param name="expl">左边条件表达式</param>
		/// <param name="expr">右边条件表达式</param>
		/// <returns>条件语句</returns>
		public static Conditional And(ExpressionEx expl, ExpressionEx expr)
		{
			var conditional = new Conditional();

			var booleanQuery = new BooleanQuery();

			Query ql = Convert(expl);
			if (ql is BooleanQuery)
			{
				foreach (object variable in ((BooleanQuery)ql).Clauses())
				{
					booleanQuery.Clauses().Add(variable);
				}
			}
			else
			{
				booleanQuery.Add(ql, BooleanClause.Occur.MUST);
			}

			Query qr = Convert(expr);
			if (qr is BooleanQuery)
			{
				foreach (object variable in ((BooleanQuery)qr).Clauses())
				{
					booleanQuery.Clauses().Add(variable);
				}
			}
			else
			{
				booleanQuery.Add(qr, BooleanClause.Occur.MUST);
			}

			conditional._queryExpression = booleanQuery;

			return conditional;
		}


		/// <summary>
		/// 构造一个And条件语句
		/// </summary>
		/// <param name="expl">左边条件表达式</param>
		/// <param name="expr">右边条件表达式</param>
		/// <param name="expList">右边条件表达式</param>
		/// <returns>条件语句</returns>
		public static Conditional And(ExpressionEx expl, ExpressionEx expr, params ExpressionEx[] expList)
		{
			var conditional = new Conditional();

			var booleanQuery = new BooleanQuery();
			booleanQuery.Add(Convert(expl), BooleanClause.Occur.MUST);
			booleanQuery.Add(Convert(expr), BooleanClause.Occur.MUST);

			foreach (ExpressionEx ex in expList)
			{
				booleanQuery.Add(Convert(ex), BooleanClause.Occur.MUST);
			}

			conditional._queryExpression = booleanQuery;
			return conditional;
		}

		/// <summary>
		/// 构造一个And条件语句
		/// </summary>
		/// <param name="expl">条件语句</param>
		/// <returns></returns>
		public Conditional And(ExpressionEx expl)
		{
			if (_queryExpression.Clauses().Count == 0)
			{
				if (expl.ExpressionOperator == ExpressionOperators.NotEq 
					//||expl.ExpressionOperator == ExpressionOperators.IsNotEmpty
					)
				{
					throw new QueryException("第一个条件不允许为NOT类型");
				}
			}
			Query bq = Convert(expl);
			if (bq is BooleanQuery)
			{
				foreach (object variable in ((BooleanQuery)bq).Clauses())
				{
					_queryExpression.Clauses().Add(variable);
				}
			}
			else
			{
				_queryExpression.Add(Convert(expl), BooleanClause.Occur.MUST);
			}

			return this;
		}

		/// <summary>
		/// 构造一个And条件语句
		/// </summary>
		/// <param name="cl">条件语句</param>
		/// <param name="cr">条件语句</param>
		/// <returns>条件语句</returns>
		public static Conditional And(Conditional cl, Conditional cr)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(cl._queryExpression, BooleanClause.Occur.MUST);
			conditional._queryExpression.Add(cr._queryExpression, BooleanClause.Occur.MUST);

			return conditional;
		}

		/// <summary>
		/// 构造一个Or条件语句
		/// </summary>
		public static Conditional Or(ExpressionEx expl, ExpressionEx expr)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(Convert(expl), BooleanClause.Occur.SHOULD);
			conditional._queryExpression.Add(Convert(expr), BooleanClause.Occur.SHOULD);

			return conditional;
		}

		public Conditional Or(ExpressionEx expl)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(_queryExpression, BooleanClause.Occur.SHOULD);
			conditional._queryExpression.Add(Convert(expl), BooleanClause.Occur.SHOULD);
			return conditional;
		}

		/// <summary>
		/// 构造一个Or条件语句
		/// </summary>
		public static Conditional Or(Conditional cl, Conditional cr)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(cl._queryExpression, BooleanClause.Occur.SHOULD);
			conditional._queryExpression.Add(cr._queryExpression, BooleanClause.Occur.SHOULD);
			return conditional;
		}

		/// <summary>
		/// 构造一个Not条件语句
		/// </summary>
		public static Conditional Not(ExpressionEx exp)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(Convert(exp), BooleanClause.Occur.MUST_NOT);
			return conditional;
		}

		/// <summary>
		/// 构造一个Not条件语句
		/// </summary>
		/// <returns></returns>
		public Conditional Not()
		{
			var booleanQuery = new BooleanQuery();
			booleanQuery.Add(_queryExpression, BooleanClause.Occur.MUST_NOT);
			_queryExpression = booleanQuery;
			return this;
		}

		/// <summary>
		/// 构造一个Not条件语句
		/// </summary>
		public Conditional Not(Conditional cr)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(_queryExpression, BooleanClause.Occur.MUST);
			conditional._queryExpression.Add(cr._queryExpression, BooleanClause.Occur.MUST_NOT);
			return conditional;
		}


		/// <summary>
		///escape: + - && || ! ( ) { } [ ] ^ " ~ * ? : \ 
		/// </summary>
		/// <param name="srcStr"></param>
		/// <returns></returns>
		private static string EscapeStr(string srcStr)
		{
			if (srcStr == null)
			{
				return null;
			}
			var sb = new StringBuilder(srcStr);
			for (int i = 0; i < EscapeCharsCount; i++)
			{
				sb = sb.Replace(EscapeChars[i], EscapeCharOutputs[i]);
			}
			return sb.ToString();
		}


		private static string PrepareValueString(string str)
		{
			if (str.Contains("-")) //negative number
			{
				return "\"" + str + "\"";
			}
			//return EscapeStr(str);
			return str;
		}

		private static Query Convert(ExpressionEx exp)
		{
			#region TODO 参数设置

			int maxSupportedIntValue = int.MaxValue;
			int minSupportedIntValue = int.MinValue;
			long minSupportedLongValue = long.MinValue;
			long maxSupportedLongValue = long.MaxValue;
			float minSupportedFloatValue = float.MinValue;
			float maxSupportedFloatValue = float.MaxValue;
			long minSupportedDateNum = 000000000000;

			#endregion

			
			switch (exp.ExpressionOperator)
			{
				#region eq

				case ExpressionOperators.Eq:
					object value = exp.Value;

					#region

					//if (value is bool)
					//{
					//    if((bool)value)
					//    { value = SysDefinition.DATA_TYPE_BOOL_TRUE; }
					//    else
					//    { value = SysDefinition.DATA_TYPE_BOOL_FALSE; }
					//}

					#endregion

					if (value is DateTime)
					{
						string startTime = DateTools.DateToString((DateTime)exp.Value, DateTools.Resolution.MINUTE);
						return new TermQuery(new Term(exp.PropertyName, startTime));
					}
					return new TermQuery(new Term(exp.PropertyName, PrepareValueString(value.ToString())));

				#endregion

				#region ge

				case ExpressionOperators.Ge:
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.INT)
					{
						return NumericRangeQuery.NewIntRange(exp.PropertyName, (int)exp.Value, maxSupportedIntValue, true, true);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.LONG)
					{
						return NumericRangeQuery.NewLongRange(exp.PropertyName, (long)exp.Value, maxSupportedLongValue, true, true);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.FLOAT)
					{
						return NumericRangeQuery.NewFloatRange(exp.PropertyName, (float)exp.Value, maxSupportedFloatValue, true, true);
					}

					#region

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DOUBLE)
					{
						return NumericRangeQuery.NewDoubleRange(exp.PropertyName, (double)exp.Value, double.MaxValue, true, true);
					}

					#endregion

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DATETIME)
					{
						string startTime = DateTools.DateToString((DateTime)exp.Value, DateTools.Resolution.MINUTE);
						long end = long.Parse(startTime);
						return NumericRangeQuery.NewLongRange(exp.PropertyName, end, maxSupportedLongValue, true, true);
					}
					return new TermRangeQuery(exp.PropertyName, PrepareValueString(exp.Value.ToString()), null, true, true);
				#endregion

				#region gt

				case ExpressionOperators.Gt:
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.INT)
					{
						return NumericRangeQuery.NewIntRange(exp.PropertyName, (int)exp.Value, maxSupportedIntValue, false, false);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.LONG)
					{
						return NumericRangeQuery.NewLongRange(exp.PropertyName, (long)exp.Value, maxSupportedLongValue, false, false);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.FLOAT)
					{
						return NumericRangeQuery.NewFloatRange(exp.PropertyName, (float)exp.Value, maxSupportedFloatValue, false, false);
					}

					#region

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DOUBLE)
					{
						return NumericRangeQuery.NewDoubleRange(exp.PropertyName, (double)exp.Value, double.MaxValue, false, false);
					}

					#endregion

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DATETIME)
					{
						string startTime = DateTools.DateToString((DateTime)exp.Value, DateTools.Resolution.MINUTE);
						long end = long.Parse(startTime);
						return NumericRangeQuery.NewLongRange(exp.PropertyName, end, maxSupportedLongValue, false, false);
					}
					return new TermRangeQuery(exp.PropertyName, PrepareValueString(exp.Value.ToString()), null, false, false);

				#endregion

				#region le

				case ExpressionOperators.Le:
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.INT)
					{
						return NumericRangeQuery.NewIntRange(exp.PropertyName, minSupportedIntValue, (int)exp.Value, true, true);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.LONG)
					{
						return NumericRangeQuery.NewLongRange(exp.PropertyName, minSupportedLongValue, (long)exp.Value, true, true);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.FLOAT)
					{
						return NumericRangeQuery.NewFloatRange(exp.PropertyName, minSupportedFloatValue, (float)exp.Value, true, true);
					}

					#region

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DOUBLE)
					{
						return NumericRangeQuery.NewDoubleRange(exp.PropertyName, double.MinValue, (double)exp.Value, true, true);
					}

					#endregion

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DATETIME)
					{
						string endTime = DateTools.DateToString((DateTime)exp.Value, DateTools.Resolution.MINUTE);
						long end = long.Parse(endTime);
						return NumericRangeQuery.NewLongRange(exp.PropertyName, minSupportedDateNum, end, true, true);
					}
					return new TermRangeQuery(exp.PropertyName, null, PrepareValueString(exp.Value.ToString()), true, true);

				#endregion


				#region lt

				case ExpressionOperators.Lt:

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.INT)
					{
						return NumericRangeQuery.NewIntRange(exp.PropertyName, minSupportedIntValue, (int)exp.Value, false, false);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.LONG)
					{
						return NumericRangeQuery.NewLongRange(exp.PropertyName, minSupportedLongValue, (long)exp.Value, false, false);
					}
					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.FLOAT)
					{
						return NumericRangeQuery.NewFloatRange(exp.PropertyName, minSupportedFloatValue, (float)exp.Value, false, false);
					}

					#region

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DOUBLE)
					{
						return NumericRangeQuery.NewDoubleRange(exp.PropertyName, double.MinValue, (double)exp.Value, false, false);
					}

					#endregion

					if (exp.Value1 is RangeType && (RangeType)exp.Value1 == RangeType.DATETIME)
					{
						string startTime = DateTools.DateToString((DateTime)exp.Value, DateTools.Resolution.MINUTE);
						long end = long.Parse(startTime);
						return NumericRangeQuery.NewLongRange(exp.PropertyName, minSupportedDateNum, end, false, false);
					}

					return new TermRangeQuery(exp.PropertyName, null, PrepareValueString(exp.Value.ToString()), false, false);

				#endregion

				#region noteq

				case ExpressionOperators.NotEq:

					object value1 = exp.Value;
					if (value1 is bool)
					{
						if ((bool)value1)
						{
							value1 = SysDefinition.DATA_TYPE_BOOL_TRUE;
						}
						else
						{
							value1 = SysDefinition.DATA_TYPE_BOOL_FALSE;
						}
					}
					if (value1 is DateTime)
					{
						value1 = DateTools.DateToString((DateTime)value1, DateTools.Resolution.MINUTE);
					}
					var bq = new BooleanQuery();
					bq.Add(new BooleanClause(new TermQuery(new Term(exp.PropertyName, PrepareValueString(value1.ToString()))),
											 BooleanClause.Occur.MUST_NOT));
					return bq;

				#endregion

				#region between

				case ExpressionOperators.Between:
					int precisionStep = 4;
					if (exp.Value4 is int)
					{
						precisionStep = (int)exp.Value4;
					}
					switch (exp.Value3 is RangeType ? (RangeType)exp.Value3 : (RangeType)0)
					{
						case RangeType.INT:
							return NumericRangeQuery.NewIntRange(exp.PropertyName, precisionStep, (int)exp.Value, (int)exp.Value1, (bool)exp.Value2,
																 (bool)exp.Value2);
						case RangeType.LONG:
							return NumericRangeQuery.NewLongRange(exp.PropertyName, precisionStep, (long)exp.Value, (long)exp.Value1, (bool)exp.Value2,
																  (bool)exp.Value2);
						case RangeType.FLOAT:
							return NumericRangeQuery.NewFloatRange(exp.PropertyName, precisionStep, (float)exp.Value, (float)exp.Value1, (bool)exp.Value2,
																   (bool)exp.Value2);
						case RangeType.DOUBLE:
							return NumericRangeQuery.NewDoubleRange(exp.PropertyName, precisionStep, (double)exp.Value, (double)exp.Value1,
																	(bool)exp.Value2, (bool)exp.Value2);
						case RangeType.DATETIME:
							string startTime = DateTools.DateToString((DateTime)exp.Value, DateTools.Resolution.MINUTE);
							string endTime = DateTools.DateToString((DateTime)exp.Value1, DateTools.Resolution.MINUTE);
							long start = long.Parse(startTime);
							long end = long.Parse(endTime);

							return NumericRangeQuery.NewLongRange(exp.PropertyName, precisionStep, start, end, (bool)exp.Value2, (bool)exp.Value2);
						default:
							{
								return new TermRangeQuery(exp.PropertyName, PrepareValueString(exp.Value.ToString()),
														  PrepareValueString(exp.Value1.ToString()), (bool)exp.Value2,
														  (bool)exp.Value2);
							}
					}

				#endregion

				case ExpressionOperators.Like:
					int prefixLength = 0;
					float similarity = 0.5f;
					if (exp.Value4 is float)
					{
						similarity = (float)exp.Value4;
					}

					var boolQuery = new BooleanQuery();
					var wildQuery = new WildcardQuery(new Term(exp.PropertyName, PrepareValueString("*" + exp.Value + "*")));
					boolQuery.Add(wildQuery, BooleanClause.Occur.SHOULD);
					boolQuery.Add(new FuzzyQuery(new Term(exp.PropertyName, PrepareValueString(exp.Value.ToString())), similarity), BooleanClause.Occur.SHOULD);
					return boolQuery;

				case ExpressionOperators.Fuzzy:
					similarity = 0.9f;
					if (exp.Value4 is float)
					{
						similarity = (float)exp.Value4;
					}
					prefixLength = 0;
					if (exp.Value3 is int)
					{
						prefixLength = (int)exp.Value3;
					}
					return new FuzzyQuery(new Term(exp.PropertyName, PrepareValueString(exp.Value.ToString())), similarity, prefixLength);
//				case ExpressionOperators.IsEmpty:
//					return new TermQuery(new Term(exp.PropertyName, SysDefinition.DATA_TYPE_NULL_OR_EMPTY));
//
//				case ExpressionOperators.IsNotEmpty:
//					var bq2 = new BooleanQuery();
//					bq2.Add(new BooleanClause(new TermQuery(new Term(exp.PropertyName, SysDefinition.DATA_TYPE_NULL_OR_EMPTY)),
//											  BooleanClause.Occur.MUST_NOT));
//					return bq2;

				case ExpressionOperators.StartsWith:
					return new PrefixQuery(new Term(exp.PropertyName, PrepareValueString(exp.Value.ToString())));
				case ExpressionOperators.EndWith:
					return new WildcardQuery(new Term(exp.PropertyName, PrepareValueString("*" + exp.Value)));
				case ExpressionOperators.Contains:
					return new WildcardQuery(new Term(exp.PropertyName, PrepareValueString("*" + exp.Value + "*")));
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public Conditional And(Conditional filter)
		{
			if (filter != null)
			{
				_queryExpression.Add(filter._queryExpression, BooleanClause.Occur.MUST);
			}
			return this;
		}

		public Conditional Or(Conditional filter)
		{
			var conditional = new Conditional();
			conditional._queryExpression.Add(_queryExpression, BooleanClause.Occur.SHOULD);
			conditional._queryExpression.Add(filter._queryExpression, BooleanClause.Occur.SHOULD);
			return conditional;
		}
	}

	public class SysDefinition
	{
		public static bool DATA_TYPE_BOOL_TRUE=true;
		public static bool DATA_TYPE_BOOL_FALSE=false;
	}
	public class QueryException : System.Exception
	{
		public QueryException(string message):base(message)
		{
		}
	}
}