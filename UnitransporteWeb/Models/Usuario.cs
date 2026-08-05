namespace UnitransporteWeb.Models;

public enum Cargo
{
    Administrador,
    Motorista,
}

public class Usuario
{
    public string Id { get; set; } = "";
    public string NomeDaPessoa { get; set; } = "";
    public string NomeDeUsuario { get; set; } = "";
    public string Email { get; set; } = "";
    public Cargo Cargo { get; set; }
    public bool Ativo { get; set; } = true;
}
