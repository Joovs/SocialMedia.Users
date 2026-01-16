using MediatR;
using SocialMedia.Users.Application.Shared;
using SocialMedia.Users.Domain.Entities.UserEntity;
using SocialMedia.Users.Domain.Entities.UserEntity.Repositories;
using SocialMedia.Users.Domain.Exceptions;
using SocialMedia.Users.Domain.Services.JwtServices;

namespace SocialMedia.Users.Application.Users.Commands.UserLogin;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserCommandResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
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
        User user = await _userRepository.GetByEmailAsync(req.Email);

        // 401 - Invalid credentials or user does not exist
        if (user == null || user.Email != request.request.Email)
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
