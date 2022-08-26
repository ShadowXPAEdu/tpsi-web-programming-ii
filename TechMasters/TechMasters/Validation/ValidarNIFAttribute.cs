using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using ValidarNIFAppCode;

namespace Trabalho_Pratico.Validacao
{
    public class ValidarNIFAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
                return ValidarNIF.ValidarNumeroIdentificacaoFiscal(value.ToString());

            return false;
        }
    }
}

