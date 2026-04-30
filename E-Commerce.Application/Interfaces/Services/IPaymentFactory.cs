using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Interfaces.Services;

public interface IPaymentFactory : IScopedService
{
    IPaymentService GetPaymentService(PaymentMethod paymentMethod);
}