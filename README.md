# Get Bot

[![Telegram](https://telegram.org/img/t_logo.png)](https://telegram.org/)

Get Bot is a telegram bot purposed to keep track of people getting timestamps (term pending). "Getting" is an activity which involves letting other people know that you know what time it is. When viewing time in an HH:MM format, it sometimes forms patterns that are recongnizable by a popular reference - for example one of the most famous being 13:37 for [leetspeak](https://en.wikipedia.org/wiki/Leet), or [04:20](https://en.wikipedia.org/wiki/420_(cannabis_culture)). Somewhere around lunch-time you take a look at the clock, and notice its once again 13:37. You proudly shout "Get!", but all you receive is a bunch of questioning looks and shooking heads. What a shame. Fortunately we live in the future, and there is always a community interested in common leisures - just a Telegram group chat away! This bot is designed to keep track of timestamp gets in a certain channel. The most common use for this is to just keep track on who was first to declare a certain point of time.

Currently there is no "official" bot to use on a channel, so this bot must be built and hosted individiually.

## Setting up for development and building
This bot is a .NET Framework application, so you are required to have visual studio installed for making this to work. There is also a workaround for building the project through command line only.
### Visual Studio
#### Installation & Building
_todo_

### CMD / Terminal
#### Installation & Building
- You need the following requirements installed
  - [.NET Framework 4.6](https://www.microsoft.com/en-us/download/details.aspx?id=48130)
  - [Microoft Build Tools](https://www.microsoft.com/en-us/download/details.aspx?id=48159)
  - [.NET Framework 4.6 Targeting Pack](https://www.microsoft.com/en-us/download/details.aspx?id=48136)
  - [Nuget executable](https://www.nuget.org/downloads) - Pay attention to where you save this executable (later referred to as `[nuget_executable]`)
- Clone repository `git clone https://github.com/Veixer/GetterBot.git` or by downloading the zip.
- Navigate to the project root. We will refer to this by `[project_root]`
- Navigate to `[project_root]/GetterBot/`. Inside, copy the file `App.config.template` and rename it as `App.config`. Open up the file and proceed to fill in your [Telegram key](https://core.telegram.org/api/obtaining_api_id) and [database connection string](https://msdn.microsoft.com/en-us/library/jj653752(v=vs.110).aspx) used by the application. 
- Run `path/to/your/nuget.exe restore GetterBot.sln` to install packets required by the application.
- Run `C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe [project_root]/GetterBot.sln`

After the last command you should be able to navigate to `[project_root]/bin/Debug/` and execute the GetterBot application there.

## Bot Usage
You start by adding the bot to the group which you would like to conduct observations on time-getting.

### Command List
List of keywords and commands that the bot listens to.
| Command    | Description                                               |
|:----------:|-----------------------------------------------------------| 
| `get`      | Assign get. Not a command, just text is accepted          | 
| `/top`     | Output scoreboard of getters                              | 
| `/nextget` | Output which point of time is next get possible.          | 
| `/getlist` | List of all possible points of time available for getting | 

#### TODO
- [ ] Instructions on setting up project on visual studio
- [ ] Cooldown - a cooldown so that a user can only get once every 10-15 seconds
- [ ] `/help` command for displaying all possible and needed info
- [ ] `/stats` & `/statsme` general stats list for channel/certain user
- 
#### Hall of Fame (List of Contributor)
[@Veixer](https://github.com/Veixer/) - Creator of the project
[@Rantti](https://github.com/Rantti/) - Wrote this readme and helped planning