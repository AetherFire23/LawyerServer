﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreBase.Entities;

[TsClass]
public class Case : EntityBase
{
    public Guid ManagerLawyerId { get; set; }
    public virtual Lawyer? ManagerLawyer { get; set; }

    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; } = new Client();

    public ICollection<CasePart> Participants { get; set; } = new List<CasePart>();


    public AccountStatement AccountStatement { get; set; }

    public string DistrictName { get; set; } = string.Empty;
    public string CourtAffairNumber { get; set; } = string.Empty;
    public string CaseNumber { get; set; } = string.Empty; // cases can have different filenumbers if it comes again many times 
    public ChamberNames CourtType { get; set; }
    public int CourtNumber { get; set; }
}

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasMany(p => p.Participants)
            .WithOne(p => p.Case)
            .HasForeignKey(k => k.CaseId);
    }
}
