# PokecordCatcher
Paying for an autocatcher for some shitty Pokemon clone on Discord in 2018? haha lol what a joke

This'll catch you all the pokemon that you want, I guess.

## how 2 use
Download the latest release. [release](https://github.com/ExtraConcentratedJuice/pokecord-catcher/releases/tag/1.0)
Keep in mind that if you are updating from an older release, the configuration might've changed.

Configure configuration.json. Everthing in there is fairly obvious.

Run the bot.

### How do I run the bot???
Download, install the .NET Core runtime from [HERE](https://www.microsoft.com/net/download/thank-you/dotnet-runtime-2.1.1-windows-hosting-bundle-installer)

Navigate to your bot directory and open a command prompt, then run this command:

`dotnet PokecordCatcher.dll`

After that you'll have the bot catch Pokemans for you xd

### OK now I have these cool pokeman hax but how do I filter stuff???
there are built in commands and stuff to help you do that

set OwnerID and UserbotPrefix in config.json first, YOU SHOULD HAVE IT CONFIGURED. If you're confused on whether to include quotes or not, check the example configuration.

Add pokemon names to WhitelistedPokemon if you want to filter to certain pokemon, add guild IDs to WhitelistedGuilds if you want to limit the bot to certain servers.

### Commands

You can toggle the bot's filtering with some commands.

`<prefix>` is the prefix that you set for Userbot in config.json

`<prefix>status` - displays the bot's toggled properties

`<prefix>reload` - reloads config.json 

`<prefix>toggleguilds` - toggles guild whitelisting

`<prefix>togglepokemon` - toggles pokemon whitelisting

`<prefix>echo <message>` - has the bot say something

`<prefix>display <pokemon name>` - has the bot display pokemon of the supplied name

`<prefix>trade <pokemon name>` - has the bot trade all of its pokemon of the supplied name


your settings will persist accross restarts


### FAQ
Q: yo why is the bot recognizing some pokemon as other pokemon


A: I scraped the shit out of bulbapedia for the pokemon list so it isn't perfect, just use the hashing tool to hash and add pokemon to poke.json



Q: something isnt working help pls


A: check all of the issues on the repo for your problem, if you can't find one then make one



Q: goddamn this autocatcher sucks wtf


A: yeah i know lol how about you make it better by submitting a pull request huh