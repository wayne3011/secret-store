﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecretStore.Domain.Interfaces.Repositories;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Domain.Models;
using SecretStore.Domain.Services.Options;

namespace SecretStore.Domain.Services;

public class TokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly ITokensRepository _tokensRepository;
    
    public TokenService(IOptions<JwtConfiguration> jwtConfiguration, ITokensRepository tokensRepository)
    {
        _tokensRepository = tokensRepository;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public async Task<Tokens> Issue(Guid userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.WriteToken(GenerateAccessToken(userId));
        var refreshToken = handler.WriteToken(GenerateRefreshToken(userId));

        var oldToken = await _tokensRepository.Get(userId);
        if (oldToken is not null)
        {
            var token = handler.ReadToken(oldToken.RefreshToken);
            if (token.ValidTo > DateTime.UtcNow)
            {
                throw new AlreadyAuthorizeException("The user is already logged in.");
            }
        }

        var guid = await _tokensRepository.Add(userId, refreshToken);
        return new Tokens()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = userId,
            Id = guid
        };
    }

    public async Task<Tokens> Reissue(string refreshToken)
    {
        var tokens = await _tokensRepository.Get(refreshToken);

        if (tokens is null)
        {
            throw new InvalidRefreshTokenException("Invalid refresh token!");
        }
        
        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.WriteToken(GenerateAccessToken(tokens.UserId));
        var newRefreshToken = handler.WriteToken(GenerateRefreshToken(tokens.UserId));

        var guid = await _tokensRepository.Update(tokens.UserId, refreshToken);

        return new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            UserId = tokens.UserId,
            Id = guid
        };
    }
    public async Task Delete(string refreshToken)
    {
        var tokens = await _tokensRepository.Get(refreshToken);
        if (tokens == null) throw new InvalidRefreshTokenException("Invalid refresh token!");
        await _tokensRepository.Delete(tokens.Id);
    }

    private JwtSecurityToken GenerateAccessToken(Guid userId)
    {
        return new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            expires: DateTime.UtcNow + _jwtConfiguration.AccessLifetime,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
                SecurityAlgorithms.HmacSha256),
            claims: new[]
            {
                new Claim("UserId", userId.ToString())
            });
    }

    private JwtSecurityToken GenerateRefreshToken(Guid userId)
    {
        return new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            expires: DateTime.UtcNow + _jwtConfiguration.RefreshLifetime,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
                SecurityAlgorithms.HmacSha256),
            claims: new[]
            {
                new Claim("UserId", userId.ToString())
            });
    }
}