using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace GetterBot
{
	class DBClass
	{
		public static void AddGet(MessageEventArgs e)
		{
			int messageId = e.Message.MessageId;
			DateTime time = e.Message.Date;

			using (var db = new TelegramContext())
			{
				var addGet = new botget { messageid = messageId, get_date = time };
				db.botgets.Add(addGet);
				db.SaveChanges();
				Console.WriteLine("Lisätty");
			}
		}
	}
}
