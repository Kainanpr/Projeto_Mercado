using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//OK
namespace MercadoETEC.model.domain
{
    public class Telefone 
    {
        private int id;
        private string numero;

        //Construtor sem argumentos
        public Telefone() { }

        //Construtor com argumentos
        public Telefone(string numero)
        {
            this.numero = numero;
        }

        //Get e Set
        public int Id { get { return id; } set { id = value; } }
        public string Numero { get { return numero; } set { numero = value; } }
        

    }//Fim da classe
}//Fim namespace