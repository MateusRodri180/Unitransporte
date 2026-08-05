using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public abstract class SecurePageModel : PageModel
{
    public readonly DataStore Store;

    protected SecurePageModel(DataStore store)
    {
        Store = store;
    }

    public Usuario? CurrentUser =>
        Store.Usuarios.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("UsuarioId"));

    /// <summary>Chame no início de cada OnGet/OnPost. Retorna um IActionResult de redirecionamento
    /// quando o usuário não está logado ou não tem o cargo exigido; caso contrário, null.</summary>
    protected IActionResult? RequireLogin(Cargo? somenteCargo = null)
    {
        var user = CurrentUser;
        if (user is null) return RedirectToPage("/Login");

        if (somenteCargo.HasValue && user.Cargo != somenteCargo.Value)
        {
            return RedirectToPage(user.Cargo == Cargo.Administrador ? "/Entregas" : "/Motorista");
        }

        return null;
    }
}
