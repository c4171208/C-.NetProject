using System;
using System.Collections.Generic;

namespace QueueManagmentDB.EF.Models;

public partial class Queue
{
    public int QueId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DesignatedTime { get; set; }

    public virtual User? User { get; set; }
}
