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
            TileObjectData.newTile.CoordinateHeights = new int[] { 16};
            TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(Terraria.Enums.AnchorType.SolidTile, 3, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
            //dustType = mod.DustType("Sparkle");
            drop = mod.ItemType("WirelessTeleporterBase");
            AddMapEntry(new Color(200, 200, 200));
            // Set other values here
        }
    }
}
