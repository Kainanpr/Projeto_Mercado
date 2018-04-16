using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoETEC.model.domain
{
    public class Categoria
    {

        /**
         * 
         */
        public Categoria()
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
        private HashSet<Produto> produtos;

    }
}