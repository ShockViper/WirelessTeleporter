﻿using Terraria.ModLoader;
using Terraria.ID;

namespace WirelessTeleporter.Items
{
    class WirelessServerM4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wireless Server MK4");
            Tooltip.SetDefault("Even better server for 8 teleports");
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
            item.createTile = mod.TileType("WirelessServer");
            item.placeStyle = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "ServerUpgradeMK4");
            recipe.AddIngredient(mod.ItemType("WirelessServerM3"));
            recipe.AddIngredient(ItemID.Wire, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "BasicServer");
            recipe.AddIngredient(mod, "ServerUpgradeMK2");
            recipe.AddIngredient(mod, "ServerUpgradeMK3");
            recipe.AddIngredient(mod, "ServerUpgradeMK4");
            recipe.AddIngredient(mod.ItemType("WirelessServerFrame"));
            recipe.AddIngredient(ItemID.Wire, 20);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
