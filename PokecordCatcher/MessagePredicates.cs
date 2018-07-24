using Discord.WebSocket;
using System.Linq;

namespace PokecordCatcherBot
{
    public static class MessagePredicates
    {
        public static bool TradeMessage(SocketMessage x, SocketMessage context) =>
            x.Channel.Id == context.Channel.Id && x.Author.Id == PokecordCatcher.POKECORD_ID && x.Embeds.Count > 0 && x.Embeds.First().Title?.StartsWith("Trade between ") == true;

        public static bool PokemonListingMessage(SocketMessage x, SocketMessage context) =>
            x.Channel.Id == context.Channel.Id && x.Author.Id == PokecordCatcher.POKECORD_ID && x.Embeds.Count > 0 && x.Embeds.First().Title?.StartsWith("Your") == true;

        public static bool SuccessfulCatchMessage(SocketMessage x, SocketMessage context, ulong userId) =>
            x.Channel.Id == context.Channel.Id && x.Author.Id == PokecordCatcher.POKECORD_ID && x.MentionedUsers.Any(y => y.Id == userId) && x.Content.StartsWith("Congratulations");
    }
}
