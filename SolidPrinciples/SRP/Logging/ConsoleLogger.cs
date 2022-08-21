using System;

namespace SRP.Logging
{
	internal class ConsoleLogger : ILogger
	{
		public void Log(String message) => Console.WriteLine(message);
	}
}
