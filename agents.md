# Instruções para Agentes - Portal do Publicador

Este documento define as diretrizes arquiteturais e tecnológicas para agentes de IA que geram ou modificam código neste repositório. Siga estas regras estritamente para manter a consistência do projeto.

## 1. Stack Tecnológico e Arquitetura

* **Arquitetura Base:** Vertical Slices Architecture. Agrupe a UI, lógica de negócios, validação e persistência por funcionalidade/domínio. **NÃO** utilize camadas técnicas estritas (como o padrão N-Tier tradicional).
* **Frontend:** Blazor WebAssembly + Fluent UI Blazor v5.
* **Backend:** ASP.NET Core Web API.
* **Banco de Dados:** SQLite é utilizado em ambas as pontas.
  * Servidor: `ServerDbContext`.
  * Cliente (Navegador): `ClientDbContext` rodando sobre OPFS (Origin Private File System) nativo.
* **Buffer Assíncrono:** IndexedDB.
* **Mapeamento e Validação:** Mapster e FluentValidation (devem ficar centralizados no projeto `Shared`).

## 2. Acesso a Dados e Produtividade (Minimal Boilerplate)

* **Sem Repositórios:** **NÃO** utilize o padrão *Repository*. 
* **Acesso Direto:** Os componentes Blazor devem injetar instâncias de `DbContext` (via *Factory*) diretamente no *Code-Behind*. Execute LINQ e grave dados diretamente no SQLite local (OPFS).
* **Chaves Primárias:** Use EXCLUSIVAMENTE `GUID` (UUIDs) para todas as chaves primárias. Isso é mandatório para permitir a criação de registros *offline* sem depender de chaves geradas pelo servidor.
* **Regras Compartilhadas:** O `SharedDbContext` define as entidades. Execute as validações (FluentValidation) e conversões (Mapster) tanto no cliente (para feedback em tempo real) quanto no servidor (para integridade transacional).

## 3. Sincronização de Dados (Offline-First)

A aplicação é rigorosamente *offline-first*. Todo fluxo de dados deve obedecer o seguinte motor de sincronização:

* **Sincronização por Deltas:** O EF Core no Blazor deve rastrear e extrair *apenas* as propriedades modificadas. Envie apenas os deltas na sincronização.
* **Fila no IndexedDB:** Para evitar travar a UI e contornar o *lock* exclusivo da OPFS, o Blazor deve salvar os deltas no IndexedDB.
* **Envio Background:** Um *Service Worker* consome o IndexedDB e usa a **Background Sync API** para enviar à API (com fallback de worker padrão caso a API não seja suportada pelo navegador).
* **Idempotência no Servidor:** A API ASP.NET Core deve processar envios utilizando IDs de transação únicos para evitar corrupção por envios duplicados.

## 4. Sincronização Reversa e Atualizações da UI

* **Web Push Notifications:** Mudanças no servidor são notificadas à PWA via sinal *Push* assíncrono.
* **Recepção no Worker:** O *Service Worker* intercepta o push, faz download das mudanças da API e salva no IndexedDB silenciosamente em segundo plano.
* **Feedback ao Usuário e UI:** O *Worker* aciona a Notifications API do navegador informando o usuário, e envia um sinal ao Blazor via *BroadcastChannel*.
* **Atualização Final:** O Blazor, ao receber o sinal, lê os dados do IndexedDB, persiste no SQLite da OPFS e atualiza os componentes do Fluent UI na tela.

---

**Nota ao Agente:** Ao receber uma solicitação para criar uma nova funcionalidade, priorize criar toda a fatia (Vertical Slice) contendo: UI no Blazor, Entidades no `Shared`, persistência direta no cliente, rotina de deltas/IndexedDB, endpoints idempotentes no backend, e a sincronização via Worker, respeitando estas diretrizes e regras de estilo configuradas no ambiente.
