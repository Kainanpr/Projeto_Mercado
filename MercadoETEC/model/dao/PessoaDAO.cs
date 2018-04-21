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
        public T Create<T>(T pessoa) where T : Pessoa
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
                string query = "CALL sp_pessoa_insert (@Cpf, @Nome, @Telefone, @IdEndereco)";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Cpf", MySqlDbType.String);
                command.Parameters.Add("@Nome", MySqlDbType.String);
                command.Parameters.Add("@Telefone", MySqlDbType.String);
                command.Parameters.Add("@IdEndereco", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Cpf"].Value = pessoa.Cpf;
                command.Parameters["@Nome"].Value = pessoa.Nome;
                command.Parameters["@Telefone"].Value = pessoa.Telefone;
                command.Parameters["@IdEndereco"].Value = pessoa.Endereco.Id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Verifica se tem dados para ser lidos
                if (dr.Read())
                {
                    //Chama o metodo auxiliar para setar a Pessoa vindo do banco
                    ultimaPessoa = setPessoa(dr, ultimaPessoa);
                }
                else
                {
                    //Caso não encontre nenhuma pessoa retorna null
                    return null;
                }

                //MessageBox.Show("Conexão com banco de dados efetuada com sucesso");
            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se der erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna a ultima pessoa inserido na tabela Pessoa 
             * para que possa ser associado a um funcionado ou a um cliente
             * (Esse retorno se torna necessario para controlar os ids da tabela, pois é um campo auto_increment) */
            pessoa.Id = ultimaPessoa.Id;
            return pessoa;
        }

        /* Pesquisa uma pessoa pelo seu ID (Metodo generico)*/
        public T Read<T>(int id) where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar a  pessoa pesquisada no banco de dados. 
             * (A palavra new so é usada em tipos concretos) 
             * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
            T pessoa = Activator.CreateInstance<T>();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu id */
                string query = "SELECT p.*, e.rua, e.numero, e.cep, e.cidade, e.estado FROM Pessoa p "
                               + "LEFT JOIN Endereco e ON p.idEndereco = e.id "
                               + "WHERE p.id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Verifica se tem dados para ser lidos
                if (dr.Read())
                {
                    //Chama o metodo auxiliar para setar a Pessoa vindo do banco
                    pessoa = setPessoa(dr, pessoa);
                }
                else
                {
                    //Caso não encontre nenhuma pessoa retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna a pessoa pesquisada de acordo com seu ID */
            return pessoa;
        }

        public void Update<T>(T pessoa) where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por atualizar a pessoa na tabela pessoa do banco */
                string query = "UPDATE Pessoa SET cpf = @Cpf, nome = @Nome, telefone = @Telefone WHERE id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Cpf", MySqlDbType.String);
                command.Parameters.Add("@Nome", MySqlDbType.String);
                command.Parameters.Add("@Telefone", MySqlDbType.String);
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Cpf"].Value = pessoa.Cpf;
                command.Parameters["@Nome"].Value = pessoa.Nome;
                command.Parameters["@Telefone"].Value = pessoa.Telefone;
                command.Parameters["@Id"].Value = pessoa.Id;

                //Executar instrução sem retorno de dados
                command.ExecuteNonQuery();

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
        }

        public void Delete(int id)
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por deletar a pessoa na tabela pessoa do banco */
                string query = "DELETE FROM Pessoa WHERE id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = id;

                //Executar instrução sem retorno de dados
                command.ExecuteNonQuery();

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
        }

        public List<T> ListAll<T>() where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar todas as pessoas vindas do banco */
            List<T> pessoas = new List<T>();

            /* Variavel local responsável por armazenar a  pessoa pesquisada no banco de dados. 
             * (A palavra new so é usada em tipos concretos) 
             * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
            T pessoa = Activator.CreateInstance<T>();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu id */
                string query = "SELECT p.*, e.rua, e.numero, e.cep, e.cidade, e.estado FROM Pessoa p "
                                + "LEFT JOIN Endereco e ON p.idEndereco = e.id "
                                + "INNER JOIN " + pessoa.GetType().Name + " tg ON p.id = tg.id "
                                + "ORDER BY p.id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Percorrer esse objeto até obter todos os dados
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        /* Variavel local responsável por armazenar a pessoa pesquisada no banco de dados. 
                         * (A palavra new so é usada em tipos concretos) 
                         * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
                        pessoa = Activator.CreateInstance<T>();

                        //Chama o metodo auxiliar para setar a Pessoa vindo do banco 
                        pessoa = setPessoa(dr, pessoa);

                        //Adiciona a pessoa na lista
                        pessoas.Add(pessoa);
                    }

                }
                else
                {
                    //Caso ocorra algum problema retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna todas as pessoas pesquisadas */
            return pessoas;
        }

        public T FindByCpf<T>(string cpf) where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar a  pessoa pesquisada no banco de dados. 
             * (A palavra new so é usada em tipos concretos) 
             * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
            T pessoa = Activator.CreateInstance<T>();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu CPF */
                string query = "SELECT p.*, e.rua, e.numero, e.cep, e.cidade, e.estado FROM Pessoa  p "
                               + "LEFT JOIN Endereco e ON p.idEndereco = e.id "
                               + "WHERE p.cpf LIKE @Cpf;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros ja com valor
                command.Parameters.AddWithValue("@Cpf", cpf);

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Verifica se tem dados para ser lidos
                if (dr.Read())
                {
                    //Chama o metodo auxiliar para setar a Pessoa vindo do banco
                    pessoa = setPessoa(dr, pessoa);
                }
                else
                {
                    //Caso não encontre nenhuma pessoa retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna a pessoa pesquisada de acordo com seu ID */
            return pessoa;
        }

        public List<T> FindByName<T>(string name) where T : Pessoa
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar o todas as pessoas vindas do banco */
            List<T> pessoas = new List<T>();

            /* Variavel local responsável por armazenar a  pessoa pesquisada no banco de dados. 
             * (A palavra new so é usada em tipos concretos) 
             * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
            T pessoa = Activator.CreateInstance<T>();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu nome */
                string query = "SELECT p.*, e.rua, e.numero, e.cep, e.cidade, e.estado FROM Pessoa p "
                                + "LEFT JOIN Endereco e ON p.idEndereco = e.id "
                                + "INNER JOIN " + pessoa.GetType().Name + " tg ON p.id = tg.id "
                                + "WHERE lower(p.nome) LIKE @Name ORDER BY p.id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros ja com valor
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Percorrer esse objeto até obter todos os dados
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        /* Variavel local responsável por armazenar a pessoa pesquisada no banco de dados. 
                         * (A palavra new so é usada em tipos concretos) 
                         * (Activador é uma classe que deixa criar uma instancia de tipo generico) */
                        pessoa = Activator.CreateInstance<T>();

                        //Chama o metodo auxiliar para setar a Pessoa vindo do banco 
                        pessoa = setPessoa(dr, pessoa);

                        //Adiciona a pessoa na lista
                        pessoas.Add(pessoa);
                    }

                }
                else
                {
                    //Caso ocorra algum problema retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna todas as pessoas pesquisadas */
            return pessoas;
        }

        /* Metodo auxiliar responsavel por setar os dados da pessoa vindo do dados do banco
         * (Método generico) */
        private T setPessoa<T>(MySqlDataReader dr, T pessoa) where T : Pessoa
        {

            //Seta os dados da pessoa de acordo com o dados vindo do banco
            pessoa.Id = dr.IsDBNull(0) == false ? dr.GetInt32(0) : 0;
            pessoa.Cpf = dr.IsDBNull(1) == false ? dr.GetString(1) : null;
            pessoa.Nome = dr.IsDBNull(2) == false ? dr.GetString(2) : null;
            pessoa.Telefone = dr.IsDBNull(2) == false ? dr.GetString(3) : null;

            //Instancia um endereço para pessoa
            pessoa.Endereco = new Endereco();

            //Seta os dados do endereço da pessoa de acordo com o dados vindo do banco
            pessoa.Endereco.Id = dr.IsDBNull(3) == false ? dr.GetInt32(4) : 0;
            pessoa.Endereco.Rua = dr.IsDBNull(4) == false ? dr.GetString(5) : null;
            pessoa.Endereco.Numero = dr.IsDBNull(5) == false ? dr.GetInt32(6) : 0;
            pessoa.Endereco.Cep = dr.IsDBNull(6) == false ? dr.GetInt32(7) : 0;
            pessoa.Endereco.Cidade = dr.IsDBNull(7) == false ? dr.GetString(8) : null;
            pessoa.Endereco.Estado = dr.IsDBNull(8) == false ? dr.GetString(9) : null;

            return pessoa;
        }

    }//Fim da classe
}//Fim da interface