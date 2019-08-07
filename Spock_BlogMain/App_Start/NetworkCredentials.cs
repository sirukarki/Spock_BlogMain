namespace Spock_BlogMain
{
    internal class NetworkCredentials
    {
        private string gmailUsername;
        private string gmailPassword;

        public NetworkCredentials(string gmailUsername, string gmailPassword)
        {
            this.gmailUsername = gmailUsername;
            this.gmailPassword = gmailPassword;
        }
    }
}