﻿using ProcedureMakerServer.Interfaces;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication.AuthModels;

[TsClass]
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RoleTypes Role { get; set; }
}
