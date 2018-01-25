using GetterBot.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
				CommandHandler.Reply(e, "Gettiä ei lisätty, väärä aika");
				Console.WriteLine("Gettiä ei lisätty, väärä aika");
			}
			else
			{
				using (var db = new TelegramBotContext())
				{
					DateTime today = DateTime.Now;
					if (db.botgets.Where(b => b.get_type_id == getTypeId && DbFunctions.TruncateTime(b.get_date) == today.Date).Any())
					{
						CommandHandler.Reply(e, "Tämä getti on jo olemassa");
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
						CommandHandler.Reply(e, "Getti lisätty");
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

			using (var db = new TelegramBotContext())
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
			using (var db = new TelegramBotContext())
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

		public static string GetTopGetters()
		{
			using (var db = new TelegramBotContext())
			{
				var top = from t in db.top_gets
						  orderby t.usergets descending
						  select t;

				string topGetters = "";
				Console.WriteLine("Top-lista haettu");

				foreach (var topgetter in top)
				{
					topGetters = topGetters + topgetter.usergets + " | " + topgetter.firstname + " " + topgetter.lastname + " @" + topgetter.username + Environment.NewLine;
				}

				string topGettersMessage = "Top gettaajat:" + Environment.NewLine + "#/Etunimi/Sukunimi/Username" + Environment.NewLine + topGetters;

				return topGettersMessage;
			}
		}

		public static string FindClosestGet()
		{
			using (var db = new TelegramBotContext())
			{
				var gets = from g in db.get_type
						   select g.get_type_name;

				List<double> getTimes = new List<double>();
				double timeNow = DateTime.Now.TimeOfDay.TotalSeconds;

				foreach (var get in gets)
				{
					getTimes.Add(DateTime.Parse(get).TimeOfDay.TotalSeconds);
				}

				double closest = getTimes.Where(o => o > timeNow).OrderBy(i => Math.Abs(timeNow - i)).FirstOrDefault();

				TimeSpan timespan = TimeSpan.FromSeconds(closest);
				string hours = (timespan.Hours.ToString().Length < 10) ? "0" + timespan.Hours.ToString() : timespan.Hours.ToString();
				string minutes = (timespan.Minutes.ToString().Length < 10) ? "0" + timespan.Hours.ToString() : timespan.Hours.ToString();




				string nextGet = closest.ToString(hours + ":" + minutes);

				return nextGet;
			}
		}



	}
}
