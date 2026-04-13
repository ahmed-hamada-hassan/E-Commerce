using E_Commerce.Application.Features.Auth.Command.Register;
using E_Commerce.Application.Features.Auth.DTOs;

namespace E_Commerce.API.Contracts;

public static class AuthMappingExtenstions
{
    public static RegisterCustomerCommand ToRegisterCustomerCommand(this RegisterCustomerRequest request)
    {
        return new RegisterCustomerCommand(request.Password, request.FirstName, request.MiddleName, request.LastName, request.Email, request.UserName,
            request.Image, request.PhoneNumber, request.DateOfBirth);
    }
    public static RegisterVendorCommand ToRegisterVendorCommand(this RegisterVendorRequest request)
    {
        return new RegisterVendorCommand(request.Password, request.FirstName, request.MiddleName, request.LastName, request.Email, request.UserName,
            request.Image, request.PhoneNumber, request.DateOfBirth, request.StoreName, request.CommercialRegistrationNumber);
    }
    public static SpecificRegisterCommand ToRegisterCommand(this RegisterRequest request)
    {
        return new SpecificRegisterCommand(request.Password, request.FirstName, request.MiddleName, request.LastName, request.Email, request.UserName,
            request.Image, request.PhoneNumber, request.DateOfBirth, request.Role);
    }
}
