using RCL.Dtos;

namespace RCL.Services;

public class CartService(AuthService authService, ApiService apiService)
{
    // Local cart storage for non-logged-in users
    private readonly List<CartDto> _localCart = [];

    // Add a product to the cart
    public async Task AddToCart(string userId, int productId, int quantity)
    {
        Console.WriteLine(userId);
        if (authService.IsAuthenticated())
        {
            // If user is logged in, send directly to backend
            var dto = new CartDto
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };
            var accessToken = authService.GetTokens().accessToken;
            if (accessToken != null)
                await apiService.AddToCartAsync(dto, accessToken);
        }
        else
        {
            // If not logged in, store in local memory
            _localCart.Add(new CartDto
            {
                UserId = userId, // This can be empty or a placeholder since the user is not logged in yet
                ProductId = productId,
                Quantity = quantity
            });
        }
    }

    // Sync local cart with backend after login
    public async Task SyncCartAfterLogin()
    {
        if (_localCart.Any())
        {
            foreach (var cartItem in _localCart)
            {
                // Update each product with the logged-in user ID
                cartItem.UserId = authService.User.Id; 
                await apiService.AddToCartAsync(cartItem, authService.GetTokens().accessToken);
            }
            _localCart.Clear(); // Clear the local cart after syncing
        }
    }

    // Retrieve local cart for display
    public IEnumerable<CartDto> GetLocalCart()
    {
        return _localCart;
    }
}