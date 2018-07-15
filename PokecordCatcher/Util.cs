using PokecordCatcherBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace PokecordCatcherBot
{
    public static class Util
    {
        public static readonly Random rand = new Random();

        public static List<PokemonListing> ParsePokemonListing(string str)
        {
            var result = new List<PokemonListing>();

            foreach (var line in str.Split("\n"))
            {
                var split = line.Split('|').Select(x => x.Trim()).ToArray();

                string name = split[0];
                byte level = Byte.Parse(new string(split[1].Where(c => char.IsDigit(c)).ToArray()));
                int id = Int32.Parse(new string(split[2].Where(c => char.IsDigit(c)).ToArray()));
                double iv = 0;

                if (split.Length > 3)
                    Double.TryParse(new string(split[3].Where(c => char.IsDigit(c) || c == '.').ToArray()), out iv);

                result.Add(new PokemonListing(name, level, id, iv));
            }

            return result;
        }

        public static T ReadConfiguration<T>(string path) where T : new()
        {
            T data;

            if (File.Exists(path))
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            else
            {
                data = new T();
                File.WriteAllText("state.data", JsonConvert.SerializeObject(data));
            }

            return data;
        }
    }
}
