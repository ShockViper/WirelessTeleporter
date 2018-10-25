using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using System.IO;

namespace WirelessTeleporter.Tiles
{
    class TEServer : ModTileEntity
    {
        internal string name;
        internal int capacity;
        internal int serverID;
        internal int style;

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
                {"capacity", capacity},
                {"serverID", serverID},
                {"style", style}
            };
        }

        public override void Load(TagCompound tag)
        {
            string names = tag.Get<string>("name");
            capacity = tag.Get<int>("capacity");
            serverID = tag.Get<int>("serverID");
            style = tag.Get<int>("style");
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<WirelessServer>() && tile.frameX == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
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
