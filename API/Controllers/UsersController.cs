using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<MemberDto>> GetUsers([FromQuery] UserParams userParams)
        {
            string username = User.GetUsername();
            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(
                currentPage: users.CurrentPage,
                itemsPerPage: users.PageSize,
                totalItems: users.TotalCount,
                totalPages: users.TotalPages
            );

            return users;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<MemberDto> GetUser(int id)
        {
            return await _userRepository.GetMemberAsync(id);
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<MemberDto> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }
    }
}