using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class UsuarioModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Ativo { get; set; } = null!;

    public int? PontoColetaId { get; set; }

    public virtual ICollection<DoacaoModel> Doacoes { get; set; } = new List<DoacaoModel>();
}
