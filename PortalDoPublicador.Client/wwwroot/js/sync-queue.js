// wwwroot/js/sync-queue.js

async function syncQueue(db) {
    try {
        // 1. Pega os dados locais
        const comandosPendentes = await dbLerTodos(db, 'SyncPushQueue');

        let ultimaSync = "2000-01-01T00:00:00Z";
        const configReq = await dbLerItem(db, 'Configuracoes', 'ultimaSync');
        if (configReq) ultimaSync = configReq.valor;

        // 2. Monta o Envelope aderente a SyncRequest
        const payloadEnvio = {
            ultimaSincronizacao: ultimaSync,
            dadosPush: comandosPendentes
        };

        // 3. Dispara a chamada ÚNICA
        const respostaHTTP = await fetch('/api/sync', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payloadEnvio)
        });

        if (!respostaHTTP.ok) {
            if (respostaHTTP.status === 409) {
                const erroConflito = await respostaHTTP.json();
                console.warn("Conflito detectado no servidor", erroConflito);
                
                // Avisa as abas abertas sobre o conflito
                if (self.clients) {
                    const clients = await self.clients.matchAll();
                    clients.forEach(client => {
                        client.postMessage({
                            type: 'CONFLITO_RESOLVIDO_PELO_SERVIDOR',
                            detalhes: erroConflito
                        });
                    });
                }
                
                await limparComandosProcessados(db, comandosPendentes.map(c => c.id));
            }
            return;
        }

        // 4. Desempacota a resposta
        const respostaServidor = await respostaHTTP.json();

        // A. Limpa da SyncPushQueue os itens com sucesso
        const resultadosPush = respostaServidor.resultadosPush || [];
        const idsSucesso = resultadosPush.filter(r => r.sucesso).map(r => r.payloadId);

        // Se o servidor não devolveu detalhamento mas retornou 200, assume que todos pendentes foram processados
        const idsParaLimpar = idsSucesso.length > 0 ? idsSucesso : comandosPendentes.map(c => c.id);
        if (idsParaLimpar.length > 0) {
            await limparComandosProcessados(db, idsParaLimpar);
        }

        // B. Salva cada delta de pull individualmente na SyncPullQueue
        const dadosPull = respostaServidor.dadosPull || [];
        for (const itemPull of dadosPull) {
            await dbSalvarItem(db, 'SyncPullQueue', itemPull);
        }

        // C. Atualiza a data com o relógio oficial do servidor
        if (respostaServidor.timestamp) {
            await dbSalvarItem(db, 'Configuracoes', {
                chave: 'ultimaSync',
                valor: respostaServidor.timestamp
            });
        }

    } catch (erro) {
        console.error('Erro na sincronização bidirecional:', erro);
        throw erro;
    }
}

async function limparComandosProcessados(db, ids) {
    for (const id of ids) {
        await new Promise((resolve) => {
            const req = db.transaction('SyncPushQueue', 'readwrite').objectStore('SyncPushQueue').delete(id);
            req.onsuccess = resolve;
            req.onerror = resolve; // Ignore se não existir
        });
    }
}