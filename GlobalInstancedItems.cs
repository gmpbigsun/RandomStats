using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;
using System.Text.RegularExpressions;
using Terraria.ID;

namespace RandomStats
{
    public class GlobalInstancedItems : GlobalItem
    {

        public double randomStat;
        private readonly int rngMinValue;
        private readonly int rngMaxValue;


        public override bool InstancePerEntity => true;

        public GlobalInstancedItems()
        {
            randomStat = 0;
            rngMinValue = ModContent.GetInstance<RandomStatsConfig>().MinRandomVariance;
            rngMaxValue = ModContent.GetInstance<RandomStatsConfig>().MaxRandomVariance;
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            GlobalInstancedItems myClone = (GlobalInstancedItems)base.Clone(item, itemClone);
            myClone.randomStat = randomStat;
            return myClone;
        }

        public void SetupRandomDamage(Item item)
        {
            if (item.maxStack == 1 && randomStat == 0)
                item.GetGlobalItem<GlobalInstancedItems>().randomStat = (double)(Main.rand.Next(rngMinValue, rngMaxValue) / 100.0);
        }

        public void SetupArmorDefense(Item item)
        {
            if ((item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0) && randomStat > 0)
                item.defense = (int)(item.OriginalDefense * (float)randomStat);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.damage > 0 && randomStat != 0 && item.maxStack == 1)
                damage *= (float)randomStat;

            base.ModifyWeaponDamage(item, player, ref damage);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            SetupArmorDefense(item);
            base.UpdateEquip(item, player);
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            base.UpdateAccessory(item, player, hideVisual);
        }

        public override void OnCreate(Item item, ItemCreationContext context)
        {
            SetupRandomDamage(item);
            SetupArmorDefense(item);
            base.OnCreate(item, context);
        }

        public override bool OnPickup(Item item, Player player)
        {
            SetupRandomDamage(item);
            SetupArmorDefense(item);
            return base.OnPickup(item, player);
        }

        public override void OnSpawn(Item item, IEntitySource source)
        {
            SetupRandomDamage(item);
            SetupArmorDefense(item);
            base.OnSpawn(item, source);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.damage > 0 && item.maxStack == 1)
            {
                string damageNumberRegex = "[0-9]+";
                string newText = Regex.Replace(tooltips[1].Text, damageNumberRegex, (randomStat == 0 ? "" : (int)(item.damage * (float)randomStat) + " ") + "[" + (int)(item.OriginalDamage * (double)(rngMinValue / 100.0)) + "-" + (int)(item.OriginalDamage * (double)(rngMaxValue / 100.0)) + "]");
                tooltips[1].Text = newText;
            }
            else if (item.defense > 0 && item.maxStack == 1 && (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0))
            {
                string defenseNumberRegex = "[0-9]+";
                string newText = Regex.Replace(tooltips[2].Text, defenseNumberRegex, (randomStat == 0 ? "" : (int)(item.OriginalDefense * (float)randomStat) + " ") + "[" + (int)(item.OriginalDefense * (double)(rngMinValue / 100.0)) + "-" + (int)(item.OriginalDefense * (double)(rngMaxValue / 100.0)) + "]");
                tooltips[2].Text = newText;
            }
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            randomStat = tag.GetDouble("randomStat");
            SetupArmorDefense(item);
            SetupRandomDamage(item);
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag["randomStat"] = randomStat;
            base.SaveData(item, tag);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(randomStat);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            randomStat = reader.ReadDouble();
        }
    }
}


