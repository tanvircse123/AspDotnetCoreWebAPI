using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CityInfo.API.Data;
using CityInfo.API.Entities;
using CityInfo.API.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private CityInfoContext _context;

        public AuthenticationController(IConfiguration configuration, CityInfoContext context)
        {
            _config = configuration;
            _context = context;
        }

        



        [HttpPost("Authenticate")]
        public ActionResult Authenticate(AuthenticationRequestBody req){
            var user = ValidateUserCred(req.UserName,req.Password);
            if(user == null){
                return Unauthorized();
            }
            // you get a user now createthe token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_config["Authentication:SecretKey"])
            );
            // create security string key from the appsettings.deveopment.json
            // now with an algrithm create a signingCredentials
            var signingcred = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            // create claim for 
            // claim is a key value pair dictionary type data structure that is used
            // for setting user propertise nothing more than that
            // you add claim and then you give this claim to create token
            // inside the token you will get all the value (when you decode it)
            var claimForToken = new List<Claim>();
            claimForToken.Add(new Claim("UserId",user.UserId.ToString()));
            claimForToken.Add(new Claim("Name",user.UserName.ToString())); 
            //claimForToken.Add(new Claim("UserId",user.UserId.ToString())); 
            //claimForToken.Add(new Claim("UserId",user.UserId.ToString()));

            var fromNow = DateTime.UtcNow;
            var UntillValid = DateTime.UtcNow.AddHours(24);

            var jwtSecurityToken = new JwtSecurityToken(
                _config["Authentication:Issuer"],
                _config["Authentication:Audience"],
                claimForToken,
                fromNow,
                UntillValid,
                signingcred);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);





        }

        //there will be a endpoint for creating user
        [HttpPost("createuser")]
        public async Task<ActionResult> CreateUser(CreateUserDto createUserDto){
            var user = await _context.Users.AnyAsync(s=>s.UserName == createUserDto.UserName);
            if(user){
                return BadRequest("User with this username already exists");
            }
            // new user
            // hash the password
            var sha256 = SHA256.Create();
            byte[] hashbytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(createUserDto.Password));
            var passwordhash = Convert.ToBase64String(hashbytes);
            var newUser = new User{
                UserName = createUserDto.UserName,
                Password = passwordhash,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                City = createUserDto.City
            };
            var nuser = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return Ok(nuser.Entity);
        }

        [NonAction]
        private User? ValidateUserCred(string username, string password)
        {
            /// first hash the password 
            var sha256 = SHA256.Create();
            byte[] hashbytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var passwordhash = Convert.ToBase64String(hashbytes);
            var user = _context.Users.FirstOrDefault(s => s.UserName == username && s.Password == passwordhash);
            if (user == null)
            {
                return null;
            }
            return user;

        }
    }
}