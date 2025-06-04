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
            Exiled.Events.Handlers.Player.ChangingRole += Event_Handlers.Playerskibidi.OnChangingRole;
            Exiled.Events.Handlers.Player.Died += Event_Handlers.Playerskibidi.OnDeath;
            Exiled.Events.Handlers.Player.TriggeringTesla += Event_Handlers.Playerskibidi.OnTesla;
            Exiled.Events.Handlers.Player.Spawned += Event_Handlers.Playerskibidi.OnCISpawned;



            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;

            Exiled.Events.Handlers.Scp079.Recontained -= Event_Handlers.Playerskibidi.OnOvercharge;
            Exiled.Events.Handlers.Player.ChangingRole -= Event_Handlers.Playerskibidi.OnChangingRole;
            Exiled.Events.Handlers.Player.Died -= Event_Handlers.Playerskibidi.OnDeath;
            Exiled.Events.Handlers.Player.TriggeringTesla -= Event_Handlers.Playerskibidi.OnTesla;
            Exiled.Events.Handlers.Player.Spawned -= Event_Handlers.Playerskibidi.OnCISpawned;

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
                foreach (var lplayer in Player.List)
                {
                    if (lplayer.Role.Team == Team.ChaosInsurgency)
                    {
                        lplayer.IsBypassModeEnabled = true;

                    }
                }
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
                foreach (var lplayer in Player.List)
                {
                    if (lplayer.Role.Team == Team.ChaosInsurgency)
                    {
                        lplayer.IsBypassModeEnabled = false;

                    }
                }
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
                    if (player.CurrentRoom.Type != RoomType.HczHid)
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
                    if (player.Role.Type != RoleTypeId.NtfSpecialist)
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
                    if (player.CurrentRoom.Type != RoomType.HczHid)
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
            System.Random rng = new System.Random();
            int liczba = rng.Next(1, 6);


            if (przejmuje)
            {
                float duration = 5f;
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
                yield return Timing.WaitForSeconds(5f);
                switch (liczba)
                {
                    case 1:
                        player.Broadcast(5, "1a");
                        break;
                    case 2:
                        player.Broadcast(5, "2a");
                        break;
                    case 3:
                        player.Broadcast(5, "3a");
                        break;
                    case 4:
                        player.Broadcast(5, "4a");
                        break;
                    case 5:
                        player.Broadcast(5, "5a");
                        break;
                }
                Plugin.Instance.czyhack = false;
                Plugin.Instance.CICASSIE = true;
                foreach (var lplayer in Player.List)
                {
                    if (lplayer.Role.Team == Team.ChaosInsurgency)
                    {
                        lplayer.IsBypassModeEnabled = true;

                    }
                }
            }
            else
            {
                float duration = 5f;
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
                yield return Timing.WaitForSeconds(5f);
                switch (liczba)
                {
                    case 1:
                        player.Broadcast(5, "1b");
                        break;
                    case 2:
                        player.Broadcast(5, "2b");
                        break;
                    case 3:
                        player.Broadcast(5, "3b");
                        break;
                    case 4:
                        player.Broadcast(5, "4b");
                        break;
                    case 5:
                        player.Broadcast(5, "5b");
                        break;
                }
                Plugin.Instance.czyhack = false;
                Plugin.Instance.CICASSIE = false;
                foreach (var lplayer in Player.List)
                {
                    if (lplayer.Role.Team == Team.ChaosInsurgency)
                    {
                        lplayer.IsBypassModeEnabled = false;

                    }
                }
            }


        }
    }
   
    }



