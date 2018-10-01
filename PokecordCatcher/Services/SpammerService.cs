using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Discord;
using Discord.WebSocket;

namespace PokecordCatcherBot.Services
{
    public class SpammerService : Service
    {
        private Thread spammerThread;

        public SpammerService(PokecordCatcher bot) : base(bot)
        {
            spammerThread = new Thread(DoSpam);
            spammerThread.Start();
        }

        private async void DoSpam()
        {
            while (true)
            {
                if (!State.SpammerEnabled || Client.ConnectionState != ConnectionState.Connected)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var chan = (ITextChannel)Client.GetChannel(Configuration.SpamChannelID);
                    await chan.SendMessageAsync(Configuration.SpammerMessages[Util.rand.Next(Configuration.SpammerMessages.Length)]);
                }
                catch
                {
                    State.SpammerEnabled = false;
                    continue;
                }
                

                Thread.Sleep(Util.rand.Next(Configuration.SpammerMinDelay, Configuration.SpammerMaxDelay));
            }
        }
    }
}
