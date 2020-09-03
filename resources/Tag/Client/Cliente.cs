using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;

namespace Cliente
{
    public class Cliente : BaseScript
    {

        public Cliente()
        {

            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["gameEventTriggered"] += new Action<string, List<dynamic>>(OnGameEventTriggered);

        }
        
        private void OnGameEventTriggered(string name, List<dynamic> args)
        {
            

            Debug.WriteLine($"game event {name} ({String.Join(", ", args.ToArray())})");

            if(name == "CEventNetworkEntityDamage")
            {
                var victimPedId = args[0];
                var attacker = args[1];
                var weapon = args[4];

               if(weapon == -1569615261 && attacker == GetPlayerPed(-1))
                {
                    TriggerServerEvent("hit", PlayerId(), NetworkGetPlayerIndexFromPed(victimPedId));
                }
            }

        }

        private void OnClientResourceStart(string resourceName)
        {

            SetCanAttackFriendly(GetPlayerPed(-1), true, false);
            NetworkSetFriendlyFireOption(true);
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("giveWeapon", new Action<int, List<object>, string>((source, args, raw) =>
            {
                GiveWeaponToPed(GetPlayerPed(-1), (uint)GetHashKey("WEAPON_KNIFE"), 999, false, false);
            }), false);

            RegisterCommand("dress", new Action<int, List<object>, string>((source, args, raw) =>
            {
                SetPedComponentVariation(-1, 3, 0, 0, 2);
                SetPedPropIndex(GetPlayerPed(-1), 1, 5, 0, true);
            }), false);
           

            RegisterCommand("tag", new Action<int, List<object>, string>((source, args, raw) =>
            {

                if (args.Count == 0)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { $"Not enough arguments, write 'tag help' to show available commands" }
                    });
                }
                else
                {
                    if (args[0].ToString() == "start")
                    {
                        if(args.Count < 2)
                        {
                            TriggerEvent("chat:addMessage", new
                            {
                                color = new[] { 255, 0, 0 },
                                args = new[] { $"You need to indicate duration time in seconds" }
                            });
                        }
                        else
                        {
                            var gameTime = args[1];
                            TriggerServerEvent("start", gameTime);
                        }
                    }
                    else if (args[0].ToString() == "join")
                    {
                        TriggerServerEvent("join", PlayerId(), GetPlayerPed(-1), Game.Player.Name);
                        // TODO: Every tick set health to 100

                        // TODO: Graphical interface
                    }
                    else if (args[0].ToString() == "players")
                    {
                        TriggerServerEvent("playersInfo");
                    }
                    else if (args[0].ToString() == "help")
                    {
                        TriggerEvent("chat:addMessage", new
                        {
                            color = new[] { 255, 0, 0 },
                            args = new[] { $"You can execute the following commands: /tag start, /tag join, /tag players" }
                        });
                    }
                }
            }), false);
        }

    }
}