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
            }
            else
            {
                try
                {
                    textBox.Text = (string)key.GetValue("text");
                    numericUpDown1.Value = (int)key.GetValue("screenclear");
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
            key.SetValue("screenclear", numericUpDown1.Value.ToString());
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
    }
}
