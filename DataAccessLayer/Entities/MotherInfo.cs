using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class MotherInfo
{
    public Guid Id { get; set; }

    public Guid? PregnancyRecordId { get; set; }

    public string? MotherName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? BloodType { get; set; }

    public string? HealthStatus { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual PregnancyRecord? PregnancyRecord { get; set; }
}
