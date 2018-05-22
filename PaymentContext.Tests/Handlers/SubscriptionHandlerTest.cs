using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTest
    {
        [TestMethod]
        public void ShouldReturnErrorDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.Document = "999999999";
            command.Email = "teste@teste.com";
            command.BarCode = "123456789";
            command.BoletoNumber = "1234567";
            command.PaymentNumber = "12345";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "WAYNE CORP";
            command.PayerDocument = "12345678911";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "batman@dc.com";
            command.Street = "asdas";
            command.Number = "asdas";
            command.Neighborhood = "asdas";
            command.City = "as";
            command.State = "as";
            command.Country = "as";
            command.ZipCode = "123456";


            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}
