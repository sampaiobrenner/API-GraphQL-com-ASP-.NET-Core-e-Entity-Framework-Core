using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIProdutos.Data;
using APIProdutos.Models.GraphQL;
using GraphQL.Types;

namespace APIProdutos.Queries
{
    public class EatMoreQuery : ObjectGraphType
    {
        public EatMoreQuery(ApplicationDbContext db)
        {
            Field<ListGraphType<ProdutoType>>(
                "produtos",
                arguments: new QueryArguments(new QueryArgument[]
                {
                    new QueryArgument<IdGraphType>{Name="id"},
                    new QueryArgument<StringGraphType>{Name="nome"}
                }),
                resolve: contexto =>
                {
                    var filtroNome = contexto.GetArgument<string>("nome");

                    var query = db.Produtos.AsQueryable();

                    if (!string.IsNullOrEmpty(filtroNome))
                        query = query.Where(x => x.Nome == filtroNome);

                    return query.ToList();
                });
        }
    }
}