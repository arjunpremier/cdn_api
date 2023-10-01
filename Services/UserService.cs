using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using cdn_web.Models;


namespace cdn_web.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _userContext;

        public UserService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userContext.Users
                .Select(user => MapUser(user))
                .ToListAsync();
        }

        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userContext.Users.FindAsync(id);

            if (user == null)
                return new NotFoundResult();

            return MapUser(user);
        }

        public async Task PostUser(User userDTO)
        {
            var user = new User
            {
                IsComplete = userDTO.IsComplete,
                UserName = userDTO.UserName,
                Mail = userDTO.Mail,
                PhoneNumber = userDTO.PhoneNumber,
                SkillSets = userDTO.SkillSets,
                Hobby = userDTO.Hobby
            };

            _userContext.Users.Add(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task<IActionResult> PutUser(int id, User userDTO)
        {
            if (id != userDTO.Id)
            {
                return new BadRequestResult();
            }

            var user = await _userContext.Users.FindAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            user.UserName = userDTO.UserName;
            user.Mail = userDTO.Mail;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.SkillSets = userDTO.SkillSets;
            user.Hobby = userDTO.Hobby;
            user.IsComplete = userDTO.IsComplete;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                return new NotFoundResult();
            }

            return new NoContentResult();
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userContext.Users.FindAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();

            return new NoContentResult();
        }
        
        private bool UserExists(int id)
        {
            return _userContext.Users.Any(e => e.Id == id);
        }

        private static User MapUser(User user) =>
       new()
       {
           Id = user.Id,
           UserName = user.UserName,
           Mail = user.Mail,
           PhoneNumber = user.PhoneNumber,
           SkillSets = user.SkillSets,
           Hobby = user.Hobby,
           IsComplete = user.IsComplete
       };
    }
}

