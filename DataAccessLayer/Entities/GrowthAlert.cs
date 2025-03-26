using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class GrowthAlert
{
    public Guid Id { get; set; }

    public Guid FetalGrowthRecordId { get; set; }

    public Guid UserId { get; set; }

    public int? Week { get; set; }

    public DateTime? AlertDate { get; set; }

    public string? AlertFor { get; set; }

    public string? Issue { get; set; }

    public string? Severity { get; set; }

    public string? Recommendation { get; set; }

    public bool? IsResolved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual FetalGrowthRecord FetalGrowthRecord { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
