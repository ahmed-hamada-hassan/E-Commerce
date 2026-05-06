using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Feedbacks.Queries.Admin_Get_Pending_Feedbacks;

internal sealed class AdminGetPendingFeedbacksQueryValidator : CursorPaginationValidator<AdminGetPendingFeedbacksQuery, Guid>
{
    public AdminGetPendingFeedbacksQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
