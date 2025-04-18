﻿using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Role
{
    public Guid Id { get; set; }

    public string? RoleName { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
