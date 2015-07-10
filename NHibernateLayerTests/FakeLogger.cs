using System;
using System.Text;
using Cravens.Infrastructure.Logging;

namespace NHibernateLayerTests
{
    public class FakeLogger : ILogger
    {
        private readonly StringBuilder _data;
        public string Data { get { return _data.ToString(); } }

        public FakeLogger()
        {
            _data = new StringBuilder();
        }

        public void Debug(string message)
        {
            AddMsg("Debug", message, null);
        }

        public void Error(string message, Exception e)
        {
            AddMsg("Error", message, e);
        }

        public void Error(string message)
        {
            AddMsg("Error", message, null);
        }

        public void Info(string message)
        {
            AddMsg("Info", message, null);
        }

        public void Warn(string message)
        {
            AddMsg("Warn", message, null);
        }

        private void AddMsg(string level, string message, Exception e)
        {
            string msg = DateTime.Now + "\t" + level + "\t" + message;
            if(e!=null)
            {
                msg += e.ToString();
            }
            _data.AppendLine(msg);
        }
    }
}