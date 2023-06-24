using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class Doacao
{
    public int Id { get; set; }

    public string TipoMovimento { get; set; } = null!;

    public DateOnly DataMovimento { get; set; }

    public int? Quantidade { get; set; }

    public int ProdutoId { get; set; }

    public int UsuarioId { get; set; }

    public virtual Produto Produto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
