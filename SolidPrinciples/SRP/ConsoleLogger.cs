using System;

namespace SRP
{
	internal class ConsoleLogger : ILogger
	{
		public void Log(String message) => Console.WriteLine(message);
	}
}
