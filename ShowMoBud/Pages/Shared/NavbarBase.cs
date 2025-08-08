using Microsoft.AspNetCore.Components;

namespace ShowMoBud.Pages.Shared
{
    public class NavbarBase : ComponentBase
    {

        protected bool _mobileMenuOpen { get; set; }

        [Parameter] public EventCallback OnOpenRegister { get; set; }

        protected void OpenMenu() => _mobileMenuOpen = true;
        protected void CloseMenu() => _mobileMenuOpen = false;

        protected async Task TriggerRegister()
        {

            if (_mobileMenuOpen)
            {

                _mobileMenuOpen = false;

            }

            if (OnOpenRegister.HasDelegate)
            {
                await OnOpenRegister.InvokeAsync();
            }
        }
    }
}
