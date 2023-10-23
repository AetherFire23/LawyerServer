﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneOf;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;
using ProcedureMakerServer.Exceptions;
using ProcedureMakerServer.Models;
using Crypt = BCrypt.Net.BCrypt;
namespace ProcedureMakerServer.Authentication;


public class AuthManager : IAuthManager
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly ProcedureContext _context;

    public AuthManager(IUserRepository userRepository,
                       IJwtTokenManager jwtTokenManager,
                       ProcedureContext context)
    {
        _userRepository = userRepository;
        _jwtTokenManager = jwtTokenManager;
        _context = context;
    }


    // should rechange everything to the atualy datatypes i wanna send I guess
    public async Task<OneOf<LoginResult, InvalidCredentials>> GenerateTokenIfCorrectCredentials(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByName(loginRequest.Username);

        if (user is null) return new InvalidCredentials();
        if (!Crypt.Verify(loginRequest.Password, user.HashedPassword))
        {
            return new InvalidCredentials();
        }

        string token = await _jwtTokenManager.GenerateToken(user);
        UserDto userDto = await _userRepository.MapUserDto(user.Id);

        var req = new LoginResult()
        {
            Token = token,
            UserDto = userDto,
        };

        return req; ;
    }

    public async Task<OneOf<RegisterResult, InvalidRequestMessage>> TryRegister(RegisterRequest registerRequest)
    {
        var testedUser = await _userRepository.GetUserByName(registerRequest.Username);
        bool isUserExists = testedUser is not null;

        if (isUserExists) return new InvalidRequestMessage("");

        string hashedPassword = Crypt.HashPassword(registerRequest.Password);

        User user = new User()
        {
            HashedPassword = hashedPassword,
            Name = registerRequest.Username,
        };

        Role role = new Role()
        {
            RoleType = registerRequest.Role,
        };

        UserRole userRole = new UserRole()
        {
            Role = role,
            User = user,
        };

        var lawyer = new Lawyer()
        {
            UserId = user.Id,
            FirstName = user.Name,
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        await _context.UserRoles.AddAsync(userRole);

        await _context.Lawyers.AddAsync(lawyer);
        await _context.SaveChangesAsync();

        var userDto = await _userRepository.MapUserDto(user.Id);
        return new RequestResult(RequestResultTypes.Success, JsonConvert.SerializeObject(userDto));
    }
}