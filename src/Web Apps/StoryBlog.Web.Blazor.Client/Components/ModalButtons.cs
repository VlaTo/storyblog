namespace StoryBlog.Web.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModalButton
    {
        public string Title
        {
            get;
        }

        public ModalButton(string title)
        {
            Title = title;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ModalButtons
    {
        public static ModalButton OkButton
        {
            get;
        }

        public static ModalButton CancelButton
        {
            get;
        }

        public static ModalButton[] NoButtons
        {
            get;
        }

        public static ModalButton[] OkCancel()
        {
            return new[]
            {
                OkButton,
                CancelButton
            };
        }

        static ModalButtons()
        {
            OkButton = new ModalButton("Ok");
            CancelButton = new ModalButton("Cancel");

            NoButtons = new ModalButton[0];
        }
    }
}