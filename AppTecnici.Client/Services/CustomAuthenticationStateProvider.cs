using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AppTecnici.Client.Services
{
    // Provider di autenticazione personalizzato per gestire lo stato di login/logout in Blazor WebAssembly
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        // Servizio per la gestione del SessionStorage (mantiene la sessione attiva fino alla chiusura del tab/browser)
        private readonly ISessionStorageService _sessionStorage;

        // Iniezione delle dipendenze per l'accesso allo storage di sessione
        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        // Determina se l'utente corrente è autenticato recuperando lo stato salvato nel SessionStorage
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Verifica la presenza del flag di autenticazione e del nome utente
                var isAuthenticated = await _sessionStorage.GetItemAsync<bool>("is_authenticated");
                var username = await _sessionStorage.GetItemAsync<string>("auth_username");

                if (isAuthenticated && !string.IsNullOrEmpty(username))
                {
                    // Crea le credenziali (ClaimsIdentity) dell'utente autenticato
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }, "CustomAuth");

                    var user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }
            }
            catch
            {
                // Gestione di sicurezza in caso di errori di lettura dal SessionStorage
            }

            // Restituisce uno stato anonimo (utente non autenticato) se la sessione non è valida o assente
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Salva le credenziali nel SessionStorage e notifica l'app che l'utente ha effettuato l'accesso
        public async Task MarkUserAsAuthenticated(string username)
        {
            await _sessionStorage.SetItemAsync("is_authenticated", true);
            await _sessionStorage.SetItemAsync("auth_username", username);

            // Crea l'identità autenticata con il nome utente specificato
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }, "CustomAuth");

            var user = new ClaimsPrincipal(identity);

            // Aggiorna in tempo reale la UI tramite il sistema di notifiche di Blazor
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Rimuove i dati dal SessionStorage e notifica l'app che l'utente ha effettuato il logout
        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorage.RemoveItemAsync("is_authenticated");
            await _sessionStorage.RemoveItemAsync("auth_username");

            // Crea uno stato anonimo vuoto
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

            // Aggiorna la UI ridirezionando l'utente verso le componenti riservate agli utenti anonimi
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}