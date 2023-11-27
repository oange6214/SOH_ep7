using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.Configuration.Messages;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.Dtos.Generic;
using SohatNotebook.Entities.Dtos.Incoming;

namespace SohatNotebook.Api.Controllers.v1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : BaseController
{
    public UsersController(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager) : base(unitOfWork, userManager)
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
    public async Task<IActionResult> AddUser(UserDto userDto)
    {
        var user = new User()
        {
            LastName = userDto.LastName,
            FirstName = userDto.FirstName,
            Email = userDto.Email,
            DateOfBirth = Convert.ToDateTime(userDto.DateOfBirth),
            Phone = userDto.Phone,
            Country = userDto.Country,
            Status = 1
        };
        
        await _unitOfWork.Users.Add(user);
        await _unitOfWork.CompleteAsync();

        return CreatedAtRoute("GetUser", new { id = user.Id }, user); // return a 201
    }

    // Get
    [HttpGet]
    [Route("GetUser", Name = "GetUser")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _unitOfWork.Users.GetById(id);

        var result = new Result<User>();

        if (user != null)
        {
            result.Content = user;

            return Ok(result);
        }

        result.Error = PopulateError(404,
            ErrorMessages.Generic.ObjectNotFound,
            ErrorMessages.Generic.UnableToProcess);

        return BadRequest(result);
    }

}