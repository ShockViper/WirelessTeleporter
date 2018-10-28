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
using Terraria.ID;

namespace WirelessTeleporter.Tiles
{
    class TETeleport : ModTileEntity
    {
        internal string name;
        internal int teleportID;
        internal int rangeMultiplier = 1;
        internal int style;
        internal Point16 position = new Point16(-1, -1);
        internal Point16 connectedTo=new Point16(-1,-1);
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

        public Rectangle RangeRect()
        {
            Rectangle rect= new Rectangle((Position.X + 1) * 16 - range * 16, (Position.Y + 1) * 16 - range * 16, range * 16 * 2, range * 16 * 2);
            Dust.QuickBox(rect.TopLeft(), rect.BottomRight(), 10, Color.White, null);
            return rect;
        }

        private void InitAfterPlace(int i, int j, int stil, int id)
        {
            TETeleport tmp = (TETeleport)TileEntity.ByID[id];
            tmp.range = (Main.maxTilesY / 4) * tmp.rangeMultiplier;
            tmp.name = "Teleport" + id;
            tmp.position = new Point16(i, j);
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
            info += "Range: " + range;
            return info;
        }

        public bool TryTeleport(Point16 dest)
        {
            bool result = false;
            Main.NewText("teleporting from:" + this.position.ToString() + " To:" + dest.ToString());
            Rectangle[] array = new Rectangle[2];
            array[0].X = (int)(this.position.X * 16f);
            array[0].Width = 48;
            array[0].Height = 48;
            array[0].Y = (int)(this.position.Y * 16f - (float)array[0].Height);
            array[1].X = (int)(dest.X * 16f);
            array[1].Width = 48;
            array[1].Height = 48;
            array[1].Y = (int)(dest.Y * 16f - (float)array[1].Height);
            for (int i = 0; i < 2; i++)
            {
                Vector2 value = new Vector2((float)(array[1].X - array[0].X), (float)(array[1].Y - array[0].Y));
                if (i == 1)
                {
                    value = new Vector2((float)(array[0].X - array[1].X), (float)(array[0].Y - array[1].Y));
                }
                if (!Wiring.blockPlayerTeleportationForOneIteration)
                {

                    for (int j = 0; j < 255; j++)
                    {
                        if (Main.player[j].active && !Main.player[j].dead && !Main.player[j].teleporting && array[i].Intersects(Main.player[j].getRect()))
                        {
                            result = true;
                            Vector2 vector = Main.player[j].position + value;
                            Main.player[j].teleporting = true;
                            if (Main.netMode == 2)
                            {
                                RemoteClient.CheckSection(j, vector, 1);
                            }
                            Main.player[j].Teleport(vector, 0, 0);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendData(65, -1, -1, null, 0, (float)j, vector.X, vector.Y, 0, 0, 0);
                            }
                        }
                    }
                }
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].teleporting && Main.npc[k].lifeMax > 5 && !Main.npc[k].boss && !Main.npc[k].noTileCollide)
                    {
                        int type = Main.npc[k].type;
                        if (!NPCID.Sets.TeleportationImmune[type] && array[i].Intersects(Main.npc[k].getRect()))
                        {
                            Main.npc[k].teleporting = true;
                            Main.npc[k].Teleport(Main.npc[k].position + value, 0, 0);
                        }
                    }
                }

            }
            for (int l = 0; l < 255; l++)
            {
                Main.player[l].teleporting = false;
            }
            for (int m = 0; m < 200; m++)
            {
                Main.npc[m].teleporting = false;
            }
            return result;
        }

        public bool CheckPlayerInRange()
        {
            Rectangle rect = new Rectangle();
            rect.X = (int)(this.position.X * 16f);
            rect.Width = 48;
            rect.Height = 48;
            rect.Y = (int)(this.position.Y * 16f - (float)rect.Height);
            for (int j = 0; j < 255; j++)
            {
                if (rect.Intersects(Main.player[j].getRect()))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Point16> CheckServersInRange(Point16 pos)
        {
            List<Point16> temp=new List<Point16>();
            foreach ( Point16 server in WirelessWorld.servers)
            {
                Rectangle sRect=new Rectangle((server.X+1)*16,(server.Y+1)*16,16,16);
                if (((TETeleport)TileEntity.ByPosition[pos]).RangeRect().Intersects(sRect))
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
                {"connectedTo", rangeMultiplier},
                {"rangeMultiplier", connectedTo},
                {"style", style},
                {"pos", position }
            };
        }

        public override void Load(TagCompound tag)
        {
            name = tag.Get<string>("name");
            teleportID = tag.Get<int>("teleportID");
            rangeMultiplier = tag.Get<int>("rangeMultiplier");
            style = tag.Get<int>("style");
            connectedTo = tag.Get<Point16>("connectedTo");
            position = tag.Get<Point16>("pos");

            if (connectedTo!=new Point16(-1, -1))
            {
                TEServer server = (TEServer)TileEntity.ByPosition[connectedTo];
                server.teleports.Add(this);
            }
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<WirelessTeleporterBase>() && tile.frameX / 18 == 0 && (tile.frameY % 18)== 0;
        }

        public override void OnKill()
        {
            TEServer server = (TEServer)TileEntity.ByPosition[connectedTo];
            server.teleports.Remove(this);
            Main.NewText("teleport killed");
            base.OnKill();
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
