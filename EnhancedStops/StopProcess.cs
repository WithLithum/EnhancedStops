// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using EnhancedStops.Util;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace EnhancedStops
{
    internal static class StopProcess
    {
        private static Ped _currentPed;
        private static GameFiber _process;
        private static bool _isBeingDisposed;

        private static readonly ObjectPool _pool = new ObjectPool();
        private static readonly NativeMenu _menu = new NativeMenu("", "Interaction Menu");
        private static readonly NativeItem _itemCheckId = new NativeItem("Request status check", "Requests dispatch to check for the ped status.");

        private static readonly NativeMenu _arrestMenu = new NativeMenu("", "Arrest Interactions");
        private static readonly NativeItem _itemCheckIdArrested = new NativeItem("Request status check", "Requests dispatch to check for the suspect status.");
        private static readonly NativeItem _itemGracefulRemoveFromCar = new NativeItem("Remove from Vehicle", "Gracefully removes the suspect from it's current vehicle.");
        private static readonly NativeItem _itemCallTransport = new NativeItem("Request transport unit", "If subject set down on the ground, requests a transport unit.");

        private static readonly NativeMenu _trafficStopMenu = new NativeMenu("", "Traffic Stop");
        private static readonly NativeItem _itemCheckVehicle = new NativeItem("Request Vehicle Check", "Request dispatch to check the vehicle status.");
        private static readonly NativeItem _itemCheckDriver = new NativeItem("Request Driver Status Check", "Requests dispatch to check the driver's status.");
        private static readonly NativeListItem<string> _itemCheckPassengers = new NativeListItem<string>("Request Passenger Status Check", "Requests dispatch to check the status of the specified passenger.",
            "Front", "Left-Rear", "Right-Rear");

        public static void Main()
        {
            Game.DisplayHelp("SEE BIG MESSAGE BELOW");
            Game.DisplaySubtitle("SEE BIG MESSAGE ABOVE");
            Game.RawFrameRender += Game_RawFrameRender;

            GameFiber.Hibernate();
        }

        private static void Game_RawFrameRender(object sender, GraphicsEventArgs e)
        {
            e.Graphics.DrawRectangle(new RectangleF(3f, 3f, 500f, 500f), Color.Black);
            e.Graphics.DrawText("Greetings. So for whether reason, you loaded EnhancedStops with RPH.", "Arial", 9f, new PointF(9f, 9f), Color.Red);
            e.Graphics.DrawText("This isn't a normal behavior and in fact you should put it inside plugins/LSPDFR folder.", "Arial", 9f, new PointF(9f, 25f), Color.Red);
            e.Graphics.DrawText("This message won't go away until you unload me.", "Arial", 9f, new PointF(9f, 40f), Color.Red);
        }

        internal static void Initialize()
        {
            _process = new GameFiber(Fiber, "EnhancedStops process");
            _ = GameFiber.StartNew(Render);

            _process.Start();

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
            _menu.Banner = Globals.BackgroundRect;

            _pool.Add(_menu);

            _arrestMenu.Add(_itemCheckIdArrested);
            _arrestMenu.Banner = Globals.BackgroundRect;
            _arrestMenu.Add(_itemGracefulRemoveFromCar);
            _arrestMenu.Add(_itemCallTransport);
            _pool.Add(_arrestMenu);

            _trafficStopMenu.Add(_itemCheckVehicle);
            _trafficStopMenu.Banner = Globals.BackgroundRect;
            _trafficStopMenu.Add(_itemCheckDriver);
            _trafficStopMenu.Add(_itemCheckPassengers);
            _pool.Add(_trafficStopMenu);

            // They does the same thing
            // The second is for avoiding the bug
            _itemCheckId.Activated += ItemCheckId_Activated;
            _itemCheckIdArrested.Activated += ItemCheckId_Activated;
            _itemGracefulRemoveFromCar.Activated += ItemGracefulRemoveFromCar_Activated;
            _itemCallTransport.Activated += _itemCallTransport_Activated;
            _itemCheckPassengers.Activated += _itemCheckPassengers_Activated;

            _itemCheckDriver.Activated += ItemCheckDriver_Activated;
            _itemCheckVehicle.Activated += ItemCheckVehicle_Activated;

            while (!_isBeingDisposed)
            {
                GameFiber.Yield();

                if (_arrestMenu.Visible && _currentPed)
                {
                    _itemCallTransport.Enabled = _currentPed.IsStill;
                    _itemGracefulRemoveFromCar.Enabled = _currentPed.IsInAnyPoliceVehicle;
                }

                if (_menu.Visible && _currentPed && Config.MustIdentifyBeforeStatusCheck)
                {
                    _itemCheckId.Enabled = Functions.HasPedBeenIdentified(_currentPed);
                }

                // If it is key down and no menu displayed
                if (Game.IsKeyDown(Config.MenuKey) && !_pool.AreAnyVisible)
                {
                    var pull = Functions.GetCurrentPullover();
                    if (pull != null)
                    {
                        // Assign currentped as pullover suspect
                        _currentPed = Functions.GetPulloverSuspect(pull);
                        _trafficStopMenu.Visible = !_trafficStopMenu.Visible;
                        continue;
                    }

                    // Get closet human ped
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
                        if (truePed.Metadata.IsTransportActive != null && truePed.Metadata.IsTransportActive)
                        {
                            continue;
                        }

                        _itemGracefulRemoveFromCar.Enabled = truePed.IsInAnyVehicle(false);
                        _arrestMenu.Visible = !_arrestMenu.Visible;
                    }
                }

            }
        }

        private static void _itemCheckPassengers_Activated(object sender, EventArgs e)
        {
            var vehicle = _currentPed.CurrentVehicle;

            // Maybe we can perform this check dynamically?
            if (vehicle.IsSeatFree(_itemCheckPassengers.SelectedIndex))
            {
                Game.DisplaySubtitle("~r~The selected passenger seat was empty.");
                return;
            }

            var ped = vehicle.GetPedOnSeat(_itemCheckPassengers.SelectedIndex);

            if (!ped)
            {
                Game.LogTrivial("ES: found invalid ped after check");
                Game.DisplaySubtitle("~r~Unexpected error.");
                return;
            }

            if (ped.IsDead)
            {
                Game.DisplaySubtitle("~r~The selected passenger was dead.");
                return;
            }

            // Actual call
            var persona = Functions.GetPersonaForPed(ped);
            Radio.DisplayPedId(persona);
        }

        private static void _itemCallTransport_Activated(object sender, EventArgs e)
        {
            if (!_currentPed.IsStill)
            {
                Game.DisplaySubtitle("~r~The ped must be standing still.");
                return;
            }

            Functions.SetPedAsArrested(_currentPed, true, false);
            Functions.RequestSuspectTransport(_currentPed);

            Functions.PlayScannerAudioUsingPosition(Globals.RadioTransportRequired, _currentPed.Position);

            _currentPed.Metadata.TransportActive = true;
            _arrestMenu.Visible = false;
        }

        private static void ItemCheckVehicle_Activated(object sender, EventArgs e)
        {
            Radio.DisplayVehicleInfo(_currentPed.CurrentVehicle);
        }

        private static void ItemCheckDriver_Activated(object sender, EventArgs e)
        {
            ItemCheckId_Activated(sender, e);
        }

        private static void ItemGracefulRemoveFromCar_Activated(object sender, EventArgs e)
        {
            if (_currentPed && _currentPed.IsInAnyVehicle(false))
            {
                _currentPed.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
                _arrestMenu.Visible = false;
            }
        }

        private static void ItemCheckId_Activated(object sender, EventArgs e)
        {
            if (_currentPed)
            {
                Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 2500);

                if (Config.IdentifyOnStatusCheck)
                {
                    Functions.SetPedAsIdentified(_currentPed, true);
                }

                // Moving it here so the code below is not vulernable
                // to InvalidHandleableException
                var persona = Functions.GetPersonaForPed(_currentPed);

                Radio.DisplayPedId(persona);
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
