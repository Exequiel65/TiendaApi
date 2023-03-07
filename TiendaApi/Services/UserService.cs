using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Negocio.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using TiendaApi.Dtos;
using TiendaApi.Helpers;

namespace TiendaApi.Services
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<Usuario> passwordHasher)
        {
            _jwt = jwt.Value;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var usuario = new Usuario
            {
                Nombres = registerDto.Nombres,
                ApellidoMaterno = registerDto.ApellidoMaterno,
                ApellidoPaterno = registerDto.ApellidoPaterno,
                Email = registerDto.Email,
                Username = registerDto.Username
            };

            usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);

            var usuarioExiste = _unitOfWork.Usuarios
                                            .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                                            .FirstOrDefault();

            if (usuarioExiste == null)
            {
                var rolPredeterminado = _unitOfWork.Roles
                                                .Find(u => u.Nombre == Autorizacion.rol_predeterminado.ToString())
                                                .FirstOrDefault();
                try
                {
                    usuario.Roles.Add(rolPredeterminado);
                    _unitOfWork.Usuarios.Add(usuario);
                    await _unitOfWork.SaveAsync();

                    return $"El usuario {registerDto.Username} ha sido registrado exitosamente";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }

            }
            else
            {
                return $"El usuario con {registerDto.Username} ya se encuentra registrado.";
            }
        }

        public async Task<DatosUsuarioDto> GetTokenAsync(LoginDto model)
        {
            DatosUsuarioDto datosUsuariosDto = new DatosUsuarioDto();
            var usuario = await _unitOfWork.Usuarios
                        .GetByUserNameAsync(model.Username);

            if (usuario == null)
            {
                datosUsuariosDto.EstaAutenticado = false;
                datosUsuariosDto.Mensaje = $"No existe ningún usuario con el username {model.Username}.";
                return datosUsuariosDto;

            }
            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

            if (resultado == PasswordVerificationResult.Success)
            {
                datosUsuariosDto.EstaAutenticado = true;
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                datosUsuariosDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                datosUsuariosDto.Email = usuario.Email;
                datosUsuariosDto.UserName = usuario.Username;
                datosUsuariosDto.Roles = usuario.Roles
                                            .Select(u => u.Nombre)
                                            .ToList();

                return datosUsuariosDto;
            }
            datosUsuariosDto.EstaAutenticado = false;
            datosUsuariosDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Username}";
            return datosUsuariosDto;
        }

        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
            var usuario = await _unitOfWork.Usuarios.GetByUserNameAsync(model.Username);
            if (usuario == null)
            {
                return $"No existe algún usuario registrado con la cuenta {model.Username}";
            }
            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);
            if (resultado == PasswordVerificationResult.Success)
            {
                var rolExiste = _unitOfWork.Roles
                                            .Find(u => u.Nombre.ToLower() == model.Role.ToLower())
                                            .FirstOrDefault();
                if (rolExiste != null)
                {
                    var usuarioTieneRol = usuario.Roles.Any(u => u.Id == rolExiste.Id);
                    if (usuarioTieneRol == false)
                    {
                        usuario.Roles.Add(rolExiste);
                        _unitOfWork.Usuarios.Update(usuario);
                        await _unitOfWork.SaveAsync();
                    }
                    return $"Rol {model.Role} agregado a la cuenta {model.Username} de forma exitosa.";
                }
                return $"Rol {model.Role} no encontrado";
            }
            return $"Credenciales incorrectas para el usuario {usuario.Username}";
        }

        public JwtSecurityToken CreateJwtToken (Usuario usuario)
        {
            var roles = usuario.Roles;
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.Nombre));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("uid", usuario.Id.ToString())
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
                );
            return jwtSecurityToken;
        }

    }

   
}
