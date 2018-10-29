﻿using System;
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
    class ServerUpgradeMK4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Server Upgrade MK4");
            Tooltip.SetDefault("Third upgrade for server");
        }

        public override void SetDefaults()
        {
            item.width = 49;
            item.height = 31;
            item.useTurn = false;
            item.autoReuse = false;
            item.consumable = true;
            item.rare = 1;
            item.maxStack = 99;
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "ServerChip",4);
            recipe.AddIngredient(mod, "GoldWireSpool", 2);
            recipe.AddIngredient(ItemID.SpectreBar, 5);
            recipe.AddIngredient(ItemID.Wire, 30);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
