using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace TurtleBugNET
{
    [ComVisible(true),
        Guid("18B7CD4C-C2DD-404B-BCD7-8B904DF3FAEE"),
        ClassInterface(ClassInterfaceType.None)]
    public class TurtleBugNetPlugin : Interop.BugTraqProvider.IBugTraqProvider2, Interop.BugTraqProvider.IBugTraqProvider
    {
        private List<TicketItem> selectedTickets = new List<TicketItem>();

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
            string[] revPropNames = new string[0];
            string[] revPropValues = new string[0];
            string dummystring = "";
            return GetCommitMessage2( hParentWnd, parameters, "", commonRoot, pathList, originalMessage, "", out dummystring, out revPropNames, out revPropValues );
        }

        public string GetCommitMessage2( IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList,
                               string originalMessage, string bugID, out string bugIDOut, out string[] revPropNames, out string[] revPropValues )
        {
            try
            {
                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                binding.Name = "BugNetServicesSoap";

                string endpointStr = "http://www.ledsys.co.uk/BugNet/WebServices/BugNetServices.asmx";
                var endpoint = new System.ServiceModel.EndpointAddress(endpointStr);
                
                BugNET.BugNetServicesSoapClient client = new BugNET.BugNetServicesSoapClient(binding, endpoint);
                

                client.Open();
                if (!client.LogIn("steve", ""))
                {
                    throw new Exception("login failed");
                }

                //List<BugNET.Issue> issues = client.GetProjectIssueList(3, "");

                //var tickets = issues.Select(issue => new TicketItem(issue.Id, issue.Title)).ToList();

                revPropNames = new string[2];
                revPropValues = new string[2];
                revPropNames[0] = "bugtraq:issueIDs";
                revPropNames[1] = "myownproperty";
                revPropValues[0] = "13, 16, 17";
                revPropValues[1] = "myownvalue";

                bugIDOut = bugID + "added";

                //MyIssuesForm form = new MyIssuesForm( tickets );
                //if ( form.ShowDialog( ) != DialogResult.OK )
                //    return originalMessage;

                //StringBuilder result = new StringBuilder( originalMessage );
                //if ( originalMessage.Length != 0 && !originalMessage.EndsWith( "\n" ) )
                //    result.AppendLine( );

                //foreach ( TicketItem ticket in form.TicketsFixed )
                //{
                //    result.AppendFormat( "Fixed #{0}: {1}", ticket.Number, ticket.Summary );
                //    result.AppendLine( );
                //    selectedTickets.Add( ticket );
                //}


                //return result.ToString( );
                return "";
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.ToString( ) );
                throw;
            }
        }

        public string CheckCommit( IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList, string commitMessage )
        {
            return "the commit log message is not correct.";
        }

        public string OnCommitFinished( IntPtr hParentWnd, string commonRoot, string[] pathList, string logMessage, int revision )
        {
            // we now could use the selectedTickets member to find out which tickets
            // were assigned to this commit.
            CommitFinishedForm form = new CommitFinishedForm( selectedTickets );
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
            OptionsForm form = new OptionsForm( );
            if ( form.ShowDialog( ) != DialogResult.OK )
                return "";

            string options = form.ComboProject.SelectedValue + ";";
            options += form.ComboCommitStatus.SelectedValue + ";";
            return options;
        }

    }
}
