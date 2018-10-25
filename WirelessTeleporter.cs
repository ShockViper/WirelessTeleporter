using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using WirelessTeleporter.Tiles;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria;

namespace WirelessTeleporter
{
    public class WirelesTeleporter : Mod
    {
        internal static WirelesTeleporter instance;
        internal ServerInfoUI serverUI;
        private UserInterface serverUserInterface;
        public static string hovername;
        public static bool hovering = false;

        public override void Load()
        {
            instance = this;
            serverUI = new ServerInfoUI();
            serverUI.Activate();
            serverUserInterface = new UserInterface();
            serverUserInterface.SetState(serverUI);

        }
        public override void Unload()
        {
            instance = null;
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
                    "Wireless Teleport: Server Info",
                    delegate
                    {
                        if (ServerInfoUI.visible)
                        {
                            Main.hoverItemName = hovername;
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


        }

    }
