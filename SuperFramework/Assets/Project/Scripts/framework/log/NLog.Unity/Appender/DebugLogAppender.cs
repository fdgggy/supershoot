using NLog;
using UnityEngine;

namespace NLog.Unity {
    public class DebugLogAppender : MonoBehaviour {
        DefaultLogMessageFormatter _defaultFormatter;
        TimestampFormatter _timestampFormatter;

        void Awake()
        {
            _defaultFormatter = new DefaultLogMessageFormatter();
            _timestampFormatter = new TimestampFormatter();
        }
        void OnEnable() {
            LoggerFactory.AddAppender(log);
        }

        void OnDisable() {
            LoggerFactory.RemoveAppender(log);
        }

        void log(Logger logger, LogLevel logLevel, string message) {
            message = _defaultFormatter.FormatMessage(logger, logLevel, message);
            message = _timestampFormatter.FormatMessage(logger, logLevel, message);

            if (logLevel <= LogLevel.Info) {
                Debug.Log(message);
            } else if (logLevel == LogLevel.Warn) {
                Debug.LogWarning(message);
            } else {
                Debug.LogError(message);
            }
        }
    }
}
