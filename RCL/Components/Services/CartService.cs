using Microsoft.AspNetCore.Components.ProtectedBrowserStorage;
using RCL.Dtos;

namespace RCL.Components.Services;

public sealed class CartService(ProtectedSessionStorage storage)
{
    private const string CartKey = "cart";

    private readonly List<CartItemDto> _items = new();

    public IReadOnlyList<CartItemDto> Items => _items;

    public event Action? Changed;

    public int TotalItems => _items.Sum(i => i.Quantity);
    public decimal TotalPrice => _items.Sum(i => i.UnitPrice * i.Quantity);

    public async Task InitializeAsync()
    {
        var result = await storage.GetAsync<string>(CartKey);
        Changed?.Invoke();
    }
    
    public void Add(CartItemDto item, int quantity = 1)
    {
        if (quantity <= 0) return;

        var existing = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing is not null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            item.Quantity = quantity;
            _items.Add(item);
        }

        Changed?.Invoke();
    }

    public void Remove(int productId)
    {
        _items.RemoveAll(i => i.ProductId == productId);
        Changed?.Invoke();
    }

    public void SetQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return;

        if (quantity <= 0) _items.Remove(item);
        else item.Quantity = quantity;

        Changed?.Invoke();
    }

    public void Clear()
    {
        _items.Clear();
        Changed?.Invoke();
    }
}