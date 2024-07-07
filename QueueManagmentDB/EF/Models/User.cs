using System;
using System.Collections.Generic;

namespace QueueManagmentDB.EF.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Password { get; set; }

    public byte[]? Salt { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Queue> Queues { get; set; } = new List<Queue>();
}
