using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria;

namespace WirelessTeleporter
{
    public enum UImode
    {
        Server,
        Teleport
    }

    public class WirelesTeleporter : Mod
    {
        internal static WirelesTeleporter instance;
        internal static ServerInfoUI serverUI;
        private static UserInterface serverUserInterface;
        public static string hovername;
        public static bool hovering = false;

        public override void Load()
        {
            instance = this;
            serverUserInterface = new UserInterface();
            serverUI = new ServerInfoUI();
        }

        public override void Unload()
        {
            instance = null;
        }
        
        public static void ActivateUI(UImode type)
        {
            if (serverUI != null) { serverUI.Deactivate(); }
            switch (type)
            {
                case UImode.Server:
                    serverUI = new ServerInfoUI();
                    serverUI.Activate();
                    serverUserInterface.SetState(serverUI);
                    break;
                case UImode.Teleport:
                    break;
                default:
                    return;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (serverUserInterface != null && ServerInfoUI.visible)
                serverUserInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Wireless Teleport: Info",
                    delegate
                    {
                        if (ServerInfoUI.visible )
                        {
                            serverUserInterface.Draw(Main.spriteBatch, new GameTime());
                            hovername = "";
                        }
                        if (Main.hoverItemName == "" && hovering) { Main.hoverItemName = hovername; };
                        hovering = false;
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostUpdateInput()
        {
            if (!Main.instance.IsActive)
            {
                return;
            }
            ServerInfoUI.CheckMouse(null);
        }

    }

}
