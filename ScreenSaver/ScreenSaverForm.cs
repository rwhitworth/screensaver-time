﻿#region Comments and license
/*
 * ScreenSaverForm.cs
 * By Frank McCown
 * Summer 2010
 * 
 * Modified Dec 2014 & Jan 2015 by Ryan Whitworth
 * 
 * Released by Frank McCown under the "Feel free to modify this code" license in 2010
 * Released by Ryan Whitworth under the "Feel free to modify this code" license in 2015
 * 
 * http://www.harding.edu/fmccown/screensaver/screensaver.html
 */
#endregion

#region Using Statements
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

#region ScreenSaver namespace
namespace ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        #region Win32 API functions
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out System.Drawing.Rectangle lpRect);
        #endregion

        private System.Drawing.Point mouseLocation;
        private bool previewMode = false;
        private Random rand = new Random();
        private const int mouse_move_amount = 7;

        public ScreenSaverForm()
        {
            InitializeComponent();
        }
        public ScreenSaverForm(System.Drawing.Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
        }
        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            System.Drawing.Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new System.Drawing.Point(0, 0);

            // Make text smaller
            textLabel.Font = new System.Drawing.Font("Arial", 6);

            previewMode = true;
        }
        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            LoadSettings();

            Cursor.Hide();
            TopMost = true;

            moveTimer.Interval = 2000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();
        }
        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            // Move text to new location
            textLabel.Left = rand.Next(Math.Max(1, Bounds.Width - textLabel.Width));
            textLabel.Top = rand.Next(Math.Max(1, Bounds.Height - textLabel.Height));
        }
        private void LoadSettings()
        {
            textLabel.Text = @"ScreenSaver 1.0";
        }
        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - e.X) > mouse_move_amount ||
                        Math.Abs(mouseLocation.Y - e.Y) > mouse_move_amount)
                        Application.Exit();
                }

                // Update current mouse location
                mouseLocation = e.Location;
            }
        }
        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }
        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }
    }
}
#endregion

namespace ScreenSaver2d
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Boolean runonce = false;
        SpriteFont sp;
        Random r;
        System.Drawing.Rectangle rr = System.Windows.Forms.SystemInformation.VirtualScreen;
        int Mouse_X = Mouse.GetState().X;
        int Mouse_Y = Mouse.GetState().Y;
        int last_screen_clear = 0;
        string s = DateTime.Now.ToLongTimeString();
        int slen;
        int shi;
        Vector2 vec;
        #endregion
        #region constants
        const int mouse_move = 7;
        #endregion

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = ".";
            r = new Random();
        }

        public Microsoft.Xna.Framework.Color create_color(int red, int green, int blue)
        {
            Microsoft.Xna.Framework.Color c = new Microsoft.Xna.Framework.Color();
            if (red > 256 || green > 256 || blue > 256)
            {
                throw new Exception("create_color: invalid parameter");
            }
            c.R = (byte)red;
            c.G = (byte)green;
            c.B = (byte)blue;
            return c;
        }
        public Microsoft.Xna.Framework.Color HSVtoRGB(int h)
        {
            const float single4 = 0.99609375f;
            const float single5 = 0.99609375f;

            float single;
            float single1;
            float single2;
            float single3 = ((float)h / 256f) * 6f;
            int num = Convert.ToInt32(Math.Floor((double)single3));
            float single6 = single3 - (float)num;
            float single7 = 0.00389099121f;
            float single8 = single5 * (1f - single4 * single6);
            float single9 = single5 * (1f - single4 * (1f - single6));
            switch (num)
            {
                case 0:
                    {
                        single = single5;
                        single1 = single9;
                        single2 = single7;
                        break;
                    }
                case 1:
                    {
                        single = single8;
                        single1 = single5;
                        single2 = single7;
                        break;
                    }
                case 2:
                    {
                        single = single7;
                        single1 = single5;
                        single2 = single9;
                        break;
                    }
                case 3:
                    {
                        single = single7;
                        single1 = single8;
                        single2 = single5;
                        break;
                    }
                case 4:
                    {
                        single = single9;
                        single1 = single7;
                        single2 = single5;
                        break;
                    }
                case 5:
                    {
                        single = single5;
                        single1 = single7;
                        single2 = single8;
                        break;
                    }
                default:
                    {
                        single = 0f;
                        single1 = 0f;
                        single2 = 0f;
                        break;
                    }
            }

            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color();
            single = single * 255f;
            single1 = single1 * 255f;
            single2 = single2 * 255f;
            color = create_color(Convert.ToInt32(single), Convert.ToInt32(single1), Convert.ToInt32(single2));
            return color;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.Position = new Microsoft.Xna.Framework.Point(0, 0);
            this.Window.IsBorderless = true;
            this.IsMouseVisible = false;

            graphics.PreferredBackBufferWidth = rr.Width;
            graphics.PreferredBackBufferHeight = rr.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sp = Content.Load<SpriteFont>("Gentium Book Basic 14");
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (r.Next(0, 10) > 6)
            {
                #region deal with mouse
                if (Mouse_X == 0 || Mouse_Y == 0)
                {
                    Mouse_X = Mouse.GetState().X;
                    Mouse_Y = Mouse.GetState().Y;
                }

                if (Keyboard.GetState().GetPressedKeys().Length != 0)
                    Exit();

                int MXH = Mouse_X + mouse_move;
                int MXL = Mouse_X - mouse_move;
                int MYH = Mouse_Y + mouse_move;
                int MYL = Mouse_Y - mouse_move;
                if (MXH < Mouse.GetState().X || MXL > Mouse.GetState().X)
                    Exit();
                if (MYH < Mouse.GetState().Y || MYL > Mouse.GetState().Y)
                    Exit();

                Mouse_X = Mouse.GetState().X;
                Mouse_Y = Mouse.GetState().Y;
                #endregion
            }
            #region runonce stuff
            if (runonce == false)
            {
                System.Drawing.Rectangle rr = System.Windows.Forms.SystemInformation.VirtualScreen;
                runonce = true;
            }
            #endregion

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            s = DateTime.Now.ToLongTimeString();
            vec = sp.MeasureString(s);
            slen = (int)vec.X;
            shi = (int)vec.Y;

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(sp, s, new Vector2(r.Next(0, rr.Width - slen), r.Next(0, rr.Height - shi)), HSVtoRGB(r.Next(0, 255)));                
            }

            spriteBatch.End();

            if ((int)gameTime.TotalGameTime.TotalSeconds % 10 == 0)
            {
                if (last_screen_clear != (int)gameTime.TotalGameTime.TotalSeconds)
                {
                    last_screen_clear = (int)gameTime.TotalGameTime.TotalSeconds;
                    GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                }
            }

            System.Threading.Thread.Sleep(1);
            base.Draw(gameTime);
        }
    }
}