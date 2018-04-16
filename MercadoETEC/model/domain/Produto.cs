using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoETEC.model.domain
{
    public class Produto
    {

        /**
         * 
         */
        public Produto()
        {
        }

        /**
         * 
         */
        private int id;

        /**
         * 
         */
        private String nome;

        /**
         * 
         */
        private decimal preco;

        /**
         * 
         */
        private int quantidade;

        /**
         * 
         */
        private Categoria categoria;

        /**
         * 
         */
        private HashSet<Fornecedor> fornecedor;

        /**
         * 
         */
        private Carrinho carrinho;

    }
}