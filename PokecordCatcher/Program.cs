using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PokecordCatcherBot
{
    public class Program
    {
        static void Main() => new PokecordCatcher().Run().GetAwaiter().GetResult();
    }
}
