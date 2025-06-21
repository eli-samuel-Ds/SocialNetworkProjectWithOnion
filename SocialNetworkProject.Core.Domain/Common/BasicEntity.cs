namespace SocialNetworkProject.Core.Domain.Common
{
    public abstract class BasicEntity<TKey>
    {
        public required TKey Id { get; set; }
    }
}
