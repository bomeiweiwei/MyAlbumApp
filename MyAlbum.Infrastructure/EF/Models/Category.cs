using System;
using System.Collections.Generic;

namespace MyAlbum.Infrastructure.EF.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<PhotoAlbum> PhotoAlbums { get; set; } = new List<PhotoAlbum>();
}
