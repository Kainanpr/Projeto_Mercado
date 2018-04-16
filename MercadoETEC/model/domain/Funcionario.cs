using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoETEC.model.domain
{
    public class Funcionario : Pessoa
    {

        /**
         * 
         */
        public Funcionario()
        {
        }

        /**
         * 
         */
        private String funcao;

        /**
         * 
         */
        private decimal salario;

        /**
         * 
         */
        private DateTime dataInicio;

        /**
         * 
         */
        private DateTime dataSaida;

    }
}