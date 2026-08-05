using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class UsuariosModel : SecurePageModel
{
    public UsuariosModel(DataStore store) : base(store)
    {
    }

    [BindProperty]
    public string NomeDaPessoa { get; set; } = "";

    [BindProperty]
    public string NomeDeUsuario { get; set; } = "";

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public Cargo Cargo { get; set; } = Cargo.Motorista;

    public string? Erro { get; set; }

    public IActionResult OnGet()
    {
        var guard = RequireLogin(Models.Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "usuarios";
        ViewData["CurrentUser"] = CurrentUser;
        return Page();
    }

    public IActionResult OnPost()
    {
        var guard = RequireLogin(Models.Cargo.Administrador);
        if (guard is not null) return guard;

        ViewData["ActivePage"] = "usuarios";
        ViewData["CurrentUser"] = CurrentUser;

        if (string.IsNullOrWhiteSpace(NomeDaPessoa) || string.IsNullOrWhiteSpace(NomeDeUsuario) || string.IsNullOrWhiteSpace(Email))
        {
            Erro = "Preencha nome, usuário e e-mail.";
            return Page();
        }

        if (Store.Usuarios.Any(u => string.Equals(u.NomeDeUsuario, NomeDeUsuario.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            Erro = "Já existe um usuário com esse nome de usuário.";
            return Page();
        }

        Store.AddUsuario(NomeDaPessoa.Trim(), NomeDeUsuario.Trim(), Email.Trim(), Cargo);
        return RedirectToPage("/Usuarios");
    }

    public IActionResult OnPostToggle(string id)
    {
        var guard = RequireLogin(Models.Cargo.Administrador);
        if (guard is not null) return guard;

        Store.ToggleUsuarioAtivo(id);
        return RedirectToPage("/Usuarios");
    }
}
