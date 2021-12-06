using EnhancedStops.Util;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStops
{
    internal static class SprintProcess
    {
        private static GameFiber countdownTimer;
        private static GameFiber activateTimer;
        private static GameFiber deactivateTimer;
        private static bool cooldown;
        private static bool sprinting;
        private static int cooldownTimer;
        private static int deactivateTimerCount;

        internal static void Init()
        {
            countdownTimer = GameFiber.StartNew(CountdownTimer);
            activateTimer = GameFiber.StartNew(ActivateProcess);
            deactivateTimer = GameFiber.StartNew(DeactivateTimer);
        }

        internal static void ActivateProcess()
        {
            while (true)
            {
                GameFiber.Yield();
                if (!sprinting && Game.IsKeyDown(Config.SuperSprintKey) && !cooldown)
                {
                    cooldownTimer = Config.SuperSprintCooldown;
                    NativeFunction.Natives.x6DB47AA77FD94E09(Game.LocalPlayer, 2.2f);
                    sprinting = true;
                }
            }
        }

        internal static void CountdownTimer()
        {
            while (true)
            {
                GameFiber.Yield();

                if (cooldown && cooldownTimer >= 1)
                {
                    GameFiber.Sleep(cooldownTimer);
                    cooldown = false;
                }
            }
        }

        internal static void DeactivateTimer()
        {
            while (true)
            {
                GameFiber.Yield();

                if (deactivateTimerCount >= 1)
                {
                    GameFiber.Sleep(deactivateTimerCount);
                    NativeFunction.Natives.x6DB47AA77FD94E09(Game.LocalPlayer, 1f);
                    cooldown = true;
                    sprinting = false;
                }

            }
        }
    }
}
