using FargoApi.Services.Whatsapp;
using project_API.Repositories;
using project_API.Services.Auth;

namespace project_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReviewController: ControllerBase
{
        
    private readonly IAuthRepository _authRepository;
    private readonly AuthManger _authManger;
    private readonly TokenRepository _tokenRepository;

    private readonly appdbcontext _context;
    public ReviewController( IAuthRepository authRepository, appdbcontext context, AuthManger authManger, TokenRepository tokenRepository)
    {
        _authRepository = authRepository;
        _context = context;
        _authManger = authManger;
        _tokenRepository = tokenRepository;
    }
    
    [HttpPost("Add")]
    public async Task<IActionResult> AddReview([FromBody] LoginRequest request)
    {
        return Ok();
    }
    [HttpPost("Edit")]
    public async Task<IActionResult> EditReview([FromBody] LoginRequest request)
    {
        return Ok();
    }
    [HttpPost("Delete")]
    public async Task<IActionResult> DeleteReview([FromBody] LoginRequest request)
    {
        return Ok();
    }
    

    

    
    
}