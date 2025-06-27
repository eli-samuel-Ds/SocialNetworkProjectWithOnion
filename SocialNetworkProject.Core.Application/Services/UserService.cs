using AutoMapper;
using SocialNetworkProject.Core.Application.Dtos.ApplicationUser;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Domain.Entities;
using SocialNetworkProject.Core.Domain.Interfaces.Generic;

namespace SocialNetworkProject.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<ApplicationUser> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(UserDto dto)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            await _userRepository.AddAsync(user);
        }
    }
}
