using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class PhotoAlbum
{
    public long PhotoAlbumId { get; set; }

    public int CategoryId { get; set; }

    public int? UploaderMemberId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string StoragePath { get; set; } = null!;

    public string? OriginalFileName { get; set; }

    public string? ContentType { get; set; }

    public long? SizeBytes { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public string? ThumbPath { get; set; }

    public byte Visibility { get; set; }

    public byte Status { get; set; }

    public long ViewCount { get; set; }

    public int LikeCount { get; set; }

    public int CommentCount { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Member? UploaderMember { get; set; }
}
