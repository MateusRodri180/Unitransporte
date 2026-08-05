using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class VeiculosModel : SecurePageModel
{
    public VeiculosModel(DataStore store) : base(store)
    {
    }

    [BindProperty]
    public string Marca { get; set; } = "";

    [BindProperty]
    public string Modelo { get; set; } = "";

    [BindProperty]
    public string Placa { get; set; } = "";

    [BindProperty]
    public string Cor { get; set; } = "";

    public string? Erro { get; set; }

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "veiculos";
        ViewData["CurrentUser"] = CurrentUser;
        return Page();
    }

    public IActionResult OnPost()
    {
        var guard = RequireLogin(Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "veiculos";
        ViewData["CurrentUser"] = CurrentUser;

        if (string.IsNullOrWhiteSpace(Modelo) || string.IsNullOrWhiteSpace(Placa) || string.IsNullOrWhiteSpace(Marca))
        {
            Erro = "Preencha ao menos modelo, marca e placa.";
            return Page();
        }

        if (Store.Veiculos.Any(v => string.Equals(v.Placa, Placa.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            Erro = "Já existe um veículo cadastrado com essa placa.";
            return Page();
        }

        Store.AddVeiculo(Modelo.Trim(), Placa.Trim().ToUpper(), Cor.Trim(), Marca.Trim());
        return RedirectToPage("/Veiculos");
    }
}
