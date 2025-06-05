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
                    if (player.CurrentRoom.Type != RoomType.HczServerRoom)
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
                    if (player.CurrentRoom.Type != RoomType.HczServerRoom)
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
                float duration = 25f;
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
                foreach (Room room in Room.List)
                {
                    room.TurnOffLights(30);
                }
                switch (liczba)
                {
                    case 1:
                        Cassie.Message("pitch_0.85 System . integrity . compromised", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.9 Attention . Unauthorized access detected . in high security sector", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.8 Core override . in progress . please standby", true, true, true);


                        break;
                    case 2:
                        Cassie.Message("glitch_1 Chaos Insurgency has infiltrated . _ system takeover imminent", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.7 C A S S I E . breach . detected", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.75 Unauthorized users are gaining root control", true, true, true);

                        break;
                    case 3:
                        Cassie.Message("pitch_0.95 Warning . system control shifting", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1 . Emergency . protocol . failure .", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("glitch_1 System compromised . security lockdown ineffective", true, true, true);

                        break;
                    case 4:
                        Cassie.Message("pitch_0.6 glitched_signal . access granted . to insurgent force", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("voice_china_male glitch_2 Chaos . rising", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.5 Override accepted . welcome back . insurgency", true, true, true);

                        break;
                    case 5:
                        Cassie.Message("pitch_0.8 You . lost . control", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.75 Chaos Insurgency . now owns . the foundation", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_0.85 System loyalty . has been overwritten", true, true, true);

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
                float duration = 25f;
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
                        Cassie.Message("pitch_1.05 Foundation control . restored", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.1 Command override . successful", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1 C A S S I E . back . under Foundation supervision", true, true, true);

                        break;
                    case 2:
                        Cassie.Message("pitch_1.1 Mobile Task Force . has secured . system core", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.05 Intrusion . neutralized . integrity restored", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.1 Unauthorized access . purged", true, true, true);

                        break;
                    case 3:
                        Cassie.Message("pitch_1.05 Chaos Insurgency . access revoked", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("glitch_1 Security lockdown . reinitiated . threat level decreased", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.1 You were never in control", true, true, true);

                        break;
                    case 4:
                        Cassie.Message("pitch_1.05 Nice try . insurgency . not today", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.1 System rejection . insurgent protocol . obsolete", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.15 MTF engineers . fixed your mistake", true, true, true);

                        break;
                    case 5:
                        Cassie.Message("pitch_1.05 Order . restored", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.05 Insurgency corruption . purged", true, true, true);
                        yield return Timing.WaitForSeconds(10f);
                        Cassie.Message("pitch_1.1 Foundation systems . operational . and secure", true, true, true);

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



