using API_Project.DTO;
using API_Project.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<APPUser> userManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<APPUser> _signInManager;

        public UserController(UserManager<APPUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, SignInManager<APPUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Registertion(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                APPUser user = new APPUser();
                user.UserName = registerDTO.UserName;
                user.Email = registerDTO.Email;
                user.PhoneNumber = registerDTO.PhoneNumber;
                IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password);
                if (result.Succeeded)
                {
                    return Ok("USer Added Successfully");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid == true)
            {
                APPUser user = await userManager.FindByNameAsync(loginDTO.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                    if (found)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item));
                        }
                        SecurityKey securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        SigningCredentials signingCred = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signingCred
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo
                        });
                    }
                }

            }
            return Unauthorized();
        }
        [HttpPost("CreateRole")]
        [Authorize("Admin")]

        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("role Already Exist");
            }
            IdentityRole role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("role created successfully");
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("User Logged Out");
        }
        [HttpPost("assignrole")]
        [Authorize("Admin")]
        public async Task<IActionResult> Asignrole([FromBody] AssignRoleDTO assignRoleDto)
        {
            var user = await userManager.FindByIdAsync(assignRoleDto.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!await _roleManager.RoleExistsAsync(assignRoleDto.Role))
            {
                return NotFound("Role not found");
            }

            var result = await userManager.AddToRoleAsync(user, assignRoleDto.Role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Role assigned successfully");
        }
        [HttpPost("AddAdmin")]
        [Authorize("Admin")]

        public async Task<IActionResult> AddAdmin([FromBody] string userid)
        {
            var u= await userManager.FindByIdAsync(userid);
            if(u== null)
            {
                return NotFound("User With THis id not found");
            }
            var isAdmin = await userManager.IsInRoleAsync(u, "Admin");
            if (isAdmin)
            {
                return BadRequest("User is already an Admin.");
            }

            var result = await userManager.AddToRoleAsync(u, "Admin");

            if (result.Succeeded)
            {
                return Ok($"User '{u.UserName}' has been assigned the Admin role.");
            }
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new { Errors = errors });
        }
    }
}
