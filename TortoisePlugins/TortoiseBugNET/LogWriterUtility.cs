using System;
using System.Collections.Generic;
using System.IO;

namespace LogWriterUtility
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// </summary>
    public class LogWriter
    {
        private static LogWriter _instance;
        private static Queue<Log> _logQueue;
        private static string _logDir = "";
        private static string _logFile = "";
        private static int _maxLogAge;
        private static int _queueSize;
        private static DateTime _lastFlushed = DateTime.Now;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter() { }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter GetInstance(string logDirectory = "", string logFile = "", int maxLogAge = 0, int queueSize = 0)
        {
            // If the instance is null then create one and init the Queue
            if (_instance == null && logDirectory != "" && logFile != "")
            {
                _instance = new LogWriter();
                _logQueue = new Queue<Log>();
                _logDir = logDirectory;
                _logFile = logFile;
                _maxLogAge = maxLogAge;
                _queueSize = queueSize;
            }
            else if (_instance == null)
            {
                throw new TypeInitializationException("LogWriterUtility.LogWriter", new Exception("Log file and log directory must be set before this class is used"));
            }
            return _instance;
        }

        public void WriteExceptionToLog(Exception ex)
        {
            while (true)
            {
                WriteToLog(ex.Message);
                WriteToLog(ex.StackTrace);
                WriteToLog(ex.ToString());
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    continue;
                }
                break;
            }
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToLog(string message)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (_logQueue)
            {
                // Create the entry and push to the Queue
                var logEntry = new Log(message);
                _logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (_logQueue.Count >= _queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }
        }

        private static bool DoPeriodicFlush()
        {
            var logAge = DateTime.Now - _lastFlushed;
            if (logAge.TotalSeconds >= _maxLogAge)
            {
                _lastFlushed = DateTime.Now;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private static void FlushLog()
        {
            while (_logQueue.Count > 0)
            {
                var entry = _logQueue.Dequeue();
                var logPath = _logDir + entry.LogDate + "_" + _logFile;

                // This could be optimised to prevent opening and closing the file for each write
                using (var fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
                {
                    using (var log = new StreamWriter(fs))
                    {
                        log.WriteLine("{0}\t{1}", entry.LogTime, entry.Message);
                    }
                }
            }
        }
    }

    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }

        public Log(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }
}
