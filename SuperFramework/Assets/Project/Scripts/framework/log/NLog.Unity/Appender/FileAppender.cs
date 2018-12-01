using NLog;
using System;
using System.IO;
using UnityEngine;

namespace NLog.Unity {
    public class FileAppender : MonoBehaviour {
        FileWriter _fileWriter;
        DefaultLogMessageFormatter _defaultFormatter;
        TimestampFormatter _timestampFormatter;

        void Awake() {
            string path = GetLogPath();
            DateTime now = DateTime.Now;
            string dataTime = string.Concat(new object[] { "Super_", now.Year.ToString(), ".", now.Month.ToString(), ".", now.Day});
            string fileName = path + "/" + dataTime + "_Log.log";

            _fileWriter = new FileWriter(fileName);
            _defaultFormatter = new DefaultLogMessageFormatter();
            _timestampFormatter = new TimestampFormatter();
        }

        void OnEnable() {
            LoggerFactory.AddAppender(write);
        }

        void OnDisable() {
            LoggerFactory.RemoveAppender(write);
        }
        string GetLogPath()
        {
            string path = string.Empty;
            if (Application.isMobilePlatform)
            {
                path = Application.persistentDataPath + "/Gamelog";
            }
            else
            {
                path = Application.dataPath + "/../Gamelog";
            }

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
        void write(Logger logger, LogLevel logLevel, string message) {
            message = _defaultFormatter.FormatMessage(logger, logLevel, message);
            message = _timestampFormatter.FormatMessage(logger, logLevel, message);
            _fileWriter.WriteLine(message);
        }
    }
}