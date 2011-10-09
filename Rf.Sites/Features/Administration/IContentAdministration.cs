namespace Rf.Sites.Features.Administration
{
    public interface IContentAdministration
    {
        dynamic GetContent(string id);
        string[] GetTags();
        void UpdateContent(string id, dynamic content);
        string InsertContent(dynamic content);
    }
}