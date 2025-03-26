using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class GrowthMetric
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Unit { get; set; }

    public string? Description { get; set; }

    public double? MinValue { get; set; }

    public double? MaxValue { get; set; }

    public int? Week { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }
}
