using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Profile for mapping between GetUserResult and GetUserResponse
/// </summary>
public class GetUserResponseProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUserResponse
    /// </summary>
    public GetUserResponseProfile()
    {
        CreateMap<GetUserResult, GetUserResponse>();
    }
}