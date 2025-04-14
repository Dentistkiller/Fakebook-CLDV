using System;
using System.Collections.Generic;

namespace YourProjectName.Models;

public partial class Post
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual User? User { get; set; }
}
