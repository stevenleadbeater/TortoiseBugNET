using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogWriterUtility;
using System.Configuration;
using System.Security.Principal;

namespace TurtleBugNET
{
    public partial class OptionsForm : Form
    {
        private LogWriter _logger = LogWriter.GetInstance(ConfigurationManager.AppSettings["LogDirectory"], ConfigurationManager.AppSettings["LogFileName"]);
        public OptionsForm( )
        {
            InitializeComponent( );
        }

        public ComboBox ComboProject { get { return comboProject; } }

        public ComboBox ComboCommitStatus { get { return comboCommitStatus; } }

        public TextBox TextBugNetUrl { get { return textBugNetUrl; } }

        public TextBox TextUserName { get { return textUserName; } }

        public TextBox TextPassword { get { return textPassword; } }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                binding.Name = "BugNetServicesSoap";
                binding.AllowCookies = true;

                string endpointStr = textBugNetUrl.Text;
                var endpoint = new System.ServiceModel.EndpointAddress(endpointStr);

                BugNET.BugNetServicesSoapClient client = new BugNET.BugNetServicesSoapClient(binding, endpoint);
                
                client.LogIn(textUserName.Text, textPassword.Text);

                comboProject.DisplayMember = "Text";
                comboProject.ValueMember = "Value";
                var projects = (from item in client.GetProjects()
                    select new
                    {
                        Text = item.Name,
                        Value = item.Id
                    }).ToArray();
                comboProject.DataSource = projects;
                client.LogOut();
            }
            catch (Exception ex)
            {
                _logger.WriteExceptionToLog(ex);
                throw;
            }
        }

        private void comboProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboProject_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                binding.Name = "BugNetServicesSoap";
                binding.AllowCookies = true;

                //http://www.ledsys.co.uk/BugNet/WebServices/BugNetServices.asmx

                string endpointStr = textBugNetUrl.Text;
                var endpoint = new System.ServiceModel.EndpointAddress(endpointStr);

                BugNET.BugNetServicesSoapClient client = new BugNET.BugNetServicesSoapClient(binding, endpoint);

                client.LogIn(textUserName.Text, textPassword.Text);

                comboCommitStatus.DisplayMember = "Text";
                comboCommitStatus.ValueMember = "Value";
                var comitStati = (from item in client.GetStatusObjects(Convert.ToInt32(comboProject.SelectedValue.ToString()))
                                  select new
                                  {
                                      Text = item.Name,
                                      Value = item.Id
                                  }).ToArray();
                comboCommitStatus.DataSource = comitStati;
                client.LogOut();
            }
            catch (Exception ex)
            {
                _logger.WriteExceptionToLog(ex);
                throw;
            }
        }

    }
}
