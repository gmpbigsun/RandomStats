using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RandomStats
{
    class RandomStatsModPlayer : ModPlayer
    {
        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (item.maxStack == 1 && item.GetGlobalItem<GlobalInstancedItems>().randomStat == 0)
            {
                item.GetGlobalItem<GlobalInstancedItems>().SetupRandomDamage(item);
            }
            base.PostBuyItem(vendor, shopInventory, item);
        }
    }
}


