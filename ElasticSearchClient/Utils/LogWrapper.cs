using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using log4net;
using log4net.Config;

namespace ElasticSearch.Utils
{
	internal sealed class LogWrapper
	{
		private static readonly Dictionary<string, LogWrapper> _logs = new Dictionary<string, LogWrapper>();
		private readonly ILog log;

		static LogWrapper()
		{
			string loggingConfigFile = "logging.config";
			string configFileValue = ConfigurationManager.AppSettings["LoggingConfigFile"];

			if (!string.IsNullOrEmpty(configFileValue))
				loggingConfigFile = configFileValue;


			/* Configure log4net based on a config file rather than a linked .config file. 
            * This allows to change logging without restarting the application pool.
                * */
			var configFile = new FileInfo(loggingConfigFile);

			if (configFile.Exists)
			{
				configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, loggingConfigFile));
			}
			else
			{
				FileStream file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, loggingConfigFile));
				var sw = new StreamWriter(file);
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sw.WriteLine("<log4net debug=\"true\">");
				sw.WriteLine("<appender name=\"RollingLogRootFileAppender\" type=\"log4net.Appender.RollingFileAppender\">");
				sw.WriteLine("<file value=\"Logs\\log.txt\" />");
				sw.WriteLine("<appendToFile value=\"true\" />");
				sw.WriteLine("<maxSizeRollBackups value=\"100\" />");
				sw.WriteLine("<maximumFileSize value=\"1MB\" />");
				sw.WriteLine("<rollingStyle   value= \"Date\"   />");
				sw.WriteLine("<datePattern   value= \"yyyyMMdd\"   />");
				sw.WriteLine("<layout type=\"log4net.Layout.PatternLayout\">");
				sw.WriteLine("<conversionPattern value=\"%date %-5level %logger - %message%newline\" />");
				sw.WriteLine("</layout>");
				sw.WriteLine("</appender>");
				sw.WriteLine("<root>");
				sw.WriteLine("<level value=\"DEBUG\" />");
				sw.WriteLine("<appender-ref ref=\"RollingLogRootFileAppender\" />");
				sw.WriteLine("</root>");
				sw.WriteLine("</log4net>");
				sw.Flush();
				sw.Close();
				configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, loggingConfigFile));
			}

			XmlConfigurator.ConfigureAndWatch(configFile);
		}

		/// <summary>
		/// Creates a new instance of the logging wrapper by walking the stack to 
		/// find the calling class and configures the log based on this.
		/// </summary>
		private LogWrapper()
		{
		}

		public LogWrapper(string loggerName)
		{
			log = LogManager.GetLogger(loggerName);
		}

		public bool IsDebugEnabled
		{
			get { return log.IsDebugEnabled; }
		}

		public bool IsErrorEnabled
		{
			get { return log.IsErrorEnabled; }
		}

		public bool IsInfoEnabled
		{
			get { return log.IsInfoEnabled; }
		}

		public bool IsWarnEnabled
		{
			get { return log.IsWarnEnabled; }
		}

		private bool IsMoreEnabled
		{
			get
			{
				if (HttpContext.Current == null)
					return false;


				return false;
			}
		}

		#region Debug

		public void Debug(object message, System.Exception exception)
		{
			log.Debug(message, exception);
		}

		public void Debug(object message)
		{
			log.Debug(message);
		}

		public void DebugFormat(IFormatProvider provider, string format, params object[] args)
		{
			log.DebugFormat(provider, format, args);
		}

		public void DebugFormat(string format, params object[] args)
		{
			log.DebugFormat(format, args);
		}

		#endregion

		#region Error

		public void Error(object message, System.Exception exception)
		{
			log.Error(message, exception);
		}

		public void Error(object message)
		{
			log.Error(message);
		}

		public void Error(System.Exception exception)
		{
			log.Error(null, exception);
		}

		public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
		{
			log.ErrorFormat(provider, format, args);
		}

		public void ErrorFormat(string format, params object[] args)
		{
			log.ErrorFormat(format, args);
		}

		public void ErrorFormat(System.Exception exception, string format, params object[] args)
		{
			log.Error(string.Format(format, args), exception);
		}

		#endregion

		#region Info

		public void Info(object message, System.Exception exception)
		{
			log.Info(message, exception);
		}

		public void Info(object message)
		{
			log.Info(message);
		}

		public void InfoFormat(IFormatProvider provider, string format, params object[] args)
		{
			log.InfoFormat(provider, format, args);
		}

		public void InfoFormat(string format, params object[] args)
		{
			log.InfoFormat(format, args);
		}

		#endregion

		#region Warn

		public void Warn(object message, System.Exception exception)
		{
			log.Warn(message, exception);
		}

		public void Warn(object message)
		{
			log.Warn(message);
		}

		public void WarnFormat(IFormatProvider provider, string format, params object[] args)
		{
			log.WarnFormat(provider, format, args);
		}

		public void WarnFormat(string format, params object[] args)
		{
			log.WarnFormat(format, args);
		}

		#endregion

		#region Method Debug (Uses call-stack to output method name)

		#region Delegates

		/// <summary>
		/// Delegate to allow custom information to be logged
		/// </summary>
		/// <param name="logOutput">Initialized <see cref="StringBuilder"/> object which will be appended to output string</param>
		public delegate void LogOutputMapper(StringBuilder logOutput);

		#endregion

		public void MethodDebugFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat(provider,
				                string.Format("Page: {2}, MethodName: {1}, {0}", format, GetDebugCallingMethod(),
				                              GetDebugCallingPage()), args);
		}

		public void MethodDebugFormat(string format, params object[] args)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat(
					string.Format("Page: {2}, MethodName: {1}, {0}", format, GetDebugCallingMethod(), GetDebugCallingPage()), args);
		}

		public void MethodDebug(string message)
		{
			if (log.IsDebugEnabled)
				log.Debug(string.Format("Page: {2}, MethodName: {1}, {0}", message, GetDebugCallingMethod(), GetDebugCallingPage()));
		}

		// With Log Prefix

		public void MethodDebugFormat(IFormatProvider provider, string logPrefix, string format, params object[] args)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat(provider,
				                string.Format("{0}| {1} , MethodName: {2} , Page: {3}", logPrefix, format, GetDebugCallingMethod(),
				                              GetDebugCallingPage()), args);
		}

		public void MethodDebugFormat(string logPrefix, string format, params object[] args)
		{
			if (log.IsDebugEnabled)
				log.DebugFormat(
					string.Format("{0}| Page: {3}, MethodName: {2} , {1}", logPrefix, format, GetDebugCallingMethod(),
					              GetDebugCallingPage()), args);
		}

		public void MethodDebug(string logPrefix, string message)
		{
			if (log.IsDebugEnabled)
				log.Debug(string.Format("{0}| Page: {3}, MethodName: {2}, {1}", logPrefix, message, GetDebugCallingMethod(),
				                        GetDebugCallingPage()));
		}

		// With Log Prefix and delegate to add custom logging info
		public void MethodDebugFormat(string logPrefix, LogOutputMapper customLogOutput, string format, params object[] args)
		{
			if (log.IsDebugEnabled)
			{
				var additionalLogData = new StringBuilder();
				if (customLogOutput != null)
					customLogOutput(additionalLogData);

				log.DebugFormat(
					string.Format("{0}| Page: {3}, MethodName: {2}, {1}, {4}", logPrefix, format, GetDebugCallingMethod(),
					              GetDebugCallingPage(), additionalLogData), args);
			}
		}

		/// <summary>
		/// Returns calling method name using current stack 
		/// and assuming that first non Logging method is the parent
		/// </summary>
		/// <returns>Method Name</returns>
		private string GetDebugCallingMethod()
		{
			// Walk up the stack to get parent method
			var st = new StackTrace();
			if (st != null)
			{
				for (int i = 0; i < st.FrameCount; i++)
				{
					StackFrame sf = st.GetFrame(i);
					MethodBase method = sf.GetMethod();
					if (method != null)
					{
						string delaringTypeName = method.DeclaringType.FullName;
						if (delaringTypeName != null && delaringTypeName.IndexOf("Logging") < 0)
							return method.Name;
					}
				}
			}

			return "Unknown Method";
		}

		public string CurrentStackTrace()
		{
			var sb = new StringBuilder();
			// Walk up the stack to return everything
			var st = new StackTrace();
			if (st != null)
			{
				for (int i = 0; i < st.FrameCount; i++)
				{
					StackFrame sf = st.GetFrame(i);
					MethodBase method = sf.GetMethod();
					if (method != null)
					{
						Type declaringType = method.DeclaringType;
						//If the MemberInfo object is a global member, (that is, it was obtained from Module.GetMethods(), 
						//which returns global methods on a module), then the returned DeclaringType will be null reference
						if (declaringType == null)
							continue;
						string declaringTypeName = declaringType.FullName;
						if (declaringTypeName != null && declaringTypeName.IndexOf("Logging") < 0)
						{
							sb.AppendFormat("{0}.{1}(", declaringTypeName, method.Name);

							ParameterInfo[] paramArray = method.GetParameters();

							if (paramArray.Length > 0)
							{
								for (int j = 0; j < paramArray.Length; j++)
								{
									sb.AppendFormat("{0} {1}", paramArray[j].ParameterType.Name, paramArray[j].Name);
									if (j + 1 < paramArray.Length)
									{
										sb.Append(", ");
									}
								}
							}
							sb.AppendFormat(")\n - {0}, {1}", sf.GetFileLineNumber(), sf.GetFileName());
						}
					}
					else
					{
						sb.Append("The method returned null\n");
					}
				}
			}
			else
			{
				sb.Append("Unable to get stack trace");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Returns ASP.NET method name which called current method. 
		/// Uses call stack and assumes that all methods starting with 'ASP.' are the ASP.NET page methods
		/// </summary>
		/// <returns>Class Name of the ASP.NET page</returns>
		private string GetDebugCallingPage()
		{
			// Walk up the stack to get calling method which is compiled ASP.Net page
			var st = new StackTrace();
			if (st != null)
			{
				for (int i = 0; i < st.FrameCount; i++)
				{
					StackFrame sf = st.GetFrame(i);
					MethodBase method = sf.GetMethod();
					if (method != null && method.DeclaringType != null)
					{
						string declaringTypeName = method.DeclaringType.FullName;
						if (declaringTypeName != null && declaringTypeName.IndexOf("ASP.") == 0)
							return declaringTypeName;
					}
				}
			}

			return "Unknown Page";
		}

		#endregion

		#region ILogMore methods

		public bool IsMoreDebugEnabled
		{
			get { return log.IsDebugEnabled && IsMoreEnabled; }
		}

		public bool IsMoreInfoEnabled
		{
			get { return log.IsInfoEnabled && IsMoreEnabled; }
		}

		public bool IsMoreErrorEnabled
		{
			get { return log.IsErrorEnabled && IsMoreEnabled; }
		}

		public bool IsMoreWarnEnabled
		{
			get { return log.IsWarnEnabled && IsMoreEnabled; }
		}

		public bool IsMoreFatalEnabled
		{
			get { return log.IsFatalEnabled && IsMoreEnabled; }
		}

		public void MoreInfo(params object[] traceMessages)
		{
			if (log.IsInfoEnabled && null != traceMessages) log.Info(string.Concat(traceMessages));
		}

		public void MoreError(params object[] traceMessages)
		{
			if (log.IsErrorEnabled && null != traceMessages) log.Error(string.Concat(traceMessages));
		}

		public void MoreWarn(params object[] traceMessages)
		{
			if (log.IsWarnEnabled && null != traceMessages) log.Warn(string.Concat(traceMessages));
		}

		public void MoreDebug(params object[] traceMessages)
		{
			if (log.IsDebugEnabled && null != traceMessages) log.Debug(string.Concat(traceMessages));
		}

		[Obsolete("Fatal is not a supported level.")]
		public void MoreFatal(params object[] traceMessages)
		{
			if (log.IsErrorEnabled && null != traceMessages) log.Error(string.Concat(traceMessages));
		}

		#endregion

		#region Exception Logging

		/// <summary>
		/// _logs exception 
		/// </summary>
		/// <param name="exc">Exception to log</param>
		/// <param name="policyName">Policy name to append to logged exception</param>
		/// <remarks>
		/// Does not rethrow exceptions. Use throw; statement to rethrow original exception within catch() block
		/// </remarks>
		/// <returns>true if successful</returns>
		[Obsolete("This is a bad pattern and should not be used")]
		public bool HandleException(System.Exception exc, string policyName)
		{
			log.Warn(policyName, exc);
			return true;
		}

		#endregion

		#region Helper Functions

		private static string ExtractClassName(MethodBase callingMethod)
		{
			string name;

			if (callingMethod == null)
			{
				name = "Unknown";
			}
			else
			{
				Type callingType = callingMethod.DeclaringType;

				if (callingType != null)
				{
					// This is the typical way to get a name on a managed stack.
					name = callingType.FullName;
				}
				else
				{
					// In an unmanaged stack, or in a static function without
					// a declaring type, try getting everything up to the
					// function being called (everything before the last dot).
					name = callingMethod.Name;
					int lastDotIndex = name.LastIndexOf('.');
					if (lastDotIndex > 0)
					{
						name = name.Substring(0, lastDotIndex);
					}
				}
			}

			return name;
		}

		#endregion

		public static LogWrapper GetLogger(string loggerName)
		{
			if (_logs.ContainsKey(loggerName))
			{
				return _logs[loggerName];
			}

			var log = new LogWrapper(loggerName);
			_logs.Add(loggerName, log);
			return log;
		}

		public static LogWrapper GetLogger()
		{
			/*
				 * Get the calling method, to determine the class name.
				 * */
			var stackFrame = new StackFrame(1);
			string name = ExtractClassName(stackFrame.GetMethod());
			return GetLogger(name);
		}


		public static LogWrapper GetLogger(Type type)
		{
			return GetLogger(type.GetType().ToString());
		}
	}
}