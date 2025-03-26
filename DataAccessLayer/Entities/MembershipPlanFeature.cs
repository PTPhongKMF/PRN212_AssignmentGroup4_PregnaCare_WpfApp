using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class MembershipPlanFeature
{
    public Guid Id { get; set; }

    public Guid MembershipPlanId { get; set; }

    public Guid FeatureId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Feature Feature { get; set; } = null!;

    public virtual MembershipPlan MembershipPlan { get; set; } = null!;
}
