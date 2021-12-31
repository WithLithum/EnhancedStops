// Copyright (C) WithLithum & contributors 2021, 2022.
// See NOTICE for full notice (including exceptions)
// See LICENSE for the license.

using EnhancedStops.Util;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Drawing;

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
            _pool.Add(_trafficStopMenu);

            // They does the same thing
            // The second is for avoiding the bug
            _itemCheckId.Activated += ItemCheckId_Activated;
            _itemCheckIdArrested.Activated += ItemCheckId_Activated;
            _itemGracefulRemoveFromCar.Activated += ItemGracefulRemoveFromCar_Activated;
            _itemCallTransport.Activated += _itemCallTransport_Activated;

            _itemCheckDriver.Activated += ItemCheckDriver_Activated;
            _itemCheckVehicle.Activated += ItemCheckVehicle_Activated;

            while (!_isBeingDisposed)
            {
                GameFiber.Yield();

                if (_arrestMenu.Visible && _currentPed )
                {
                    _itemCallTransport.Enabled = _currentPed.IsStill;
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

        private static void _itemCallTransport_Activated(object sender, EventArgs e)
        {
            if (!_currentPed.IsStill)
            {
                Game.DisplaySubtitle("~r~The ped must be standing still.");
                return;
            }

            if (Functions.GetPedArrestingOfficer(_currentPed) != Game.LocalPlayer.Character)
            {
                Game.DisplaySubtitle("~r~The ped is not arrested by you.");
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
