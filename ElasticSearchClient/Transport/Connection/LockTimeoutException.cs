using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ElasticSearch.Client
{
	/// <summary>
	/// Thrown when a lock times out.
	/// </summary>
	[Serializable]
	internal class LockTimeoutException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LockTimeoutException"/> class.
		/// </summary>
		public LockTimeoutException()
			: base("Timeout waiting for lock")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LockTimeoutException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public LockTimeoutException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LockTimeoutException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		/// <param name="innerException">
		/// The inner exception.
		/// </param>
		public LockTimeoutException(string message, System.Exception innerException)
			: base(message, innerException)
		{
		}

#if DEBUG

		/// <summary>
		/// Initializes a new instance of the <see cref="LockTimeoutException"/> class.
		/// </summary>
		/// <param name="blockingStackTrace">The blocking stack trace.</param>
		public LockTimeoutException(StackTrace blockingStackTrace)
		{
			BlockingStackTrace = blockingStackTrace;
		}

#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="LockTimeoutException"/> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		protected LockTimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

#if DEBUG

		/// <summary>
		/// Gets BlockingStackTrace.
		/// </summary>
		public StackTrace BlockingStackTrace { get; private set; }
#endif
	}
}