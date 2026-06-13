using Api.DTOs;
using API.DTOs;
using API.DTOs.Identity;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, UserResponseDTO>();
            CreateMap<Tasks, TaskResponseDTO>();
        }
    }
}

