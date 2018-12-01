using System.Net;
using NLog;
using UnityEngine;

namespace NLog.Unity {
    public class ClientSocketAppender : MonoBehaviour {
        public bool cacheLogsWhenNotConnected = true;
        SocketAppender _socket;
        DefaultLogMessageFormatter _defaultFormatter;
        TimestampFormatter _timestampFormatter;
        ColorCodeFormatter _colorFormatter;

        void Awake() {
            _socket = new SocketAppender();
            _defaultFormatter = new DefaultLogMessageFormatter();
            _timestampFormatter = new TimestampFormatter();
            _colorFormatter = new ColorCodeFormatter();

            if (cacheLogsWhenNotConnected) {
                LoggerFactory.AddAppender(send);
            }
        }

        public void OnConnect(string sendToIpAddress, int sendOnPort) {
            if (!cacheLogsWhenNotConnected) {
                LoggerFactory.AddAppender(send);
            }

            _socket.Connect(IPAddress.Parse(sendToIpAddress), sendOnPort);
        }

        void OnDisable() {
            _socket.Disconnect();

            if (!cacheLogsWhenNotConnected) {
                LoggerFactory.RemoveAppender(send);
            }
        }

        void send(Logger logger, LogLevel logLevel, string message) {
            message = _defaultFormatter.FormatMessage(logger, logLevel, message);
            message = _timestampFormatter.FormatMessage(logger, logLevel, message);
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                message = _colorFormatter.FormatMessage(logLevel, message);
            }
            _socket.Send(logLevel, message);
        }
    }
}