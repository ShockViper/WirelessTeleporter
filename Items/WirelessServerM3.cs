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
    class WirelessServerM3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wireless Server MK3");
            Tooltip.SetDefault("Midline server for 6 teleports");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 39;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 1;
            item.maxStack = 1;
            item.value = Item.sellPrice(0, 50);
            item.createTile = mod.TileType("WirelessServer");
            item.placeStyle = 2;
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar,1);//chip
            recipe.AddIngredient(mod.ItemType("WirelessServerM2"));//clorophite
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
