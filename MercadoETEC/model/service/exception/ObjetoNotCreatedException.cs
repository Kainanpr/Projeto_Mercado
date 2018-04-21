using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoETEC.model.service.exception
{
    //Criando nossa propria excecao se caso algum objeto nao seja encontrado no banco de dados
    class ObjetoNotCreatedException : Exception
    {
        //Repassa a mensagem do erro para o construtor da superclasse
        public ObjetoNotCreatedException(String mensagem) : base(mensagem) { }

    }
}
