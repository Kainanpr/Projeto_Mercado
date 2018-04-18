using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//OK
namespace MercadoETEC.model.domain
{
    public abstract class Pessoa
    {
        private int id;
        private string cpf;
        private string nome;
        private Endereco endereco;
        private List<Telefone> telefones = new List<Telefone>();
        private List<Carrinho> carrinhos = new List<Carrinho>();

        //Construtor sem argumentos
        public Pessoa() { }

        //Construtor com argumentos
        public Pessoa(string cpf, string nome, Endereco endereco)
        {
            this.cpf = cpf;
            this.nome = nome;
            this.endereco = endereco;
        }

        //Get e Set
        public int Id { get { return id; } set { id = value; } }
        public string Cpf { get { return cpf; } set { cpf = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public Endereco Endereco { get { return endereco; } set { endereco = value; } }
        public List<Telefone> Telefones { get { return telefones; } set { telefones = value; } }
        public List<Carrinho> Carrinhos { get { return carrinhos; } set { carrinhos = value; } }


    }//Fim da classe
}//Fim namespace
