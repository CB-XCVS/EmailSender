using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Collections.Specialized;

namespace EmailSender
{
    public partial class Form1 : Form
    {
        // Load the emails variable which will be used to store all of the emails to send something to
        string[] emails;
        public Form1()
        {
            InitializeComponent();
        }
        // Get all of the previous settings
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.toList.Count > 0)
            {
                if (Properties.Settings.Default.toList[0] == "empty")
                {
                    return;
                }
                mailAddr.Text = Properties.Settings.Default.email;
                passWrd.Text = Properties.Settings.Default.password;
                smtpSvr.Text = Properties.Settings.Default.smtpSvr;
                usrName.Text = Properties.Settings.Default.username;
                List<string> list = new List<string>();
                foreach (string item in Properties.Settings.Default.toList)
                {
                    list.Add(item);
                }
                emails = list.ToArray();
                label6.Text = "To: " + emails.Length;
            }
        }

        // Set the people to send an email to
        private void button3_Click(object sender, EventArgs e)
        {
            // Create the Dialog
            using (var frm = new SelectPeopleForm())
            {
                // If everything worked out then we can get the emails from the dialog/form
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    emails = frm.GetText();
                }
            }
            // Set the 'To:' label to 'To:' and the amount of emails to send the msg to
            label6.Text = "To: " + emails.Length;
            
        }

        // Reset all of the saved and current settings
        private void button2_Click(object sender, EventArgs e)
        {
            // Ask user if they want to erase all current and saved settings
            if (MessageBox.Show("Are you sure you want to reset ALL of your current AND SAVED settings?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // If they said 'Yes' then we are here and now removing everything
                textBox1.Text = "";
                textBox2.Text = "";
                mailAddr.Text = "";
                smtpSvr.Text = "";
                usrName.Text = "";
                passWrd.Text = "";
                emails = null;
                Properties.Settings.Default.email = "empty";
                Properties.Settings.Default.password = null;
                Properties.Settings.Default.smtpSvr = null;
                Properties.Settings.Default.username = null;
                StringCollection sc = new StringCollection();
                sc.Add("empty");
                Properties.Settings.Default.toList = sc;
                Properties.Settings.Default.Save();
            }
            // If not we go here and nothing happens
        }

        // Send the emails
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = emails.Length;
            string error = null;
            foreach (var email in emails)
            {

                progressBar1.Value = progressBar1.Value + 1;
                try
                {
                    // Setup the client
                    var smtpClient = new SmtpClient(smtpSvr.Text)
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(usrName.Text, passWrd.Text),
                        EnableSsl = true,

                    };
                    // Send the emails
                    smtpClient.Send(mailAddr.Text, email, textBox1.Text, textBox2.Text);
                }
                catch (Exception ex)
                {
                    // Set variable for the error msg
                    error = ex.Message;
                }
                

                
            }
            // Notify user if the emails got sent or not

            // Check if an error occurred
            if (error == null)
            {
                // If 'error' = null then there was no error
                MessageBox.Show("EmailSender has sent all of the emails requested!");
            }
            else
            {
                // But if 'error' isn't null then there was an error and we need to notify the user of that
                MessageBox.Show("An error has occurred. More information: " + error + Environment.NewLine + Environment.NewLine + "If you are using Gmail: You will need to create a App Password for EmailSender");
            }
            
        }

        // Save all settings for later
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.email = mailAddr.Text;
            Properties.Settings.Default.password = passWrd.Text;
            Properties.Settings.Default.smtpSvr = smtpSvr.Text;
            Properties.Settings.Default.username = usrName.Text;
            StringCollection sc = new StringCollection();
            if (emails != null)
            {
                foreach (var email in emails)
                {
                    sc.Add(email);
                }
                Properties.Settings.Default.toList = sc;
            }
            Properties.Settings.Default.Save();
        }
    }
}
