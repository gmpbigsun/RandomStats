using System;
using Terraria.ModLoader;

namespace RandomStats
{
	public class RandomStats : Mod
	{
        public override void Load()
        {
            base.Load();
            //On.Terraria.Main.DamageVar += (orig, damage, luck) => (int)Math.Round(damage);
        }
    }
}