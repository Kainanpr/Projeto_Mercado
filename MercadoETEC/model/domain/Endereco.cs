using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//OK
namespace MercadoETEC.model.domain
{
    public class Endereco
    {
        private int id;
        private string rua;
        private int numero;
        private int cep;
        private string cidade;
        private string estado;

        //Construtor sem argumentos
        public Endereco() { }

        //Construtor com argumentos
        public Endereco(string rua, int numero, int cep, string cidade, string estado)
        {
            this.rua = rua;
            this.numero = numero;
            this.cep = cep;
            this.cidade = cidade;
            this.estado = estado;
        }

        //Get e Set
        public int Id { get { return id; } set { id = value; } }
        public string Rua { get { return rua; } set { rua = value; } }
        public int Numero { get { return numero; } set { numero = value; } }
        public int Cep { get { return cep; } set { cep = value; } }
        public string Cidade { get { return cidade; } set { cidade = value; } }
        public string Estado { get { return estado; } set { estado = value; } }

    }//Fim da classe
}//Fim namespace
