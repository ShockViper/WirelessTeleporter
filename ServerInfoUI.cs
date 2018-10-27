using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            TEServer old;// = new TEServer;
            if (btnTeleport.connectedTo != new Point16(-1, -1))
            {
                old = (TEServer)TileEntity.ByPosition[btnTeleport.connectedTo];
                old.teleports.Remove(btnTeleport);
            }
            btnTeleport.connectedTo = btnServer.position;
            btnServer.teleports.Add(btnTeleport);
            //setInfo(btnServer, btnTeleport, true);
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
            if (sourceTeleport.TryTeleport(btnTeleport.position)) { ServerInfoUI.visible = false; }
            base.Click(evt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsMouseHovering)
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
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            info = new UIPanel();
            info.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(info);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            info.Left.Set(500f, 0f);
            info.Top.Set(300f, 0f);
            info.Width.Set(300f, 0f);
            info.Height.Set(300f, 0f);
            //info.BackgroundColor = Color.BlueViolet;

            // Next, we create another UIElement that we will place. Since we will be calling `info.Append(playButton);`, Left and Top are relative to the top left of the info UIElement. 
            // By properly nesting UIElements, we can position things relatively to each other easily.
            // Texture2D buttonPlayTexture = mod.GetTexture("Terraria/UI/ButtonPlay");
            txtName = new UITextBox();
            txtName.Left.Set(10f, 0f);
            txtName.Top.Set(10f, 0f);
            txtName.Width.Set(250f, 0f);
            txtName.Height.Set(40f, 0f);

            info.Append(txtName);
            //UIHoverImageButton playButton = new UIHoverImageButton(buttonPlayTexture, "Reset Coins Per Minute Counter");
            //playButton.Left.Set(110, 0f);
            //playButton.Top.Set(10, 0f);
            //playButton.Width.Set(22, 0f);
            //playButton.Height.Set(22, 0f);
            //// UIHoverImageButton doesn't do anything when Clicked. Here we assign a method that we'd like to be called when the button is clicked.
            //playButton.OnClick += new MouseEvent(PlayButtonClicked);
            //info.Append(playButton);

            Texture2D btnClose = ModLoader.GetTexture("WirelessTeleporter/UI/BtnClose");
            UIHoverImageButton closeButton = new UIHoverImageButton(btnClose, "Close"); // Localized text for "Close"
            closeButton.Left.Set(270, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(22, 0f);
            closeButton.Height.Set(22, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            info.Append(closeButton);

            //// UIMoneyDisplay is a fairly complicated custom UIElement. UIMoneyDisplay handles drawing some text and coin textures.
            //// Organization is key to managing UI design. Making a contained UIElement like UIMoneyDisplay will make many things easier.
            //moneyDiplay = new UIMoneyDisplay();
            //moneyDiplay.Left.Set(15, 0f);
            //moneyDiplay.Top.Set(20, 0f);
            //moneyDiplay.Width.Set(100f, 0f);
            //moneyDiplay.Height.Set(0, 1f);
            //info.Append(moneyDiplay);
            Recalculate();
            base.Append(info);
            SetToCenter();

            // As a recap, ExampleUI is a UIState, meaning it covers the whole screen. We attach info to ExampleUI some distance from the top left corner.
            // We then place playButton, closeButton, and moneyDiplay onto info so we can easily place these UIElements relative to info.
            // Since info will move, this proper organization will move playButton, closeButton, and moneyDiplay properly when info moves.
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
            info.Top.Set((Main.screenHeight / 2) - (info.Height.Pixels / 2), 0f);
            info.Recalculate();
        }

        public void AddConnectPanel(TETeleport teleport , List<Point16> servers, Point16 connectedServer)
        {
            float lastpos;
            UIPanel serverPanel = new UIPanel();
            serverPanel.Left.Set(10f, 0f);
            serverPanel.Top.Set(55f, 0f);
            serverPanel.Width.Set(250f, 0f);
            serverPanel.Height.Set(200f, 0f);
            lastpos = 0f;
            int cntServer = 0;
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
                serverPanel.Append(tServerPanel);
                lastpos = lastpos + tServerPanel.Height.Pixels + 5f;

            }
            lastpos += 15f;
            if (lastpos < 120f) { lastpos = 120f; }
            serverPanel.Height.Set(lastpos, 0f);
            info.Append(serverPanel);
            info.Height.Set(serverPanel.Top.Pixels + serverPanel.Height.Pixels + 10, 0f);
            SetToCenter();
        }

        public void AddTeleportPanel(Point16 thisPos, Point16 connectedServer)
        {
            float lastpos;
            UIPanel teleportPanel = new UIPanel();
            teleportPanel.Left.Set(10f, 0f);
            teleportPanel.Top.Set(55f, 0f);
            teleportPanel.Width.Set(250f, 0f);
            teleportPanel.Height.Set(200f, 0f);
            lastpos = 5f;
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
                    teleportPanel.Append(error);
                }
                else
                {
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
                        teleportPanel.Append(tTeleportPanel);
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
                teleportPanel.Append(error);
            }
            info.Append(teleportPanel);
            info.Height.Set(teleportPanel.Top.Pixels + teleportPanel.Height.Pixels + 10, 0f);
            SetToCenter();
        }


        public static void CheckMouse(GameTime gameTime)
        {
            oldMouse = curMouse;
            curMouse = Mouse.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.

            // Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }
}
