using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Tools;

namespace AntiToolRotation
{
    internal sealed class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
               original: AccessTools.Method(typeof(Game1), nameof(Game1.didPlayerJustLeftClick)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(Game1_DidPlayerJustLeftClick_Prefix))
            );
        }

        private static bool Game1_DidPlayerJustLeftClick_Prefix(ref bool __result, bool ignoreNonMouseHeldInput = false)
        {
            if (Game1.player.CurrentTool is MeleeWeapon)
            {
                __result = false;
                
                Vector2 position = ((!Game1.wasMouseVisibleThisFrame) ? Game1.player.GetToolLocation() : new Vector2(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y));
                Game1.player.lastClick = new Vector2((int)position.X, (int)position.Y);
                Game1.player.BeginUsingTool();

                return false;
            }
            return true;
        }



    }
}