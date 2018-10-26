using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace WirelessTeleporter.Tiles
{
    class TETeleport : ModTileEntity
    {
        internal string name;
        internal int teleportID;
        internal int connectedTo;
        internal int range = 20;

        public override void Update()
        {
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
        }

        public Rectangle rangeRect()
        {
            Rectangle rect= new Rectangle((Position.X + 1) * 16 - range * 16, (Position.Y + 1) * 16 - range * 16, range * 16 * 2, range * 16 * 2);
            Dust.QuickBox(rect.TopLeft(), rect.BottomRight(), 10, Color.White, null);
            return rect;
        }

        private void InitAfterPlace(int i, int j, int stil, int id)
        {
            TETeleport tmp = (TETeleport)TileEntity.ByID[id];
            tmp.range = range;
            tmp.name = "Teleport" + id;
        }
        public static Point16 GetTopLeft(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int posx = i - ((tile.frameX % 54) / 18);
            int posy = j - ((tile.frameY % 18) / 18);
            return new Point16(posx, posy);
        }

        public string GetTeleportInfo()
        {
            string info = "";
            info =  "Name : " + name + "\n";
            info += "Con  : " + connectedTo + "\n";
            info += "Rng  : " + range;
            return info;
        }

        public List<Point16> CheckServersInRange(Point16 pos)
        {
            List<Point16> temp=new List<Point16>();
            foreach ( Point16 server in WirelessWorld.servers)
            {
                Rectangle sRect=new Rectangle((server.X+1)*16,(server.Y+1)*16,16,16);
                if (((TETeleport)TileEntity.ByPosition[pos]).rangeRect().Intersects(sRect))
                {
                    temp.Add(server);
                }
                               
            }
            Main.NewText("Servers in range"+temp.Count);
            return temp;
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
            return tile.active() && tile.type == mod.TileType<WirelessTeleporterBase>() && tile.frameX / 18 == 0 && (tile.frameY % 18)== 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            Main.NewText("teleport:i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }

            int id = Place(i, j);

            InitAfterPlace(i, j, style, id);
            return id;
        }
    }
}
