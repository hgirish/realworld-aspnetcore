using RealWorld.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Comments
{
    public class CommentEnvelope
    {
        public CommentEnvelope(Comment comment)
        {
            Comment = comment;
        }

        public Comment Comment { get; }
    }
}
