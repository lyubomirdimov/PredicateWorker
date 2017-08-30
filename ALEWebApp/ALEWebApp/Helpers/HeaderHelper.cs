using System.Web.Mvc;

namespace ALEWebApp.Helpers
{
    public enum MessageType
    {
        Success = 1,
        Failure = 2,
        Warning = 3,
        Info = 4
    }

    public static class HeaderHelper
    {


        /// <summary>
        /// Display header message on postback
        /// </summary>
        public static void SetHeaderMessage(this Controller currentController, MessageType messageType, string message)
        {
            switch (messageType)
            {
                case MessageType.Success:
                    currentController.TempData["Success"] = message;
                    break;
                case MessageType.Failure:
                    currentController.TempData["Failure"] = message;
                    break;
                case MessageType.Warning:
                    currentController.TempData["Warning"] = message;
                    break;
                case MessageType.Info:
                    currentController.TempData["Info"] = message;
                    break;
                default:
                    break;
            }
        }
    }
}
