using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.domain;

namespace MercadoETEC.model.dao.interfaceDAO
{
    /*Estabelece um contrato, ou seja, quem implementar essa interface 
     *terá que implementar seus metodos abstratos */
    interface IPessoaDAO
    {
        T Create<T> (T pessoa) where T : Pessoa;
        T Read<T>(int id) where T : Pessoa;
        T FindByCpf<T>(string cpf) where T : Pessoa;
        void Update<T>(T pessoa) where T : Pessoa;
        void Delete(int id);
        List<Pessoa> ListAll();
        List<Pessoa> FindByName(string name);

    }//Fim da interface
}//Fim da namespace
