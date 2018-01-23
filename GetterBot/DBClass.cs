using GetterBot.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
			int userId = e.Message.From.Id;
			DateTime getDateTime = e.Message.Date;
			int getSeconds = getDateTime.Second;
			string weekday = getDateTime.DayOfWeek.ToString();
			string getTime = getDateTime.ToString("HH:mm");
			int getTypeId = FindGetType(getTime);

			if (getTypeId == 0)
			{
				Console.WriteLine("Gettiä ei lisätty, väärä aika");
			}
			else
			{
				using (var db = new TelegramContext())
				{
					DateTime today = DateTime.Now;
					if (db.botgets.Where(b => b.get_type_id == getTypeId && DbFunctions.TruncateTime(b.get_date) == today.Date).Any())
					{
						Console.WriteLine("Tämä getti on jo olemassa");
					}
					else
					{
						var newGet = new botget { messageid = messageId,
							userid = userId,
							get_date = getDateTime,
							get_seconds = getSeconds,
							get_weekday = weekday,
							get_type_id = getTypeId
						};
						db.botgets.Add(newGet);
						db.SaveChanges();
						Console.WriteLine("Getti lisätty");
					}
				}
			}
			
		}

		public static void AddUser(MessageEventArgs e)
		{
			string userName = e.Message.From.Username;
			int userId = e.Message.From.Id;
			string languageCode = e.Message.From.LanguageCode;
			string firstName = e.Message.From.FirstName;
			string lastName = e.Message.From.LastName;

			using (var db = new TelegramContext())
			{
				if (db.users.Where(u => u.userid == userId).Any())
				{
					Console.WriteLine("Käyttäjä " + userId + " löytyy jo tietokannasta!");
				}
				else
				{
					var addUser = new user { username = userName, userid = userId, languagecode = languageCode, firstname = firstName, lastname = lastName };
					db.users.Add(addUser);
					db.SaveChanges();
					Console.WriteLine("Käyttäjä lisätty tietokantaan: " + userId);
				}
			}
		}

		public static int FindGetType(string time)
		{
			using (var db = new TelegramContext())
			{				
				if (db.get_type.Where(gt => gt.get_type_name == time).Any())
				{
					return db.get_type.Where(gt => gt.get_type_name == time).Select(gt => gt.id).FirstOrDefault();
				}
				else
				{
					return 0;
				}
			}
		}




	}
}
