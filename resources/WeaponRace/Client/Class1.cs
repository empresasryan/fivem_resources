using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace RandomWeapons
{
    public class Class1 : BaseScript
    {

        private int curWeapon;
        private int ped;

        private static readonly string[] weaponNames = {
            "WEAPON_RPG", "WEAPON_PISTOL", "WEAPON_COMBATPISTOL", "WEAPON_APPISTOL", "WEAPON_PISTOL50",
            "WEAPON_MICROSMG", "WEAPON_SMG", "WEAPON_ASSAULTSMG", "WEAPON_ASSAULTRIFLE",
            "WEAPON_CARBINERIFLE", "WEAPON_ADVANCEDRIFLE", "WEAPON_MG", "WEAPON_COMBATMG", "WEAPON_PUMPSHOTGUN",
            "WEAPON_SAWNOFFSHOTGUN", "WEAPON_ASSAULTSHOTGUN", "WEAPON_BULLPUPSHOTGUN", "WEAPON_SNIPERRIFLE",
            "WEAPON_HEAVYSNIPER", "WEAPON_GRENADELAUNCHER", "WEAPON_MINIGUN",
            "WEAPON_GRENADE", "WEAPON_MOLOTOV", "WEAPON_SNSPISTOL", "WEAPON_SPECIALCARBINE",
            "WEAPON_HEAVYPISTOL", "WEAPON_BULLPUPRIFLE", "WEAPON_HOMINGLAUNCHER",
            "WEAPON_VINTAGEPISTOL", "WEAPON_MUSKET", "WEAPON_MARKSMANRIFLE",
            "WEAPON_HEAVYSHOTGUN", "WEAPON_GUSENBERG", "WEAPON_RAILGUN", "WEAPON_COMBATPDW",
            "WEAPON_MARKSMANPISTOL", "WEAPON_MACHINEPISTOL",
            "WEAPON_REVOLVER", "WEAPON_COMPACTRIFLE", "WEAPON_DBSHOTGUN", "WEAPON_FLAREGUN",
            "WEAPON_AUTOSHOTGUN", "WEAPON_COMPACTLAUNCHER", "WEAPON_MINISMG", "WEAPON_PIPEBOMB", "WEAPON_KNIFE"
        };
        public Class1()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["gameEventTriggered"] += new Action<string, List<dynamic>>(OnGameEventTriggered);
            EventHandlers["playerSpawned"] += new Action<object>(OnPlayerSpawned);
            curWeapon = 0;
        }

        private void OnPlayerSpawned(object spawnInfo)
        {
            ped = GetPlayerPed(-1);
            Debug.WriteLine("Spawned!");
            GiveWeaponToPed(ped, (uint)GetHashKey(weaponNames[curWeapon]), 1000, false, true);
        }

        private void OnGameEventTriggered(string name, List<dynamic> args)
        {
            Debug.WriteLine($"game event {name} ({String.Join(", ", args.ToArray())})");
            if (name == "CEventNetworkEntityDamage")
            {
                if (args[3] == 1 && IsEntityAPed((int)args[0]))
                {
                    ped = GetPlayerPed(-1);
                    if (ped != (int)args[0])
                    {
                        RemoveAllPedWeapons(ped, true);
                        GiveWeaponToPed(ped, (uint)GetHashKey(weaponNames[++curWeapon]), 1000, false, true);
                        Debug.WriteLine("Trigger event");
                        TriggerServerEvent("weaponrace:newLevel", Game.Player.Name, weaponNames[curWeapon]);
                    }
                }
            }
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("nextW", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                ped = GetPlayerPed(-1);
                RemoveAllPedWeapons(ped, true);
                GiveWeaponToPed(ped, (uint)GetHashKey(weaponNames[++curWeapon]), 1000, false, true);
            }), false);

            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                // account for the argument not being passed
                var model = "adder";
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                }

                // check if the model actually exists
                // assumes the directive using static CitizenFX.Core.Native.API;`
                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[CarSpawner]", $"It might have been a good thing that you tried to spawn a {model}. Who even wants their spawning to actually ^*succeed?" }
                    });
                    return;
                }

                // create the vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                // set the player ped into the vehicle and driver seat
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                // tell the player
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner]", $"Woohoo! Enjoy your new ^*{model}!" }
                });
            }), false);

            RegisterCommand("resetW", new Action<int, List<object>, string>((source, args, raw) =>
            {
                curWeapon = 0;
                ped = GetPlayerPed(-1);
                RemoveAllPedWeapons(ped, true);
                GiveWeaponToPed(ped, (uint)GetHashKey(weaponNames[++curWeapon]), 1000, false, true);
            }), false);

            RegisterCommand("killYou", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Game.PlayerPed.Kill();
            }), false);

            ped = GetPlayerPed(-1);
            curWeapon = 0;
            RemoveAllPedWeapons(ped, true);
            GiveWeaponToPed(ped, (uint)GetHashKey(weaponNames[curWeapon]), 1000, false, true);

            Tick += OnTick;
        }
        private async Task OnTick()
        {
            await Delay(100);
            Game.Player.WantedLevel = 0;
        }
    }
}
