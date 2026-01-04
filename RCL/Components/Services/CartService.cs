using System.Text.Json;
using RCL.Dtos;

namespace RCL.Components.Services;

public sealed class CartService(AuthStateService auth, IKeyValueStore store, RestService rest)
{
    private const string CartKey = "cart_v1";

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly List<CartItemDto> _items = new();
    public IReadOnlyList<CartItemDto> Items => _items;

    public event Action? Changed;

    public int TotalItems => _items.Sum(i => i.Quantity);
    public decimal TotalPrice => _items.Sum(i => i.UnitPrice * i.Quantity);

    public async Task InitializeAsync()
    {
        if (auth.IsSupplier)
        {
            _items.Clear();
            await store.RemoveAsync(CartKey);
            Changed?.Invoke();
            return;
        }

        var json = await store.GetAsync(CartKey);
        if (string.IsNullOrWhiteSpace(json))
            return;

        List<CartStoredLineDto>? stored;
        try
        {
            stored = JsonSerializer.Deserialize<List<CartStoredLineDto>>(json, JsonOpts);
        }
        catch
        {
            await store.RemoveAsync(CartKey);
            return;
        }

        if (stored is null || stored.Count == 0)
            return;

        var clean = stored
            .Where(x => x.Quantity > 0)
            .GroupBy(x => x.ProductId)
            .Select(g => new CartStoredLineDto { ProductId = g.Key, Quantity = g.Sum(v => v.Quantity) })
            .ToList();

        _items.Clear();

        foreach (var line in clean)
        {
            var p = await rest.GetProduct(line.ProductId);
            if (p is null) continue;

            var qty = Math.Min(line.Quantity, Math.Max(p.Stock, 0));
            if (qty <= 0) continue;

            _items.Add(new CartItemDto
            {
                ProductId = p.Id,
                Name = p.Name,
                UnitPrice = p.FinalPrice,
                Quantity = qty,
                ImageData = p.ImageData
            });
        }

        await PersistAsync();
        Changed?.Invoke();
    }

    public void Add(CartItemDto item, int quantity = 1)
    {
        if (auth.IsSupplier) return;
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

        _ = PersistAsync();
        Changed?.Invoke();
    }

    public void Remove(int productId)
    {
        _items.RemoveAll(i => i.ProductId == productId);
        _ = PersistAsync();
        Changed?.Invoke();
    }

    public void SetQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return;

        if (quantity <= 0) _items.Remove(item);
        else item.Quantity = quantity;

        _ = PersistAsync();
        Changed?.Invoke();
    }

    public void Clear()
    {
        _items.Clear();
        _ = store.RemoveAsync(CartKey);
        Changed?.Invoke();
    }

    private async Task PersistAsync()
    {
        var stored = _items
            .Where(i => i.Quantity > 0)
            .Select(i => new CartStoredLineDto { ProductId = i.ProductId, Quantity = i.Quantity })
            .ToList();

        if (stored.Count == 0)
        {
            await store.RemoveAsync(CartKey);
            return;
        }

        var json = JsonSerializer.Serialize(stored, JsonOpts);
        await store.SetAsync(CartKey, json);
    }
}
