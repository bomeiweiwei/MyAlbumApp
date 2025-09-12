using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Comment
{
    public long CommentId { get; set; }

    public long PhotoAlbumId { get; set; }

    public int MemberId { get; set; }

    public long? ParentCommentId { get; set; }

    public string Content { get; set; } = null!;

    public byte Status { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Member Member { get; set; } = null!;

    public virtual Comment? ParentComment { get; set; }

    public virtual PhotoAlbum PhotoAlbum { get; set; } = null!;
}
