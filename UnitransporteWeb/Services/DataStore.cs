using UnitransporteWeb.Models;

namespace UnitransporteWeb.Services;

public class DataStore
{
    private int _seq = 100;
    private readonly object _lock = new();

    public List<Usuario> Usuarios { get; } = new();
    public List<Veiculo> Veiculos { get; } = new();
    public List<Encomenda> Encomendas { get; } = new();
    public List<Minuta> Minutas { get; } = new();

    public DataStore()
    {
        Usuarios.AddRange(new[]
        {
            new Usuario { Id = "u1", NomeDaPessoa = "Ezequias Cavalcante", NomeDeUsuario = "ezequias", Email = "ezequias@unitransporte.com.br", Cargo = Cargo.Administrador, Ativo = true },
            new Usuario { Id = "u2", NomeDaPessoa = "Carlos Andrade", NomeDeUsuario = "carlos.a", Email = "carlos@unitransporte.com.br", Cargo = Cargo.Motorista, Ativo = true },
            new Usuario { Id = "u3", NomeDaPessoa = "Marcos Vinícius", NomeDeUsuario = "marcos.v", Email = "marcos@unitransporte.com.br", Cargo = Cargo.Motorista, Ativo = true },
            new Usuario { Id = "u4", NomeDaPessoa = "Renata Souza", NomeDeUsuario = "renata.s", Email = "renata@unitransporte.com.br", Cargo = Cargo.Motorista, Ativo = false },
        });

        Veiculos.AddRange(new[]
        {
            new Veiculo { Id = "v1", Modelo = "Fiorino", Placa = "FZR-4A21", Cor = "Branco", Marca = "Fiat" },
            new Veiculo { Id = "v2", Modelo = "Saveiro", Placa = "QTX-1B09", Cor = "Prata", Marca = "Volkswagen" },
            new Veiculo { Id = "v3", Modelo = "HR", Placa = "BWK-9C77", Cor = "Branco", Marca = "Hyundai" },
        });

        Encomendas.AddRange(new[]
        {
            new Encomenda
            {
                Id = "e1", ClienteNome = "Loja Ferreira Materiais", ClienteTelefone = "(17) 99111-2233",
                Observacoes = "Retirar no depósito dos fundos", Dimensoes = "40x30x20cm", Peso = 6.5, ValorDoFrete = 38m,
                PontoDeColeta = "Votuporanga - Centro", PontoDeDestino = "Santa Fé do Sul - Zona Norte",
                StatusDaEntrega = StatusEncomenda.Pendente, MotoristaId = "u2", VeiculoId = "v1",
                CriadaEm = new DateTime(2026, 8, 3, 9, 0, 0),
            },
            new Encomenda
            {
                Id = "e2", ClienteNome = "Farmácia Vida Nova", ClienteTelefone = "(17) 99222-3344",
                Dimensoes = "20x15x10cm", Peso = 1.2, ValorDoFrete = 22m,
                PontoDeColeta = "Votuporanga - Vila Regina", PontoDeDestino = "São José do Rio Preto - Centro",
                StatusDaEntrega = StatusEncomenda.Coletada, MotoristaId = "u3", VeiculoId = "v2",
                CriadaEm = new DateTime(2026, 8, 3, 10, 30, 0),
            },
            new Encomenda
            {
                Id = "e3", ClienteNome = "Auto Peças Bandeirantes", ClienteTelefone = "(17) 99333-4455",
                Dimensoes = "60x40x40cm", Peso = 14, ValorDoFrete = 65m,
                PontoDeColeta = "Votuporanga - Distrito Industrial", PontoDeDestino = "Fernandópolis - Centro",
                StatusDaEntrega = StatusEncomenda.Entregue, MotoristaId = "u2", VeiculoId = "v1",
                CriadaEm = new DateTime(2026, 8, 2, 8, 15, 0),
            },
            new Encomenda
            {
                Id = "e4", ClienteNome = "Ótica Enxergar Bem", ClienteTelefone = "(17) 99444-5566",
                Dimensoes = "15x10x8cm", Peso = 0.4, ValorDoFrete = 18m,
                PontoDeColeta = "Votuporanga - Centro", PontoDeDestino = "Jales - Centro",
                StatusDaEntrega = StatusEncomenda.Cancelada, MotivoCancelamento = "Cliente desistiu da compra",
                CriadaEm = new DateTime(2026, 8, 1, 14, 20, 0),
            },
        });

        Minutas.Add(new Minuta
        {
            Id = "m1", NumeroDaMinuta = 1042, Data = new DateTime(2026, 8, 2, 16, 40, 0),
            NomeDoMotorista = "Carlos Andrade", NomeDoRecebedor = "José Bandeirantes", EncomendaId = "e3",
        });
    }

    private string NextId(string prefixo)
    {
        lock (_lock)
        {
            return $"{prefixo}{_seq++}";
        }
    }

    public Usuario AutenticarUsuario(string nomeDeUsuario, string senha, out string? erro)
    {
        erro = null;
        if (string.IsNullOrWhiteSpace(senha))
        {
            erro = "Informe a senha.";
            return null!;
        }

        if (string.IsNullOrWhiteSpace(nomeDeUsuario))
        {
            erro = "Informe o usuário.";
            return null!;
        }

        var usuario = Usuarios.FirstOrDefault(u =>
            string.Equals(u.NomeDeUsuario, nomeDeUsuario.Trim(), StringComparison.OrdinalIgnoreCase));

        if (usuario is null)
        {
            erro = "Usuário não encontrado.";
            return null!;
        }

        if (!usuario.Ativo)
        {
            erro = "Este usuário está desativado.";
            return null!;
        }

        return usuario;
    }

    public Usuario AddUsuario(string nomeDaPessoa, string nomeDeUsuario, string email, Cargo cargo)
    {
        var usuario = new Usuario
        {
            Id = NextId("u"),
            NomeDaPessoa = nomeDaPessoa,
            NomeDeUsuario = nomeDeUsuario,
            Email = email,
            Cargo = cargo,
            Ativo = true,
        };
        Usuarios.Add(usuario);
        return usuario;
    }

    public void ToggleUsuarioAtivo(string id)
    {
        var usuario = Usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario is not null) usuario.Ativo = !usuario.Ativo;
    }

    public Veiculo AddVeiculo(string modelo, string placa, string cor, string marca)
    {
        var veiculo = new Veiculo { Id = NextId("v"), Modelo = modelo, Placa = placa, Cor = cor, Marca = marca };
        Veiculos.Add(veiculo);
        return veiculo;
    }

    public Encomenda AddEncomenda(Encomenda dados)
    {
        dados.Id = NextId("e");
        dados.StatusDaEntrega = StatusEncomenda.Pendente;
        dados.CriadaEm = DateTime.Now;
        Encomendas.Add(dados);
        return dados;
    }

    public void CancelarEncomenda(string id, string motivo)
    {
        var encomenda = Encomendas.FirstOrDefault(e => e.Id == id);
        if (encomenda is null) return;
        encomenda.StatusDaEntrega = StatusEncomenda.Cancelada;
        encomenda.MotivoCancelamento = motivo;
    }

    public void ColetarEncomenda(string id)
    {
        var encomenda = Encomendas.FirstOrDefault(e => e.Id == id);
        if (encomenda is not null) encomenda.StatusDaEntrega = StatusEncomenda.Coletada;
    }

    public void EntregarEncomenda(string id, string nomeDoRecebedor)
    {
        var encomenda = Encomendas.FirstOrDefault(e => e.Id == id);
        if (encomenda is null) return;

        encomenda.StatusDaEntrega = StatusEncomenda.Entregue;
        var motorista = Usuarios.FirstOrDefault(u => u.Id == encomenda.MotoristaId);
        var maiorNumero = Minutas.Count > 0 ? Minutas.Max(m => m.NumeroDaMinuta) : 1000;

        Minutas.Add(new Minuta
        {
            Id = NextId("m"),
            NumeroDaMinuta = maiorNumero + 1,
            Data = DateTime.Now,
            NomeDoMotorista = motorista?.NomeDaPessoa ?? "Motorista",
            NomeDoRecebedor = nomeDoRecebedor,
            EncomendaId = id,
        });
    }

    public decimal CalcularFrete(double peso, string dimensoes)
    {
        if (peso <= 0) return 0m;

        const decimal taxaBase = 12m;
        const decimal taxaPorKg = 2.8m;
        const decimal adicionalVolumeGrande = 15m;

        var valor = taxaBase + (decimal)peso * taxaPorKg;

        var match = System.Text.RegularExpressions.Regex.Match(
            dimensoes ?? "", @"(\d+(?:[.,]\d+)?)\s*x\s*(\d+(?:[.,]\d+)?)\s*x\s*(\d+(?:[.,]\d+)?)",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var lados = new[] { match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value }
                .Select(s => double.Parse(s.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture));
            if (lados.Any(l => l > 50)) valor += adicionalVolumeGrande;
        }

        return Math.Round(valor, 2);
    }
}
