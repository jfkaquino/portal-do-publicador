using Microsoft.JSInterop;

namespace PortalDoPublicador.Client.Infrastructure.Sync;

public class SyncService(IJSRuntime jsRuntime, PullProcessor pullProcessor)
{
    private bool _estaSincronizando;

    public bool EstaSincronizando => _estaSincronizando;

    public event Action? OnSincronizacaoConcluida;
    public event Action? OnEstadoAlterado;

    public async Task<bool> SincronizarAsync()
    {
        if (_estaSincronizando) return false;

        try
        {
            _estaSincronizando = true;
            OnEstadoAlterado?.Invoke();

            // 1. Dispara o envio de Push (e recebimento do Pull no IndexedDB) via JS
            await jsRuntime.InvokeVoidAsync("SincronizacaoOffline.dispararSync");

            // 2. Processa a fila de Pull recebida no IndexedDB e persiste no SQLite local (OPFS)
            await pullProcessor.ProcessarFilaPullAsync();

            OnSincronizacaoConcluida?.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[SyncService] Erro ao sincronizar: {ex.Message}");
            return false;
        }
        finally
        {
            _estaSincronizando = false;
            OnEstadoAlterado?.Invoke();
        }
    }
}
