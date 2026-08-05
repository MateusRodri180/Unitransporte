using Microsoft.AspNetCore.Mvc;
using UnitransporteWeb.Models;
using UnitransporteWeb.Services;

namespace UnitransporteWeb.Pages;

public class IndexModel : SecurePageModel
{
    public IndexModel(DataStore store) : base(store)
    {
    }

    public IActionResult OnGet()
    {
        var guard = RequireLogin();
        if (guard is not null) return guard;

        return RedirectToPage(CurrentUser!.Cargo == Cargo.Administrador ? "/Entregas" : "/Motorista");
    }
}
