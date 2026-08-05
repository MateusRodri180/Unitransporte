using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class MotoristaModel : SecurePageModel
{
    public MotoristaModel(DataStore store) : base(store)
    {
    }

    public List<Encomenda> MinhasEncomendas { get; set; } = new();

    [BindProperty]
    public string EncomendaId { get; set; } = "";

    [BindProperty]
    public string NomeRecebedor { get; set; } = "";

    public string? EntregandoId { get; set; }

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Cargo.Motorista);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "motorista";
        ViewData["CurrentUser"] = CurrentUser;
        Carregar();
        return Page();
    }

    public IActionResult OnGetEntregar(string id)
    {
        var guard = RequireLogin(Cargo.Motorista);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "motorista";
        ViewData["CurrentUser"] = CurrentUser;
        EntregandoId = id;
        Carregar();
        return Page();
    }

    public IActionResult OnPostColetar(string id)
    {
        var guard = RequireLogin(Cargo.Motorista);
        if (guard is not null) return guard;

        Store.ColetarEncomenda(id);
        return RedirectToPage("/Motorista");
    }

    public IActionResult OnPostFinalizarEntrega()
    {
        var guard = RequireLogin(Cargo.Motorista);
        if (guard is not null) return guard;

        if (!string.IsNullOrWhiteSpace(NomeRecebedor))
        {
            Store.EntregarEncomenda(EncomendaId, NomeRecebedor.Trim());
        }

        return RedirectToPage("/Motorista");
    }

    private void Carregar()
    {
        MinhasEncomendas = Store.Encomendas
            .Where(e => e.MotoristaId == CurrentUser!.Id && e.StatusDaEntrega != StatusEncomenda.Cancelada)
            .OrderBy(e => e.StatusDaEntrega == StatusEncomenda.Coletada ? 0 : 1)
            .ToList();
    }
}
