using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace WirelessTeleporter
{
    public class WirelessWorld : ModWorld
    {
        public static int activeServers = 0;
        public const int maxServers = 4;
        public static int totalCapacity = 0;
        public static List<Point16> servers;
        internal static bool timerStarted = false;
        internal static int timer;

        public static bool checkTimerText()
        {
            if (timerStarted) { return false; }
            timerStarted = true;
            timer = 300;
            return true;
        }

        public override void Initialize()
        {
            activeServers = 0;
            servers = new List<Point16>();
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {"activeServers", activeServers}
            };
        }

        public override void Load(TagCompound tag)
        {
            activeServers = tag.GetInt("activeServers");
            if(activeServers > maxServers) { activeServers = 0; }
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (timerStarted)
            {
                timer--;
                if (timer <= 0) { timerStarted = false; }
            }
        }
    }
}
