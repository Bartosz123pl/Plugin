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



namespace Plugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "";
        public override string Author => "Bartek123pl";
        public override Version Version => new Version(1, 0, 3);

        public static Plugin Instance;





        public override void OnEnabled()
        {
            Instance = this;

            Exiled.Events.Handlers.Player.Shot += Event_Handlers.Playerskibidi.OnHit;
            Exiled.Events.Handlers.Player.UsedItem += Event_Handlers.Playerskibidi.OnSCP_500;


            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Instance = null;

            Exiled.Events.Handlers.Player.Shot -= Event_Handlers.Playerskibidi.OnHit;
            Exiled.Events.Handlers.Player.UsedItem -= Event_Handlers.Playerskibidi.OnSCP_500;

            base.OnDisabled();
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Teleportacja : ICommand
    {
        public string Command => "teleport";

        public string[] Aliases => new[] { "tp" };

        public string Description => "Teleportuje gracza do innego gracza/pokoju";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 1)
            {
                if (int.TryParse(arguments.At(0), out int id))
                {


                    if (Exiled.API.Features.Player.List.FirstOrDefault(p => p.Id == id) == null)
                    {
                        response = "Nie ma gracza o takim ID";
                        return false;
                    }
                    if (sender is PlayerCommandSender playerSender)
                    {
                        Player player = Player.Get(playerSender);
                        Player target = Player.Get(id);


                        player.Position = target.Position;
                        response = "Pomyślnie przeteleportowano";
                        return true;
                    }

                }
                else
                {
                    string targetArg = arguments.At(0);
                    string roomName = targetArg;

                    Room targetRoom = Room.List.FirstOrDefault(r =>
                        r.Type.ToString().Contains(roomName));

                    if (targetRoom == null)
                    {
                        response = $"Nie znaleziono pokoju pasującego do: {roomName}";
                        return false;
                    }
                    if (sender is PlayerCommandSender playerSender)
                    {
                        Player player = Player.Get(playerSender);
                        player.Position = targetRoom.Position;
                        response = $"Zostałeś teleportowany do pokoju: {targetRoom.Type}.";
                        return true;
                    }
                }
            }

            else if (arguments.Count == 2)
            {
                if (!int.TryParse(arguments.At(0), out int idg))
                {
                    response = "Podaj ID gracza, jako liczbę całkowitą";
                    return false;
                }
                Player source = Player.Get(idg);
                if (source == null)
                {
                    response = $"Nie znaleziono gracza o ID {idg}.";
                    return false;
                }
                string targetArg = arguments.At(1);
                if (int.TryParse(targetArg, out int targetId))
                {
                    Player target = Player.Get(targetId);
                    if (target == null)
                    {
                        response = $"Nie znaleziono gracza o ID {targetId}.";
                        return false;
                    }

                    source.Position = target.Position;
                    response = $"Gracz {source.Nickname} został teleportowany do gracza {target.Nickname}.";
                    return true;
                }
                else
                {

                    string roomName = targetArg;

                    Room targetRoom = Room.List.FirstOrDefault(r =>
                        r.Type.ToString().Contains(roomName));

                    if (targetRoom == null)
                    {
                        response = $"Nie znaleziono pokoju pasującego do: {roomName}";
                        return false;
                    }

                    source.Position = targetRoom.Position;
                    response = $"Gracz {source.Nickname} został teleportowany do pokoju: {targetRoom.Type}.";
                    return true;
                }
            }
            response = "Użyj: tp {ID}/{Pokój} / tp {ID} {ID}/{Pokój}";
            return false;




        }

    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Pokoje : ICommand
    {
        public string Command => "pokoje";

        public string[] Aliases => new[] { "pk" };

        public string Description => "Wyspisuje listę pokoi";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "LczArmory\r\nLczCurve\r\nLczStraight\r\nLcz914\r\nLczCrossing\r\nLczTCross\r\nLczCafe\r\nLczPlants\r\nLczToilets\r\nLczAirlock\r\nLcz173\r\nLczClassDSpawn\r\nLczCheckpointB\r\nLczGlassBox\r\nLczCheckpointA\r\nHcz079\r\nHczEzCheckpointA\r\nHczEzCheckpointB\r\nHczArmory\r\nHcz939\r\nHczHid\r\nHcz049\r\nHczCrossing\r\nHcz106\r\nHczNuke\r\nHczTesla\r\nHczCurve\r\nHcz096\r\nEzVent\r\nEzIntercom\r\nEzGateA\r\nEzDownstairsPcs\r\nEzCurve\r\nEzPcs\r\nEzCrossing\r\nEzCollapsedTunnel\r\nEzConference\r\nEzChef\r\nEzStraight\r\nEzStraightColumn\r\nEzCafeteria\r\nEzUpstairsPcs\r\nEzGateB\r\nEzShelter\r\nPocket\r\nSurface\r\nHczStraight\r\nEzTCross\r\nLcz330\r\nEzCheckpointHallwayA\r\nEzCheckpointHallwayB\r\nHczTestRoom\r\nHczElevatorA\r\nHczElevatorB\r\nHczCrossRoomWater\r\nHczCornerDeep\r\nHczIntersectionJunk\r\nHczIntersection\r\nHczStraightC\r\nHczStraightPipeRoom\r\nHczStraightVariant\r\nEzSmallrooms\r\nHcz127\r\nHczServerRoom\r\n ";
            return true;
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MassTeleportacja : ICommand
    {
        public string Command => "massteleport";

        public string[] Aliases => new[] { "mtp" };

        public string Description => "Teleportuje wszystkich graczy w pobliżu do innego gracza/pokoju";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 1)
            {
                if (int.TryParse(arguments.At(0), out int id))
                {


                    if (Exiled.API.Features.Player.List.FirstOrDefault(p => p.Id == id) == null)
                    {
                        response = "Nie ma gracza o takim ID";
                        return false;
                    }
                    if (sender is PlayerCommandSender playerSender)
                    {
                        Player player = Player.Get(playerSender);
                        Player target = Player.Get(id);


                        float range = 10f;

                        var nearbyPlayers = Player.List
     .Where(p => Vector3.Distance(p.Position, player.Position) <= range)
     .ToList();


                        foreach (Player p in nearbyPlayers)
                        {
                            p.Position = player.Position;
                        }


                        response = $"Przeteleportowano {nearbyPlayers.Count} graczy do gracza o ID: {id}.";
                        return true;
                    }

                }
                else
                {
                    string targetArg = arguments.At(0);
                    string roomName = targetArg;

                    Room targetRoom = Room.List.FirstOrDefault(r =>
                        r.Type.ToString().Contains(roomName));

                    if (targetRoom == null)
                    {
                        response = $"Nie znaleziono pokoju pasującego do: {roomName}";
                        return false;
                    }
                    if (sender is PlayerCommandSender playerSender)
                    {
                        Player player = Player.Get(playerSender);
                        float range = 10f;

                        var nearbyPlayers = Player.List
      .Where(p => Vector3.Distance(p.Position, player.Position) <= range)
      .ToList();


                        foreach (Player p in nearbyPlayers)
                        {
                            p.Position = targetRoom.Position;
                        }


                        response = $"Przeteleportowano {nearbyPlayers.Count} graczy do pokoju: {targetRoom.Type}.";
                        return true;
                    }
                }
            }

            response = "Użyj: mtp {ID}/{Pokój}";
            return false;


        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Dekontaminacja : ICommand
    {
        public string Command => "Dekontaminacja";

        public string[] Aliases => new[] { "dek" };

        public string Description => "Dekontaminuje wskazany pokój";

        public bool SanitizeResponse { get; } = true;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 1) 
            {
                string targetArg = arguments.At(0);
                string roomName = targetArg;

                Room targetRoom = Room.List.FirstOrDefault(r =>
                    r.Type.ToString().Contains(roomName));

                if (targetRoom == null)
                {
                    response = $"Nie znaleziono pokoju pasującego do: {roomName}";
                    return false;
                }
 List<Player> playersInRoom = Player.List
    .Where(p => Room.Get(p.Position) == targetRoom)
    .ToList();
                foreach (Player p in playersInRoom)
                {
                    p.EnableEffect(EffectType.Decontaminating, 10, 12);
                }
                foreach (Door door in Door.List)
                {
                    if (door.Rooms.Contains(targetRoom))
                    {
                        door.ChangeLock(DoorLockType.AdminCommand);
                    }
                }
                response = "Dokonano dekontaminacji";
                return true;
            }
            else
            {

            response = "Użyj: dek {Pokój}";
            return false;
            }

        }
    }
}