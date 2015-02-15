using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using LogWriterUtility;

namespace TurtleBugNET
{
    public partial class CommitFinishedForm : Form
    {
        private readonly string[] _parameterArray;
        private readonly List<TicketItem> _selectedTickets;
        private readonly LogWriter _logger = LogWriter.GetInstance("c:\\temp\\", "TurtleBugNET.log");

        public CommitFinishedForm(List<TicketItem> selectedTickets, string[] parameterArray)
        {
            InitializeComponent( );
            var selectedIssuesString = "Selected Issues :";

            foreach (var ticket in selectedTickets)
            {
                selectedIssuesString += ticket.Number.ToString( ) + ", ";
            }

            label1.Text = selectedIssuesString.Remove(selectedIssuesString.Length - 2, 2);
            _parameterArray = parameterArray;
            _selectedTickets = selectedTickets;

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            var binding = new System.ServiceModel.BasicHttpBinding();
            binding.Name = "BugNetServicesSoap";
            binding.AllowCookies = true;

            var endpointStr = _parameterArray[0];
            var endpoint = new System.ServiceModel.EndpointAddress(endpointStr);

            var client = new BugNET.BugNetServicesSoapClient(binding, endpoint);

            client.Open();
            if (!client.LogIn(_parameterArray[1], _parameterArray[2]))
            {
                throw new Exception("login failed");
            }
            var comitStati = (from item in client.GetStatusObjects(Convert.ToInt32(_parameterArray[3]))
                              where item.Id == int.Parse(_parameterArray[4])
                              select item 
                              ).First();

            checkBoxSetStatus.Text += comitStati.Name + '"';
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var binding = new System.ServiceModel.BasicHttpBinding
            {
                Name = "BugNetServicesSoap",
                AllowCookies = true
            };

            var endpointStr = _parameterArray[0];
            var endpoint = new System.ServiceModel.EndpointAddress(endpointStr);

            var client = new BugNET.BugNetServicesSoapClient(binding, endpoint);


            client.Open();
            if (!client.LogIn(_parameterArray[1], _parameterArray[2]))
            {
                throw new Exception("login failed");
            }
            foreach (var ticket in _selectedTickets)
            {
                try
                {
                    if (checkBoxSetStatus.Checked)
                    {
                        client.UpdateIssueStatus(int.Parse(_parameterArray[3]), ticket.Number, int.Parse(_parameterArray[4]), _parameterArray[1]);
                    }
                    if (checkBoxReassign.Checked)
                    {
                        client.ReassignIssueToCreator(int.Parse(_parameterArray[3]), ticket.Number, _parameterArray[1]);
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteExceptionToLog(ex);
                }
            }
        }
    }
}
