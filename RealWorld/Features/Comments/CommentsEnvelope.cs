using RealWorld.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Comments
{
    public class CommentsEnvelope
    {
        public CommentsEnvelope(List<Comment> comments)
        {
            Comments = comments;
        }

        public List<Comment> Comments { get; }
    }
}
