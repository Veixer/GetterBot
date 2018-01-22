using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace GetterBot
{
	class Program
	{
		private static string telegramkey = System.Configuration.ConfigurationManager.AppSettings["Telegramkey"];
		protected static readonly TelegramBotClient Bot = new TelegramBotClient(telegramkey);

		static void Main(string[] args)
		{
			StartProgram();
		}

		private static void StartProgram()
		{
			Bot.OnMessage += CommandHandler.Bot_HandleMessage;

			Bot.StartReceiving();
			Console.WriteLine("Ohjelma käynnistetty, paina ENTER sulkeaksesi");
			Console.ReadLine();
			Bot.StopReceiving();
		}
	}
}
