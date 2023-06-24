using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Entities;

public partial class PontoColeta
{
    public int Id { get; set; }

    public string NomePonto { get; set; } = null!;

    public string? Ativo { get; set; }

    public int EnderecoId { get; set; }

    public virtual Endereco Endereco { get; set; } = null!;
}
