﻿using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Entities.BaseEntities;
using ProcedureMakerServer.Enums;

namespace ProcedureMakerServer.Dtos;

public class FileDto
{
    public string CaseNumber { get; set; } = string.Empty;
    public CourtType CourtType { get; set; }
    public Client CurrentLawyersClient { get; set; } = new Client();
    public List<CourtMemberBase> CourtMembers { get; set; } = new();


}
