using Terraria.ModLoader;
using Terraria.ID;

namespace WirelessTeleporter.Items
{
    class WirelessServer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wireless Server");
            Tooltip.SetDefault("Basic server for 2 teleports");
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
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod,"BasicServer");
            recipe.AddIngredient(mod.ItemType("WirelessServerFrame"));
            recipe.AddIngredient(ItemID.Wire,5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
