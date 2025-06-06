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
    }
}
