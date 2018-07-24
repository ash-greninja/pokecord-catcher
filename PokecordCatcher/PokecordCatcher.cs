using Discord;
using Discord.WebSocket;
using PokecordCatcherBot.Models;
using PokecordCatcherBot.Services;
using System;
using System.Threading.Tasks;

namespace PokecordCatcherBot
{
    public class PokecordCatcher
    {
        public const ulong POKECORD_ID = 365975655608745985;

        public DiscordSocketClient Client { get; }
        public Configuration Configuration { get; private set; }
        public State State { get; private set; }
        public ResponseGrabber ResponseGrabber { get; }

        public CommandService Commands { get; }
        public CatcherService Catcher { get; }
        public SpammerService Spammer { get; }

        public PokecordCatcher()
        {
            UpdateConfiguration("config.json");
            State = Util.ReadConfiguration<State>("state.data");

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
#if DEBUG
                LogLevel = LogSeverity.Debug,
#else
                LogLevel = LogSeverity.Info,
#endif
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance,
            });

            ResponseGrabber = new ResponseGrabber(Client);

            if (Configuration.EnableLogging)
                Logger.StartLogging();

            Client.Log += Log;

            Commands = new CommandService(this);
            Catcher = new CatcherService(this, "poke.json");
            Spammer = new SpammerService(this);
        }

        private async Task Log(LogMessage x) => Console.WriteLine($"[{x.Severity.ToString()}] {x.Message}");

        public void UpdateConfiguration(string fileName)
        {
            Configuration = Util.ReadConfiguration<Configuration>("config.json");

            if (Configuration.CatchMinDelay > Configuration.CatchMaxDelay)
            {
                Console.WriteLine("WARNING: Your CatchMinDelay is greater than your CatchMaxDelay. Setting it to 0.");
                Configuration.CatchMaxDelay = 0;
                Configuration.CatchMinDelay = 0;
            }
        }

        public async Task Run()
        {
            await Client.LoginAsync(TokenType.User, Configuration.Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
