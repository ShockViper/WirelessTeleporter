﻿using System;
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
    class WirelessServerFrame : ModTile
    {

        public override void SetDefaults()
        {
            
            Main.tileSolid[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Origin = new Point16(0, 3);
            ModifyObjectData();
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(200, 200, 200));
            // Set other values here
        }

        public virtual int ItemType(int frameX, int frameY)
        {
            return mod.ItemType("WirelessServerFrame");
        }

        public virtual void ModifyObjectData()
        {
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // the 3rd and 4th numbers should be 16*width blocks and 16 * height blocks respectively.
            int type = ItemType(frameX, frameY);
            Item.NewItem(i * 16, j * 16, 48, 64, type );
            if (type== mod.ItemType("WirelessServerFrame")) { return; }
            WirelessWorld.servers.Remove(TEServer.GetTopLeft(i, j));
            WirelessWorld.totalCapacity -= ((TEServer)TileEntity.ByPosition[TEServer.GetTopLeft(i, j)]).capacity;
            mod.GetTileEntity<TEServer>().Kill(i, j);
            //Item.NewItem(i * 16, j * 16, 48, 64, mod.ItemType(item));
        }
    }
}
