using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace WirelessTeleporter
{
	public class UITextBox : UIElement
	{
		private static List<UITextBox> textBoxes = new List<UITextBox>();

		private const int padding = 2;
        private readonly string defaultText = "name";
        private int cursorPosition = 0;
		private bool hasFocus = false;
		private int cursorTimer = 0;
        private string Text  = string.Empty;

        public UITextBox()
		{
			this.SetPadding(padding);
			textBoxes.Add(this);
		}

		public UITextBox(string defaultText) : this()
		{
			this.defaultText = defaultText;
		}

        public void SetText(string txt)
        {
            Text = txt;
        }

		public void Reset()
		{
			Text = string.Empty;
			cursorPosition = 0;
			hasFocus = false;
			CheckBlockInput();
		}

        public static Rectangle GetFullRectangle(UIElement element)
        {
            Vector2 vector = new Vector2(element.GetDimensions().X, element.GetDimensions().Y);
            Vector2 position = new Vector2(element.GetDimensions().Width, element.GetDimensions().Height) + vector;
            vector = Vector2.Transform(vector, Main.UIScaleMatrix);
            position = Vector2.Transform(position, Main.UIScaleMatrix);
            Rectangle result = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
            int width = Main.spriteBatch.GraphicsDevice.Viewport.Width;
            int height = Main.spriteBatch.GraphicsDevice.Viewport.Height;
            result.X = Utils.Clamp<int>(result.X, 0, width);
            result.Y = Utils.Clamp<int>(result.Y, 0, height);
            result.Width = Utils.Clamp<int>(result.Width, 0, width - result.X);
            result.Height = Utils.Clamp<int>(result.Height, 0, height - result.Y);
            return result;
        }

        public override void Update(GameTime gameTime)
		{
			cursorTimer++;
			cursorTimer %= 60;

			if (ServerInfoUI.MouseClicked && Parent != null)
			{
				Rectangle dim = GetFullRectangle(this);
				MouseState mouse = ServerInfoUI.curMouse;
				bool mouseOver = mouse.X > dim.X && mouse.X < dim.X + dim.Width && mouse.Y > dim.Y && mouse.Y < dim.Y + dim.Height;
				if (!hasFocus && mouseOver)
				{
					hasFocus = true;
					CheckBlockInput();
				}
				else if (hasFocus && !mouseOver)
				{
					hasFocus = false;
                    UpdateName();
                    CheckBlockInput();
					cursorPosition = Text.Length;
				}
			}
			else if (ServerInfoUI.curMouse.RightButton == ButtonState.Pressed && ServerInfoUI.oldMouse.RightButton == ButtonState.Released && Parent != null && hasFocus)
			{
				Rectangle dim = GetFullRectangle(this);
				MouseState mouse = ServerInfoUI.curMouse;
				bool mouseOver = mouse.X > dim.X && mouse.X < dim.X + dim.Width && mouse.Y > dim.Y && mouse.Y < dim.Y + dim.Height;
				if (!mouseOver)
				{
					hasFocus = false;
					cursorPosition = Text.Length;
                    UpdateName();
					CheckBlockInput();
				}
			}

			if (hasFocus)
			{
				if (ServerInfoUI.KeyTyped(Keys.Enter) || ServerInfoUI.KeyTyped(Keys.Tab) || ServerInfoUI.KeyTyped(Keys.Escape))
				{
                    UpdateName();
                    hasFocus = false;
					CheckBlockInput();
                 }
            }
			base.Update(gameTime);
		}

        private void UpdateName()
        {
            if (ServerInfoUI.activeTeleport != null) { ServerInfoUI.activeTeleport.name = Text; }
            if (ServerInfoUI.activeServer != null) { ServerInfoUI.activeServer.name = Text; }

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
		{
            if (hasFocus)
            {
                PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                Text = Main.GetInputText(this.Text);
                cursorPosition = Text.Length;
            }
            Texture2D texture = ModLoader.GetTexture("WirelessTeleporter/UI/TextBox");
			CalculatedStyle dim = GetDimensions();
			int innerWidth = (int)dim.Width - 2 * padding;
			int innerHeight = (int)dim.Height - 2 * padding;
			spriteBatch.Draw(texture, dim.Position(), new Rectangle(0, 0, padding, padding), Color.White);
			spriteBatch.Draw(texture, new Rectangle((int)dim.X + padding, (int)dim.Y, innerWidth, padding), new Rectangle(padding, 0, 1, padding), Color.White);
			spriteBatch.Draw(texture, new Vector2(dim.X + padding + innerWidth, dim.Y), new Rectangle(padding + 1, 0, padding, padding), Color.White);
			spriteBatch.Draw(texture, new Rectangle((int)dim.X, (int)dim.Y + padding, padding, innerHeight), new Rectangle(0, padding, padding, 1), Color.White);
			spriteBatch.Draw(texture, new Rectangle((int)dim.X + padding, (int)dim.Y + padding, innerWidth, innerHeight), new Rectangle(padding, padding, 1, 1), Color.White);
			spriteBatch.Draw(texture, new Rectangle((int)dim.X + padding + innerWidth, (int)dim.Y + padding, padding, innerHeight), new Rectangle(padding + 1, padding, padding, 1), Color.White);
			spriteBatch.Draw(texture, new Vector2(dim.X, dim.Y + padding + innerHeight), new Rectangle(0, padding + 1, padding, padding), Color.White);
			spriteBatch.Draw(texture, new Rectangle((int)dim.X + padding, (int)dim.Y + padding + innerHeight, innerWidth, padding), new Rectangle(padding, padding + 1, 1, padding), Color.White);
			spriteBatch.Draw(texture, new Vector2(dim.X + padding + innerWidth, dim.Y + padding + innerHeight), new Rectangle(padding + 1, padding + 1, padding, padding), Color.White);

			bool isEmpty = Text.Length == 0;
			string drawText = isEmpty ? defaultText : Text;
			DynamicSpriteFont font = Main.fontMouseText;
			Vector2 size = font.MeasureString(drawText);
			float scale = (innerHeight / size.Y)*1.25f;
			if (isEmpty && hasFocus)
			{
				drawText = string.Empty;
				isEmpty = false;
			}
			Color color = Color.Black;
			if (isEmpty)
			{
				color *= 0.75f;
			}
			spriteBatch.DrawString(font, drawText, new Vector2(dim.X + padding, dim.Y + padding), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			if (!isEmpty && hasFocus && cursorTimer < 30)
			{
				float drawCursor = font.MeasureString(drawText.Substring(0, cursorPosition)).X * scale;
				spriteBatch.DrawString(font, "|", new Vector2(dim.X + padding + drawCursor, dim.Y + padding), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
            if (IsMouseHovering)
            {
                Main.hoverItemName = "Click to change name";
            }
        }


		private static void CheckBlockInput()
		{
			Main.blockInput = false;
            foreach (UITextBox textBox in textBoxes)
			{
				if (textBox.hasFocus)
				{
					Main.blockInput = true;
					break;
				}
			}
		}
	}
}