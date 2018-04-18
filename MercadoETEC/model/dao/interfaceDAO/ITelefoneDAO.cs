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
    interface ITelefoneDAO
    {
        void Create(Telefone telefone);
        Telefone Read(int id);
        void Update(Telefone telefone);
        void Delete(int id);
        List<Telefone> ListAll();
        List<Telefone> FindByNumero(int numero);
        void DeleteByNumero(int numero);

    }//Fim da interface
}//Fim da namespace