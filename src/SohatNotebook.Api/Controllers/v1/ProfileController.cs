using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.Dtos.Errors;
using SohatNotebook.Entities.Dtos.Generic;
using SohatNotebook.Entities.Dtos.Incoming.Profile;

namespace SohatNotebook.Api.Controllers.v1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController : BaseController
{
    public ProfileController(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager) : base(unitOfWork, userManager)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
        var result = new Result<User>();


        if (loggedInUser == null)
        {

            result.Error = new Error
            {
                Code = 400,
                Message = "User not found",
                Type = "Bad Request"
            };

            return BadRequest(result);
        }

        var identityId = new Guid(loggedInUser.Id);

        var profile = await _unitOfWork.Users.GetByIdentityId(identityId);

        if (profile == null)
        {
            result.Error = new Error
            {
                Code = 400,
                Message = "User not found",
                Type = "Bad Request"
            };

            return BadRequest(result);
        }

        result.Content = profile;

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto profile)
    {
        // If the model is valid
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid payload");
        }

        var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);

        if (loggedInUser == null)
        {
            return BadRequest("User not found");
        }

        var identityId = new Guid(loggedInUser.Id);

        var userProfile = await _unitOfWork.Users.GetByIdentityId(identityId);

        if (userProfile == null)
        {
            return BadRequest("User not found");
        }

        userProfile.Address = profile.Address;
        userProfile.Sex = profile.Sex;
        userProfile.MobileNumber = profile.MobileNumber;
        userProfile.Country = profile.Country;

        var isUpdated = await _unitOfWork.Users.UpdateUserProfile(userProfile);

        if (isUpdated)
        {
            await _unitOfWork.CompleteAsync();

            return Ok(userProfile);
        }


        return BadRequest("Something went wrong, please try again later"); 
    }
}
