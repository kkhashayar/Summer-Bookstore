using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Summer_Bookstore.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    readonly IMapper _mapper;
    readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }
}
