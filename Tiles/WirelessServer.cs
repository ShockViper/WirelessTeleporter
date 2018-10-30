using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WirelessTeleporter.Tiles
{
    class WirelessServer : WirelessServerFrame
    {
        public static string styleName = "WirelessServer";
        public static string serverInfo = "";
        private int capacity = 2;
        private int glowFrame = 0;
        private const int glowFrameHeight = 70;


        public override void ModifyObjectData()
        {
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 5;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TEServer>().Hook_AfterPlacement, -1, 0, true);
        }

        public override int ItemType(int frameX, int frameY)
        {
            int style = frameX / 54;
            int type;
            switch (style)
            {
                case 1:
                    styleName = "WirelessServerM2";
                    capacity = 4;
                    break;
                case 2:
                    styleName = "WirelessServerM3";
                    capacity = 6;
                    break;
                case 3:
                    styleName = "WirelessServerM4";
                    capacity = 8;
                    break;
                case 4:
                    styleName = "WirelessServerM5";
                    capacity = 10;
                    break;
                default:
                    styleName = "WirelessServer";
                    capacity = 2;
                    break;
            }
            return type=mod.ItemType(styleName);
        }

        public override bool CanPlace(int i, int j)
        {
            if (WirelessWorld.activeServers >= WirelessWorld.maxServers)
            {
                if (WirelessWorld.checkTimerText()) { Main.NewText("Max server capacity exceeded (4)"); }
                return false;
            }
            return base.CanPlace(i, j);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.1f;
            g = 0.2f;
            b = 0.1f;
        }

        public override void RightClick(int i, int j)
        {
            Point16 topleft = TEServer.GetTopLeft(i, j);
            if (TryUpgrade(topleft.X, topleft.Y))
            {
                Main.player[Main.myPlayer].tileInteractionHappened = true;
                return;
            }
            if (!ServerInfoUI.visible)
            {
                ServerInfoUI.visible = true;
                ServerInfoUI.activeServer = (TEServer)TileEntity.ByPosition[topleft];
                ServerInfoUI.activePos = new Point16(topleft.X + 1, topleft.Y + 4);
                WirelesTeleporter.ActivateUI(UImode.Server);
                WirelesTeleporter.serverUI.SetName(ServerInfoUI.activeServer.name);
                WirelesTeleporter.serverUI.AddServerPanel();
            }
        }
        
        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.noThrow = 2;
            MouseOverBoth(i, j);
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOverBoth(i, j);
        }

        private void MouseOverBoth(int i, int j)
        {
            Point16 topleft = TEServer.GetTopLeft(i, j);
            WirelesTeleporter.hovering = true;

             if (true)
            {
                string info = ((TEServer)TileEntity.ByPosition[topleft]).GetServerInfo();
                WirelesTeleporter.hovername = info;
            }
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
			if (frameCounter > 20)
			{
				frameCounter = 0;
                glowFrame= Main.rand.Next(4);
			}
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPos = zero + 16f * new Vector2(i, j) - Main.screenPosition;
            Rectangle frame = new Rectangle(tile.frameX , tile.frameY + (glowFrame * glowFrameHeight), 16, 16);
            Color lightColor = Lighting.GetColor(i, j, Color.White);
            Color color = Color.Lerp(Color.White, lightColor, 0.5f);
            spriteBatch.Draw(mod.GetTexture("Tiles/WirelessServer_Glow"), drawPos, frame, color);
        }

        private bool TryUpgrade(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item item = player.inventory[player.selectedItem];
            int style = Main.tile[i, j].frameX /54;
            bool success = false;
            if (style == 0 && item.type == mod.ItemType("ServerUpgradeMK2"))
            {
                SetStyle(i, j, 1);
                success = true;
            }
            else if (style == 1 && item.type == mod.ItemType("ServerUpgradeMK3"))
            {
                SetStyle(i, j, 2);
                success = true;
            }
            else if (style == 2 && item.type == mod.ItemType("ServerUpgradeMK4"))
            {
                SetStyle(i, j, 3);
                success = true;
            }
            else if (style == 3 && item.type == mod.ItemType("ServerUpgradeMK5"))
            {
                SetStyle(i, j, 4);
                success = true;
            }
            if (success)
            {
                TEServer server = (TEServer)TileEntity.ByPosition[new Point16(i, j)];
                server.style = style + 1;
                server.capacity = (server.style+1) * 2;
                item.stack--;
                if (item.stack <= 0)
                {
                    item.SetDefaults(0);
                }

                if (player.selectedItem == 58)
                {
                    Main.mouseItem = item.Clone();
                }
            }
            return success;
        }

        private void SetStyle(int i, int j, int style)
        {
            for(int y = 0; y<4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Main.tile[i+x, j + y].frameX = (short)((54 * style)+(x*18));
                }
            }
            int newType = ItemType(i, j);
            Main.NewText("upgraded to type:" + newType);
        }

    }

}
