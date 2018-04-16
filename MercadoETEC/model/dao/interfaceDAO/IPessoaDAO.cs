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
        Pessoa Create(Pessoa pessoa);
        Pessoa Read(int id);
        void Update(Pessoa pessoa);
        void Delete(int id);
        List<Pessoa> ListAll();
        List<Pessoa> FindByName(string name);

    }//Fim da interface
}//Fim da namespace
