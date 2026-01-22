using MediatR;
using SocialMedia.Users.Application.Repositories;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Exceptions;
using SocialMedia.Users.Domain.Services.JwtServices;
using SocialMedia.Users.Domain.Services.PasswordHashes;

namespace SocialMedia.Users.Application.Users.Commands.UserLogin;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserCommandResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtService jwtService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        LoginUserCommandRequest req = request.request;
        // 400 - Missing required data
        if (req == null || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
        {
            throw new InvalidRequestDataException("Faltan datos en la petición");
        }

        // Search for user
        User user = await _userRepository.GetByEmailAsync(req.Email, cancellationToken);

        // 401 - Invalid credentials or user does not exist
        if (user == null || user.Email != req.Email)
        {
            throw new InvalidCredentialsException("Credenciales inválidas");
        }

        // Verify password against stored hash
        if (!_passwordHasher.VerifyPassword(req.Password, user.Password))
        {
            throw new InvalidCredentialsException("Credenciales inválidas");
        }

        // 200 - OK 
        LoginUserCommandResponse response = new LoginUserCommandResponse
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.Lastname,
            Email = user.Email,
            Token = _jwtService.GenerateToken(user.Email, user.Id),
            Message = "Autenticación exitosa",
            HttpStatus = 200
        };

        return Result<LoginUserCommandResponse>.Success(response, "200");
    }
}