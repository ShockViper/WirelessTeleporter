using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
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
        public bool CheckTooFar()
        {
            int range = 4;
            Rectangle rect = new Rectangle();
            rect.X = (int)((ServerInfoUI.activePos.X -  Player.tileRangeX) * 16f);
            rect.Width = Player.tileRangeX*2*16;
            rect.Height = Player.tileRangeY*2*16;
            rect.Y = (int)((ServerInfoUI.activePos.Y + Player.tileRangeY) * 16f - (float)rect.Height);
            Rectangle pRect = Main.player[Main.myPlayer].getRect();
 //           Dust.QuickBox(rect.TopLeft(), rect.BottomRight(), 10, Color.Blue, null);
 //           Dust.QuickBox(pRect.TopLeft(), pRect.BottomRight(), 10, Color.Red, null);
            if (rect.Intersects(pRect))
            {
                return false;
            }
            return true;
        }
        public override void PostUpdate()
        {
            if (ServerInfoUI.visible && ServerInfoUI.activePos != new Point16(-1,-1))
            {
                if (CheckTooFar()) { ServerInfoUI.visible = false; }
            }
            base.PostUpdate();
            if (timerStarted)
            {
                timer--;
                if (timer <= 0) { timerStarted = false; }
            }
        }
    }
}
