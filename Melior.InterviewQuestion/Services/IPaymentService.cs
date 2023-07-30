using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Services
{
    public interface IPaymentService
    {
        CreatePaymentResult MakePayment(MakePaymentRequest request);
    }
}
