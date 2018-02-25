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
			int chatId = (int)e.Message.Chat.Id;
			string chatTitle = e.Message.Chat.Title;

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
					if (db.botgets.Where(b => b.get_type_id == getTypeId && DbFunctions.TruncateTime(b.get_date) == today.Date && b.chatid == chatId).Any())
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
							get_type_id = getTypeId,
							chatid = chatId,
							chattitle = chatTitle
						};
						db.botgets.Add(newGet);
						db.SaveChanges();
						CommandHandler.Reply(e, "Getti lisätty. Onnistuit gettaamaan " + getSeconds + " sekunnissa!");
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

		public static string GetTopGetters(MessageEventArgs e)
		{
			int chatId = (int)e.Message.Chat.Id;

			using (var db = new TelegramBotContext())
			{
				var top = from t in db.top_gets
						  where t.chatid == chatId
						  orderby t.usergets descending
						  select t;

				string topGetters = "";
				Console.WriteLine("Top-lista haettu");

				foreach (var topgetter in top)
				{
					topGetters = topGetters + topgetter.usergets + " | " + topgetter.firstname + " " + topgetter.lastname + " " + topgetter.username + Environment.NewLine;
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
				string hours = (timespan.Hours.ToString().Length < 2) ? "0" + timespan.Hours.ToString() : timespan.Hours.ToString();
				string minutes = (timespan.Minutes.ToString().Length < 2) ? "0" + timespan.Minutes.ToString() : timespan.Minutes.ToString();
				string nextGet = "Seuraava getti on: " + hours + ":" + minutes;

				return nextGet;
			}
		}

		public static string GetGetList()
		{
			using (var db = new TelegramBotContext())
			{
				var gList = from gl in db.get_type
							  where gl.get_enabled == true
							  orderby gl.get_type_hour, gl.get_type_minute
							  select gl.get_type_name;

				string getDbList = "";
				string nextGetRaw = FindClosestGet();
				string nextGet = nextGetRaw.Substring(nextGetRaw.Length - 5);

				foreach (var get in gList)
				{					
					if (get == nextGet)
					{
						getDbList = getDbList + "*" + get + " <-- Seuraava get*" + Environment.NewLine;
					}
					else
					{
						getDbList = getDbList + get + Environment.NewLine;
					}
					
				}

				string getList = "*Getit käytettävissä:*" + Environment.NewLine + getDbList;

				return getList;
			}
		}



		public static string GetGlobalStats(MessageEventArgs e)
		{
			using (var db = new TelegramBotContext())
			{
				string globalStats = "";
				var addLine = Environment.NewLine;

				var getCount = db.botgets.Count();				
				var channelGetCount = db.botgets.Where(x => x.chatid == e.Message.Chat.Id).Count();
				var mostGettedType = db.most_getted_type;
				var mostGettedWeekday = db.most_getted_weekday;
				var globalGetTimeAverage = db.botgets.Select(r => r.get_seconds).Average();
				var channelGetTimeAverage = db.botgets.Where(og => og.chatid == e.Message.Chat.Id).Select(g => g.get_seconds).Average();
				var mostDankGets = db.most_dankgets;
				var mostTrueDankGets = db.most_truedankgets;
				var mostLeetGets = db.most_leetgets;
				
				string gCount = "Yleinen gettimäärä" + addLine + "- " + getCount.ToString() + addLine + addLine;
				string cGetCount = "Kanavan gettimäärä" + addLine + "- " + channelGetCount.ToString() + addLine + addLine;
				string mGettedType = "Yleisin getti";
				string mGettedWeekday = "Yleisin gettipäivä";
				string gGetTimeAverage = "Yleinen getin keskiarvo" + addLine + "- " + globalGetTimeAverage.ToString() + "s" + addLine + addLine;
				string cGetTimeAverage = "Kanavan getin keskiarvo" + addLine + "- " + channelGetTimeAverage.ToString() + "s" + addLine + addLine;
				string mDankGets = "Eniten 16:20 gettejä kanavalla";
				string mTrueDankGets = "Eniten 4:20 gettejä kanavalla";
				string mLeetGets = "Eniten 13:37 gettejä kanavalla";
				

				

				foreach (var mgt in mostGettedType)
				{
					mGettedType = mGettedType + addLine + "- " + mgt.get_type_name;
				}

				foreach (var mgw in mostGettedWeekday)
				{
					mGettedWeekday = mGettedWeekday + addLine + "- " + mgw.get_weekday;
				}

				foreach (var mdg in mostDankGets)
				{
					mDankGets = mDankGets + addLine + "- " + mdg.firstname + " " + mdg.lastname + " " + mdg.username;
				}

				foreach (var mtdg in mostTrueDankGets)
				{
					mTrueDankGets = mTrueDankGets + addLine + "- " + mtdg.firstname + " " + mtdg.lastname + " " + mtdg.username;
				}

				foreach (var mlg in mostLeetGets)
				{
					mLeetGets = mLeetGets + addLine + "- " + mlg.firstname + " " + mlg.lastname + " " + mlg.username;
				}

				globalStats = "*~~~ STATISTICS ~~~*" + addLine +  addLine + gCount + cGetCount + mGettedType + addLine + addLine + mGettedWeekday + addLine + addLine + gGetTimeAverage + cGetTimeAverage + mDankGets + addLine + addLine + mTrueDankGets + addLine + addLine + mLeetGets;



				return globalStats;
			}
		}





	}
}
