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
    public int PendentesCount { get; set; }
    public int ColetadosCount { get; set; }
    public int ConcluidosCount { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Tab { get; set; } = "pendentes";

    public IActionResult OnGet(string? tab)
    {
        var guard = RequireLogin(Cargo.Motorista);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "motorista";
        ViewData["CurrentUser"] = CurrentUser;
        Tab = tab ?? "pendentes";

        var todas = Store.Encomendas
            .Where(e => e.MotoristaId == CurrentUser!.Id && e.StatusDaEntrega != StatusEncomenda.Cancelada)
            .ToList();

        PendentesCount = Math.Max(1, todas.Count(e => e.StatusDaEntrega == StatusEncomenda.Pendente));
        ColetadosCount = todas.Count(e => e.StatusDaEntrega == StatusEncomenda.Coletada);
        ConcluidosCount = todas.Count(e => e.StatusDaEntrega == StatusEncomenda.Entregue);

        MinhasEncomendas = Tab switch
        {
            "coletados" => todas.Where(e => e.StatusDaEntrega == StatusEncomenda.Coletada).ToList(),
            "concluidos" => todas.Where(e => e.StatusDaEntrega == StatusEncomenda.Entregue).ToList(),
            _ => todas.Where(e => e.StatusDaEntrega == StatusEncomenda.Pendente).ToList()
        };

        return Page();
    }
}