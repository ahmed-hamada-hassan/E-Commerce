using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Infrastructure.Services.Payments;

public class PaymentFactory : IPaymentFactory
{
    private readonly IEnumerable<IPaymentService> _paymentServices;

    public PaymentFactory(IEnumerable<IPaymentService> paymentServices)
    {
        _paymentServices = paymentServices;
    }

    public IPaymentService GetPaymentService(PaymentMethod paymentMethod)
    {
        var service = _paymentServices.FirstOrDefault(s => s.Method == paymentMethod);

        if(service is null)
            throw new NotImplementedException($"Payment method {paymentMethod} is not implemented yet.");

        return service;
    }
}