﻿using Terraria.ModLoader;
using Terraria.ID;

namespace WirelessTeleporter.Items
{
    class ServerUpgradeMK2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Server Upgrade MK2");
            Tooltip.SetDefault("First upgrade to your server.\nSupport for 4 teleports");
        }

        public override void SetDefaults()
        {
            item.width = 49;
            item.height = 31;
            item.useTurn = false;
            item.autoReuse = false;
            item.consumable = true;
            item.rare = 1;
            item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "ServerChip",2);
            recipe.AddIngredient(mod, "GoldWireSpool", 1);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
