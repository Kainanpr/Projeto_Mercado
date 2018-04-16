using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//OK
namespace MercadoETEC.model.domain
{
    public class Cliente : Pessoa
    {
        private string email;

        //Construtor sem argumentos
        public Cliente() { }

        //Construtor com argumentos
        public Cliente(string cpf, string nome, Endereco endereco, string email) : base(cpf, nome, endereco)
        {
            this.email = email;
        }

        //Get e Set
        public string Email { get { return email; } set { email = value; } }

    }//Fim da classe
}//Fim namespace