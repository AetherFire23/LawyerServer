﻿namespace ProcedureMakerServer.Authentication.ReturnModels;




public class RegisterResult
{
    public UserDto User { get; set; }

    public RegisterResult(UserDto user)
    {
        User = user;
    }
}
