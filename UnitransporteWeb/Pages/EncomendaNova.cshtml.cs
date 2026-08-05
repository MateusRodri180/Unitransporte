using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class EncomendaNovaModel : SecurePageModel
{
    public EncomendaNovaModel(DataStore store) : base(store)
    {
    }

    [BindProperty]
    public string ClienteNome { get; set; } = "";

    [BindProperty]
    public string ClienteTelefone { get; set; } = "";

    [BindProperty]
    public string PontoDeColeta { get; set; } = "";

    [BindProperty]
    public string PontoDeDestino { get; set; } = "";

    [BindProperty]
    public string Dimensoes { get; set; } = "";

    [BindProperty]
    public string Peso { get; set; } = "";

    [BindProperty]
    public string? MotoristaId { get; set; }

    [BindProperty]
    public string? VeiculoId { get; set; }

    [BindProperty]
    public string Observacoes { get; set; } = "";

    public string? Erro { get; set; }
    public bool Sucesso { get; set; }

    public List<Usuario> Motoristas => Store.Usuarios.Where(u => u.Cargo == Cargo.Motorista && u.Ativo).ToList();

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "nova";
        ViewData["CurrentUser"] = CurrentUser;
        return Page();
    }

    public IActionResult OnPost()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "nova";
        ViewData["CurrentUser"] = CurrentUser;

        var pesoOk = double.TryParse(Peso.Replace(',', '.'), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var pesoValor);

        if (string.IsNullOrWhiteSpace(ClienteNome) || string.IsNullOrWhiteSpace(PontoDeColeta) ||
            string.IsNullOrWhiteSpace(PontoDeDestino) || !pesoOk || pesoValor <= 0)
        {
            Erro = "Preencha cliente, pontos de coleta/destino e peso.";
            return Page();
        }

        var frete = Store.CalcularFrete(pesoValor, Dimensoes ?? "");

        Store.AddEncomenda(new Encomenda
        {
            ClienteNome = ClienteNome.Trim(),
            ClienteTelefone = (ClienteTelefone ?? "").Trim(),
            Observacoes = string.IsNullOrWhiteSpace(Observacoes) ? null : Observacoes.Trim(),
            Dimensoes = string.IsNullOrWhiteSpace(Dimensoes) ? "não informado" : Dimensoes.Trim(),
            Peso = pesoValor,
            ValorDoFrete = frete,
            PontoDeColeta = PontoDeColeta.Trim(),
            PontoDeDestino = PontoDeDestino.Trim(),
            MotoristaId = string.IsNullOrWhiteSpace(MotoristaId) ? null : MotoristaId,
            VeiculoId = string.IsNullOrWhiteSpace(VeiculoId) ? null : VeiculoId,
        });

        Sucesso = true;
        ClienteNome = ClienteTelefone = PontoDeColeta = PontoDeDestino = Dimensoes = Peso = Observacoes = "";
        MotoristaId = VeiculoId = null;

        return Page();
    }

    /// <summary>Endpoint chamado via fetch pelo JS para recalcular o frete em tempo real.</summary>
    public IActionResult OnGetCalcularFrete(string peso, string dimensoes)
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        var ok = double.TryParse((peso ?? "").Replace(',', '.'), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var pesoValor);

        var frete = ok ? Store.CalcularFrete(pesoValor, dimensoes ?? "") : 0m;
        return new JsonResult(new { frete = frete.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) });
    }
}
