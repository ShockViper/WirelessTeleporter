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
    class WirelessServer : ModTile
    {
        public override void SetDefaults()
        {
            
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(0, 3);
            //TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(Terraria.Enums.AnchorType.SolidTile, 3, 0);
            //TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
            //dustType = mod.DustType("Sparkle");
            //drop = mod.ItemType("WirelessTeleporterBase");
            AddMapEntry(new Color(200, 200, 200));
            // Set other values here
        }

 

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // the 3rd and 4th numbers should be 16*width blocks and 16 * height blocks respectively.
            Item.NewItem(i * 16, j * 16, 48, 64, mod.ItemType("WirelessTeleporterBase"));
        }
    }
}
