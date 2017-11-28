using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Service.Notification
{
    public class NotificationService : INotificationService
    {
        public void SendPushNotification(string token, string title, string message)
        {
            try
            {
                var applicationID = "AAAAUbXD6sk:APA91bHG0g7IsFi5_jnjB8lcxJMSd-y72Ahc4EfMTFIQw7kNu3lEmNkdPuV7RoeGy9IAP7e7JF64VRMy8BZqLv9GJ8LPmBbrq05b0dAQnr9_cFOnhWTIEunYPY60m9DXOyggrAmPoFS_";
                var senderId = "350941866697";
                var deviceId = token;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = message,
                        title = title,
                        sound = "Enabled"
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }   
        }
    }
}
