using System;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace AnimalPetStatusMod
{
    public class AnimalPetStatusMod : Mod
    {
        public static AnimalPetStatusModConfig ModConfig { get; private set; }

        private static int _secondCounter;

        public override void Entry(params object[] objects)
        {
            // Load config file (config.json).
            ModConfig = new AnimalPetStatusModConfig().InitializeConfig(BaseConfigPath);

            // Execute a handler when the save file is loaded.
            PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;

            Log.Info("[AnimalPetStatusMod] Animal Pet Status Mod => Initialized");
            Log.Info("[AnimalPetStatusMod] Update interval: " + ModConfig.UpdateInterval + " seconds.");
        }

        private static void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e)
        {
            // Only load the event handler after the save file has been loaded.
            GameEvents.OneSecondTick += GameEvents_OneSecondTick;
        }

        private static void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            // The logic in this handler will be executed once every <ModConfig.UpdateInterval> seconds.
            _secondCounter++;
            if (_secondCounter < ModConfig.UpdateInterval)
            {
                return;
            }
            _secondCounter = 0;

            // Only check for animal status if the player is somewhere in the farm.
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

            // For each existing animal, play the "anger cloud"-ish emote thingy if it hasn't been pet yet today.
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
