using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Items
{
	public class BossBags : GlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag" && (arg == ItemID.DestroyerBossBag || arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.TwinsBossBag))
			{
                if (Main.rand.Next(100) <= 20)
                {
                    player.QuickSpawnItem(mod.ItemType("ServerChip"), Main.rand.Next(1, 3));
                }
			}
		}
	}
}