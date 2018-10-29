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
    class WirelessTeleporterBase : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Wireless Teleporter Base");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Teleporter);
            item.createTile = mod.TileType("WirelessTeleporterBase");
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod,"GoldWireSpool",2);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Lens,4);
            recipe.AddIngredient(ItemID.Teleporter);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
