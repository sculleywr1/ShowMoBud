using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ShowMoBud.DTO;

namespace ShowMoBud.Pages.Authentication
{
    public class RegisterBase : ComponentBase
    {

        [Inject] protected ILocalStorageService LocalStorage { get; set; } = default;
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback<bool> OnCloseRegister { get; set; }
        [Parameter] public EventCallback<bool> OnShowIdVerification { get; set; }

        protected RegistrationDTO registration = new() { IdVerificationStatus = false };
        protected string? errorMessage;


        protected async Task HandleRegister()
        {

            try
            {
                await LocalStorage.SetItemAsync("registration", registration);

                await OnCloseRegister.InvokeAsync(true);

                if(OnShowIdVerification.HasDelegate)
                {
                    await OnShowIdVerification.InvokeAsync(true);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

        }


    }
}
