using System;

namespace ElasticSearch.Client.QueryString
{
	internal enum ExpressionOperators
	{
		Eq,
		Gt,
		Ge,
		Lt,
		Le,
		NotEq,
		Between,
		Like,
//		IsEmpty,
//		IsNotEmpty,
		StartsWith,
		EndWith,
		Contains,
		Fuzzy
	}

	internal enum RangeType
	{
		STRING = 0,
		INT,
		DOUBLE,
		FLOAT,
		DATETIME,
		LONG
	}

	/// <summary>
	/// 表示一个条件表达式
	/// </summary>
	public class ExpressionEx
	{
		private ExpressionEx()
		{
		}

		internal string PropertyName { get; private set; }
		private object _value;
		internal object Value
		{
			get { return _value; }
			private set
			{
				_value = value;
				//				if(value is string)
				//				{
				//				//如果是string，则替换lucene的关键字
				//				_value = _value.ToString().ReplaceLuceneKeywordChar();
				//				} 
			}
		}

		internal object Value1 { get; private set; }
		internal object Value2 { get; private set; }
		internal object Value3 { get; private set; }
		internal object Value4 { get; private set; }


		internal ExpressionOperators ExpressionOperator { get; private set; }

		/// <summary>
		/// 等于条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx Eq(string propertyName, object value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				ExpressionOperator = ExpressionOperators.Eq
			};
			return exp;
		}

		/// <summary>
		/// 大于条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx Gt(string propertyName, float value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.FLOAT,
				ExpressionOperator = ExpressionOperators.Gt
			};
			return exp;
		}
		public static ExpressionEx Gt(string propertyName, long value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.LONG,
				ExpressionOperator = ExpressionOperators.Gt
			};
			return exp;
		}

		public static ExpressionEx Gt(string propertyName, double value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DOUBLE,
				ExpressionOperator = ExpressionOperators.Gt
			};
			return exp;
		}

		public static ExpressionEx Gt(string propertyName, int value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.INT,
				ExpressionOperator = ExpressionOperators.Gt
			};
			return exp;
		}

		public static ExpressionEx Gt(string propertyName, DateTime value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DATETIME,
				ExpressionOperator = ExpressionOperators.Gt
			};
			return exp;
		}

		/// <summary>
		/// 大于等于条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx Ge(string propertyName, float value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.FLOAT,
				ExpressionOperator = ExpressionOperators.Ge
			};
			return exp;
		}
		public static ExpressionEx Ge(string propertyName, long value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.LONG,
				ExpressionOperator = ExpressionOperators.Ge
			};
			return exp;
		}
		public static ExpressionEx Ge(string propertyName, double value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DOUBLE,
				ExpressionOperator = ExpressionOperators.Ge
			};
			return exp;
		}

		public static ExpressionEx Ge(string propertyName, int value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.INT,
				ExpressionOperator = ExpressionOperators.Ge
			};
			return exp;
		}

		public static ExpressionEx Ge(string propertyName, DateTime value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DATETIME,
				ExpressionOperator = ExpressionOperators.Ge
			};
			return exp;
		}

		/// <summary>
		/// 小于条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx Lt(string propertyName, float value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.FLOAT,
				ExpressionOperator = ExpressionOperators.Lt
			};
			return exp;
		}
		public static ExpressionEx Lt(string propertyName, long value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.LONG,
				ExpressionOperator = ExpressionOperators.Lt
			};
			return exp;
		}

		public static ExpressionEx Lt(string propertyName, double value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DOUBLE,
				ExpressionOperator = ExpressionOperators.Lt
			};
			return exp;
		}

		public static ExpressionEx Lt(string propertyName, int value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.INT,
				ExpressionOperator = ExpressionOperators.Lt
			};
			return exp;
		}

		public static ExpressionEx Lt(string propertyName, DateTime value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DATETIME,
				ExpressionOperator = ExpressionOperators.Lt
			};
			return exp;
		}

		/// <summary>
		/// 小于等于条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx Le(string propertyName, float value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.FLOAT,
				ExpressionOperator = ExpressionOperators.Le
			};
			return exp;
		}

		public static ExpressionEx Le(string propertyName, long value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.LONG,
				ExpressionOperator = ExpressionOperators.Le
			};
			return exp;
		}

		public static ExpressionEx Le(string propertyName, double value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DOUBLE,
				ExpressionOperator = ExpressionOperators.Le
			};
			return exp;
		}

		public static ExpressionEx Le(string propertyName, int value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.INT,
				ExpressionOperator = ExpressionOperators.Le
			};
			return exp;
		}

		public static ExpressionEx Le(string propertyName, DateTime value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				Value1 = RangeType.DATETIME,
				ExpressionOperator = ExpressionOperators.Le
			};
			return exp;
		}

		/// <summary>
		/// 不等于条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx NotEq(string propertyName, object value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				ExpressionOperator = ExpressionOperators.NotEq
			};
			return exp;
		}

		/// <summary>
		/// Between条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="valueLo">起始值</param>
		/// <param name="valueHi">结束值</param>
		/// <param name="includeBoundary">是否包含起始边界值</param>
		/// <param name="rangeType">RANGE类型</param>
		/// <returns>一个表达式实例</returns>
		private static ExpressionEx Between(string propertyName, object valueLo, object valueHi, bool includeBoundary,
											RangeType rangeType)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = valueLo,
				Value1 = valueHi,
				Value2 = includeBoundary,
				Value3 = rangeType,
				ExpressionOperator = ExpressionOperators.Between
			};
			return exp;
		}

		public static ExpressionEx Between(string propertyName, object valueLo, object valueHi, bool includeBoundary)
		{
			return Between(propertyName, valueLo, valueHi, includeBoundary, RangeType.STRING);
		}

		/// <summary>
		/// 范围查询条件表达式
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="valueLo"></param>
		/// <param name="valueHi"></param>
		/// <returns></returns>
		public static ExpressionEx Between(string propertyName, object valueLo, object valueHi)
		{
			return Between(propertyName, valueLo, valueHi, true);
		}

		public static ExpressionEx Between(string propertyName, float valueLo, float valueHi)
		{
			return Between(propertyName, valueLo, valueHi, true, RangeType.FLOAT);
		}
		public static ExpressionEx Between(string propertyName, long valueLo, long valueHi)
		{
			return Between(propertyName, valueLo, valueHi, true, RangeType.LONG);
		}
		public static ExpressionEx Between(string propertyName, double valueLo, double valueHi)
		{
			return Between(propertyName, valueLo, valueHi, true, RangeType.DOUBLE);
		}

		public static ExpressionEx Between(string propertyName, int valueLo, int valueHi)
		{
			return Between(propertyName, valueLo, valueHi, true, RangeType.INT);
		}

		/// <summary>
		/// 范围查询
		/// </summary>
		/// <param name="propertyName">字段名</param>
		/// <param name="valueLo">起始值</param>
		/// <param name="valueHi">结束值</param>
		/// <param name="includeBoundry">是否包括边界（两边的值是否包含在结果中）</param>
		/// <returns></returns>
		public static ExpressionEx Between(string propertyName, int valueLo, int valueHi, bool includeBoundry)
		{
			return Between(propertyName, valueLo, valueHi, includeBoundry, RangeType.INT);
		}

		/// <summary>
		/// 范围查询
		/// </summary>
		/// <param name="propertyName">字段名</param>
		/// <param name="start">起始值</param>
		/// <param name="end">结束值</param>
		/// <param name="includeboundry">是否包含边界值（两边的值是否包含在结果中）</param>
		/// <returns></returns>
		public static ExpressionEx Between(string propertyName, DateTime start, DateTime end, bool includeboundry)
		{
			return Between(propertyName, start, end, includeboundry, RangeType.DATETIME);
		}

		public static ExpressionEx Between(string propertyName, DateTime start, DateTime end)
		{
			return Between(propertyName, start, end, true, RangeType.DATETIME);
		}

		/// <summary>
		/// 相似条件表达式
		/// </summary>
		/// <param name="propertyName">属性名称</param>
		/// <param name="value">表达式右边的值</param>
		/// <returns>一个表达式实例</returns>
		public static ExpressionEx Like(string propertyName, object value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				ExpressionOperator = ExpressionOperators.Like
			};
			return exp;
		}

		public static ExpressionEx Fuzzy(string property, object value, float minSimilarity, int prefixLength)
		{
			return new ExpressionEx
			       	{
				PropertyName = property,
				ExpressionOperator = ExpressionOperators.Fuzzy,
				Value = value,
				Value4 = minSimilarity,
				Value3 = prefixLength
			};
		}

		///// <summary>
		///// 判断是否为Empty的条件表达式,只能用于String类型的字段
		///// </summary>
		///// <param name="propertyName">属性名称</param>
		///// <returns>一个表达式实例</returns>
		//public static ExpressionEx IsEmpty(string propertyName)
		//{
		//    var exp = new ExpressionEx
		//    {
		//        PropertyName = propertyName,
		//        ExpressionOperator = ExpressionOperators.IsEmpty
		//    };
		//    return exp;
		//}

		///// <summary>
		///// 判断是否不为Empty的条件表达式,只能用于String类型的字段
		///// </summary>
		///// <param name="propertyName">属性名称</param>
		///// <returns>一个表达式实例</returns>
		//public static ExpressionEx IsNotEmpty(string propertyName)
		//{
		//    var exp = new ExpressionEx
		//    {
		//        PropertyName = propertyName,
		//        ExpressionOperator = ExpressionOperators.IsNotEmpty
		//    };
		//    return exp;
		//}

		public static ExpressionEx StartsWith(string propertyName, string value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				ExpressionOperator = ExpressionOperators.StartsWith
			};
			return exp;
		}

		public static ExpressionEx EndsWith(string propertyName, string value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				ExpressionOperator = ExpressionOperators.EndWith
			};
			return exp;
		}

		public static ExpressionEx Contains(string propertyName, string value)
		{
			var exp = new ExpressionEx
			{
				PropertyName = propertyName,
				Value = value,
				ExpressionOperator = ExpressionOperators.Contains
			};
			return exp;
		}

		/// <summary>
		/// 设置查询精度，仅对数值类范围查询有效
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public ExpressionEx PrecisionStep(int i)
		{
			Value4 = i;
			return this;
		}

		/// <summary>
		/// 设置Fuzzy查询最小区配相识度，值范围：0~1，默认0.5，仅对like查询有效
		/// </summary>
		/// <param name="similiarity"></param>
		public ExpressionEx MinSimilarity(float similiarity)
		{
			Value4 = similiarity;
			return this;
		}
	}
}