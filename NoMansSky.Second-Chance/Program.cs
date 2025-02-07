﻿using NoMansSky.ModTemplate.Configuration;
using NoMansSky.ModTemplate.Configuration.Implementation;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using Reloaded.ModHelper;
using NoMansSky.Api;
using System;
using libMBIN.NMS;

#if DEBUG
using System.Diagnostics;
#endif

namespace NoMansSky.FightOrFlight
{
    public class Program : IMod
    {
        /// <summary>
        /// Used for writing text to the Reloaded log.
        /// </summary>
        private ILogger _logger = null!;

        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private IModLoader _modLoader = null!;

        /// <summary>
        /// Stores the contents of your mod's configuration. Automatically updated by template.
        /// </summary>
        private Config _configuration = null!;

        /// <summary>
        /// An interface to Reloaded's the function hooks/detours library.
        /// See: https://github.com/Reloaded-Project/Reloaded.Hooks
        ///      for documentation and samples. 
        /// </summary>
        private IReloadedHooks _hooks = null!;

        /// <summary>
        /// Configuration of the current mod.
        /// </summary>
        private IModConfig _modConfig = null!;

        /// <summary>
        /// Encapsulates your mod logic.
        /// </summary>
        private Mod _mod = null!;

        /// <summary>
        /// Instance of game class.
        /// </summary>
        private IGame gameInstance = null!;

        /// <summary>
        /// Instance of the game loop.
        /// </summary>
        private IGameLoop gameLoop = null!;

        /// <summary>
        /// Instance of the memory manager.
        /// </summary>
        private IMemoryManager memoryMgr = null!;

        /// <summary>
        /// Instance of the mod logger.
        /// </summary>
        private IModLogger Logger = null!;

        /// <summary>
        /// Entry point for your mod.
        /// </summary>
        public void StartEx(IModLoaderV1 loaderApi, IModConfigV1 modConfig)
        {
#if DEBUG
        // Attaches debugger in debug mode; ignored in release. 
        // Use this if you want to breakpoint your mod
        Debugger.Launch();
#endif

            _modLoader = (IModLoader)loaderApi;
            _modConfig = (IModConfig)modConfig;
            _logger = (ILogger)_modLoader.GetLogger();
            _modLoader.GetController<IReloadedHooks>().TryGetTarget(out _hooks!);

            // Your config file is in Config.json.
            // Need a different name, format or more configurations? Modify the `Configurator`.
            // If you do not want a config, remove Configuration folder and Config class.
            var configurator = new Configurator(_modLoader.GetModConfigDirectory(_modConfig.ModId));
            _configuration = configurator.GetConfiguration<Config>(0);
            _configuration.ConfigurationUpdated += OnConfigurationUpdated;

            /*
                Your mod code starts below.
                Visit https://github.com/Reloaded-Project for additional optional libraries.
            */

            // Create Mog Logger.
            Logger = new ModLogger(modConfig, _logger);

            // create memory manager. Doing this early in case it's needed during initialization.
            memoryMgr = new MemoryManager();
            memoryMgr.AddConverter(new NMSStringConverter(memoryMgr), alwaysRegister: true);
            memoryMgr.AddConverter(new ArrayConverter(memoryMgr), alwaysRegister: true);
            memoryMgr.AddConverter(new ListConverter(memoryMgr), alwaysRegister: true);
            memoryMgr.AddConverter(new NMSTemplateConverter(memoryMgr), alwaysRegister: true);


            // The API publishes the instance of the Game class so mods can access it.
            // The line below is where this mod aquires the Game instance that was published.
            _modLoader.GetController<IGame>().TryGetTarget(out gameInstance);
            _modLoader.GetController<IGameLoop>().TryGetTarget(out gameLoop);

            if (gameInstance == null)
            {
                Logger.WriteLine("Critical Error! Failed to get the game instance from the API. Nothing will work until this is fixed.", LogLevel.Error);
                return;
            }

            if (gameLoop == null)
            {
                Logger.WriteLine("Critical Error! Failed to get the game loop from the API. Nothing will work until this is fixed.", LogLevel.Error);
                return;
            }

            _mod = new Mod(_modConfig, _hooks, Logger);
        }

        private void OnConfigurationUpdated(IConfigurable obj)
        {
            /*
                This is executed when the configuration file gets 
                updated by the user at runtime.
            */

            // Replace configuration with new.
            _configuration = (Config)obj;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");

            // Apply settings from configuration.
            // ... your code here.
        }

        /* Mod loader actions. */
        public void Suspend()
        {
            /*  Some tips if you wish to support this (CanSuspend == true)

                A. Undo memory modifications.
                B. Deactivate hooks. (Reloaded.Hooks Supports This!)
            */
        }

        public void Resume()
        {
            /*  Some tips if you wish to support this (CanSuspend == true)

                A. Redo memory modifications.
                B. Re-activate hooks. (Reloaded.Hooks Supports This!)
            */
        }

        public void Unload()
        {
            /*  Some tips if you wish to support this (CanUnload == true).

                A. Execute Suspend(). [Suspend should be reusable in this method]
                B. Release any unmanaged resources, e.g. Native memory.
            */
        }

        /*  If CanSuspend == false, suspend and resume button are disabled in Launcher and Suspend()/Resume() will never be called.
            If CanUnload == false, unload button is disabled in Launcher and Unload() will never be called.
        */
        public bool CanUnload() => false;
        public bool CanSuspend() => false;

        /* Automatically called by the mod loader when the mod is about to be unloaded. */
        public Action Disposing { get; } = null!;
    }
}