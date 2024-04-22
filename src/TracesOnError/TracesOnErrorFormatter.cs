using System.Text;

namespace SSI.Extensions.Logging.TracesOnError
{
    internal class TracesOnErrorFormatter : ITracesOnErrorFormatter
    {
        public TracesOnErrorFormatter()
        {
        }

        public string Process(IList<LogEntry> logs)
        {
            var sb = new StringBuilder();
            var padding = new string(' ', 2);

            var last = logs.LastOrDefault();
            if (last?.Exception != null)
            {
                sb.AppendLine("Exception:");
                sb.Append(padding);
                sb.AppendLine(last.Exception.ToString().Replace(Environment.NewLine, Environment.NewLine + padding));
                sb.AppendLine();
            }

            WriteTracesToStringBuilder(logs, padding, sb);

            return sb.ToString();
        }

        public void WriteTracesToStringBuilder(IList<LogEntry> logs, string padding, StringBuilder sb)
        {
            sb.AppendLine("Traces:");
            foreach (var log in logs)
            {
                WriteMessageToStringBuilder(log, sb, padding);
            }
        }

        private void WriteMessageToStringBuilder(LogEntry log, StringBuilder sb, string padding = "")
        {
            sb.Append(padding);
            sb.Append(log.Category);
            sb.Append('[');
            sb.Append(log.EventId);
            sb.Append(']');
            sb.Append(log.Message.Replace(Environment.NewLine, " "));
            if (log.Scopes.Any())
            {
                foreach (var scopeObject in log.Scopes)
                {
                    sb.Append(scopeObject);
                    sb.Append("=>");
                }
            }

        }
    }
}