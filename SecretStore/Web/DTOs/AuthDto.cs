﻿namespace SecretStore.Web.DTOs;

public class AuthDto
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}