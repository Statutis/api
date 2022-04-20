using Microsoft.AspNetCore.Mvc;
using Statutis.Core.Interfaces.Business;

namespace Statutis.API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : Controller
{

    public UserController(IAuthService authService)
    {
        
    }
    
}