using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace AnimalPetStatusMod
{
    public class AnimalPetStatusModConfig : Config
    {
        // Number of seconds before checking if the animals were pet. The longer this interval, the less strain on the game, I hope.
        public int UpdateInterval { get; set; }
        public override T GenerateDefaultConfig<T>()
        {
            UpdateInterval = 4;
            return this as T;
        }
    }
}
