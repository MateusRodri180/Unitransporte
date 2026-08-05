using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class LoginModel : PageModel
{
    private readonly DataStore _store;

    public LoginModel(DataStore store)
    {
        _store = store;
    }

    [BindProperty]
    public string NomeDeUsuario { get; set; } = "";

    [BindProperty]
    public string Senha { get; set; } = "";

    public string? Erro { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        var usuario = _store.AutenticarUsuario(NomeDeUsuario, Senha, out var erro);
        if (usuario is null)
        {
            Erro = erro;
            return Page();
        }

        HttpContext.Session.SetString("UsuarioId", usuario.Id);
        return RedirectToPage("/Index");
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }
}
