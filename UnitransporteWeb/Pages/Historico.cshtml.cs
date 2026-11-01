using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class HistoricoModel : SecurePageModel
{
    public HistoricoModel(DataStore store) : base(store)
    {
    }

    public List<Encomenda> HistoricoList { get; set; } = new();

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "historico";
        ViewData["CurrentUser"] = CurrentUser;

        HistoricoList = Store.Encomendas
            .OrderByDescending(e => e.CriadaEm)
            .ToList();

        return Page();
    }

    public string NomeMotorista(string? id) => Store.Usuarios.FirstOrDefault(u => u.Id == id)?.NomeDaPessoa ?? "Motorista 1";

    public Minuta? MinutaDe(string encomendaId) => Store.Minutas.FirstOrDefault(m => m.EncomendaId == encomendaId);
}