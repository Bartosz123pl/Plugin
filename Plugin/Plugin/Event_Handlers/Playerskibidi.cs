using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using Exiled.API.Enums;
using System.Linq;
using InventorySystem.Items.Armor;
using Exiled.API.Features.Items;
namespace Plugin.Event_Handlers
{
    public static class Playerskibidi
    {
        public static void OnHit(ShotEventArgs ev)
        {
            if (ev?.Target == null || ev.Item == null || ev.Hitbox == null || Plugin.Instance?.Config == null)
                return;
            if (Plugin.Instance.Config.Bleeding && ev.Target.IsHuman && (ev.Item.Type == ItemType.GunA7 || ev.Item.Type == ItemType.GunAK ||ev.Item.Type == ItemType.GunCOM15 || ev.Item.Type == ItemType.GunCOM18 || ev.Item.Type == ItemType.GunCom45 ||ev.Item.Type == ItemType.GunCrossvec || ev.Item.Type == ItemType.GunE11SR || ev.Item.Type == ItemType.GunFRMG0 ||ev.Item.Type == ItemType.GunFSP9 || ev.Item.Type == ItemType.GunLogicer || ev.Item.Type == ItemType.GunRevolver ||ev.Item.Type == ItemType.GunShotgun) && (ev.Hitbox.HitboxType == HitboxType.Limb || (ev.Hitbox.HitboxType == HitboxType.Body) ||(ev.Hitbox.HitboxType == HitboxType.Headshot) && !ev.Target.IsGodModeEnabled))
            {
              
                double bron = 1;
                double pancerz = 1;
            switch (ev.Firearm.AmmoType)
                {
                    case AmmoType.Nato9:
                        bron = 1.2;
                        break;
                    case AmmoType.Ammo12Gauge:
                        bron = 0.8;
                        break;
                    case AmmoType.Ammo44Cal:
                        bron = 0.7;
                        break;
                }
                switch (ev.Target.CurrentArmor.Type)
                {
                    case ItemType.ArmorLight:
                        pancerz = 1.2;
                        break;
                    case ItemType.ArmorCombat:
                        pancerz = 1.4;
                        break;
                    case ItemType.ArmorHeavy:
                        pancerz = 1.6;
                        break;
                }
                Random rnd = new Random();
                {
                    switch (ev.Hitbox.HitboxType)
                    {
                        case HitboxType.Body:
                            //krwawienie
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 30)
                            {
                                if (ev.Target.GetEffect(EffectType.Bleeding)?.Intensity == 0)
                                {
                                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Bleeding, 10);
                                }
                                else if (ev.Target.GetEffect(EffectType.Bleeding)?.Intensity != 0)
                                {
                                    float czas = ev.Target.GetEffect(EffectType.Bleeding)?.Duration ?? 0;
                                    ev.Target.ChangeEffectIntensity(EffectType.Bleeding, (byte)(ev.Target.GetEffect(EffectType.Bleeding)?.Intensity + 10), czas);
                                }
                            }
                            //złamanie żebra
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 30)
                            {
                                ev.Target.EnableEffect(EffectType.Exhausted, 1);
                            }
                            //przebicie płuca i zapadnięcie go
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 10)
                            {
                                ev.Target.EnableEffect(EffectType.Asphyxiated, 1);
                            }
                            break;
                        case HitboxType.Headshot:
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 25)
                            {
                                if (ev.Target.GetEffect(EffectType.Bleeding)?.Intensity == 0)
                                {
                                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Bleeding, 10);
                                }
                                else if (ev.Target.GetEffect(EffectType.Bleeding)?.Intensity != 0)
                                {
                                    float czas = ev.Target.GetEffect(EffectType.Bleeding)?.Duration ?? 0;
                                    ev.Target.ChangeEffectIntensity(EffectType.Bleeding, (byte)(ev.Target.GetEffect(EffectType.Bleeding)?.Intensity + 10), czas);
                                }
                            }
                            //uszkodzenie mózgu 1
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 50)
                            {
                                ev.Target.EnableEffect(EffectType.Blurred, 5);
                            }
                            //uszkodzenie mózgu 2
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 50)
                            {
                                ev.Target.EnableEffect(EffectType.Concussed, 5);
                            }
                            //uszkodzenie mózgu 3
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 50)
                            {
                                ev.Target.EnableEffect(EffectType.Deafened, 5);
                            }
                            //uszkodzenie mózgu 4
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 50)
                            {
                                ev.Target.EnableEffect(EffectType.Flashed, 5);
                            }
                            //uszkodzenie mózgu 5
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 50)
                            {
                                ev.Target.EnableEffect(EffectType.Ensnared, 5);
                            }
                    break;
                        case HitboxType.Limb:
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 35)
                            {
                                if (ev.Target.GetEffect(EffectType.Bleeding)?.Intensity == 0)
                                {
                                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Bleeding, 10);
                                }
                                else if (ev.Target.GetEffect(EffectType.Bleeding)?.Intensity != 0)
                                {
                                    float czas = ev.Target.GetEffect(EffectType.Bleeding)?.Duration ?? 0;
                                    ev.Target.ChangeEffectIntensity(EffectType.Bleeding, (byte)(ev.Target.GetEffect(EffectType.Bleeding)?.Intensity + 10), czas);
                                }
                            }
                            //uszkodzenie nóg
                            if (((double)rnd.Next(0, 100)) * bron * pancerz < 50)
                            {
                                ev.Target.EnableEffect(EffectType.Disabled, 5);
                            }
                            break;
                    }
                }
            }
        }
        public static void OnSCP_500(UsedItemEventArgs ev)
        {
            if (Plugin.Instance.Config.Bleeding == true)
            {
                if (ev.Item.Type == ItemType.SCP500)
                {
                    if (ev.Player.GetEffect(EffectType.Disabled).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Disabled);
                    }
                    if (ev.Player.GetEffect(EffectType.Bleeding).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Bleeding);
                    }
                    if (ev.Player.GetEffect(EffectType.Ensnared).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Ensnared);
                    }
                    if (ev.Player.GetEffect(EffectType.Flashed).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Flashed);
                    }
                    if (ev.Player.GetEffect(EffectType.Blurred).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Blurred);
                    }
                    if (ev.Player.GetEffect(EffectType.Asphyxiated).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Asphyxiated);
                    }
                    if (ev.Player.GetEffect(EffectType.Exhausted).Intensity >= 1)
                    {
                        ev.Player.DisableEffect(EffectType.Exhausted);
                    }
                }
            }
        }
    }
}

