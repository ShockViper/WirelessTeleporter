using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using Microsoft.Xna.Framework.Input;
using WirelessTeleporter.Tiles;
using Terraria.DataStructures;

namespace WirelessTeleporter
{
    class UIServerPanel :UIPanel
    {
        public bool connected;
        internal TEServer btnServer;
        internal TETeleport btnTeleport;
        internal UIText txt = new UIText("");

        public override void OnInitialize()
        {
            base.OnInitialize();
            this.Width.Set(10f, 0f);
            this.Height.Set(10f, 0f);
        }
        public override void Click(UIMouseEvent evt)           
        {
            if (btnServer.capacity <= btnServer.teleports.Count || btnTeleport.connectedTo == btnServer.position) { return; }
            TEServer old;
            if (btnTeleport.connectedTo != new Point16(-1, -1))
            {
                old = (TEServer)TileEntity.ByPosition[btnTeleport.connectedTo];
                old.teleports.Remove(btnTeleport);
            }
            btnTeleport.connectedTo = btnServer.position;
            btnServer.teleports.Add(btnTeleport);
            connected = true;
            foreach(UIServerPanel pnl in WirelesTeleporter.serverUI.serverpanels)
            {
                if(pnl !=null)
                {
                    if (this != pnl)
                    {
                        pnl.connected = false;
                    }
                    pnl.SetInfo(pnl.btnServer, pnl.btnTeleport,true);
                    
                }
                Parent.RecalculateChildren();
            }
            base.Click(evt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (connected) { this.BackgroundColor= Color.Blue; } else { this.BackgroundColor =Color.Blue*0.2f; }
            base.Draw(spriteBatch);
            if (IsMouseHovering)
            {
                Main.hoverItemName = "Click to connect to server";
            }
        }

        public void SetInfo(TEServer server,TETeleport teleport,bool reset=false)
        {
            btnServer = server;
            btnTeleport = teleport;            
            txt.SetText( server.name + ":" + server.position.ToString() + " " + server.teleports.Count + "/" + server.capacity);
            txt.Top.Set(-5f, 0f);
            txt.Left.Set(0f, 0f);
            txt.Width.Set(this.Width.Pixels-5, 0f);
            txt.Height.Set(this.Height.Pixels-5, 0f);
            if (!reset)
            {
                this.Append(txt);
            }
        }

        
    }

    class UITeleportPanel : UIPanel
    {
        internal TEServer btnServer;
        internal TETeleport btnTeleport;
        internal TETeleport sourceTeleport;
        internal UIText txt = new UIText("");

        public override void OnInitialize()
        {
            base.OnInitialize();
            this.Width.Set(10f, 0f);
            this.Height.Set(10f, 0f);
        }
        public override void Click(UIMouseEvent evt)
        {
            if (sourceTeleport != null)
            {
                if (sourceTeleport.TryTeleport(btnTeleport.position)) { ServerInfoUI.visible = false; }
            }
            base.Click(evt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsMouseHovering && sourceTeleport!=null)
            {
                Main.hoverItemName = "Click to teleport ";
            }
        }

        public void SetInfo(TEServer server, TETeleport teleport, TETeleport src, bool reset = false)
        {
            btnServer = server;
            btnTeleport = teleport;
            sourceTeleport = src;
            txt.SetText(teleport.name + ":" + teleport.position.ToString());
            txt.Top.Set(-5f, 0f);
            txt.Left.Set(0f, 0f);
            txt.Width.Set(this.Width.Pixels - 5, 0f);
            txt.Height.Set(this.Height.Pixels - 5, 0f);
            if (!reset)
            {
                this.Append(txt);
            }
        }


    }

    class ServerInfoUI : UIState
    {
        public static TETeleport activeTeleport;
        public static TEServer activeServer;
        public static Point16 activePos;
        public static MouseState curMouse;
        public static MouseState oldMouse;
        public UIServerPanel[] serverpanels = { null, null, null, null };
        private UIServerPanel tServerPanel;
        private UITeleportPanel tTeleportPanel;

        public static bool MouseClicked
        {
            get
            {
                return curMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released;
            }
        }

        public UIPanel info;
        private UITextBox txtName;
        public static bool visible = false;

        public override void OnInitialize()
        {
            info = new UIPanel();
            info.SetPadding(0);
            info.Left.Set(500f, 0f);
            info.Top.Set(300f, 0f);
            info.Width.Set(300f, 0f);
            info.Height.Set(300f, 0f);

            txtName = new UITextBox();
            txtName.Left.Set(10f, 0f);
            txtName.Top.Set(10f, 0f);
            txtName.Width.Set(250f, 0f);
            txtName.Height.Set(40f, 0f);
            info.Append(txtName);

            Texture2D btnClose = ModLoader.GetTexture("WirelessTeleporter/UI/BtnClose");
            UIHoverImageButton closeButton = new UIHoverImageButton(btnClose, "Close");
            closeButton.Left.Set(270, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(22, 0f);
            closeButton.Height.Set(22, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            info.Append(closeButton);
            base.Append(info);

            SetToCenter();
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuOpen);
            activeServer = null;
            activeTeleport = null;
            visible = false;
        }


        public void SetName(string name)
        {
            this.txtName.SetText(name);
        }

        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            if (!info.IsMouseHovering) { visible = false; }
        }

        private void SetToCenter()
        {
            info.Left.Set((Main.screenWidth / 2) - (info.Width.Pixels / 2), 0f);
            float top = (Main.screenHeight / 2) - (info.Height.Pixels + Player.defaultHeight);
            if (top < 0) { top = 0f; }
            info.Top.Set(top, 0f);
            info.Recalculate();
        }

        public void AddServerPanel()
        {
            UIPanel infoPanel = new UIPanel();
            infoPanel.Left.Set(10f, 0f);
            infoPanel.Top.Set(55f, 0f);
            infoPanel.Width.Set(250f, 0f);
            infoPanel.Height.Set(55f, 0f);

            UIText cap = new UIText("Capacity: "+activeServer.capacity);
            cap.Width.Set(0f, 1);
            cap.Left.Set(0f, 0f);
            cap.Top.Set(-5f, 0f);
            infoPanel.Append(cap);

            UIText world = new UIText("Server cap: " + WirelessWorld.activeServers+"/"+WirelessWorld.maxServers);
            world.Width.Set(0f, 1);
            world.Left.Set(0f, 0f);
            world.Top.Set(15f, 0f);
            infoPanel.Append(world);
            info.Append(infoPanel);

            UIPanel teleportsConnected = new UIPanel();
            teleportsConnected.Left.Set(10f, 0f);
            teleportsConnected.Top.Set(115f, 0f);
            teleportsConnected.Width.Set(250f, 0f);
            teleportsConnected.Height.Set(200f, 0f);

            float lastpos = -5f;
            UIText txt = new UIText("Connected teleports:");
            txt.Width.Set(0f, 1);
            txt.Left.Set(0f, 0f);
            txt.Top.Set(lastpos, 0f);
            teleportsConnected.Append(txt);

            lastpos += 20;
            foreach (TETeleport teleport in activeServer.teleports)
            {
                tTeleportPanel = new UITeleportPanel();
                tTeleportPanel.SetInfo(activeServer, teleport, null);
                tTeleportPanel.Width.Set(0f, 1);
                tTeleportPanel.Left.Set(0f, 0f);
                tTeleportPanel.Top.Set(lastpos, 0f);
                tTeleportPanel.Height.Set(30f, 0f);
                tTeleportPanel.Recalculate();
                teleportsConnected.Append(tTeleportPanel);
                lastpos = lastpos + tTeleportPanel.Height.Pixels + 5f;
            }
            lastpos += 15f;
            if (lastpos < 120f) { lastpos = 120f; }
            teleportsConnected.Height.Set(lastpos, 0f);
            info.Append(teleportsConnected);
            info.Height.Set(teleportsConnected.Top.Pixels + teleportsConnected.Height.Pixels + 10, 0f);
            SetToCenter();
        }

        public void AddConnectPanel(TETeleport teleport , List<Point16> servers, Point16 connectedServer)
        {
            float lastpos=-5f;
            UIPanel serversInRangePanel = new UIPanel();
            serversInRangePanel.Left.Set(10f, 0f);
            serversInRangePanel.Top.Set(55f, 0f);
            serversInRangePanel.Width.Set(250f, 0f);
            serversInRangePanel.Height.Set(200f, 0f);

            int cntServer = 0;
            UIText txt = new UIText("Servers in range:");
            txt.Width.Set(0f, 1);
            txt.Left.Set(0f, 0f);
            txt.Top.Set(lastpos, 0f);
            serversInRangePanel.Append(txt);

            lastpos += 20;
            foreach (Point16 server in servers)
            {
                TEServer tmp = (TEServer)TileEntity.ByPosition[server];
                tServerPanel = new UIServerPanel();
                if (tmp.position == connectedServer) { tServerPanel.connected = true; }
                tServerPanel.SetInfo(tmp,teleport);
                tServerPanel.Width.Set(0f,1);
                tServerPanel.Left.Set(0f, 0f);
                tServerPanel.Top.Set(lastpos, 0f);
                tServerPanel.Height.Set(30f, 0f);
                tServerPanel.Recalculate();
                serverpanels[cntServer] = tServerPanel;
                cntServer++;
                serversInRangePanel.Append(tServerPanel);
                lastpos = lastpos + tServerPanel.Height.Pixels + 5f;
            }
            lastpos += 15f;
            if (lastpos < 120f) { lastpos = 120f; }
            serversInRangePanel.Height.Set(lastpos, 0f);
            info.Append(serversInRangePanel);
            info.Height.Set(serversInRangePanel.Top.Pixels + serversInRangePanel.Height.Pixels + 10, 0f);
            SetToCenter();
        }

        public void AddTeleportPanel(Point16 thisPos, Point16 connectedServer)
        {
            float lastpos=-5f;
            UIPanel remoteTeleportsPanel = new UIPanel();
            remoteTeleportsPanel.Left.Set(10f, 0f);
            remoteTeleportsPanel.Top.Set(55f, 0f);
            remoteTeleportsPanel.Width.Set(250f, 0f);
            remoteTeleportsPanel.Height.Set(200f, 0f);
            if (connectedServer != new Point16(-1, -1))
            {
                TEServer server = (TEServer)TileEntity.ByPosition[connectedServer];
                TETeleport thisTeleport = (TETeleport)TileEntity.ByPosition[thisPos];
                if (server.teleports.Count == 1)
                {
                    UIText error = new UIText("No other teleports \nconnected to server!");
                    error.Top.Set(lastpos, 0f);
                    error.Left.Set(10f, 0f);
                    error.TextColor = Color.Red;
                    remoteTeleportsPanel.Append(error);
                }
                else
                {
                    UIText txt = new UIText("Remote teleports:");
                    txt.Width.Set(0f, 1);
                    txt.Left.Set(0f, 0f);
                    txt.Top.Set(lastpos, 0f);
                    remoteTeleportsPanel.Append(txt);
                    lastpos += 20;

                    foreach (TETeleport teleport in server.teleports)
                    {
                        if (teleport.position == thisPos) { continue; }

                        tTeleportPanel = new UITeleportPanel();
                        tTeleportPanel.SetInfo(server, teleport, thisTeleport);
                        tTeleportPanel.Width.Set(0f, 1);
                        tTeleportPanel.Left.Set(0f, 0f);
                        tTeleportPanel.Top.Set(lastpos, 0f);
                        tTeleportPanel.Height.Set(30f, 0f);
                        tTeleportPanel.Recalculate();
                        remoteTeleportsPanel.Append(tTeleportPanel);
                        lastpos = lastpos + tTeleportPanel.Height.Pixels + 5f;

                    }
                }
            }
            else
            {
                UIText error = new UIText("Not connected to server!");
                error.Top.Set(lastpos, 0f);
                error.Left.Set(10f, 0f);
                error.TextColor = Color.Red;
                remoteTeleportsPanel.Append(error);
            }
            info.Append(remoteTeleportsPanel);
            info.Height.Set(remoteTeleportsPanel.Top.Pixels + remoteTeleportsPanel.Height.Pixels + 10, 0f);
            SetToCenter();
        }

        public static void CheckMouse(GameTime gameTime)
        {
            oldMouse = curMouse;
            curMouse = Mouse.GetState();
        }

        public static bool KeyTyped(Keys key)
        {
            return !Main.keyState.IsKeyDown(key) && Main.oldKeyState.IsKeyDown(key);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 
            // Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if(visible)
            {
                if (KeyTyped(Keys.Escape)) { visible = false; }
            }
        }
    }
}
