using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PokecordCatcherBot
{
    public class PokemonComparer
    {
        private readonly ReadOnlyDictionary<string, List<byte[]>> hashes;
        
        public PokemonComparer(Dictionary<string, List<byte[]>> pokemonHashes)
        {
            hashes = new ReadOnlyDictionary<string, List<byte[]>>(pokemonHashes);
        }

        public string GetPokemon(byte[] image)
        {
            Bitmap bitmap;

            using (var stream = new MemoryStream(image))
                bitmap = (Bitmap)Image.FromStream(stream);

            var hash = ImagePhash.ComputeDigest(bitmap.ToLuminanceImage());

            return Compare(hash);
        }

        public string GetPokemon(Stream image)
        {
            var bitmap = (Bitmap)Image.FromStream(image);
            var hash = ImagePhash.ComputeDigest(bitmap.ToLuminanceImage());

            return Compare(hash);
        }

        private string Compare(Digest hash, double minSimilarity = 0)
        {
            Dictionary<string, List<double>> similarities = new Dictionary<string, List<double>>();

            foreach (var x in hashes)
            {
                foreach (var h in x.Value)
                {
                    var correlation = ImagePhash.GetCrossCorrelation(hash.Coefficents, h);

                    if (correlation >= 1.0)
                    {
                        Console.WriteLine($"Detected '{x.Key}' with a similarity of {correlation}");
                        return x.Key;
                    }

                    if (similarities.TryGetValue(x.Key, out var val))
                        val.Add(correlation);
                    else
                        similarities.Add(x.Key, new List<double> { correlation });
                }
            }

            var sim = similarities.OrderByDescending(x => x.Value.Max()).First();

            if (sim.Value.Any(x => x >= minSimilarity))
            {
                Console.WriteLine($"Detected '{sim.Key}' with a similarity of {sim.Value}");
                return sim.Key;
            }

            Console.WriteLine($"Failed to find a Pokemon that satisfies the minimum similarity of {minSimilarity}.");
            return null;
        }
    }
}
