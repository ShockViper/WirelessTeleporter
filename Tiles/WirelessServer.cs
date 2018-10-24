using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace WirelessTeleporter.Tiles
{
    class WirelessServer : WirelessServerFrame
    {
        public static string styleName = "WirelessServer";
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
                    break;
                case 2:
                    styleName = "WirelessServerM3";
                    break;
                case 3:
                    styleName = ("WirelessServerM4");
                    break;
                case 4:
                    styleName = ("WirelessServerM5");
                    break;
                default:
                    styleName = ("WirelessServer");
                    break;
            }
            return type=mod.ItemType(styleName);
        }

        public override void MouseOver(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i - (tile.frameX % 36 / 18);
            int top = j - (tile.frameY % 36 / 18);
            Main.tileSign[tile.type] = true;
            Main.signX = left * 16 + 16;
            Main.signY = top * 16;
            int sign= Sign.ReadSign(i, j);
            Sign.TextSign(sign, Name +" "+this.styleName);
            Main.signBubble = false;

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
