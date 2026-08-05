namespace UnitransporteWeb.Models;

public enum StatusEncomenda
{
    Pendente,
    Coletada,
    Entregue,
    Cancelada,
}

public class Encomenda
{
    public string Id { get; set; } = "";
    public string ClienteNome { get; set; } = "";
    public string ClienteTelefone { get; set; } = "";
    public string? Observacoes { get; set; }
    public string Dimensoes { get; set; } = "";
    public double Peso { get; set; }
    public decimal ValorDoFrete { get; set; }
    public string PontoDeColeta { get; set; } = "";
    public string PontoDeDestino { get; set; } = "";
    public StatusEncomenda StatusDaEntrega { get; set; } = StatusEncomenda.Pendente;
    public string? MotivoCancelamento { get; set; }
    public string? MotoristaId { get; set; }
    public string? VeiculoId { get; set; }
    public DateTime CriadaEm { get; set; } = DateTime.Now;
}
