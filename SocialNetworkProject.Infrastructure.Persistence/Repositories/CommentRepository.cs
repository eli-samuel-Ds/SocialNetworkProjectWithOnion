using SocialNetworkProject.Core.Domain.Entities;
using SocialNetworkProject.Core.Domain.Interfaces;
using SocialNetworkProject.Infrastructure.Persistence.Contexts;
using SocialNetworkProject.Infrastructure.Persistence.Repositories.Generic;

namespace SocialNetworkProject.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(SocialNetworkProjectContext context) : base(context) { }
    }
}
