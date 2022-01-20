using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.UserAdministration;
using Microsoft.AspNetCore.Identity;
using Blazored.LocalStorage;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using System.Linq;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Application.Services.Multimedia;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    /**
     * Class used to manage the authentication of the users to the page.
     */
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;

        protected readonly SignInManager<Usuario> SignInManager;

        protected readonly UserManager<Usuario> UserManager;

        private readonly IAuthenticationService authenticationService;

        private readonly JWTKeyModel keyModel;


        public CustomAuthenticationStateProvider(
            SignInManager<Usuario> _signInManager,
            UserManager<Usuario> _userManager,
            ILocalStorageService _localStorageService,
            IAuthenticationService _authenticationService,
            IOptions<JWTKeyModel> _jwtKeyModel)
        {
            SignInManager = _signInManager;
            UserManager = _userManager;
            localStorageService = _localStorageService;
            authenticationService = _authenticationService;
            keyModel = _jwtKeyModel.Value;
        }

        /*
         * Function:    Used to set the claims to the ClaimsIdentity of the user received in the parameters
         * 
         * Requieres:   The model of the user to which the ClaimsIdentity would be configured
         * Returns:     The ClaimsIdentity of the user received in the parameters
         */
        private ClaimsIdentity GetClaimIdentity(Usuario user, List<Perfil> profilesAndPermissions)
        {
            List<Claim> claimsAuthentication = new List<Claim>();

            var profiles = user.UsuariosYPerfiles;
            var permissionsList = new List<Permiso>();
            if (profiles != null && profiles.Count != 0)
            {
                foreach (var profile in profiles)
                {
                    var permissionsOfProfile = profilesAndPermissions?.Find(p => p.NombrePerfil == profile.IDPerfil)?.PerfilesYPermisos;
                    if (permissionsOfProfile != null && permissionsOfProfile.Count > 0)
                    {
                        foreach (var permission in permissionsOfProfile)
                        {
                            permissionsList.Add(permission.Permiso);
                        }
                    }
                }
            }

            claimsAuthentication.Add(new Claim(ClaimTypes.Name, user.Email));

            foreach (var permission in Enum.GetValues(typeof(AuthorizationPermissions)).Cast<AuthorizationPermissions>())
            {
                claimsAuthentication.Add(new Claim(permission.ToString(), permissionsList.Exists(p => p.IDPermiso == (int)permission) ? "true" : "false"));
            }
            return new ClaimsIdentity(claimsAuthentication.ToList(), "apiauth_type");
        }

        /*
         * Function:    Used to get the state of the user register in the page if any.
         * 
         * Returns:     An AuthenticationState with the info of the register user if any.
         */
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            var token = await localStorageService.GetItemAsync<string>("token");
            if (token != null)
            {
                var list = ValidateJwtToken(token);
                if (list != null)
                {
                    var claimsList = new List<Claim>();
                    var email = list.First(x => x.Type == "unique_name").Value;
                    claimsList.Add(new Claim(ClaimTypes.Name, email));
                    foreach (Claim currentClaim in list)
                    { 
                        if (currentClaim.Type != "nbf" && currentClaim.Type != "exp" && currentClaim.Type != "iat")
                        {
                            claimsList.Add(currentClaim);
                        }
                    }
                    identity = new ClaimsIdentity(claimsList.ToList(), "apiauth_type");
                    var newTokenGenerated = GenerateJwtToken(identity);
                    await localStorageService.SetItemAsync<string>("token", newTokenGenerated);
                }
            }

            var user = new ClaimsPrincipal(identity);

            //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            return await Task.FromResult(new AuthenticationState(user));
        }

        /*
         * Function:    Used to mark an user as authenticated if the password and email are valids.
         * 
         * Requieres:   A DTO with the information filled in the form of the authentication.
         * Returns:     A boolean indicating if the authentication succeeded.
         */
        public async Task<bool> AuthenticateLogin(LogInModel logInInfo)
        {
            var userToCheck = await UserManager.FindByEmailAsync(logInInfo.Correo);

            ClaimsIdentity identity;

            if (userToCheck != null)
            {
                var loginResult = await SignInManager.CheckPasswordSignInAsync(userToCheck, logInInfo.Contraseña, false);

                if (loginResult.Succeeded)
                {

                    var profilesAndPermissions = await authenticationService.GetAllProfilesWithDetailsAsync();
                    userToCheck = await authenticationService.GetUserWithDetailsAsync(userToCheck.Id);
                    identity = GetClaimIdentity(userToCheck, profilesAndPermissions);
                    var token = GenerateJwtToken(identity);
                    await localStorageService.SetItemAsync<string>("token", token);
                }
                else
                {
                    return await Task.FromResult(false);
                }
            }
            else
            {
                return await Task.FromResult(false);
            }


            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            return await Task.FromResult(true);
        }

        /*
         * Funtion:     Used to mark the actual user of the page to logout.
         * 
         * Returns:     A bool indicating if the operation succeeded.
         */
        public async Task<bool> Logout()
        {
            await localStorageService.RemoveItemAsync("token");

            ClaimsIdentity identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);


            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            return await Task.FromResult(true);
        }

        public string GenerateJwtToken(ClaimsIdentity identity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keyModel.JWT_Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public List<Claim> ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keyModel.JWT_Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var tokenToReturn = jwtToken.Claims.ToList();

                return tokenToReturn;
            }
            catch
            {
                return null;
            }
        }
    }
}