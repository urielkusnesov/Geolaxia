namespace Service.Notification
{
    public interface INotificationService
    {
        void SendPushNotification(string token, string title, string message, string activity);
    }
}
