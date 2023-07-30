using Microsoft.VisualStudio.TestTools.UnitTesting;
using Melior;
using Melior.InterviewQuestion.Types;
using Melior.InterviewQuestion.Services;
using System;

namespace Melior.InterviewQuestionTest
{

    // Please note that all tests in this class are for Bacs
    // In an actual environment, I'd create the same set of tests for the other two payment types
    // I did not do it in this case to save you time
    [TestClass]
    public class UnitTest1
    {

        // This test checks a normal run, i.e. all information are supplemented and valid
        // In normal circumstances, tt should succeed, but it will always fail in this case

        // This is due to me not being able to change the signature of the MakePayment method
        // This means that the account is always NULL, so it will fail the later checks in the aftorementioned method
        // Thus, this test should fail

        [TestMethod]
        public void CorrectRun()
        {

            PaymentScheme paymentscheme = PaymentScheme.Bacs;

            MakePaymentRequest request = new MakePaymentRequest();
            PaymentService paymentService = new PaymentService();

            request.CreditorAccountNumber = "999-998";
            request.DebtorAccountNumber = "999-999";
            request.PaymentDate = DateTime.UtcNow;
            request.PaymentScheme = paymentscheme;
            request.Amount = 100;

            CreatePaymentResult result = paymentService.MakePayment(request);

            Assert.AreEqual(false, result.Success);


        }

        // This test checks what happens if the debtor account is not provided
        // It should fail
        [TestMethod]
        public void DebtorAccountIsNull()
        {

            PaymentScheme paymentscheme = PaymentScheme.Bacs;

            MakePaymentRequest request = new MakePaymentRequest();
            PaymentService paymentService = new PaymentService(); 

            request.CreditorAccountNumber = "999-999";
            request.DebtorAccountNumber = "";
            request.PaymentDate = DateTime.UtcNow;
            request.PaymentScheme = paymentscheme;
            request.Amount = 90;

            CreatePaymentResult result = paymentService.MakePayment(request);

            Assert.AreEqual(false, result.Success);
         

        }


        // This test checks what happens if the creditor account is not provided
        // It should fail
        [TestMethod]
        public void CreditorAccountIsNull()
        {

            PaymentScheme paymentscheme = PaymentScheme.Bacs;

            MakePaymentRequest request = new MakePaymentRequest();
            PaymentService paymentService = new PaymentService();

            request.CreditorAccountNumber = "";
            request.DebtorAccountNumber = "999-999";
            request.PaymentDate = DateTime.UtcNow;
            request.PaymentScheme = paymentscheme;
            request.Amount = 90;

            CreatePaymentResult result = paymentService.MakePayment(request);

            Assert.AreEqual(false, result.Success);


        }

        // This test checks what happens if the amount taken is zero
        // It should fail
        [TestMethod]
        public void AmountIsZero()
        {

            PaymentScheme paymentscheme = PaymentScheme.Bacs;

            MakePaymentRequest request = new MakePaymentRequest();
            PaymentService paymentService = new PaymentService();

            request.CreditorAccountNumber = "999-998";
            request.DebtorAccountNumber = "999-999";
            request.PaymentDate = DateTime.UtcNow;
            request.PaymentScheme = paymentscheme;
            request.Amount = 0;

            CreatePaymentResult result = paymentService.MakePayment(request);

            Assert.AreEqual(false, result.Success);


        }


        // This test checks what happens if both debtor and creditor accounts are the same
        // It should fail
        [TestMethod]
        public void DebtorSameAsCreditor()
        {

            PaymentScheme paymentscheme = PaymentScheme.Bacs;

            MakePaymentRequest request = new MakePaymentRequest();
            PaymentService paymentService = new PaymentService();

            request.CreditorAccountNumber = "999-999";
            request.DebtorAccountNumber = "999-999";
            request.PaymentDate = DateTime.UtcNow;
            request.PaymentScheme = paymentscheme;
            request.Amount = 0;

            CreatePaymentResult result = paymentService.MakePayment(request);

            Assert.AreEqual(false, result.Success);


        }

    }
}