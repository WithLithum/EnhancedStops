using EnhancedStops.Util;
using LemonUI;
using LemonUI.Menus;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Menus;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnhancedStops
{
    internal static class StopProcess
    {
        private static Ped _currentPed;
        private static GameFiber _process;
        private static GameFiber _render;
        private static bool _isBeingDisposed;

        private static readonly ObjectPool _pool = new ObjectPool();
        private static readonly NativeMenu _menu = new NativeMenu(Globals.ModName, "Interaction Menu");
        private static readonly NativeItem _itemCheckId = new NativeItem("Request status check", "Requests dispatch to check for the ped status.");

        private static readonly NativeMenu _arrestMenu = new NativeMenu(Globals.ModName, "Arrest Interactions");
        private static readonly NativeItem _itemCheckIdArrested = new NativeItem("Request status check", "Requests dispatch to check for the suspect status.");
        private static readonly NativeItem _itemGracefulRemoveFromCar = new NativeItem("Remove from Vehicle", "Gracefully removes the suspect from it's current vehicle.");

        private static readonly NativeMenu _generalActionsMenu = new NativeMenu(Globals.ModName, "General Actions");
        private static readonly NativeItem _itemSlowDownTraffic = new NativeItem("Slow Down Traffic", "Slows down traffic in the current area.");

        internal static void Initialize()
        {
            _process = new GameFiber(Fiber, "EnhancedStops process");
            _render = new GameFiber(Render, "EnhancedStops rendering");

            _process.Start();
            _render.Start();

            Game.DisplayNotification(
                Globals.ModIconDictionary,
                Globals.ModIconTexture,
                "EnhancedStops",
                "~b~by ~y~WithLithum",
                "Initialized."
            );
        }

        internal static void DisposePeds()
        {
            _isBeingDisposed = true;

            if (_currentPed)
            {
                _currentPed.Dismiss();
                _currentPed = null;
            }

            _process.Abort();
        }

        internal static void Fiber()
        {
            GameFiber.Yield();

            // Create menus
            _menu.Add(_itemCheckId);
            _pool.Add(_menu);

            _arrestMenu.Add(_itemCheckIdArrested);
            _arrestMenu.Add(_itemGracefulRemoveFromCar);
            _pool.Add(_arrestMenu);

            // They does the same thing
            // The second is for avoiding the bug
            _itemCheckId.Activated += _itemCheckId_Activated;
            _itemCheckIdArrested.Activated += _itemCheckId_Activated;
            _itemGracefulRemoveFromCar.Activated += _itemGracefulRemoveFromCar_Activated;

            while (!_isBeingDisposed)
            {
                GameFiber.Yield();

                // If it is key down and no menu displayed
                if (Game.IsKeyDown(Keys.G) && !_pool.AreAnyVisible)
                {
                    // Get closet ped
                    // Not player
                    var closePed = World.GetClosestEntity(World.GetEntities(Game.LocalPlayer.Character.Position, 4f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed), Game.LocalPlayer.Character.Position);

                    // We check if the ped exists & is a ped
                    if (!closePed || !closePed.Model.IsPed) continue;
                    var truePed = (Ped)closePed;

                    // Assign currentped so that menus recognize the ped and
                    // performs valid actions
                    _currentPed = truePed;
                    if (Functions.IsPedStoppedByPlayer(truePed))
                    {
                        // Simple display menus
                        _menu.Visible = !_menu.Visible;
                        continue;
                    }

                    if (Functions.IsPedArrested(truePed))
                    {
                        _itemGracefulRemoveFromCar.Enabled = truePed.IsInAnyVehicle(false);
                        _arrestMenu.Visible = !_arrestMenu.Visible;
                    }
                }

            }
        }

        private static void _itemGracefulRemoveFromCar_Activated(object sender, EventArgs e)
        {
            if (_currentPed && _currentPed.IsInAnyVehicle(false))
            {
                _currentPed.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
            }
        }

        private static void _itemCheckId_Activated(object sender, EventArgs e)
        {
            if (_currentPed)
            {
                Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 2500);

                // Add a delay here
                GameFiber.StartNew(() =>
                {
                    var persona = Functions.GetPersonaForPed(_currentPed);
                    Radio.DisplayConversation(Game.LocalPlayer.Name, $"Requesting check for ~y~{persona.FullName}, born {persona.Birthday.ToShortDateString()}");
                    GameFiber.Sleep(2500);
                    Radio.DisplayConversation("Dispatch", "Copy, stand by...");
                    GameFiber.Sleep(MathHelper.GetRandomInteger(1500, 3000));
                    Game.DisplayNotification("commonmenu", "shop_mask_icon_a", "Dispatch", "Ped Status",
                        $"~y~{persona.FullName}~s~, born {persona.Birthday.ToShortDateString()}~n~License: {Radio.GetLicenseStateString(persona.ELicenseState)}~s~~n~Wanted: {Radio.GetWantedString(persona.Wanted)}");
                }, "EnhancedStops status check (ped)");
            }
        }

        internal static void Render()
        {
            while (!_isBeingDisposed)
            {
                GameFiber.Yield();
                _pool.Process();
            }
        }
    }
}
