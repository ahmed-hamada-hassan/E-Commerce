using E_Commerce.Application.Features.Users.Commands.UpdateUser;
using E_Commerce.Application.Features.Users.DTOs;
using Scalar.AspNetCore;

namespace E_Commerce.API.Contracts;

internal static class CustomerMappingExtenstions
{
    public static UpdateUserCommand ToUpdateCustomerCommand(this UpdateCustomerInfoRequest request, Guid userId)
    {
        return new UpdateUserCommand(userId, 
            string.IsNullOrWhiteSpace(request.FirstName) ? null : request.FirstName, 
            string.IsNullOrWhiteSpace(request.MiddleName) ? null : request.MiddleName, 
            string.IsNullOrWhiteSpace(request.LastName) ? null : request.LastName,
            string.IsNullOrWhiteSpace(request.Email) ? null : request.Email, 
            string.IsNullOrWhiteSpace(request.UserName) ? null : request.UserName, 
            string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber,
            request.DateOfBirth);
    }
}
