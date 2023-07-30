using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;
using System.Configuration;

namespace Melior.InterviewQuestion.Services
{
    public class PaymentService : IPaymentService
    {
        public CreatePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            Account account = null;


            // Check the type of data store of the current request
            if (dataStoreType == "Backup")
            {
                var accountDataStore = new BackupAccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }
            else
            {
                var accountDataStore = new AccountDataStore();
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }

            var result = new CreatePaymentResult();


            // Switch to check the payment type and if it was successful
            // Split it into separate classes and methods to comply with SOLID
            // This is due to these checks being outside of this method's scope
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    PaymentSchemeBacs resultBacs = new PaymentSchemeBacs();
                    result = resultBacs.GetResult(account);
                    break;

                case PaymentScheme.FasterPayments:
                    PaymentSchemeFasterPayments resultFasterPayments = new PaymentSchemeFasterPayments();
                    result = resultFasterPayments.GetResultFasterPayment(account, request.Amount);
                    break;

                case PaymentScheme.Chaps:
                    PaymentSchemeChaps resultChaps = new PaymentSchemeChaps();
                    result = resultChaps.GetResult(account);
                    break;
            }


            // Make the actual payment, i.e. deduct amount from the balance if the payment was successful
            if (result.Success)
            {
                account.Balance -= request.Amount;

                if (dataStoreType == "Backup")
                {
                    var accountDataStore = new BackupAccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    var accountDataStore = new AccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
            }

            return result;
        }
    }


    // An abstract class which is overriden by the three payment types to comply with SOLID
    public abstract class PaymentSchemeResult
    {
        public abstract CreatePaymentResult GetResult(Account account);

    }


    // Class and method for checking success status of Bacs payments
    public class PaymentSchemeBacs : PaymentSchemeResult
    {
        public override CreatePaymentResult GetResult(Account account)
        {
            var result = new CreatePaymentResult();

            // Merged all if-elses into one IF statement for readability
            if ((account == null) || (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs)))
            {
                result.Success = false;
            }

            return result;
        }
    }

    // Class and method for checking success status of Chaps payments
    public class PaymentSchemeChaps : PaymentSchemeResult
    {
        public override CreatePaymentResult GetResult(Account account)
        {
            var result = new CreatePaymentResult();

            // Merged all if-elses into one IF statement for readability
            if ((account == null) || (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)) || (account.Status != AccountStatus.Live))
            {
                result.Success = false;
            }
           

            return result;
        }
    }


    // Class and method for checking success status of Faster Payments
    public class PaymentSchemeFasterPayments
    {
        public CreatePaymentResult GetResultFasterPayment(Account account, decimal amount)
        {
            var result = new CreatePaymentResult();

            // Merged all if-elses into one IF statement for readability
            if ((account == null) || (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)) || (account.Balance < amount))
            {
                result.Success = false;
            }


            return result;
        }
    }




}
