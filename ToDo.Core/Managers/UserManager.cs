using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDo.Common.Exceptions;
using ToDo.Core.Managers.Interfaces;
using ToDo.DbModel.Models;
using ToDo.ModelViews.ModelView;

namespace ToDo.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly tododbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserManager(tododbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        #region public

        public LoginRespones Login(LoginRequest userLogin)
        {
            var dbUser = _context.Users
                            .FirstOrDefault(x => x.Email.ToLower().Equals(userLogin.Email.ToLower()))
                            ?? throw new ServiceValidationException("Invalid email or password");
            if(!VerifyHashPassword(userLogin.Password, dbUser.Password))
            {
                throw new ServiceValidationException("Invalid email or password");
            }
            var reslut = _mapper.Map<LoginRespones>(dbUser);
            reslut.Token = $"Bearer {GenerateJWTToken(dbUser)}";
            return reslut;
        }

        public LoginRespones SignUp(UserRegistration userRegistration)
        {

            var dbUser = _context.Users
                            .FirstOrDefault(x => x.Email.ToLower().Equals(userRegistration.Email.ToLower()));

            if (dbUser != null)
            {
                throw new ServiceValidationException("User alreadly exict");
            }

            var hashedPassword = HashPassword(userRegistration.Password);
            dbUser = _context.Users.Add(new User
            {
                Email = userRegistration.Email,
                Password = hashedPassword,
                ConfirmPassword = hashedPassword,
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,

            }).Entity;
            var result = _mapper.Map<LoginRespones>(dbUser);    
            result.Token = $"Bearer {GenerateJWTToken(dbUser)}";
            _context.SaveChanges();
            return result;
        }

        public UserModel UpdateInfoUser(UserModel currentUser, UserModel userModel)
        {
            var dbUser = _context.Users
                            .FirstOrDefault(x => x.Id == currentUser.Id)
                            ?? throw new ServiceValidationException("User not found");

            dbUser.FirstName = userModel.FirstName;
            dbUser.LastName = userModel.LastName;
            dbUser.Email = userModel.Email;
            dbUser.IsAdmin = userModel.IsAdmin;

            _context.SaveChanges();
            return _mapper.Map<UserModel>(dbUser);
            
        }

        public void Delete(UserModel currentUser, int id)
        {
            if (currentUser.Id == id)
                throw new ServiceValidationException(401, "You have no access to delete your self");

            if (!currentUser.IsAdmin) 
                throw new ServiceValidationException(401, "You have no access to delete any user");
            
            var dbUser = _context.Users
                .FirstOrDefault(x => x.Id == id)
                ?? throw new ServiceValidationException("User not found");
            dbUser.Archived = true;
            _context.SaveChanges();
            

        }

        #endregion public

        #region private

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private static bool VerifyHashPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        
        private string GenerateJWTToken(User user)
        {
            var jwtKey = _configuration["Jwt:KeySecret"];
            var secuirtyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(secuirtyKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName}  {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("DateOfJoining", user.CreatedDate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var issuer = _configuration["Jwt:Isseur"];
            var audience = _configuration["Jwt:Isseur"];
            var token = new JwtSecurityToken(issuer, audience, claims,
                            expires: DateTime.Now.AddDays(12), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion private
    }
}
