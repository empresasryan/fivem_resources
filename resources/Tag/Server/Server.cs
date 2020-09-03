using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{

    public class Server : BaseScript
    {
        List<PlayerC> players = new List<PlayerC>();
        List<string> playerNames = new List<string>();
        int counter = 0;
        bool gameStarted = false;
        int totalTime = -1;
        PlayerC playerActive = null;
        bool first = false;
        int startCounter = 0;

        // Detectar que un jugador a golpeado a otro
        // Cambiar quien pilla
        // Sumar segundos del contador activo al que pillaba
        // Reiniciar el contador para el siguiente en pillar

        public class PlayerC
        {
            public int id;
            public int pedId;
            public string name;
            public float time;
            public bool tagging;

            public PlayerC(int id, int pedId, string name, float time, bool tagging)
            {
                this.id = id;
                this.pedId = pedId;
                this.name = name;
                this.time = time;
                this.tagging = tagging;
            }
        }

        public Server()
        {
            EventHandlers["onResourceStart"] += new Action<string>(OnResourceStart);
            EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop);
            EventHandlers["join"] += new Action<int, int, string>(Join);
            EventHandlers["playersInfo"] += new Action(PlayersInfo);
            EventHandlers["start"] += new Action<int>(Start);
            EventHandlers["hit"] += new Action<int, int>(Hit);
        }

        private async Task OnTick()
        {
            await Delay(1000);
            if (first)
            {
                if(startCounter == 0)
                {
                    // Choose random player
                    gameStarted = true;
                    var aleatorio = new Random();
                    var numeroAleatorio = aleatorio.Next(0, players.Count - 1);
                    players[numeroAleatorio].tagging = true;
                    playerActive = players[numeroAleatorio];
                    TriggerClientEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[Tag]", $"It's a {totalTime} seconds game." }
                    });
                    TriggerClientEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[Tag]", $"Player {players[numeroAleatorio].name}, it's your turn, GO!" }
                    });
                    first = false;
                }
                else
                {
                    // Cuenta atrás
                    TriggerClientEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[Tag]", $"...{startCounter}" }
                    });
                }
                startCounter--;
            }
            if(gameStarted && counter < totalTime)
            {
                    playerActive.time++;
                    counter++;
                    TriggerClientEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[Tag]", $"Suma {counter}" }
                    });
            }
            else if(gameStarted && counter == totalTime)
            {
                var winnerWinnerChickenDinner = players[0];
                for (int i = 1; i < players.Count; i++)
                {
                    if (players[i].time < winnerWinnerChickenDinner.time)
                    {
                        winnerWinnerChickenDinner = players[i];
                    }
                }

                TriggerClientEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[Tag]", $"And the winner is.............{winnerWinnerChickenDinner.name}" }
                });

                gameStarted = false;
                counter = 0;
                totalTime = -1;
                players.Clear();
                playerNames.Clear();
            }
        }

        private void Hit(int taggerId, int victimId)
        {
            if (gameStarted)
            {
                Debug.WriteLine($"Attacker: {taggerId}, Victim: {victimId}");
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].id == taggerId && players[i].tagging)
                    {
                        Debug.WriteLine($"Player {players[i].name} hit");
                        players[i].time += counter;
                        players[i].tagging = false;
                        for (int o = 0; o < players.Count; o++)
                        {
                            if (players[o].id == victimId)
                            {
                                Debug.WriteLine($"Victim {players[o].name} got hit");
                                TriggerClientEvent("chat:addMessage", new
                                {
                                    color = new[] { 255, 0, 0 },
                                    args = new[] { "[Tag]", $"Now {players[o].name} is tagging" }
                                });
                                players[o].tagging = true;
                                playerActive = players[o];
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void Start(int gameTime)
        {
            if (!gameStarted && players.Count > 0)
            {
                first = true;
                totalTime = gameTime;
                startCounter = 3;
            }
            else if (players.Count <= 0)
            {
                TriggerClientEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[Tag]", $"Not enough players, you need 2 players at least" }
                });
            }
            else
            {
                TriggerClientEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[Tag]", $"Game already started" }
                });
            }
        }

        private void Join(int playerId, int pedId, string playerName)
        {
            if (!gameStarted)
            {
                var addPlayer = true;
                if (players.Count > 0)
                {
                    foreach (var player in players)
                    {
                        if (player.id == playerId)
                        {
                            TriggerClientEvent("chat:addMessage", new
                            {
                                color = new[] { 255, 0, 0 },
                                args = new[] { "[Tag]", $"{playerName}, you are already inside the game." }
                            });
                            addPlayer = false;
                            break;
                        }
                    }
                }
                if (addPlayer)
                {
                    players.Add(new PlayerC(playerId, pedId, playerName, 0, false));
                    playerNames.Add(playerName);
                    TriggerClientEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[Tag]", $"Player {playerName} added to the game." }
                    });
                }
            }
            
        }

        private void PlayersInfo()
        {
            if (playerNames.Count == 0)
            {
                TriggerClientEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[Tag]", $"Not players inside the game yet" }
                });
            }
            else
            {
                TriggerClientEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[Tag]", $"Players inside the game: {String.Join(", ", playerNames)}" }
                });
            }
        }

        private void OnResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            Tick += OnTick;
            Debug.WriteLine("Tag loaded!");
        }

        private void OnResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            Debug.WriteLine("Tag Stopped!");
        }

    }
}