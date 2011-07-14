using NHibernate.Validator.Constraints;

namespace Rf.Sites.Entities
{
    public class Comment : Entity
    {
        [NotNullNotEmpty]
        public virtual string CommenterName { get; set; }

        public virtual string CommenterEmail { get; set; }
        
        public virtual string CommenterWebsite { get; set; }

        [NotNullNotEmpty, Length(2000)]
        public virtual string Body { get; set; }

        public virtual bool IsFromSiteMaster { get; set; }

        public virtual bool AwaitsModeration { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Email: {1}, Website: {2}, Body first 50 chars: {3}, Bodylength: {4}",
              CommenterName, CommenterEmail, CommenterWebsite,
              Body != null ? Body.Length > 50 ? Body.Substring(0, 50) : Body : "null",
              Body != null ? Body.Length : 0);
        }
    }
}