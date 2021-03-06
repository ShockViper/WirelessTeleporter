﻿using Terraria.ModLoader;
using Terraria.ID;

namespace WirelessTeleporter.Items
{
    class WirelessServerFrame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wireless Server Frame");
            Tooltip.SetDefault("Frame for Wireless servers");
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
            item.createTile = mod.TileType("WirelessServerFrame");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar,10);
            recipe.AddIngredient(ItemID.Wire,10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 10);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
