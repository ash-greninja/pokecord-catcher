using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using PokecordCatcherBot.Attributes;
using PokecordCatcherBot.Models;

namespace PokecordCatcherBot.Modules
{
    public class CommandService : Service
    {
        private readonly List<MethodInfo> commandMethods;

        public CommandService(PokecordCatcher bot) : base(bot)
        {
            commandMethods = FindCommandMethods();

            Client.MessageReceived += async x => Task.Run(async () => await OnMessage(x));
        }

        private async Task OnMessage(SocketMessage msg)
        {
            if (!msg.Content.StartsWith(Configuration.UserbotPrefix) || msg.Author.Id != Configuration.OwnerID)
                return;

            var args = msg.Content.Split(' ').ToList();
            var commandName = args[0].Substring(Configuration.UserbotPrefix.Length);
            args.RemoveAt(0);

            var command = commandMethods.FirstOrDefault(x => String.Equals(x.GetCustomAttribute<CommandAttribute>().Name, commandName, StringComparison.OrdinalIgnoreCase));

            if (command != null)
            {
                try
                {
                    var t = (Task)command.Invoke(this, new object[] { msg, args.ToArray() });
                    await t;
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
        }

        private List<MethodInfo> FindCommandMethods()
        {
            List<MethodInfo> methods = new List<MethodInfo>();

            methods.AddRange(typeof(PokecordCatcher).Assembly.GetTypes()
                .SelectMany(x => x.GetMethods()).Where(x => x.GetCustomAttribute(typeof(CommandAttribute)) != null));

            return methods;
        }

        [Command(nameof(Status), "Displays information about the bot's state.")]
        public async Task Status(SocketMessage msg, string[] args)
        {
            var props = typeof(State).GetProperties();
            await msg.Channel.SendMessageAsync($"```{String.Join('\n', props.Select(x => $"{x.Name}: {x.GetValue(State)}"))}```");
        }

        [Command(nameof(Reload), "Reload the bot's configuration.")]
        public async Task Reload(SocketMessage msg, string[] args)
        {
            bot.UpdateConfiguration("config.json");
            await msg.Channel.SendMessageAsync("Configuration reloaded.");
        }

        [Command(nameof(ToggleGuilds), "Toggle guild whitelisting.")]
        public async Task ToggleGuilds(SocketMessage msg, string[] args)
        {
            State.WhitelistGuilds = !State.WhitelistGuilds;
            File.WriteAllText("state.data", JsonConvert.SerializeObject(State));
            await msg.Channel.SendMessageAsync("Whitelisting of guilds has been toggled to " + State.WhitelistGuilds);
        }

        [Command(nameof(TogglePokemon), "Toggle pokemon whitelisting.")]
        public async Task TogglePokemon(SocketMessage msg, string[] args)
        {
            State.WhitelistPokemon = !State.WhitelistPokemon;
            File.WriteAllText("state.data", JsonConvert.SerializeObject(State));
            await msg.Channel.SendMessageAsync("Whitelisting of pokemon has been toggled to " + State.WhitelistPokemon);
        }

        [Command(nameof(ToggleSpam), "Toggle pokemon whitelisting.")]
        public async Task ToggleSpam(SocketMessage msg, string[] args)
        {
            State.SpammerEnabled = !State.SpammerEnabled;
            File.WriteAllText("state.data", JsonConvert.SerializeObject(State));
            await msg.Channel.SendMessageAsync("Spam has been toggled to " + State.SpammerEnabled);
        }

        [Command(nameof(Echo), "Has the bot say something.")]
        public async Task Echo(SocketMessage msg, string[] args) => 
            await msg.Channel.SendMessageAsync(String.Join(' ', args));

        [Command(nameof(Display), "Displays all pokemon of a certain name.")]
        public async Task Display(SocketMessage msg, string[] args) => 
            await msg.Channel.SendMessageAsync($"{Configuration.PokecordPrefix}pokemon --name {String.Join(' ', args)}");

        [Command(nameof(Exit), "Exits the userbot program.")]
        public async Task Exit(SocketMessage msg, string[] args)
        {
            await Client.LogoutAsync();
            Environment.Exit(0);
        }

        [Command(nameof(Trade), "Trades all pokemon a certain name.")]
        public async Task Trade(SocketMessage msg, string[] args)
        {
            var list = await ResponseGrabber.SendMessageAndGrabResponse(
                (ITextChannel)msg.Channel,
                $"{Configuration.PokecordPrefix}pokemon --name {String.Join(' ', args)}",
                x => x.Channel.Id == msg.Channel.Id && x.Author.Id == PokecordCatcher.POKECORD_ID && x.Embeds.Count > 0 && x.Embeds.First().Title?.StartsWith("Your") == true,
                5
            );

            if (list == null)
            {
                await msg.Channel.SendMessageAsync("Pokecord didn't display pokemon, aborting.");
                return;
            }

            var pokemans = Util.ParsePokemonListing(list.Embeds.First().Description);

            await Task.Delay(1500);

            var trade = await ResponseGrabber.SendMessageAndGrabResponse(
                (ITextChannel)msg.Channel,
                $"{Configuration.PokecordPrefix}trade <@{Configuration.OwnerID}>",
                x => x.Channel.Id == msg.Channel.Id && x.Author.Id == PokecordCatcher.POKECORD_ID && x.Embeds.Count > 0 && x.Embeds.First().Title?.StartsWith("Trade between ") == true,
                5
            );

            await Task.Delay(1500);

            if (trade == null)
            {
                await msg.Channel.SendMessageAsync("Pokecord didn't create trade, aborting.");
                return;
            }

            await msg.Channel.SendMessageAsync($"{Configuration.PokecordPrefix}p add {String.Join(' ', pokemans.Select(x => x.Id))}");
            await Task.Delay(1500);
            await msg.Channel.SendMessageAsync($"{Configuration.PokecordPrefix}confirm");
            await Task.Delay(1500);
        }
    }
}
