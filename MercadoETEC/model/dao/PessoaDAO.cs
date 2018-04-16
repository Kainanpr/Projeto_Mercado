using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.dao.interfaceDAO;
using MercadoETEC.model.conexao;
using MercadoETEC.model.domain;

using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MercadoETEC.model.dao
{
    //Implementa a interface IPessoaDAO, sendo assim, é obrigado a implementar seus métodos abstratos
    class PessoaDAO : IPessoaDAO
    {

        /* Método responsável pela iserção da pessoa no bd.
         * Esse método retorna um objeto do tipo Pessoa que representa a ultima Pessoa
         * inserido no banco, para que essa Pessoa
         * possa ser associado a um funcionado ou a um cliente*/
        public Pessoa Create(Pessoa pessoa) 
        {
            //Recupera a instancia unica do banco de dados
            DataBase dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar a ultima pessoa inserida na tabela Pessoa.
            Pessoa ultimaPessoa = new Pessoa();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Utilizamos uma Stored Procedure para retornar a ultima pessoa inserida na tabela Pessoa. 
                 * Essa procedure foi feita pelo fato de o id ser auto_increment, sendo assim, sem o uso de procedure
                 * não teriamos o controle de qual id foi inserido por ultimo. */
                string query = "CALL sp_pessoa_insert (@Cpf, @Nome, @idEndereco)";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Cpf", MySqlDbType.String);
                command.Parameters.Add("@Nome", MySqlDbType.String);
                command.Parameters.Add("@idEndereco", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Cpf"].Value = pessoa.Cpf;
                command.Parameters["@Nome"].Value = pessoa.Nome;
                command.Parameters["@idEndereco"].Value = pessoa.Endereco.Id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Percorrer esse objeto até obter todos os dados
                while (dr.Read())
                {
                    ultimaPessoa.Id = dr.GetInt32(0);
                    ultimaPessoa.Cpf = dr.GetString(1);
                    ultimaPessoa.Nome = dr.GetString(2);
                }

                //MessageBox.Show("Conexão com banco de dados efetuada com sucesso");
            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                //Independente se der erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna a ultima pessoa inserido na tabela Pessoa 
             * ppossa ser associado a um funcionado ou a um cliente
             * (Esse retorno se torna necessario para controlar os ids da tabela, pois é um campo auto_increment) */
            return ultimaPessoa;
        }

        public Pessoa Read(int id) { return null; }
        public void Update(Pessoa pessoa) { }
        public void Delete(int id) { }
        public List<Pessoa> ListAll() { return null; }
        public List<Pessoa> FindByName(string name) { return null; }

    }//Fim da classe
}//Fim da interface