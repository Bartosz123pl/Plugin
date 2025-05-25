using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using Exiled.API.Enums;
using System.Linq;
using InventorySystem.Items.Armor;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Scp079;
using PlayerRoles;
using PluginAPI.Events;
using Exiled.API.Features.Roles;
using Exiled.API.Extensions;
namespace Plugin.Event_Handlers
{
    public static class Playerskibidi
    {
        public static void OnOvercharge(RecontainedEventArgs ev)
        {
            if (Plugin.Instance.CICASSIE == true)
            {
                Plugin.Instance.CICASSIE = false;
            }
        }

        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (Plugin.Instance.CICASSIE == true && ev.Player.Role.Team != PlayerRoles.Team.ChaosInsurgency &&
                (ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRifleman) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosConscript) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosMarauder) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRifleman)))
            {
                ev.Player.IsBypassModeEnabled = false;
            }
            if (Plugin.Instance.CICASSIE == true && ev.Player.Role.Team == PlayerRoles.Team.ChaosInsurgency &&
               !(ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRifleman) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosConscript) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosMarauder) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRifleman)))
            {
                ev.Player.IsBypassModeEnabled = true;
            }
        }
        public static void OnDeath(DiedEventArgs ev)
        {
            if (ev.TargetOldRole.IsChaos() == true && Plugin.Instance.CICASSIE == true)
            {
                ev.Player.IsBypassModeEnabled = false;
            }
        }
        public static void OnTesla(TriggeringTeslaEventArgs ev)
        {
            if (Plugin.Instance.CICASSIE == true && ev.Player.Role.Team == Team.ChaosInsurgency)
            {
                ev.IsTriggerable = false;
            }
        }
    }
}
