window.SincronizacaoOffline = {
    registrarSync: function () {
        if ('serviceWorker' in navigator && 'SyncManager' in window) {
            navigator.serviceWorker.ready.then(reg => {
                reg.sync.register('sync-queue');
            });
        } else {
            console.warn("Background Sync não suportado neste navegador. Tentando sincronização direta.");
            window.SincronizacaoOffline.dispararSync();
        }
    },
    dispararSync: async function () {
        if (typeof abrirBanco === 'function' && typeof syncQueue === 'function') {
            const db = await abrirBanco('MeuAppOfflineDb');
            await syncQueue(db);
            return true;
        }
        return false;
    }
};