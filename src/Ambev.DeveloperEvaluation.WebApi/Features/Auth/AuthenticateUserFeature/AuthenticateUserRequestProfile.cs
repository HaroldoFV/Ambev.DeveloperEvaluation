using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Profile for mapping between AuthenticateUserRequest and AuthenticateUserCommand
/// </summary>
public class AuthenticateUserRequestProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for AuthenticateUserRequest
    /// </summary>
    public AuthenticateUserRequestProfile()
    {
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();
    }
}