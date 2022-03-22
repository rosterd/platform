using Expo.Server.Client;
using Expo.Server.Models;
using Xunit;

namespace Rosterd.Helpers
{
    public class ExpoNotificationSender
    {
        [Fact]
        public async Task SendTestNotification()
        {
            var expoClient = new PushApiClient();

            var pushTicketReq = new PushTicketRequest()
            {
                PushTo = new List<string> { "ExponentPushToken[7ff_5oJeXrjwbGXxZbCYcs]" },
                PushBadgeCount = 1,
                PushTitle = "This is a test",
                PushBody = $"Testing push notification"
            };

            var result = await expoClient.PushSendAsync(pushTicketReq);
        }
    }
}
