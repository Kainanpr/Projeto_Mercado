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

        /* Construtor com argumentos
         * (id necessario no construtor, pois esse campo no banco de dados não é auto_increment)*/
        public Telefone(int id, string numero)
        {
            this.id = id;
            this.numero = numero;
        }

        //Get e Set
        public int Id { get { return id; } set { id = value; } }
        public string Numero { get { return numero; } set { numero = value; } }
        

    }//Fim da classe
}//Fim namespace