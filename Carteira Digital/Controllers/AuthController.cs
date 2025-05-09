using MediatR;
using Microsoft.AspNetCore.Mvc;
using DigitalWallet.Infrastructure.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly TokenService _tokenService;

    public AuthController(IMediator mediator, TokenService tokenService)
    {
        _mediator = mediator;
        _tokenService = tokenService;
    }

    /// Registra um novo usuário no sistema
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var command = new RegisterCommand
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Password = registerDto.Password
        };

        var user = await _mediator.Send(command);

        return Ok(new UserDto(user.Id, user.Username, user.Email));
    }

    /// Autentica um usuário e retorna um token JWT
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto loginDto)
    {
        var command = new LoginCommand
        {
            Username = loginDto.Username,
            Password = loginDto.Password
        };

        var user = await _mediator.Send(command);
        var token = _tokenService.CreateToken(user);

        return Ok(new AuthResponse(token, user.Username));
    }
}