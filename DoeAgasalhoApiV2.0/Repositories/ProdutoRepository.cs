using DoeAgasalhoApiV2._0.Data.Context;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace DoeAgasalhoApiV2._0.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DbDoeagasalhov2Context _context;

        public ProdutoRepository(DbDoeagasalhov2Context context)
        {
            _context = context;
        }

        public void ActivateProduct(int productId)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Ativo = "1";
                _context.SaveChanges();
            }
        }

        public ProdutoModel Add(ProdutoModel product)
        {
            var newProduct = new ProdutoModel
            {
                TipoId = product.TipoId,
                TamanhoId = product.TamanhoId,
                Genero = product.Genero,
                Caracteristica = product.Caracteristica,
                Ativo = "1",
                Estoque = 0
            };

            _context.Produtos.Add(newProduct);
            _context.SaveChanges();

            return newProduct;
        }

        public void DeactivateProduct(int productId)
        {
            var product = _context.Produtos.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Ativo = "0";
                _context.SaveChanges();
            }
        }

        public List<ProdutoModel> GetAll()
        {
            return _context.Produtos.ToList();
        }

        public ProdutoModel GetSingleByFilter(Func<ProdutoModel, bool> filter)
        {
            return _context.Produtos.FirstOrDefault(filter);
        }

        public List<ProdutoModel> GetAllByFilter(Func<ProdutoModel, bool> filter)
        {
            return _context.Produtos.Where(filter).ToList();
        }

        public List<ProdutoModel> GetFilteredProducts(int? tipoId, int? tamanhoId, string genero, string caracteristica)
        {
            var query = _context.Produtos.AsQueryable();

            if (tipoId.HasValue)
            {
                query = query.Where(p => p.TipoId == tipoId.Value);
            }

            if (tamanhoId.HasValue)
            {
                query = query.Where(p => p.TamanhoId == tamanhoId.Value);
            }

            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(p => p.Genero == genero);
            }

            if (!string.IsNullOrEmpty(caracteristica))
            {
                query = query.Where(p => p.Caracteristica == caracteristica);
            }

            return query.ToList();
        }

        public List<string> GetCharacteristicsByFilter(int tipoId, int tamanhoId)
        {
            var characteristics = _context.Produtos
                .Where(p => p.TipoId == tipoId && p.TamanhoId == tamanhoId)
                .Select(p => p.Caracteristica)
                .Distinct()
                .ToList();

            return characteristics;
        }

        public List<ProdutoModel> GetProductActive()
        {
            return _context.Produtos.Where(p => p.Ativo == "1").ToList();
        }

        public List<ProdutoModel> GetProductInactive()
        {
            return _context.Produtos.Where(p => p.Ativo == "0").ToList();
        }

        public void Update(ProdutoModel product)
        {
            _context.Produtos.Update(product);
            _context.SaveChanges();
        }
    }
}
