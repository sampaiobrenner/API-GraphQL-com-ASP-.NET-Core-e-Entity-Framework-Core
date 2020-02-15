using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace APIProdutos.Models.GraphQL
{
    public class ProdutoType : ObjectGraphType<Produto>
    {
        public ProdutoType()
        {
            Name = "Produto";

            Field(x => x.CodigoBarras).Description("Código de Barras");

            Field(x => x.Nome).Description("Nome do produto");

            Field(x => x.Preco).Description("Preço");
        }
    }
}