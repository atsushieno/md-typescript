// This should be imported to mono/mcs/class/System.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp.Net.WebSockets;

internal class MonoTODOAttribute : Attribute
{
	public MonoTODOAttribute ()
	{
	}

	public MonoTODOAttribute (string message)
	{
	}
}

namespace System.Net.WebSockets
{
	public sealed class ClientWebSocket : WebSocket
	{
		public ClientWebSocket ()
		{
			options = new ClientWebSocketOptions ();
		}

		WebSocketCloseStatus? close_status;
		public override WebSocketCloseStatus? CloseStatus {
			get { return close_status; }
		}

		string desc;
		public override string CloseStatusDescription {
			get { return desc; }
		}

		ClientWebSocketOptions options;
		public ClientWebSocketOptions Options {
			get { return options; }
		}

		WebSocketState state;
		public override WebSocketState State {
			get { return state; }
		}

		public override string SubProtocol {
			get { throw new NotImplementedException (); }
		}
		
		public override void Abort ()
		{
			impl.Close ();
			state = WebSocketState.Aborted;
		}

		public override Task CloseAsync (WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			return task_factory.StartNew (() => {
				impl.Close (ToCloseStatusCode (closeStatus), statusDescription);
				close_status = closeStatus;
				desc = statusDescription;
				state = WebSocketState.Closed;
				}, cancellationToken);
		}

		public override Task CloseOutputAsync (WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			throw new NotImplementedException ();
		}

		// FIXME: I'm not fully sure if this mapping is valid.
		static WebSocketSharp.CloseStatusCode ToCloseStatusCode (WebSocketCloseStatus src)
		{
			switch (src) {
			case WebSocketCloseStatus.Empty:
				return WebSocketSharp.CloseStatusCode.NO_STATUS_CODE;
			case WebSocketCloseStatus.EndpointUnavailable:
				return WebSocketSharp.CloseStatusCode.AWAY;
			case WebSocketCloseStatus.InternalServerError:
				return WebSocketSharp.CloseStatusCode.SERVER_ERROR;
			case WebSocketCloseStatus.InvalidMessageType:
				return WebSocketSharp.CloseStatusCode.INCORRECT_DATA;
			case WebSocketCloseStatus.InvalidPayloadData:
				return WebSocketSharp.CloseStatusCode.INCONSISTENT_DATA;
			case WebSocketCloseStatus.MandatoryExtension:
				return WebSocketSharp.CloseStatusCode.IGNORE_EXTENSION;
			case WebSocketCloseStatus.MessageTooBig:
				return WebSocketSharp.CloseStatusCode.TOO_BIG;
			case WebSocketCloseStatus.NormalClosure:
				return WebSocketSharp.CloseStatusCode.NORMAL;
			case WebSocketCloseStatus.PolicyViolation:
				return WebSocketSharp.CloseStatusCode.POLICY_VIOLATION;
			case WebSocketCloseStatus.ProtocolError:
				return WebSocketSharp.CloseStatusCode.PROTOCOL_ERROR;
			}
			throw new ArgumentOutOfRangeException ("unexpected webSocketCloseStatus value: " + src);
		}

		TaskFactory task_factory = new TaskFactory ();
		WebSocketSharp.WebSocket impl;
		
		public Task ConnectAsync (Uri uri, CancellationToken cancellationToken)
		{
			return task_factory.StartNew (() => {
				impl = new WebSocketSharp.WebSocket (uri.ToString (), Options.Subprotocols.ToArray ());
				impl.OnMessage += (object sender, WebSocketSharp.MessageEventArgs e) => {
					received_messages.Enqueue (e);
					receiver_wait_handle.Set ();
				};
			}, cancellationToken);
		}
		
		public override void Dispose ()
		{
			if (impl != null)
				impl.Dispose ();
		}

		AutoResetEvent receiver_wait_handle = new AutoResetEvent (false);
		Queue<WebSocketSharp.MessageEventArgs> received_messages = new Queue<WebSocketSharp.MessageEventArgs> ();

		public override Task<WebSocketReceiveResult> ReceiveAsync (ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			return task_factory.StartNew (() => {
				lock (received_messages) {
					if (received_messages.Count == 0)
						receiver_wait_handle.WaitOne ();
					var e = received_messages.Dequeue ();
					Array.Copy (e.RawData, e.RawData.Length, buffer.Array, buffer.Offset, buffer.Count);
					return new WebSocketReceiveResult (buffer.Count, OpcodeToMessageType (e.Type), true);
				}
			}, cancellationToken);
		}

		static WebSocketMessageType OpcodeToMessageType (WebSocketSharp.Opcode src)
		{
			switch (src) {
			case WebSocketSharp.Opcode.BINARY:
				return WebSocketMessageType.Binary;
			case WebSocketSharp.Opcode.TEXT:
				return WebSocketMessageType.Text;
			case WebSocketSharp.Opcode.CLOSE:
				return WebSocketMessageType.Close;
			default:
				throw new NotImplementedException ();
			}
		}

		[MonoTODO ("messageType and endOfMessage are ignored yet")]
		public override Task SendAsync (ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			return task_factory.StartNew (() => {
				byte [] arr;
				if (buffer.Offset == 0 && buffer.Count == buffer.Array.Length)
					arr = buffer.Array;
				else {
					arr = new byte [buffer.Count];
					Array.Copy (buffer.Array, buffer.Offset, arr, 0, buffer.Count);
				}
				impl.Send (arr);
				}, cancellationToken);
		}
	}
	
	public sealed class ClientWebSocketOptions
	{
		internal ClientWebSocketOptions ()
		{
			Subprotocols = new List<string> ();
		}

		internal List<string> Subprotocols { get; private set; }

		[MonoTODO]
		public X509CertificateCollection ClientCertificates { get; set; }
		
		[MonoTODO]
		public CookieContainer Cookies { get; set; }
		
		[MonoTODO]
		public ICredentials Credentials { get; set; }
		
		[MonoTODO]
		public TimeSpan KeepAliveInterval { get; set; }
		
		[MonoTODO]
		public IWebProxy Proxy { get; set; }
		
		[MonoTODO]
		public bool UseDefaultCredentials { get; set; }		
		
		public void AddSubProtocol (string subProtocol)
		{
			if (subProtocol == null)
				throw new ArgumentNullException ("subProtocol");
			if (!Subprotocols.Contains (subProtocol))
				Subprotocols.Add (subProtocol);
		}
		
		[MonoTODO]
		public void SetBuffer (int receiveBufferSize, int sendBufferSize)
		{
			throw new NotImplementedException ();
		}
		
		[MonoTODO]
		public void SetBuffer (int receiveBufferSize, int sendBufferSize, ArraySegment<byte> buffer)
		{
			throw new NotImplementedException ();
		}
		
		[MonoTODO]
		public void SetRequestHeader (string headerName, string headerValue)
		{
			throw new NotImplementedException ();
		}
	}
	
	public class HttpListenerWebSocketContext : WebSocketContext
	{
		WebSocket web_socket;
		WebSocketSharp.Net.WebSockets.HttpListenerWebSocketContext ctx;
		
		internal HttpListenerWebSocketContext (WebSocket webSocket, WebSocketSharp.Net.WebSockets.HttpListenerWebSocketContext ctx)
		{
			this.web_socket = webSocket;
			this.ctx = ctx;
		}

		// FIXME: need to convert....
		public override CookieCollection CookieCollection {
			get { throw new NotImplementedException (); }
		}
		
		public override NameValueCollection Headers {
			get { return ctx.Headers; }
		}
		
		public override bool IsAuthenticated {
			get { return ctx.IsAuthenticated; }
		}
		
		public override bool IsLocal {
			get { return ctx.IsLocal; }
		}
		
		public override bool IsSecureConnection {
			get { return ctx.IsSecureConnection; }
		}
		
		public override string Origin {
			get { return ctx.Origin; }
		}
		
		public override Uri RequestUri {
			get { return ctx.RequestUri; }
		}
		
		public override string SecWebSocketKey {
			get { return ctx.SecWebSocketKey; }
		}
		
		public override IEnumerable<string> SecWebSocketProtocols {
			get { return ctx.SecWebSocketProtocols; }
		}
		
		public override string SecWebSocketVersion {
			get { return ctx.SecWebSocketVersion; }
		}
		
		public override IPrincipal User {
			get { return ctx.User; }
		}
		
		public override WebSocket WebSocket {
			get { return web_socket; }
		}
	}
	
	public abstract class WebSocket : IDisposable
	{
		public static TimeSpan DefaultKeepAliveInterval {
			get { throw new NotImplementedException (); }
		}
		
		public static ArraySegment<byte> CreateClientBuffer (int receiveBufferSize, int sendBufferSize)
		{
			throw new NotImplementedException ();
		}
		
		public static WebSocket CreateClientWebSocket (Stream innerStream, string subProtocol, int receiveBufferSize, int sendBufferSize, TimeSpan keepAliveInterval, bool useZeroMaskingKey, ArraySegment<byte> internalBuffer)
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public static ArraySegment<byte> CreateServerBuffer (int receiveBufferSize)
		{
			return new ArraySegment<byte> (new byte [receiveBufferSize]);
		}
		
		protected static bool IsStateTerminal (WebSocketState state)
		{
			return state == WebSocketState.Closed || state == WebSocketState.Aborted;
		}
		
		protected static void ThrowOnInvalidState (WebSocketState state, params WebSocketState [] validStates)
		{
			if (validStates != null)
				foreach (var s in validStates)
					if (s == state)
						return;
			throw new WebSocketException (WebSocketError.InvalidState);
		}

		public abstract WebSocketCloseStatus? CloseStatus { get; }
		
		public abstract string CloseStatusDescription { get; }
		
		public abstract WebSocketState State { get; }
		
		public abstract string SubProtocol { get; }
		
		public abstract void Abort();
		
		public abstract Task CloseAsync (WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);
		
		public abstract Task CloseOutputAsync (WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

		public abstract void Dispose();

		public abstract Task<WebSocketReceiveResult> ReceiveAsync (ArraySegment<byte> buffer, CancellationToken cancellationToken);
		
		public abstract Task SendAsync (ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);
	}
	
	public abstract class WebSocketContext
	{
		public abstract CookieCollection CookieCollection { get; }
		
		public abstract NameValueCollection Headers { get; }
		
		public abstract bool IsAuthenticated { get; }
		
		public abstract bool IsLocal { get; }
		
		public abstract bool IsSecureConnection { get; }
		
		public abstract string Origin { get; }
		
		public abstract Uri RequestUri { get; }
		
		public abstract string SecWebSocketKey { get; }
		
		public abstract IEnumerable<string> SecWebSocketProtocols { get; }
		
		public abstract string SecWebSocketVersion { get; }
		
		public abstract IPrincipal User { get; }
		
		public abstract WebSocket WebSocket { get; }
	}
	
	[Serializable]
	public sealed class WebSocketException : Win32Exception
	{
		public WebSocketException ()
			: this ("WebSocket error")
		{
		}
		
		public WebSocketException (string message)
			: this (message, null)
		{
		}
		public WebSocketException (int nativeError)
			: this (nativeError, null)
		{
		}
		public WebSocketException (int nativeError, string message)
			: this (WebSocketError.Success, nativeError, message, null)
		{
		}
		public WebSocketException (string message, Exception innerException)
			: this (WebSocketError.Success, message, null)
		{
		}
		public WebSocketException (WebSocketError error)
			: this (error, null, null)
		{
		}
		public WebSocketException (WebSocketError error, Exception innerException)
			: this (error, null, null)
		{
		}
		public WebSocketException (WebSocketError error, int nativeError)
			: this (error, nativeError, null, null)
		{
		}
		public WebSocketException (WebSocketError error, string message)
			: this (error, message, null)
		{
		}
		public WebSocketException (WebSocketError error, int nativeError, Exception innerException)
			: this (error, nativeError, null, innerException)
		{
		}
		public WebSocketException (WebSocketError error, int nativeError, string message)
			: this (error, nativeError, message, null)
		{
		}
		public WebSocketException (WebSocketError error, string message, Exception innerException)
			: this (error, 0, message, innerException)
		{
		}
		public WebSocketException (WebSocketError error, int nativeError, string message, Exception innerException)
			: base (message)
		{
		}
		
		public override int ErrorCode {
				get { return NativeErrorCode; }
		}
		
		public WebSocketError WebSocketErrorCode { get; private set; }

		[SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			WebSocketErrorCode = (WebSocketError) (int) info.GetInt32 ("WebSocketErrorCode");
		}
	}
	
	public class WebSocketReceiveResult
	{
		public WebSocketReceiveResult (int count, WebSocketMessageType messageType, bool endOfMessage)
			: this (count, messageType, endOfMessage, null, null)
		{
		}
		
		public WebSocketReceiveResult (int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeStatusDescription)
		{
			Count = count;
			MessageType = messageType;
			EndOfMessage = endOfMessage;
			CloseStatus = closeStatus;
			CloseStatusDescription = closeStatusDescription;
		}
		
		public int Count { get; private set; }
		public WebSocketMessageType MessageType { get; private set; }
		public bool EndOfMessage { get; private set; }
		public WebSocketCloseStatus? CloseStatus { get; private set; }
		public string CloseStatusDescription { get; private set; }
	}
	
	public enum WebSocketCloseStatus
	{
		NormalClosure,
		EndpointUnavailable,
		ProtocolError,
		InvalidMessageType,
		Empty,
		InvalidPayloadData,
		PolicyViolation,
		MessageTooBig,
		MandatoryExtension,
		InternalServerError,
	}
	
	public enum WebSocketError
	{
		Success,
		InvalidMessageType,
		Faulted,
		NativeError,
		NotAWebSocket,
		UnsupportedVersion,
		UnsupportedProtocol,
		HeaderError,
		ConnectionClosedPrematurely,
		InvalidState,
	}
	
	public enum WebSocketMessageType
	{
		Text,
		Binary,
		Close,
	}
	
	public enum WebSocketState
	{
		None,
		Connecting,
		Open,
		CloseSent,
		CloseReceived,
		Closed,
		Aborted,
	}
}
