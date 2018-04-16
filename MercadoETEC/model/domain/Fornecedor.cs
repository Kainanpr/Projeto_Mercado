using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoETEC.model.domain
{
    public class Fornecedor
    {

        /**
         * 
         */
        public Fornecedor()
        {
        }

        /**
         * 
         */
        private int id;

        /**
         * 
         */
        private String cnpj;

        /**
         * 
         */
        private String nome;

        /**
         * 
         */
        private String email;

        /**
         * 
         */
        private HashSet<Produto> produtos;

        /**
         * 
         */
        private HashSet<Endereco> enderecos;

        /**
         * 
         */
        private HashSet<Telefone> telefones;

    }
}