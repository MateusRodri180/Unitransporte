using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class HistoricoModel : SecurePageModel
{
    public HistoricoModel(DataStore store) : base(store)
    {
    }

    public List<Encomenda> Concluidas { get; set; } = new();

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "historico";
        ViewData["CurrentUser"] = CurrentUser;

        Concluidas = Store.Encomendas
            .Where(e => e.StatusDaEntrega == StatusEncomenda.Entregue || e.StatusDaEntrega == StatusEncomenda.Cancelada)
            .OrderByDescending(e => e.CriadaEm)
            .ToList();

        return Page();
    }

    public Minuta? MinutaDe(string encomendaId) => Store.Minutas.FirstOrDefault(m => m.EncomendaId == encomendaId);
}
