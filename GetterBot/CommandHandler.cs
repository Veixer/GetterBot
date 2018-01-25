using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace GetterBot
{
	class CommandHandler : Program
	{
		public static void BotHandleMessage(object sender, MessageEventArgs e)
		{
			string command = e.Message.Text.ToLower();

			switch (command)
			{
				case "get":
					HandleGet(e);
					break;
				case "/top":
				case "/top@getti_bot":
					Bot.SendTextMessageAsync(e.Message.Chat.Id, HandleTop());
					break;
				case "/nextget":
				case "/nextget@getti_bot":
					Bot.SendTextMessageAsync(e.Message.Chat.Id, NextGet());
					break;
				default:
					break;
			}
		}

		private static void HandleGet(MessageEventArgs e)
		{
			//Bot.SendTextMessageAsync(e.Message.Chat.Id, "Nice get", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, e.Message.MessageId);
			DBClass.AddUser(e);
			DBClass.AddGet(e);
		}

		private static string HandleTop()
		{
			return DBClass.GetTopGetters();
		}

		public static void Reply(MessageEventArgs e, string message)
		{
			Bot.SendTextMessageAsync(e.Message.Chat.Id, message, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, e.Message.MessageId);
		}

		public static string NextGet()
		{
			return DBClass.FindClosestGet();
		}
	}
}
