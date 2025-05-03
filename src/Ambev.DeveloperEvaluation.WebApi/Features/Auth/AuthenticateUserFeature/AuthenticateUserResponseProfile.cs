using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Profile for mapping between AuthenticateUserResult and AuthenticateUserResponse
/// </summary>
public class AuthenticateUserResponseProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for AuthenticateUserResult
    /// </summary>
    public AuthenticateUserResponseProfile()
    {
        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();
    }
}