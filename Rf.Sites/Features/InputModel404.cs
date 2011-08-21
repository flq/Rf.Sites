namespace Rf.Sites.Features
{
    public class InputModel404
    {
        public string Message { get; private set; }

        public InputModel404(string message)
        {
            Message = message;
        }

        public string Title { get { return "404 - resource not found."; } }
    }
}