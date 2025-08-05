using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ShowMoBud.DTO;

namespace ShowMoBud.Pages.Authentication
{
    public class RegisterBase : ComponentBase
    {

        [Inject] protected ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

        protected RegistrationDTO registration = new();
        protected string? errorMessage;

        protected async Task HandleRegister()
        {

            try
            {

                await LocalStorage.SetItemAsync("registration", registration);
                NavigationManager.NavigateTo("/id-verification");

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

        }

    }
}
