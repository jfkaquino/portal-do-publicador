using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace PortalDoPublicador.Client.Infrastructure.Auth;

public class SimpleAuthStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Tenta ler o token do localStorage
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                // Usuário não está logado
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Usuário logado
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "UsuarioLocal") }, "offline_auth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            // Fallback caso JS Interop falhe (ex: durante prerendering, embora WASM não tenha prerender por padrão)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyUserLogin(string token)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "UsuarioLocal") }, "offline_auth");
        var user = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}
