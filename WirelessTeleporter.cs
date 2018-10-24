using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using WirelessTeleporter.Tiles;

namespace WirelessTeleporter
{
    public class WirelesTeleporter : Mod
    {
        internal static WirelesTeleporter instance;
        internal static  Dictionary<string,int> servers;

        public override void Load()
        {
            instance = this;
            servers = new Dictionary<string, int>;

        }
        public override void Unload()
        {
            instance = null;
            servers = null;

        }
    }

}
