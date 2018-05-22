using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable, 
                                        IHandler<CreateBoletoSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;


        public SubscriptionHandler(IStudentRepository repository,IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //Fail Fast validations
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assintura");
            }

            // Verificar se Document já está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            // Verificar se E-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotification("Email", "Este E-mail já está em uso");
            }

            //Gerar os VOs

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email); 
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            
            //Gerar as entidades
            var student = new Student(name, document, email); 
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                                command.BarCode, 
                                command.BoletoNumber, 
                                command.PaidDate, 
                                command.ExpireDate, 
                                command.Total, 
                                command.TotalPaid, 
                                command.Payer, 
                                new Document(command.PayerDocument, command.PayerDocumentType),
                                address, email);

            //relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as Validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            //chegar as notificações
            if (Invalid)
            {
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }



            //Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.ToString(), student.Email.Address, "Bem vindo", "Sua assinatura foi criada");

            // Retornar informações
            return new CommandResult(true, "Cadastro realizado com sucesso");
        }

    }
}
