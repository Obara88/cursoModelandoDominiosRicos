using PaymentContext.Shared.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Domain.Commands
{
    public class CommandResult : ICommandResult
    {
        public CommandResult()
        {

        }

        public CommandResult(bool success, string mensagem)
        {
            Success = success;
            Mensagem = mensagem;
        }

        public bool Success { get; set; }
        public string Mensagem { get; set; }
    }
}
