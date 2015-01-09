#region Comments and license
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
using Microsoft.Win32;
using System.Security.Permissions;
#endregion

namespace ScreenSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load display text from the Registry
        /// </summary>
        private void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\screensaver-time");
            if (key == null)
            {
                textBox.Text = "%t";
                numericUpDown1.Value = 10;
                numericUpDown2.Value = 50;
            }
            else
            {
                try
                {
                    textBox.Text = (string)key.GetValue("text", "%t");
                    numericUpDown1.Value = (int)key.GetValue("screenclear", "10");
                    numericUpDown2.Value = (int)key.GetValue("displayspeed", "50");
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\screensaver-time");

            key.SetValue("text", textBox.Text);
            key.SetValue("screenclear", numericUpDown1.Value.ToString(), RegistryValueKind.DWord);
            key.SetValue("displayspeed", numericUpDown2.Value.ToString(), RegistryValueKind.DWord);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // http://scripts.sil.org/cms/scripts/page.php?item_id=Gentium_basic
            System.Diagnostics.Process.Start(@"http://scripts.sil.org/cms/scripts/page.php?item_id=Gentium_basic");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // http://www.harding.edu/fmccown/screensaver/screensaver.html
            System.Diagnostics.Process.Start(@"http://www.harding.edu/fmccown/screensaver/screensaver.html");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // http://ryanwhitworth.com/screensavers/
            System.Diagnostics.Process.Start(@"http://ryanwhitworth.com/screensavers/");
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            label1.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Demo";
        }
    }
}
