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

                //Chama o metodo auxiliar para setar a Pessoa vindo do banco 
                ultimaPessoa = setPessoa(dr, ultimaPessoa);

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

        /* Pesquisa uma pessoa pelo seu ID */
        public Pessoa Read(int id)
        {
            //Recupera a instancia unica do banco de dados
            DataBase dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o endereco pesquisado de acordo com seu ID
            Pessoa pessoa = new Pessoa();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu id */
                string query = "SELECT id, cpf, nome, idEndereco FROM Pessoa WHERE id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Chama o metodo auxiliar para setar a Pessoa vindo do banco 
                pessoa = setPessoa(dr, pessoa);
            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }


            
            /* Chamar o metodo auxiliar para carregar os dados do 
            * endereço da pessoa de acordo com o id do endereço. 
            * Apenas chama o metodo se caso a pessoa possui um endereço */
            if(pessoa.Endereco != null)
                pessoa.Endereco = setEndereco(pessoa);
            

            /* Retorna a pessoa pesquisada de acordo com seu ID */
            return pessoa;
        }

        public void Update(Pessoa pessoa) { }
        public void Delete(int id) { }
        public List<Pessoa> ListAll() { return null; }
        public List<Pessoa> FindByName(string name) { return null; }

        //Metodo auxiliar para preencher o endereço com os dados do banco
        private Pessoa setPessoa(MySqlDataReader dr, Pessoa pessoa)
        {

            //Verifica se tem dados para ser lido
            if (dr.Read())
            {
                pessoa.Id = dr.GetInt32(0);
                pessoa.Cpf = dr.GetString(1);
                pessoa.Nome = dr.GetString(2);

                //Instancia um endereço para pessoa
                pessoa.Endereco = new Endereco();

                //Seta o id do endereço da pessoa de acordo com o dado da coluna idEndereco vindo do banco
                pessoa.Endereco.Id = dr.GetInt32(3);
            }
            else
            {
                /*Caso não tenha nenhum dado para ser lido irá lançar uma 
                 *exceção para ser recuperada posteriormente no controler */
                throw new Exception("Pessoa não encontrada");
            }
                
            return pessoa;
        }

        //Metodo auxiliar para preencher o endereço com os dados do banco
        private Endereco setEndereco(Pessoa pessoa)
        {
            //Variavel local responsavel por realizar as operações de CRUD no endereço no banco de dados
            EnderecoDAO enderecoDAO = new EnderecoDAO();

            //Seta o endereco da pessoa de acordo com o id de seu endereço(Seta nome da rua, cidade, estado etc)
            Endereco endereco = enderecoDAO.Read(pessoa.Endereco.Id);

            return endereco;
        }

    }//Fim da classe
}//Fim da interface