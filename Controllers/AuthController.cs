using System.Security.Claims;
using FargoApi.Services.Whatsapp;
using Microsoft.AspNetCore.Authorization;
using project_API.Repositories;
using project_API.Services.Auth;

namespace project_API.Controllers;

using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IAuthRepository _authRepository;
    private readonly AuthManger _authManger;
    private readonly TokenRepository _tokenRepository;
    private readonly WhatsAppManger _whatsAppManger;
    private readonly appdbcontext _context;
    public AuthController( IAuthRepository authRepository, WhatsAppManger whatsAppManger, appdbcontext context, AuthManger authManger, TokenRepository tokenRepository)
    {
        _authRepository = authRepository;
        _whatsAppManger = whatsAppManger;
        _context = context;
        _authManger = authManger;
        _tokenRepository = tokenRepository;
    }
    
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var loginSuccess = await _authRepository.Login(request.PhoneNumber, request.Code);
        if (!loginSuccess)
            return NotFound("Invalid phone number or code.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        var token = await _authManger.AuthorizeEmployee(user!);
        await _tokenRepository.Add(token);

        return Ok(new { Token = token });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
    /*  var checkNumber = await _whatsAppManger.IsWhatsappNumber(request.PhoneNumber);
      
        if (checkNumber == false)
            return BadRequest("Invalid phone number.");
        
        */
        var user = await _authRepository.Register(new User()
        {
            PhoneNumber = request.PhoneNumber,
            Name = request.Name,
            Email = request.Email,
       
        });
        if (user == false)
            return NotFound("User not found.");

        return Ok(user);
     
    }
    
    [HttpPost("SendVerificationCode")]
    public async Task<IActionResult> SendVerificationCode([FromBody] SendCodeRequest request)
    {

       var isThere =  await _authRepository.CheckUser(request.PhoneNumber);
        if (isThere == false)
            return NotFound("User not found.");

     /*   var checkNumber = await _whatsAppManger.IsWhatsappNumber(phoneNumber);
      
        if (checkNumber == false)
            return BadRequest("Invalid phone number.");
        */
        var code = await _authRepository.SendVerificationCode(request.PhoneNumber);
        if (code == false)
            return NotFound("User not found.");
        
        return Ok(code);
     
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var name = User.Identity.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new {
            name = name,
            role = role
        });
    }

    
    
 
    
    
}