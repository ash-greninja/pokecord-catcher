using Discord.WebSocket;
using PokecordCatcherBot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokecordCatcherBot
{
    public class Service
    {
        protected readonly PokecordCatcher bot;

        protected DiscordSocketClient Client { get => bot.Client; }
        protected Configuration Configuration { get => bot.Configuration; }
        protected State State { get => bot.State; }
        protected ResponseGrabber ResponseGrabber { get => bot.ResponseGrabber; }

        public Service(PokecordCatcher bot) => this.bot = bot;

        private Service() { }
    }
}
