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
            item.value = Item.buyPrice(0, 25);
            // Set other item.X values here
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LogicGate_AND,2);
            recipe.AddIngredient(ItemID.LogicGate_NAND,2);
            recipe.AddIngredient(ItemID.LogicGate_NOR,2);
            recipe.AddIngredient(ItemID.LogicGate_NXOR,2);
            recipe.AddIngredient(ItemID.LogicGate_OR,2);
            recipe.AddIngredient(ItemID.LogicGate_XOR,2);
            recipe.AddIngredient(mod.ItemType("GoldWireSpool"));
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
