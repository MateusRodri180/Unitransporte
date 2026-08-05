using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class EntregasModel : SecurePageModel
{
    public EntregasModel(DataStore store) : base(store)
    {
    }

    public List<Encomenda> Pendentes { get; set; } = new();
    public int AguardandoColeta { get; set; }
    public int ACaminho { get; set; }
    public int MotoristasAtivos { get; set; }

    [BindProperty]
    public string CancelarId { get; set; } = "";

    [BindProperty]
    public string Motivo { get; set; } = "";

    public string? CancelandoId { get; set; }

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "entregas";
        ViewData["CurrentUser"] = CurrentUser;
        Carregar();
        return Page();
    }

    public IActionResult OnGetCancelar(string id)
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "entregas";
        ViewData["CurrentUser"] = CurrentUser;
        CancelandoId = id;
        Carregar();
        return Page();
    }

    public IActionResult OnPostCancelar()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        if (!string.IsNullOrWhiteSpace(Motivo))
        {
            Store.CancelarEncomenda(CancelarId, Motivo.Trim());
        }

        return RedirectToPage("/Entregas");
    }

    public string NomeMotorista(string? id) => Store.Usuarios.FirstOrDefault(u => u.Id == id)?.NomeDaPessoa ?? "—";

    private void Carregar()
    {
        Pendentes = Store.Encomendas
            .Where(e => e.StatusDaEntrega == StatusEncomenda.Pendente || e.StatusDaEntrega == StatusEncomenda.Coletada)
            .ToList();

        AguardandoColeta = Store.Encomendas.Count(e => e.StatusDaEntrega == StatusEncomenda.Pendente);
        ACaminho = Store.Encomendas.Count(e => e.StatusDaEntrega == StatusEncomenda.Coletada);
        MotoristasAtivos = Store.Usuarios.Count(u => u.Cargo == Cargo.Motorista && u.Ativo);
    }
}
