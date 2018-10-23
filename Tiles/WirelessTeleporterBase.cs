using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace WirelessTeleporter.Tiles
{
    class WirelessTeleporterBase : ModTile
    {
        public override void SetDefaults()
        {
            
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            //TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(Terraria.Enums.AnchorType.SolidTile, 3, 0);
            //TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
            //dustType = mod.DustType("Sparkle");
            //drop = mod.ItemType("WirelessTeleporterBase");
            AddMapEntry(new Color(200, 200, 200));
            animationFrameHeight = 18;
            // Set other values here
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            /*frameCounter++;
			if (frameCounter > 8)
			{
				frameCounter = 0;
				frame++;
				if (frame > 5)
				{
					frame = 0;
				}
			}*/
            // Above code works, but since we are just mimicking another tile, we can just use the same value.
            frame = Main.tileFrame[TileID.Teleporter];
        }


        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // the 3rd and 4th numbers should be 16*width blocks and 16 * height blocks respectively.
            Item.NewItem(i * 16, j * 16, 48, 16, mod.ItemType("WirelessTeleporterBase"));
        }
    }
}
