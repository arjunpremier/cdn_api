using cdn_web.Models;
using Microsoft.AspNetCore.Mvc;

namespace cdn_web.Services
{
	public interface IUserService
	{
        Task<ActionResult<IEnumerable<User>>> GetUsers();
        Task<ActionResult<User>> GetUserById(int id);
        Task PostUser(User userDTO);
        Task<IActionResult> PutUser(int id, User userDTO);
        Task<IActionResult> DeleteUser(int id);
      }
}

