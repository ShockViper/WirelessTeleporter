using Terraria.ModLoader;
using Terraria.ID;

namespace WirelessTeleporter.Items
{
    class WirelessTeleporterBase : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Wireless Teleporter Base");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Teleporter);
            item.createTile = mod.TileType("WirelessTeleporterBase");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod,"GoldWireSpool",2);
            recipe.AddIngredient(ItemID.Diamond,2);
            recipe.AddIngredient(ItemID.Sapphire, 2);
            recipe.AddIngredient(ItemID.Lens,4);
            recipe.AddIngredient(ItemID.Teleporter);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
