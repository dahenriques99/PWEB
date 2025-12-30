namespace RCL.Services;

public abstract class LoginStateService(CartService cartService)
{
    public static bool IsLoggedIn { get; private set; }

    // Event to notify the components when the login state changes
    public static event Action? OnLoginStateChanged;

    // Method to log in
    public async Task LogIn()
    {
        IsLoggedIn = true;
        await cartService.SyncCartAfterLogin(); // Sync cart after login
        NotifyStateChanged();
    }

    // Method to log out
    public static void LogOut()
    {
        IsLoggedIn = false;
        NotifyStateChanged();
    }

    // Notify any listeners (e.g., Navbar component) that the state has changed
    private static void NotifyStateChanged() => OnLoginStateChanged?.Invoke();
}