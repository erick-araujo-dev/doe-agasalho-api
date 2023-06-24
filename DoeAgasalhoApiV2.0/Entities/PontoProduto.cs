using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Entities;

public partial class PontoProduto
{
    public int PontoColetaId { get; set; }

    public int ProdutoId { get; set; }

    public virtual Produto Produto { get; set; } = null!;
}
