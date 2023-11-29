using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.Api.Profiles;
using SohatNotebook.Configuration.Messages;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.Dtos.Generic;
using SohatNotebook.Entities.Dtos.Incoming;
using SohatNotebook.Entities.Dtos.Outgoing.Profile;

namespace SohatNotebook.Api.Controllers.v1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : BaseController
{
    public UsersController(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager) : base(mapper, unitOfWork, userManager)
    {
    }

    // Get all
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _unitOfWork.Users.All();
        var result = new PagedResult<User>
        {
            Content = users.ToList(),
            ResultCount = users.Count()
        };
        return Ok(result);
    }

    // Post
    [HttpPost]
    public async Task<IActionResult> AddUser(UserDto user)
    {
        var _mappedUser = _mapper.Map<User>(user);


        await _unitOfWork.Users.Add(_mappedUser);
        await _unitOfWork.CompleteAsync();

        var result = new Result<UserDto>
        {
            Content = user
        };

        return CreatedAtRoute("GetUser", new { id = _mappedUser.Id }, result); // return a 201
    }

    // Get
    [HttpGet]
    [Route("GetUser", Name = "GetUser")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _unitOfWork.Users.GetById(id);

        var result = new Result<ProfileDto>();

        if (user != null)
        {
            var mappedProfile = _mapper.Map<ProfileDto>(user);

            result.Content = mappedProfile;

            return Ok(result);
        }

        result.Error = PopulateError(404,
            ErrorMessages.Generic.ObjectNotFound,
            ErrorMessages.Generic.UnableToProcess);

        return BadRequest(result);
    } 

}