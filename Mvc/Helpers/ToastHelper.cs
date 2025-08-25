namespace Hospital.Helpers
{
    public class ToastHelper
    {
        public static (string? Type, string? Message) GetAndClearToast(ISession session)
        {
            var type = session.GetString("ToastType");
            var msg = session.GetString("ToastMessage");

            // svuoto subito per non farlo ripetere
            session.Remove("ToastType");
            session.Remove("ToastMessage");

            return (type, msg);
        }
    }
}
