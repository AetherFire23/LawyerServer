﻿using ProcedureMakerServer.Enums;
using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class RegisterResult
{
    public UserDto User { get; set; }
}