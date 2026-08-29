using E_Commerce.Application.Common;
using E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;
using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Features.ProductImages.Queries.Admin_Get_Images;
using E_Commerce.Application.Features.Products.Command.DeleteProduct;
using E_Commerce.Application.Features.Products.Command.RestoreProduct;
using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/products")]
[ApiController]
[Authorize(Policy = "SuperAdmin-Only")]
[EnableRateLimiting("AdminManagement")]
public class AdminProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<AdminProductResponse, Guid>>> GetProducts([FromQuery] Guid? vendorId,
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct)
    {
        var result =
            await _mediator.Send(new AdminGetProductsQuery(vendorId, paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("archived")]
    public async Task<ActionResult<CursorPagedResult<AdminArchivedProductResponse, Guid>>> GetArchivedProducts([FromQuery] Guid? vendorId,
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct)
    {
        var result =
            await _mediator.Send(new AdminGetArchivedProductsQuery(vendorId, paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("suspended")]
    public async Task<ActionResult<CursorPagedResult<AdminSuspendProductResponse, Guid>>> GetSuspendedProducts([FromQuery] Guid? vendorId,
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct)
    {
        var result =
            await _mediator.Send(new AdminGetSuspendProductsQuery(vendorId, paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{productId:guid}/available")]
    public async Task<ActionResult<AdminProductResponse>> GetProductDetails([FromRoute] Guid productId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new AdminGetProductQuery(productId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{productId:guid}/suspend")]
    public async Task<ActionResult<AdminProductResponse>> GetSuspendProductDetails([FromRoute] Guid productId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new AdminGetSuspendProductQuery(productId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{productId:guid}/archived")]
    public async Task<ActionResult<AdminProductResponse>> GetArchivedProductDetails([FromRoute] Guid productId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new AdminGetArchivedProductQuery(productId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult> SuspendProduct([FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new SuspendProductCommand(productId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPatch("{productId:guid}/unsuspend")]
    public async Task<ActionResult> RestoreProduct([FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new UnSuspendProductCommand(productId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpGet("{productId:guid}/images")]
    public async Task<ActionResult<IReadOnlyCollection<AdminImageDetailsResponse>>> GetProductImages([FromRoute] Guid productId,
        [FromQuery] Guid? vendorId, CancellationToken ct)
    {
        var query = new AdminGetImagesQuery(productId, vendorId);
        var result = await _mediator.Send(query, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpDelete("{productId:guid}/images/{imageId:guid}")]
    public async Task<ActionResult> DeleteInappropriateImage([FromRoute] Guid imageId, [FromRoute] Guid productId,
        [FromQuery] Guid? vendorId, CancellationToken ct)
    {
        var command = new AdminRemoveImageCommand(productId, imageId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{productId:guid}/images")]
    public async Task<ActionResult> ClearAllProductImages([FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new AdminClearImagesCommand(productId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}