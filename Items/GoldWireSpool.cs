using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

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
            item.value = Item.buyPrice(0, 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire,50);
            recipe.AddIngredient(ItemID.GoldOre,50);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 50);
            recipe.AddIngredient(ItemID.PlatinumOre, 50);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
