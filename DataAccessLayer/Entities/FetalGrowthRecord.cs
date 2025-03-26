using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class FetalGrowthRecord
{
    public Guid Id { get; set; }

    public Guid PregnancyRecordId { get; set; }

    public string Name { get; set; } = null!;

    public string? Unit { get; set; }

    public string? Description { get; set; }

    public int? Week { get; set; }

    public double? Value { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual PregnancyRecord PregnancyRecord { get; set; } = null!;
}
