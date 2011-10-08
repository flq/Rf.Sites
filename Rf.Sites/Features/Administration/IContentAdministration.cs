namespace Rf.Sites.Features.Administration
{
    public interface IContentAdministration
    {
        object GetContent(int id);
        string[] GetTags();
        void UpdateContent(int id, dynamic content);
        int InsertContent(dynamic content);
    }
}