using System;
using ElasticSearch.Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace ElasticSearch
{
	///ref:FluentCassandra
	///http://github.com/robconery/NoRM/tree/master/NoRM/Connections/
	internal class Connection : IConnection, IDisposable
	{
		private readonly Rest.Client _client;
		private readonly TProtocol _protocol;
		private readonly TTransport _transport;
		private bool _disposed;

		/// <summary>
		/// 
		/// </summary>
		public Connection(Server server, TSocketSettings socketSettings)
		{
			Created = DateTime.Now;
			Server = server;
			if (server.IsFramed)
			{
				_transport = new TFramedTransport(server.Host, server.Port, socketSettings);
			}
			else
			{
				var tsocket = new TSocket(server.Host, server.Port);
				_transport = new TBufferedTransport(tsocket, 1024); //TODO remove hardcode
			}
			_protocol = new TBinaryProtocol(_transport);
			_client = new Rest.Client(_protocol);
		}

		#region IConnection Members

		/// <summary>
		/// 
		/// </summary>
		public DateTime Created { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public Server Server { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsOpen
		{
			get { return _transport.IsOpen; }
		}

		/// <summary>
		/// 
		/// </summary>
		public void Open()
		{
			if (IsOpen)
				return;

			_transport.Open();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			if (!IsOpen)
				return;

			_transport.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		public Rest.Client Client
		{
			get { return _client; }
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			Close();
			_disposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Connection"/> is reclaimed by garbage collection.
		/// </summary>
		~Connection()
		{
			Dispose(false);
		}
	}
}