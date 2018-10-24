using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;

namespace WirelessTeleporter.Tiles
{
    class WirelessServer : WirelessServerFrame
    {
        public static string styleName = "WirelessServer";
        public static string serverInfo = "";
        private int capacity = 2;


        public override void ModifyObjectData()
        {
            TileObjectData.newTile.StyleHorizontal = true;
            //TileObjectData.newTile.StyleMultiplier = 5;
            TileObjectData.newTile.StyleWrapLimit = 5;
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


        public override void PlaceInWorld(int i, int j, Item item)
        {
            Tile tile = Main.tile[i, j];
            WirelesTeleporter.servers.Add(this);
            
        }

        public override void MouseOver(int i, int j)
        {
            MouseOverBoth(i, j);
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOverBoth(i, j);
        }

        private void MouseOverBoth(int i,int j)
        {
            Tile tile = Main.tile[i, j];
            int itemT = ItemType(tile.frameX, tile.frameY);
            int sign = Sign.ReadSign(i, j, true);
            if (sign > -1)
            {
                Sign.TextSign(sign, Name + " " + styleName + "\nCap: " + capacity+"\nServCount: "+WirelesTeleporter.servers.Count);
                Main.signHover = sign;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
             Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPos = zero + 16f * new Vector2(i, j) - Main.screenPosition;
            Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
            Color lightColor = Lighting.GetColor(i, j, Color.White);
            Color color = Color.Lerp(Color.White, lightColor, 0.5f);
            spriteBatch.Draw(mod.GetTexture("Tiles/WirelessServer_Glow"), drawPos, frame, color);
        }


    }

}
