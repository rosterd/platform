namespace Rosterd.Domain.Settings
{
    public class Auth0Settings
    {
        public string Domain { get; set; }

        public string Audience { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        /// <summary>
        /// The name of the database connection configured to your client
        /// </summary>
        public string Connection { get; set; }
    }
}
