using E_Commerce.API.Contracts;
using E_Commerce.Application.Common;
using E_Commerce.Application.Features.ProductImages.Commands.AddImage;
using E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;
using E_Commerce.Application.Features.ProductImages.Commands.ReorderProductImage;
using E_Commerce.Application.Features.ProductImages.Commands.ReplaceProductImage;
using E_Commerce.Application.Features.ProductImages.Commands.SetPrimary;
using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Features.ProductImages.Queries.Vendor_Get_Images;
using E_Commerce.Application.Features.Products.Command.DeleteProduct;
using E_Commerce.Application.Features.Products.Command.RestoreProduct;
using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/vendor/products")]
[ApiController]
[Authorize(Policy = "Vendor-Only")]
[EnableRateLimiting("VendorManagement")]
public class VendorProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public VendorProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<VendorProductResponse, Guid>>> GetMyProducts(
        [FromQuery] CursorPaginationParams<Guid> paginationPrams, CancellationToken ct)
    {
        var result =
            await _mediator.Send(new VendorGetProductsQuery(CurrentVendorId, paginationPrams.cursor, paginationPrams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("archived")]
    public async Task<ActionResult<CursorPagedResult<VendorArchivedProductResponse, Guid>>> GetMyArchivedProducts(
        [FromQuery] CursorPaginationParams<Guid> paginationPrams, CancellationToken ct)
    {
        var result =
            await _mediator.Send(new VendorGetArchivedProductsQuery(CurrentVendorId, paginationPrams.cursor, paginationPrams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<VendorProductResponse>> GetMyProduct([FromRoute] Guid productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new VendorGetProductQuery(productId, CurrentVendorId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddProduct([FromBody] AddProductRequest productRequest, CancellationToken ct)
    {
        var command = productRequest.ToCreateProductCommand(CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) :
            CreatedAtAction(
                nameof(GetMyProduct),
                new { productId = result.Value },
                new {ID = result.Value }
            );
    }

    [HttpPut("{productId:guid}")]
    public async Task<ActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductRequest productRequest, CancellationToken ct)
    {
        var command = productRequest.ToUpdateProductCommand(productId, CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult> ArchiveProduct([FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new ArchiveProductCommand(productId, CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPatch("{productId:guid}/restore")]
    public async Task<ActionResult> RestoreProduct([FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new RestoreProductCommand(productId, CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPost("{productId:guid}/images")]
    public async Task<ActionResult> AddProductImage([FromRoute] Guid productID, [FromForm] IEnumerable<ImageRequest> images, CancellationToken ct)
    {
        var mappedImages = images.Select(i => i.ToImageDTO()).ToList();
        var command = new AddImageCommand(productID, CurrentVendorId, mappedImages);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : 
            CreatedAtAction(
                nameof(GetProductImages),
                new { productId = productID },
                result.Value
            );
    }

    [HttpGet("{productId:guid}/images")]
    public async Task<ActionResult<IReadOnlyCollection<VendorImageDetailsResponse>>> GetProductImages([FromRoute] Guid productId, CancellationToken ct)
    {
        var query = new VendorGetImagesQuery(productId, CurrentVendorId);
        var result = await _mediator.Send(query, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{productId:guid}/images/{imageId:guid}")]
    public async Task<ActionResult<VendorImageDetailsResponse>> GetProductImage([FromRoute] Guid productId, [FromRoute] Guid imageId, CancellationToken ct)
    {
        var query = new VendorGetImageQuery(imageId, CurrentVendorId, productId);
        var result = await _mediator.Send(query, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPut("{productId:guid}/images/{imageId:guid}")]
    public async Task<ActionResult> UpdateProductImage([FromRoute] Guid productId, [FromRoute] Guid imageId,
        [FromForm] IFormFile newImage, CancellationToken ct)
    {
        var command = new ReplaceProductImageCommand(productId, CurrentVendorId, imageId, newImage);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{productId:guid}/images/{imageId:guid}")]
    public async Task<ActionResult> DeleteProductImage([FromRoute] Guid imageId, [FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new VendorRemoveImageCommand(productId, CurrentVendorId, imageId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{productId:guid}/images")]
    public async Task<ActionResult> ClearProductImages([FromRoute] Guid productId, CancellationToken ct)
    {
        var command = new VendorClearImagesCommand(productId, CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("{productId:guid}/images/{imageId:guid}/set-primary")]
    public async Task<ActionResult> SetPrimaryImage([FromRoute] Guid productId, [FromRoute] Guid imageId, CancellationToken ct)
    {
        var command = new SetPrimaryCommand(productId, CurrentVendorId, imageId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("{productId:guid}/images/reorder")]
    public async Task<ActionResult> ReorderProductImages([FromRoute] Guid productId,
        [FromBody] IEnumerable<ReorderImageRequest> newOrders, CancellationToken ct)
    {
        var mappedOrders = newOrders.Select(i => i.ToReorderImage()).ToList();
        var command = new ReorderProductImageCommand(productId, CurrentVendorId, mappedOrders);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}