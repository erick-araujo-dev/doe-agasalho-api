using DoeAgasalhoApiV2._0.Context;
using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Repositories
{
    public class PontoColetaRepository : IPontoColetaRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public PontoColetaRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        public PontoColeta GetById(int id)
        {
            return _context.PontoColeta.FirstOrDefault(p => p.Id == id);
        }

        public List<NovoPontoColetaModel> GetAll()
        {
            return _context.PontoColeta.
                Include(u => u.Endereco).
                Select(p => new NovoPontoColetaModel
            {
                NomePonto = p.NomePonto,
                Logradouro = p.Endereco.Logradouro,
                Numero = p.Endereco.Numero,
                Complemento = p.Endereco.Complemento,
                Bairro = p.Endereco.Bairro,
                Cidade = p.Endereco.Cidade,
                Estado = p.Endereco.Estado,
                Cep = p.Endereco.Cep

            }).ToList();
        }

        public PontoColeta Add(NovoPontoColetaModel novoPontoColeta)
        {
            var pontoColeta = new PontoColeta
            {
                NomePonto = novoPontoColeta.NomePonto,
                Ativo = "1"
            };

            string complemento = string.IsNullOrWhiteSpace(novoPontoColeta.Complemento) ? null : novoPontoColeta.Complemento;

            var endereco = new Endereco
            {
                Logradouro = novoPontoColeta.Logradouro,
                Numero = novoPontoColeta.Numero,
                Complemento = complemento,
                Bairro = novoPontoColeta.Bairro,
                Cidade = novoPontoColeta.Cidade,
                Estado = novoPontoColeta.Estado,
                Cep = novoPontoColeta.Cep
            };
            pontoColeta.Endereco = endereco;

            _context.PontoColeta.Add(pontoColeta);
            _context.SaveChanges();

            return pontoColeta;
        }

        public void Update(PontoColeta pontoColeta)
        {
            _context.PontoColeta.Update(pontoColeta);
            _context.SaveChanges();
        }

        public void ActivateCollectPoint(int id)
        {
            var pontoColeta = _context.PontoColeta.FirstOrDefault(p => p.Id == id);
            if (pontoColeta != null)
            {
                pontoColeta.Ativo = "1";
                _context.SaveChanges();
            }
        }

        public void DeactivateCollectPoint(int id)
        {
            var pontoColeta = _context.PontoColeta.FirstOrDefault(p => p.Id == id);
            if (pontoColeta != null)
            {
                pontoColeta.Ativo = "0";
                _context.SaveChanges();
            }
        }
    }
}
