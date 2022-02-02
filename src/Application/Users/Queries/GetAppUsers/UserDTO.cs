﻿using AutoMapper;
using Core.Entities;
using static Application.Common.Mappings.MappingProfile;

namespace Application.Users.Queries.GetAppUsers;

public class UserDTO : IMapFrom<ApplicationUser>
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string UserRole { get; set; }
    public bool IsActive { get; set; } = true;
    public string PhoneNumber { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationUser, UserDTO>()
            .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Username, opt => opt.MapFrom(s => s.UserName));
    }
}