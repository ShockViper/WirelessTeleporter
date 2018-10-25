using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;

namespace WirelessTeleporter.Tiles
{
    class TETeleport : ModTileEntity
    {
        internal string name;
        internal int teleportID;
        internal int connectedTo;

        public override void Update()
        {
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"name", name},
                {"teleportID", teleportID},
                {"connectedTo", connectedTo}
            };
        }

        public override void Load(TagCompound tag)
        {
            name = tag.Get<string>("name");
            teleportID = tag.Get<int>("teleportID");
            connectedTo = tag.Get<int>("connectedTo");
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<WirelessServer>() && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            //Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i, j);
        }
    }
}
