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
    class GoldWireSpool : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wire Spool");
            Tooltip.SetDefault("Special wire for server upgrades");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.useTurn = false;
            item.autoReuse = false;
            item.consumable = true;
            item.rare = 1;
            item.maxStack = 99;
            item.value = Item.sellPrice(0, 50);
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire,50);//chip
            recipe.AddIngredient(ItemID.GoldOre,50);//clorophite
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 50);//chip
            recipe.AddIngredient(ItemID.PlatinumOre, 50);//clorophite
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
