using static Plugin.Event_Handlers.Playerskibidi;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Loader;
using Exiled.Events.EventArgs.Player;
using System;
using Exiled.Events.Handlers;
using Plugin.Event_Handlers;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Linq;
using RemoteAdmin;
using Player = Exiled.API.Features.Player;
using static UnityEngine.GraphicsBuffer;
using UnityEngine;
using Exiled.CreditTags.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using PlayerRoles;
using MEC;



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


            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;

            Exiled.Events.Handlers.Scp079.Recontained -= Event_Handlers.Playerskibidi.OnOvercharge;
            Exiled.Events.Handlers.Player.ChangingRole -= Event_Handlers.Playerskibidi.OnChangingRole;
            Exiled.Events.Handlers.Player.Died -= Event_Handlers.Playerskibidi.OnDeath;
            Exiled.Events.Handlers.Player.TriggeringTesla -= Event_Handlers.Playerskibidi.OnTesla;

            base.OnDisabled();
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
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CzyBypass : ICommand
    {
        public string Command => "CzyBypass";

        public string[] Aliases => new[] { "czbp" };

        public string Description => "Sprawdza czy masz bypass";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender playerSender)
            {
                Player player = Player.Get(playerSender);

                if (player.IsBypassModeEnabled == true)
                {
                    response = "Masz aktywnego bypassa.";
                    return true;
                }
                else
                {
                    response = "Nie masz bypassa.";
                    return true;
                }

            }
            response = null;
            return false;
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

            if (sender is PlayerCommandSender playerSender)
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
                        response = "Nie masz wystarczających umiejętności aby to zrobić";
                        return false;
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
                }

            }
            response = "SKIBIDI";
            return false;
        }
        private IEnumerator<float> Hackuj(Player player, bool przejmuje)
        {
            System.Random rng = new System.Random();
            int liczba = rng.Next(1, 6);


            if (przejmuje)
            {
                yield return Timing.WaitForSeconds(5f);
                switch (liczba)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                }
                Plugin.Instance.czyhack = false;
                Plugin.Instance.CICASSIE = true;

            }
            else
            {
                yield return Timing.WaitForSeconds(5f);
                switch (liczba)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                }
                Plugin.Instance.czyhack = false;
                Plugin.Instance.CICASSIE = false;
            }


        }
    }
   
    }



