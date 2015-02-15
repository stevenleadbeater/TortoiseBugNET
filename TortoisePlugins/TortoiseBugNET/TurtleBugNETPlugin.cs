using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LogWriterUtility;
using Microsoft.Win32;

namespace TurtleBugNET
{
    [ComVisible(true),
        Guid("18B7CD4C-C2DD-404B-BCD7-8B904DF3FAEE"),
        ClassInterface(ClassInterfaceType.None)]
    public class TurtleBugNetPlugin : Interop.BugTraqProvider.IBugTraqProvider2, Interop.BugTraqProvider.IBugTraqProvider
    {
        private LogWriter _logger = LogWriter.GetInstance("c:\\temp\\", "TurtleBugNET.log");
        private List<TicketItem> selectedTickets = new List<TicketItem>();
        private string[] _parameterArray;

        public bool ValidateParameters(IntPtr hParentWnd, string parameters)
        {
            return true;
        }

        public string GetLinkText(IntPtr hParentWnd, string parameters)
        {
            return "Choose Issue";
        }

        public string GetCommitMessage(IntPtr hParentWnd, string parameters, string commonRoot, string[] pathList,
                                       string originalMessage)
        {
            var revPropNames = new string[0];
            var revPropValues = new string[0];
            var dummystring = "";
            return GetCommitMessage2( hParentWnd, parameters, "", commonRoot, pathList, originalMessage, "", out dummystring, out revPropNames, out revPropValues );
        }

        public string GetCommitMessage2( IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList,
                               string originalMessage, string bugID, out string bugIDOut, out string[] revPropNames, out string[] revPropValues )
        {
            try
            {
                
                
                var binding = new System.ServiceModel.BasicHttpBinding();
                binding.Name = "BugNetServicesSoap";
                binding.AllowCookies = true;

                _parameterArray = parameters.Split(';');
                var endpointStr = _parameterArray[0];
                var endpoint = new System.ServiceModel.EndpointAddress(endpointStr);
                
                var client = new BugNET.BugNetServicesSoapClient(binding, endpoint);
                

                client.Open();
                if (!client.LogIn(_parameterArray[1], _parameterArray[2]))
                {
                    throw new Exception("login failed");
                }

                var issues = client.GetProjectIssuesByProjectId(int.Parse(_parameterArray[3]));
                var tickets = issues.Select(issue => new TicketItem(issue.Id, issue.Title, issue.StatusName)).ToList();

                revPropNames = new string[0];
                revPropValues = new string[0];

                bugIDOut = bugID;

                var form = new MyIssuesForm(tickets);
                if (form.ShowDialog() != DialogResult.OK)
                    return originalMessage;

                var result = new StringBuilder(originalMessage);
                if (originalMessage.Length != 0 && !originalMessage.EndsWith("\n"))
                    result.AppendLine();
                var i = 0;
                foreach (var ticket in form.TicketsFixed)
                {
                    result.AppendFormat("Fixed #{0}: {1}", ticket.Number, ticket.Summary);
                    
                    result.AppendLine();
                    
                    selectedTickets.Add(ticket);
                    i++;
                }


                return result.ToString();
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.ToString( ) );
                throw;
            }
        }

        public string CheckCommit( IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList, string commitMessage )
        {
            _logger.WriteToLog("parameters: " + parameters);
            _logger.WriteToLog("commonURL: " + commonURL);
            _logger.WriteToLog("commonRoot: " + commonRoot);
            foreach (var path in pathList)
            {
                _logger.WriteToLog("path: " + path);
            }
            
            return null;
        }

        public string OnCommitFinished( IntPtr hParentWnd, string commonRoot, string[] pathList, string logMessage, int revision )
        {
            // we now could use the selectedTickets member to find out which tickets
            // were assigned to this commit.
            var form = new CommitFinishedForm( selectedTickets, _parameterArray );
            if ( form.ShowDialog( ) != DialogResult.OK )
                return "";
            // just for testing, we return an error string
            return "an error happened while closing the issue";
        }

        public bool HasOptions()
        {
            return true;
        }

        public string ShowOptionsDialog( IntPtr hParentWnd, string parameters )
        {
            var form = new OptionsForm( );
            if ( form.ShowDialog( ) != DialogResult.OK )
                return "";

            var options = form.TextBugNetUrl.Text + ";";
            options += form.TextUserName.Text + ";";
            options += form.TextPassword.Text + ";";
            options += form.ComboProject.SelectedValue + ";";
            options += form.ComboCommitStatus.SelectedValue + ";";
            return options;
        }

    }
}
