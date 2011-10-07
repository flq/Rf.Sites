namespace Rf.Sites.Features.Administration
{
    public interface IContentAdministration
    {
        string[] GetTags();
        void UpdateContent(dynamic content);
        int InsertContent(dynamic content);
    }
}