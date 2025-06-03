using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using PlayerRoles;
using Exiled.API.Extensions;
using Exiled.API.Features;
namespace Plugin.Event_Handlers
{
    public static class Playerskibidi
    {
        public static void OnOvercharge(RecontainedEventArgs ev)
        {
            if (Plugin.Instance.CICASSIE == true)
            {
                Plugin.Instance.CICASSIE = false;
                foreach (var player in Player.List)
                {
                    if (player.Role.Team == Team.ChaosInsurgency)
                    {
                        player.IsBypassModeEnabled = false;

                    }
                }
            }
        }

        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {

            if (Plugin.Instance.CICASSIE == true && ev.Player.Role.Team != Team.ChaosInsurgency &&
                (ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRifleman) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosConscript) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosMarauder) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRepressor)))
            {
                ev.Player.IsBypassModeEnabled = true;


            }
            if (Plugin.Instance.CICASSIE == true && ev.Player.Role.Team == Team.ChaosInsurgency &&
               !(ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRifleman) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosConscript) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosMarauder) ||
                ev.Player.PreviousRole.Equals(RoleTypeId.ChaosRepressor)))
            {
                ev.Player.IsBypassModeEnabled = false;


            }
        }
        public static void OnDeath(DiedEventArgs ev)
        {
            if (ev.Player.IsCHI == true && Plugin.Instance.CICASSIE == true)
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
            if (Plugin.Instance.CICASSIE == false && (ev.Player.Role.Team == Team.FoundationForces || ev.Player.Role.Team == Team.Scientists))
            {
                ev.IsTriggerable = false;
            }
        }
        public static void OnCISpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Team == Team.ChaosInsurgency && Plugin.Instance.CICASSIE == true)
            {
                ev.Player.IsBypassModeEnabled = true;
            }
        }

    }
}
