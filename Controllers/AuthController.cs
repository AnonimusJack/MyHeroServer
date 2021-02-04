using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyHeroServer.Data.DTOs;
using MyHeroServer.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyHeroServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<Trainer> userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Trainer> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] TrainerLoginForm loginForm)
        {
            var user = await userManager.FindByNameAsync(loginForm.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, loginForm.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddYears(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                log.Info($"Successfully logged {user.UserName} in!");
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            log.Error((user == null) ? "Error wrong username!" : $"Error wrong password for user: {user.UserName}");
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] TrainerRegistrationForm registrationForm)
        {
            var userExists = await userManager.FindByNameAsync(registrationForm.Username);
            if (userExists != null)
            {
                log.Error("Error! User already exists");
                return StatusCode(StatusCodes.Status500InternalServerError, new ServerResponse { Status = "Error", Message = "User already exists!" });
            }
            if (!await roleManager.RoleExistsAsync(TrainerRoles.Trainer))
                await roleManager.CreateAsync(new IdentityRole(TrainerRoles.Trainer));
            Trainer trainer = new Trainer()
            {
                Email = registrationForm.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registrationForm.Username
            };
            var result = await userManager.CreateAsync(trainer, registrationForm.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                    log.Error($"{error.Code}: {error.Description}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ServerResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }
            if (await roleManager.RoleExistsAsync(TrainerRoles.Trainer))
            {
                await userManager.AddToRoleAsync(trainer, TrainerRoles.Trainer);
            }
            log.Info("User registered Successfully");
            return Ok(new ServerResponse { Status = "Success", Message = "User created successfully!" });
        }
    }
}
