using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
                selectedIssuesString += ticket.Number.ToString( ) + " ";
            }

            label1.Text = selectedIssuesString;
            _parameterArray = parameterArray;
            _selectedTickets = selectedTickets;
        }

        private void button1_Click(object sender, EventArgs e)
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
                    client.UpdateIssue(int.Parse(_parameterArray[3]), ticket.Number, int.Parse(_parameterArray[4]), _parameterArray[1]);
                }
                catch (Exception ex)
                {
                    _logger.WriteExceptionToLog(ex);
                }
            }
        }
    }
}
