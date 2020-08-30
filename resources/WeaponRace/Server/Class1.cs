using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace WeaponRaceServer
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["onResourceStart"] += new Action<string>(OnResourceStart);
            EventHandlers["weaponrace:newLevel"] += new Action<string, string>(NewLevel);
        }

        private void NewLevel(string playerName, string weapon)
        {
            TriggerClientEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                args = new[] { "[WeaponSpawner]", $"Player {playerName} achieved a {weapon}" }
            });
        }

        private void OnResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            Debug.WriteLine("Hello world!");
        }
    }
}
