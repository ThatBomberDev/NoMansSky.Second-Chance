﻿using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.ModHelper;
using NoMansSky.Api;
using libMBIN.NMS.Globals;

namespace NoMansSky.FightOrFlight
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : NMSMod
    {
        /// <summary>
        /// Initializes your mod along with some necessary info.
        /// </summary>
        public Mod(IModConfig _config, IReloadedHooks _hooks, IModLogger _logger) : base(_config, _hooks, _logger)
        {
            // This is how to log a message to the Reloaded II Console.
            Logger.WriteLine("Hello World!");


            // The API relies heavily on Mod Events.
            // Below are 3 examples of using them.
            Game.OnProfileSelected += () => Logger.WriteLine("The player just selected a save file");
            Game.OnMainMenu += OnMainMenu;
            Game.OnGameJoined.AddListener(GameJoined);
        }

        /// <summary>
        /// Called once every frame.
        /// </summary>
        public override void Update()
        {
            // Here is an example of checking for keyboard keys
            if (Keyboard.IsPressed(Key.UpArrow))
            {
                Logger.WriteLine("The Up Arrow was just pressed!");
            }

            if(Game.IsInGame)
            {

                giveSecondChance();
            }

        }

        private void OnMainMenu()
        {
            Logger.WriteLine("Main Menu shown!");


            
        }

        private void giveSecondChance()
        {
            var initialShield = Game.Player.Shield;
            var memMgr = new MemoryManager();

            var criticalAmount = 30;
            var hazardousAmount = 55;

            if(initialShield < hazardousAmount)
            {
                memMgr.SetValue("GcPlayerGlobals.GunBaseClipSize", 5000);
                memMgr.SetValue("GcPlayerGlobals.MeleeCooldown", 0);
                memMgr.SetValue("GcPlayerGlobals.MeleeSpeedBoost", 5);
                memMgr.SetValue("GcPlayerGlobals.BulletClipMultiplier", 10);
                memMgr.SetValue("GcPlayerGlobals.UseHazardProtection", false);
                memMgr.SetValue("GcPlayerGlobals.FullClipReloadSpeedMultiplier", 15);

                memMgr.SetValue("GcPlayerGlobals.WeaponZoomFOV", 0.5);
                memMgr.SetValue("GcPlayerGlobals.WeaponChangeModeTime", 0.25);
            }
            else
            {
                memMgr.SetValue("GcPlayerGlobals.GunBaseClipSize", 0);
                memMgr.SetValue("GcPlayerGlobals.MeleeCooldown", 0.62);
                memMgr.SetValue("GcPlayerGlobals.MeleeSpeedBoost", 1);
                memMgr.SetValue("GcPlayerGlobals.BulletClipMultiplier", 2);
                memMgr.SetValue("GcPlayerGlobals.UseHazardProtection", true);
                memMgr.SetValue("GcPlayerGlobals.FullClipReloadSpeedMultiplier", 2);

                memMgr.SetValue("GcPlayerGlobals.WeaponZoomFOV", 0.7);
                memMgr.SetValue("GcPlayerGlobals.WeaponChangeModeTime", 0.75);
            }

            if (initialShield < criticalAmount)
            {


                memMgr.SetValue("GcGameplayGlobals.WaterLandingDamageMultiplier", 0.000f);
                
                memMgr.SetValue("GcPlayerGlobals.HealthRechargeMinTimeSinceDamage", 2);
                memMgr.SetValue("GcPlayerGlobals.ShieldRechargeMinTimeSinceDamage", 2);
                

            }

            else
            {
                memMgr.SetValue("GcGameplayGlobals.WaterLandingDamageMultiplier", 0.333f);

                memMgr.SetValue("GcPlayerGlobals.HealthRechargeMinTimeSinceDamage", 10);
                memMgr.SetValue("GcPlayerGlobals.ShieldRechargeMinTimeSinceDamage", 30);
               

            }
            
            /* Ship Shield Tracking Not Yet Implemented.
            var shipShield = Game.Player.Ship.Shield;
            if (shipShield <38)
            {
                memMgr.SetValue("GcSpaceshipGlobals.ShieldRechargeMinHitTime", 2);
                memMgr.SetValue("GcSpaceshipGlobals.ShieldRechargeRate", 10);
                
                memMgr.SetValue("GcSpaceshipGlobals.LaserOverheatTime", 25);
                memMgr.SetValue("GcSpaceshipGlobals.LaserCoolFactor", 5);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileOverheatTime", 25);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileFireRate", 15);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileReloadTime", 0.5);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileClipSize", 2000);

            }
            else
            {
                memMgr.SetValue("GcSpaceshipGlobals.ShieldRechargeMinHitTime", 60);
                memMgr.SetValue("GcSpaceshipGlobals.ShieldRechargeRate", 6);
                
                memMgr.SetValue("GcSpaceshipGlobals.LaserOverheatTime", 5);
                memMgr.SetValue("GcSpaceshipGlobals.LaserCoolFactor", 0.2);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileOverheatTime", 5);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileFireRate", 0.15);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileReloadTime", 4);
                memMgr.SetValue("GcSpaceshipGlobals.ProjectileClipSize", 500);
            }
            */

        }

        private void GameJoined()
        {
            Logger.WriteLine("The game was joined!");
        }
    }
}