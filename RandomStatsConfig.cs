using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RandomStats
{
	public class RandomStatsConfig : ModConfig
	{
        public override ConfigScope Mode => ConfigScope.ServerSide;

        //[Header("Random Stat Variance")]
        [Header("randomstat_variance")]
        [Range(100, 1000)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(125)]
        [ReloadRequired]
        public int MaxRandomVariance;

        [Range(0, 100)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(75)]
        [ReloadRequired]
        public int MinRandomVariance;
    }
}