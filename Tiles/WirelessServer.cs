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
    class WirelessServer : WirelessServerFrame
    {
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
                    type = mod.ItemType("WirelessServerM2");
                    break;
                case 2:
                    type = mod.ItemType("WirelessServerM3");
                    break;
                case 3:
                    type = mod.ItemType("WirelessServerM4");
                    break;
                case 4:
                    type = mod.ItemType("WirelessServerM5");
                    break;
                default:
                    type = mod.ItemType("WirelessServer");
                    break;
            }
            return type;
        }


    }

}
