using Terraria.ModLoader;
using Terraria.ID;

namespace WirelessTeleporter.Items
{
    class BasicServer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic Server for teleporter network");
            Tooltip.SetDefault("Basic server supports 2 teleports");
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
            recipe.AddIngredient(mod, "ServerChip",1);
            recipe.AddIngredient(mod, "GoldWireSpool", 1);
            recipe.AddIngredient(ItemID.TitaniumBar, 5);
            recipe.AddIngredient(ItemID.Wire, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "ServerChip", 1);
            recipe.AddIngredient(mod, "GoldWireSpool", 1);
            recipe.AddIngredient(ItemID.AdamantiteBar, 5);
            recipe.AddIngredient(ItemID.Wire, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
