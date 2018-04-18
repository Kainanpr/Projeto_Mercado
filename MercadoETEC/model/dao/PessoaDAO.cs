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

        //Atributo responsavel por realizara conexão com o banco de dados
        private DataBase dataBase;

        /* Método responsável pela iserção da pessoa no bd.
         * Esse método retorna um objeto de tipo Generico(qualquer tipo que extenda pessoa) que representa a ultima Pessoa
         * inserido no banco, para que essa Pessoa
         * possa ser associado a um funcionado ou a um cliente*/
        public T Create<T> (T pessoa) where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar a ultima pessoa inserida na tabela Pessoa. 
             * (A palavra new so é usada em tipos concretos) 
             * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
            T ultimaPessoa = Activator.CreateInstance<T>();

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
             * para que possa ser associado a um funcionado ou a um cliente
             * (Esse retorno se torna necessario para controlar os ids da tabela, pois é um campo auto_increment) */
            return ultimaPessoa;
        }

        /* Pesquisa uma pessoa pelo seu ID (Metodo generico)*/
        public T Read<T>(int id) where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar o endereco pesquisado de acordo com seu ID
             * (A palavra new so é usada em tipos concretos) 
             * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
            T pessoa = Activator.CreateInstance<T>();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu id */
                string query = "SELECT p.*, e.rua, e.numero, e.cep, e.cidade, e.estado, t.numero FROM Pessoa  p "
	                            + "INNER JOIN Endereco e ON p.idEndereco = e.id "
                                + "LEFT JOIN Telefone t ON p.id = t.id "
		                        + "WHERE p.id = @Id;";

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

            /* Retorna a pessoa pesquisada de acordo com seu ID */
            return pessoa;
        }

        public void Update(Pessoa pessoa) { }
        public void Delete(int id) { }
        public List<Pessoa> ListAll() { return null; }
        public List<Pessoa> FindByName(string name) { return null; }

        /* Metodo auxiliar responsavel por setar os dados da pessoa vindo do dados do banco
         * (Método generico) */
        private T setPessoa<T>(MySqlDataReader dr, T pessoa) where T : Pessoa
        {

            //Verifica se tem dados para ser lido
            if (dr.HasRows)
            {
                //Percorrer esse objeto até obter todos os dados
                while(dr.Read()) 
                {
                    pessoa.Id = dr.IsDBNull(0) == false ? dr.GetInt32(0) : 0;
                    pessoa.Cpf = dr.IsDBNull(1) == false ? dr.GetString(1) : null;
                    pessoa.Nome = dr.IsDBNull(2) == false ? dr.GetString(2) : null;

                    //Instancia um endereço para pessoa
                    pessoa.Endereco = new Endereco();
                    
                    //Seta os dados do endereço da pessoa de acordo com o dados vindo do banco
                    pessoa.Endereco.Id = dr.IsDBNull(3) == false ? dr.GetInt32(3) : 0;
                    pessoa.Endereco.Rua = dr.IsDBNull(4) == false ? dr.GetString(4) : null;
                    pessoa.Endereco.Numero = dr.IsDBNull(5) == false ? dr.GetInt32(5) : 0;
                    pessoa.Endereco.Cep = dr.IsDBNull(6) == false ? dr.GetInt32(6) : 0;
                    pessoa.Endereco.Cidade = dr.IsDBNull(7) == false ? dr.GetString(7) : null;
                    pessoa.Endereco.Estado = dr.IsDBNull(8) == false ? dr.GetString(8) : null;

                    //Verifica se o campo no BD não é null
                    if(!dr.IsDBNull(9))
                        pessoa.Telefones.Add(new Telefone(pessoa.Id, dr.GetString(9)));
                }
                
            }
            else
            {
                /*Caso não tenha nenhum dado para ser lido irá lançar uma 
                 *exceção para ser recuperada posteriormente no controler */
                throw new Exception("Pessoa não encontrada");
            }
                
            return pessoa;
        }

    }//Fim da classe
}//Fim da interface