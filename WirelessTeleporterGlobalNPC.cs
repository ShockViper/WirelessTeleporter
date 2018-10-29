using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WirelessTeleporter
{
    class WirelessTeleporterGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (((npc.type == NPCID.TheDestroyer) || (npc.type == NPCID.SkeletronPrime) || (npc.type == NPCID.Retinazer) || (npc.type == NPCID.Spazmatism)))
            {
                if (Main.rand.Next(100) <= 20)
                {
                    int stack = Main.rand.Next(1, 2);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GoldWireSpool"), stack);
                }
                if (!Main.expertMode)
                {
                    if (Main.rand.Next(100) <= 10)
                    {
                        int stack = Main.rand.Next(1, 2);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ServerChip"), stack);
                    }
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Cyborg)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("ServerChip"));
                nextSlot++;
            }
            else if (type == NPCID.Mechanic && (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3))
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("GoldWireSpool"));
                nextSlot++;
            }
        }


    }
}
