using System;
using System.Collections.Generic;
using System.Text;

namespace PokecordCatcherBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }

        public CommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
