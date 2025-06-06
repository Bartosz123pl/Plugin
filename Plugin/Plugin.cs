using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using RemoteAdmin;
using Player = Exiled.API.Features.Player;
using Exiled.API.Enums;
using PlayerRoles;
using MEC;
using UnityEngine;



namespace Plugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "";
        public override string Author => "Bartek123pl";
        public override Version Version => new Version(1, 0, 3);

        public static Plugin Instance;

        public bool CICASSIE = false;

        public bool czyhack = false;



        public override void OnEnabled()
        {
            Instance = this;

            Exiled.Events.Handlers.Scp079.Recontained += Event_Handlers.Playerskibidi.OnOvercharge;
            Exiled.Events.Handlers.Player.TriggeringTesla += Event_Handlers.Playerskibidi.OnTesla;



            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;

            Exiled.Events.Handlers.Scp079.Recontained -= Event_Handlers.Playerskibidi.OnOvercharge;
            Exiled.Events.Handlers.Player.TriggeringTesla -= Event_Handlers.Playerskibidi.OnTesla;

            base.OnDisabled();
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Hackowansko : ICommand
    {
        public string Command => "hack";

        public string[] Aliases => new[] { "hck" };

        public string Description => "Hackuje C.A.S.S.I.E na rzecz Rebelii Chaosu";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance.CICASSIE == false)
            {
                Plugin.Instance.CICASSIE = true;
                response = "Pomyślnie shackowano C.A.S.S.I.E";
                return true;

            }
            else
            {
                response = "C.A.S.S.I.E jest, już obecnie, kontrolowane przez Rebelię Chaosu";
                return true;
            }
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Unhackowansko : ICommand
    {
        public string Command => "unhack";

        public string[] Aliases => new[] { "uhck" };

        public string Description => "Przywraca kontrolę nad systemem C.A.S.S.I.E Fundacji SCP";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance.CICASSIE == false)
            {
                response = "C.A.S.S.I.E jest, już obecnie, kontrolowane przez siły fundacji";
                return true;
            }
            else
            {
                Plugin.Instance.CICASSIE = false;
                response = "Pomyślnie odhackowano C.A.S.S.I.E";
                return true;
            }
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class KontrolaCASSIE : ICommand
    {
        public string Command => "Kontrola-C.A.S.S.I.E";

        public string[] Aliases => new[] { "koncas" };

        public string Description => "Sprawdza kto kontroluje C.A.S.S.I.E";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance.CICASSIE == false)
            {
                response = "C.A.S.S.I.E jest kontrolowane przez siły fundacji";
                return true;
            }
            else
            {
                response = "C.A.S.S.I.E jest kontrolowane przez Rebelię Chaosu";
                return true;
            }
        }
    }

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Hackowanie : ICommand
    {
        public string Command => "hack";

        public string[] Aliases => new[] { "hk" };

        public string Description => "Hackuje system C.A.S.S.I.E";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (sender is ICommandSender playerSender)
            {
                Player player = Player.Get(playerSender);
                if (Plugin.Instance.CICASSIE == false)
                {
                    if (Plugin.Instance.czyhack != false)
                    {
                        response = "Już ktoś obecnie hackuje systemy";
                        return false;
                    }
                    if (player.Role.Team != Team.ChaosInsurgency)
                    {
                        response = "Co ty robisz wariacie?";
                        return false;
                    }
                    if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.KeycardChaosInsurgency)
                    {
                        response = "Trzymaj urządzenie hackujące w ręku";
                        return false;
                    }
                    Vector3 pos = new Vector3(Plugin.Instance.Config.SpawnPoint[0], Plugin.Instance.Config.SpawnPoint[1], Plugin.Instance.Config.SpawnPoint[2]);
                    if (Vector3.Distance(player.Position,  pos) > 3f)
                    {
                        response = "BRAK ZASIĘGU";
                        return false;
                    }
                    Plugin.Instance.czyhack = true;
                    Timing.RunCoroutine(Hackuj(player, true));
                    response = "Rozpoczęto przejmowanie systemu...";
                    return true;
                }
                else
                {
                    if (Plugin.Instance.czyhack != false)
                    {
                        response = "Już ktoś inny odhackowuje system";
                        return false;
                    }
                    if (player.Role.Team != Team.FoundationForces)
                    {
                        if(player.Role.Team == Team.ChaosInsurgency)
                        {
                            response = "Systemy są już przejęte";
                            return false;
                        }
                        else
                        {
                            response = "Nie masz wystarczających umiejętności aby to zrobić";
                            return false;
                        }

                    }
                    if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.KeycardContainmentEngineer)
                    {
                        response = "Weź swoją kartę do ręki!";
                        return false;
                    }
                    Vector3 pos = new Vector3(Plugin.Instance.Config.SpawnPoint[0], Plugin.Instance.Config.SpawnPoint[1], Plugin.Instance.Config.SpawnPoint[2]);
                    if (Vector3.Distance(player.Position, pos) > 3f)
                    {
                        response = "Brak połączenia";
                        return false;
                    }
                    Plugin.Instance.czyhack = true;
                    Timing.RunCoroutine(Hackuj(player, false));
                    response = "Rozpoczęto przejmowanie systemu...";
                    return true;
                }

            }
            else
            {

            }
            response = null;
            return false;
        }
        private IEnumerator<float> Hackuj(Player player, bool przejmuje)
        {



            if (przejmuje)
            {
                float duration = 60f;
                Vector3 startPosition = player.Position;
                while (duration > 0f)
                {
                    if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.KeycardChaosInsurgency)
                    {
                        Plugin.Instance.czyhack = false;
                        yield break;

                    }

                    if (Vector3.Distance(player.Position, startPosition) > 2f)
                    {
                        Plugin.Instance.czyhack = false;
                        yield break;
                    }

                    duration -= 0.2f;
                    yield return Timing.WaitForSeconds(0.2f);
                }
                foreach (Room room in Room.List)
                {
                    room.TurnOffLights(20f);
                }
                Cassie.Message("jam_043_9 warning .G6 .G4 .G5 .G6 pitch_0.85 jam_043_9 warning pitch_0.95 Cassie pitch_0.8 malfunction jam_043_9 Detected . . pitch_0.9 To all personnel . Cassie system is under jam_043_9 attack .G6 .G4 .G5 .G6\r\n");
                Plugin.Instance.czyhack = false;
                Plugin.Instance.CICASSIE = true;
            }
            else
            {
                float duration = 60f;
                Vector3 startPosition = player.Position;
                while (duration > 0f)
                {
                    if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.KeycardContainmentEngineer)
                    {
                        Plugin.Instance.czyhack = false;
                        yield break;
                    }

                    if (Vector3.Distance(player.Position, startPosition) > 2f)
                    {
                        Plugin.Instance.czyhack = false;
                        yield break;
                    }

                    duration -= 0.2f;
                    yield return Timing.WaitForSeconds(0.2f);
                }
                foreach (Room room in Room.List)
                {
                    room.TurnOffLights(20f);
                }
                Cassie.Message(".G3 .G5 .G2 pitch_0.9 Attention .G6 .G4 .G3 Attention Cassie system repair procedure is being activated . please stand by until overcharge is finished .G3 .G5 .G2 .G6 .G4 .G3");
                Plugin.Instance.czyhack = false;
                Plugin.Instance.CICASSIE = false;

            }


        }
    }
   
    }



