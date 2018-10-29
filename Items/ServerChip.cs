using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace WirelessTeleporter.Items
{
    class ServerChip : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Server Processor");
            Tooltip.SetDefault("You'll need this for a server");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.useTurn = false;
            item.autoReuse = false;
            item.consumable = true;
            item.rare = 1;
            item.maxStack = 30;
            item.value = Item.sellPrice(0, 50);
            // Set other item.X values here
        }

 
    }
}
