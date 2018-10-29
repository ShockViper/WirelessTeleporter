using System.Collections.Generic;
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
        internal int rangeMultiplier = 2;
        internal int style;
        internal Point16 position = new Point16(-1, -1);
        internal Point16 connectedTo=new Point16(-1,-1);
        internal Point16 range = new Point16(20,20);

        

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
            Rectangle rect= new Rectangle((Position.X + 1) * 16 - (range.X * 16), (Position.Y + 1) * 16 - (range.Y * 16), range.X * 32, range.Y * 32);
            return rect;
        }

        private void InitAfterPlace(int i, int j, int stil, int id)
        {
            TETeleport tmp = (TETeleport)TileEntity.ByID[id];
            tmp.range = new Point16((Main.maxTilesX / 8) * tmp.rangeMultiplier, ((Main.maxTilesY / 8) * tmp.rangeMultiplier));
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
            info += "Range: " + range.X+"/"+range.Y;
            info += "\nRight click to setup";
            return info;
        }

        public bool TryTeleport(Point16 dest)
        {
            bool result = false;
            Rectangle[] array = new Rectangle[2];
            Rectangle startPos = array[0];
            startPos.X = (int)(this.position.X * 16f);
            startPos.Width = 48;
            startPos.Height = 48;
            startPos.Y = (int)(this.position.Y * 16f - (float)startPos.Height);
            Rectangle endPos = array[1];
            endPos.X = (int)(dest.X * 16f);
            endPos.Width = 48;
            endPos.Height = 48;
            endPos.Y = (int)(dest.Y * 16f - (float)endPos.Height);
            for (int i = 0; i < 2; i++)
            {
                Vector2 value = new Vector2((float)(endPos.X - startPos.X), (float)(endPos.Y - startPos.Y));
                if (i == 1)
                {
                    value = new Vector2((float)(startPos.X - endPos.X), (float)(startPos.Y - endPos.Y));
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
            return temp;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"name", name},
                {"teleportID", teleportID},
                {"connectedTo", connectedTo},
                {"rangeMultiplier", rangeMultiplier},
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

            range = new Point16((Main.maxTilesX / 8) * rangeMultiplier, ((Main.maxTilesY / 8) * rangeMultiplier));
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
            if (connectedTo != new Point16(-1, -1))
            {
            TEServer server = (TEServer)TileEntity.ByPosition[connectedTo];
            server.teleports.Remove(this);
            }
            base.OnKill();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
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
