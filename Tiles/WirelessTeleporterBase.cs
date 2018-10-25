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
using Microsoft.Xna.Framework.Graphics;

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
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<TETeleport>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
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

        public override void MouseOver(int i, int j)
        {
            MouseOverBoth(i, j);
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOverBoth(i, j);
        }

        private void MouseOverBoth(int i, int j)
        {
            Point16 topleft = TETeleport.GetTopLeft(i, j);
            WirelesTeleporter.hovering = true;

            if (true)
            {
                string info = ((TETeleport)TileEntity.ByPosition[topleft]).GetTeleportInfo();
                WirelesTeleporter.hovername = info;
            }
        }

        public override void RightClick(int i, int j)
        {
            base.RightClick(i, j);
            Point16 topleft = TETeleport.GetTopLeft(i, j);
            List<Point16> servers= ((TETeleport)TileEntity.ByPosition[topleft]).CheckServersInRange(topleft);
            foreach(Point16 pos in servers)
            {
                Main.NewText(pos.ToString());
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            int frame = Main.tileFrame[Type];
            switch (frame)
            {
                case 0:
                    r = 0.1f;
                    g = 0.1f;
                    b = 0.1f;
                    break;
                case 1:
                    b = 0.3f;
                    break;
                case 2:
                    b = 0.5f;
                    break;
                case 3:
                    b = 0.7f;
                    break;
            }
        }
 
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            // the 3rd and 4th numbers should be 16*width blocks and 16 * height blocks respectively.
            Item.NewItem(i * 16, j * 16, 48, 16, mod.ItemType("WirelessTeleporterBase"));
            mod.GetTileEntity<TETeleport>().Kill(i, j);
        }
    }
}
