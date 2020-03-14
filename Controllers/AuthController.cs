using JwtAuthenticationAndIdentityDemo.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationAndIdentityDemo.Controllers
{
    [ApiController]
    public class AuthController:ControllerBase
    {
        #region dependencies

        UserManager<CustomUser> _userMng;
        SignInManager<CustomUser> _signInMng;
        IConfiguration _configuration;
        DatabaseContext _context;
        
        #endregion
        public AuthController(

            UserManager<CustomUser> userM,
            SignInManager<CustomUser> signInM,
            IConfiguration configuration,
            DatabaseContext cont
            
            
            )
       
        {
            _userMng = userM;
            _signInMng = signInM;
            _configuration = configuration;
            _context = cont;
        }

        //end of Ctor
        
        [HttpPost("login")]

        public async Task<object> Login( LogInModel model)
        {

           // await Task.Delay(0);
            
            var appUser = _userMng.Users.SingleOrDefault(r => r.Email == model.Email);
            var res = _userMng.FindByEmailAsync(model.Email);
            var signIn = await _signInMng.PasswordSignInAsync(appUser, model.Password, false, false);

            if( signIn.Succeeded)
               {
                
                return "Signed In Succesfully";
            }

            return Unauthorized("Invalid LogIn Attempt. Check Details and try again!!!");

        }


        [HttpPost("register")]
        public async  Task<ActionResult>  Register(SignInModel model)

        {

            var newUser = new CustomUser
            {
                
                Email = model.Email,
                UserName = model.UserName
            };

            var res = await  _userMng.CreateAsync( newUser, model.Password );
            
            if (res.Succeeded)
            {
                //Create a Claim

               IdentityUserClaim<string> emailClaim = new IdentityUserClaim<string>
                {
                    ClaimType = "Email",
                    ClaimValue = newUser.Email,
                    UserId = newUser.Id
                };

                //Create a token
                IdentityUserToken<string> userToken = new IdentityUserToken<string>
                {
                    LoginProvider ="test",
                    UserId = newUser.Id,
                    Value = GenerateJwtToken(newUser.Email , newUser),
                    Name="RegistrationToken"
                };

                //TO DO Create a  default User Role

                _context.UserTokens.Add(userToken);
                _context.UserClaims.Add(emailClaim);

                await _context.SaveChangesAsync();
                
                return Created(newUser.Id,newUser);
            
            };
            
             return Unauthorized();
            
         }

        [HttpGet("signout")]
        public async Task<string> SignOut()
        {
            
            await _signInMng.SignOutAsync();

            return "sign out successful";
        }
            

         string GenerateJwtToken(string email, CustomUser user)
            {
                var claims = new List<Claim>
              
                {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

                var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(7);

                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtIssuer"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
            }
        
    
    
        //TO DO: implement signout


    
    
    }





    }

