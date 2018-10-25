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
namespace WirelessTeleporter
{
    class ServerInfoUI : UIState
    {
        public UIPanel info;
        public static bool visible = false;

        public override void OnInitialize()
        {
            // Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
            info = new UIPanel();
            info.SetPadding(0);
            // We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(info);`. 
            // This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
            info.Left.Set(100f, 0f);
            info.Top.Set(100f, 0f);
            info.Width.Set(170f, 0f);
            info.Height.Set(70f, 0f);
            info.BackgroundColor = Color.White; ;

            // Next, we create another UIElement that we will place. Since we will be calling `info.Append(playButton);`, Left and Top are relative to the top left of the info UIElement. 
            // By properly nesting UIElements, we can position things relatively to each other easily.
            // Texture2D buttonPlayTexture = mod.GetTexture("Terraria/UI/ButtonPlay");
            UIText name = new UIText("name:");
            info.Append(name);
            //UIHoverImageButton playButton = new UIHoverImageButton(buttonPlayTexture, "Reset Coins Per Minute Counter");
            //playButton.Left.Set(110, 0f);
            //playButton.Top.Set(10, 0f);
            //playButton.Width.Set(22, 0f);
            //playButton.Height.Set(22, 0f);
            //// UIHoverImageButton doesn't do anything when Clicked. Here we assign a method that we'd like to be called when the button is clicked.
            //playButton.OnClick += new MouseEvent(PlayButtonClicked);
            //info.Append(playButton);

            //Texture2D buttonDeleteTexture = ModContent.GetTexture("Terraria/UI/ButtonDelete");
            //UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
            //closeButton.Left.Set(140, 0f);
            //closeButton.Top.Set(10, 0f);
            //closeButton.Width.Set(22, 0f);
            //closeButton.Height.Set(22, 0f);
            //closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            //info.Append(closeButton);

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

            // As a recap, ExampleUI is a UIState, meaning it covers the whole screen. We attach info to ExampleUI some distance from the top left corner.
            // We then place playButton, closeButton, and moneyDiplay onto info so we can easily place these UIElements relative to info.
            // Since info will move, this proper organization will move playButton, closeButton, and moneyDiplay properly when info moves.
        }
    }
}
