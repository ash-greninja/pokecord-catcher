using System;
using System.Collections.Generic;
using System.Text;

namespace PokecordCatcherBot.Models
{
    public class PokemonListing
    {
        public string Name { get; set; }
        public byte Level { get; set; }
        public int Id { get; set; }
        public double IV { get; set; }

        public PokemonListing(string name, byte level, int id, double iv)
        {
            Name = name;
            Level = level;
            Id = id;
            IV = iv;
        }
    }
}
