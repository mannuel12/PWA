using Microsoft.JSInterop;

namespace PWA.Services;

public class IndexedDbService
{
    private readonly IJSRuntime _jsRuntime;

    public IndexedDbService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<List<T>> GetAllAsync<T>(string storeName)
    {
        return await _jsRuntime.InvokeAsync<List<T>>("indexedDbHelper.getAll", storeName)
               ?? new List<T>();
    }

    public async Task PutAsync<T>(string storeName, T item)
    {
        await _jsRuntime.InvokeVoidAsync("indexedDbHelper.put", storeName, item);
    }

    public async Task DeleteAsync(string storeName, string key)
    {
        await _jsRuntime.InvokeVoidAsync("indexedDbHelper.delete", storeName, key);
    }
}