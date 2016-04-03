using System;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AnimalPetStatusMod
{
    public class AnimalPetStatusMod : Mod
    {
        private static int _secondCounter;
        // Number of seconds before checking if the animals were pet. The longer this interval, the less strain on the game, I hope.
        private const int SecondsToUpdate = 5;

        public override void Entry(params object[] objects)
        {
            PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            Log.Info("[AnimalPetStatusMod] Animal Pet Status Mod => Initialized");
        }

        private void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e)
        {
            // Only load the event handler after the save file has been loaded.
            GameEvents.OneSecondTick += GameEvents_OneSecondTick;
        }

        private static void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            _secondCounter++;
            if (_secondCounter < SecondsToUpdate)
            {
                return;
            }
            _secondCounter = 0;

            var currentLocation = Game1.currentLocation;
            if (currentLocation == null)
            {
                Log.Error("[AnimalPetStatusMod] Failed to retrieve current location.");
                return;
            }
            if (!currentLocation.isFarm) return;

            var animals = Game1.getFarm().getAllFarmAnimals();

            if (animals == null)
            {
                Log.Error("[AnimalPetStatusMod] Failed to retrieve farm animals.");
                return;
            }

            if (animals.Count > 0)
            {
                foreach (var animal in animals.Where(animal => !animal.wasPet))
                {
                    animal.doEmote(12);
                }
            }
        }


    }
}
