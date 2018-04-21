using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.dao;
using MercadoETEC.model.domain;

using MercadoETEC.model.service.exception;

namespace MercadoETEC.model.service
{
    /* Classe responsavel por ter as regras de negócio relacionadas ao DAO 
     * (Grava todos os dados do cliente em seus respectivos DAOs)*/
    class ClienteService
    {
        //Atributo responsavel por realizar as operações de CRUD no cliente no banco de dados
        private ClienteDAO clienteDAO = new ClienteDAO();

        //Atributo responsavel por realizar as operações de CRUD na endereco no banco de dados
        private EnderecoDAO enderecoDAO = new EnderecoDAO();

        //Atributo responsavel por realizar as operações de CRUD no telefone no banco de dados
        private TelefoneDAO telefoneDAO = new TelefoneDAO();

        public void Create(Cliente cliente)
        {
            /* Para registrar uma pessoa no banco é necessario ter um endereço cadastrado primeiro  
               porque na tabela pessoa possui um atributo idEndereco como chave estrangeira */


            /* Grava o endereço do cliente no banco de dados e retorna o ultimo endereco inserido no banco 
             * ja com seu id setado para ser associado ao cliente correto */
            cliente.Endereco = enderecoDAO.Create(cliente.Endereco);

            /* Guardar o cliente no banco de dados e retorna o ultimo cliente inserido ja com seu id setado. */
            /* Caso ocorra algum erro na gravação da pessoa o metodo retorna null. */
            /* Pode ocorrer problemas de UNIQUE na coluna CPF e NOT NULL na coluna NOME. */
            Cliente ultimoClienteInseridoBD = clienteDAO.Create(cliente);

            //Verifica se o cliente é null, se retornar null não foi salvo no banco de dados
            if (ultimoClienteInseridoBD == null)
            {
                /* Se ocorrer problema na gravação do cliente devemos excluir
                    * o endereço que foi gravado anteriormente, pois o endereço 
                    * não foi asociado ao cliente correto */
                if (cliente.Endereco != null)
                    enderecoDAO.Delete(cliente.Endereco.Id);
            }
            else
            {
                /* Caso não retornar null associa o Id do ultimo cliente inserido
                    * com o cliente que foi enviado para o metodo */
                cliente.Id = ultimoClienteInseridoBD.Id;

                /* Guardar os telefones do cliente no banco de dados */
                foreach (Telefone tel in cliente.Telefones)
                {
                    //Associa o id do telefone com o id do cliente
                    tel.Id = cliente.Id;

                    //Grava o telefone do cliente no banco e verifica se o numero é null
                    if (tel.Numero != null)
                        telefoneDAO.Create(tel);
                }
            }


        }

        public Cliente Read(int id)
        {
            //Delega a pesquisa do cliente para o DAO correspondente
            Cliente cliente = clienteDAO.Read(id);

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (cliente == null)
                throw new ObjetoNotFoundException("Cliente não encontrado");

            /* Delega a pesquisa do telefone para o DAO correspondente 
             * caso nao encontrar será retornado null*/
            Telefone tel = telefoneDAO.Read(cliente.Id);

            /* Caso não encontre nenhum telefone não será necessario lançar uma exceção, 
             * apenas não adicionamos na sua lista de telefones */
            if(tel != null)
                cliente.Telefones.Add(tel);

            return cliente;
        }

        public void Update(Cliente cliente)
        {
            //Atualiza o cliente
            clienteDAO.Update(cliente);

            //Atualiza o endereço (Verifica se a pessoa possui um endereco)
            if(cliente.Endereco != null)
                enderecoDAO.Update(cliente.Endereco);

            //Verifica se o cliente tem pelo menos 1 telefone ou mais
            if(cliente.Telefones.Count > 0)
            {
                //Pesquisa para verificar se o telefone ja foi inserido no banco
                /* Se retornar NULL quer dizer que não existe ainda */
                Telefone tel = telefoneDAO.Read(cliente.Id);

                if(tel != null)
                {
                    /* Atualiza o telefone(Nesse sistema é possivel cadastrar apenas um telefone) 
                    * Mais foi implementado como uma lista se caso precisar de mais de um telefone */
                    telefoneDAO.Update(cliente.Telefones[0]);
                }
                else
                {
                    //Cria o telefone na lista
                    telefoneDAO.Create(cliente.Telefones[0]);
                }

                
            }
               
        }

        public void Delete(Cliente cliente)
        {
            //Deleta o telefone do banco
            if(cliente.Telefones.Count > 0)
                telefoneDAO.Delete(cliente.Telefones[0].Id);

            //Delete o cliente do banco
            clienteDAO.Delete(cliente.Id);

            //Verifica se o cliente tem endereço e deleta o endereco do banco
            if(cliente.Endereco != null)
                enderecoDAO.Delete(cliente.Endereco.Id);
        }

        public List<Cliente> ListAll()
        {
            //Lista todos os clientes
            List<Cliente> clientes = clienteDAO.ListAll();

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if(clientes == null)
            {
                throw new ObjetoNotFoundException("Clientes não encontrados");
            }

            foreach (Cliente cli in clientes)
            {

                /* Delega a pesquisa do telefone para o DAO correspondente 
                 * caso nao encontrar será retornado null*/
                Telefone tel = telefoneDAO.Read(cli.Id);

                /* Caso não encontre nenhum telefone não será necessario lançar uma exceção, 
                 * apenas não adicionamos na sua lista de telefones */
                if (tel != null)
                    cli.Telefones.Add(tel);
            }


            return clientes;
        }

        public List<Cliente> FindByName(string name)
        {
            //Lista todos os clientes
            List<Cliente> clientes = clienteDAO.FindByName(name);

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (clientes == null)
            {
                throw new ObjetoNotFoundException("Cliente não encontrado");
            }

            foreach (Cliente cli in clientes)
            {

                /* Delega a pesquisa do telefone para o DAO correspondente 
                 * caso nao encontrar será retornado null*/
                Telefone tel = telefoneDAO.Read(cli.Id);

                /* Caso não encontre nenhum telefone não será necessario lançar uma exceção, 
                 * apenas não adicionamos na sua lista de telefones */
                if (tel != null)
                    cli.Telefones.Add(tel);
            }


            return clientes;
        }

        public Cliente FindByCpf(string cpf)
        {
            //Delega a pesquisa do cliente para o DAO correspondente
            Cliente cliente = clienteDAO.FindByCpf(cpf);

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (cliente == null)
                throw new ObjetoNotFoundException("Cliente não encontrado");

            /* Delega a pesquisa do telefone para o DAO correspondente 
             * caso nao encontrar será retornado null*/
            Telefone tel = telefoneDAO.Read(cliente.Id);

            /* Caso não encontre nenhum telefone não será necessario lançar uma exceção, 
             * apenas não adicionamos na sua lista de telefones */
            if (tel != null)
                cliente.Telefones.Add(tel);

            return cliente;
        }

    }//Fim da classe
}//Fim da namespace
