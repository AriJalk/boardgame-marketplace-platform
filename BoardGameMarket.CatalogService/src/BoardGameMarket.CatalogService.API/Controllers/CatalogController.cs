using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Application.UseCases;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.CatalogService.API.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogItemGetterService _catalogItemGetterService;
        private readonly GetBggItemUseCase _getBggItemUseCase;
        private readonly GetCustomItemUseCase _getCustomItemUseCase;
        private readonly GetCustomItemsUseCase _getCustomItemsUseCase;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ICatalogItemGetterService catalogItemGetterService, GetBggItemUseCase getBggItemUseCase, GetCustomItemUseCase getCustomItemUseCase, GetCustomItemsUseCase getCustomItemsUseCase, ILogger<CatalogController> logger)
        {
            _catalogItemGetterService = catalogItemGetterService;
            _getBggItemUseCase = getBggItemUseCase;
            _getCustomItemUseCase = getCustomItemUseCase;
            _getCustomItemsUseCase = getCustomItemsUseCase;
            _logger = logger;
        }

        [HttpGet("bgg/{bggId}")]
        public async Task<ActionResult<CatalogItemInfo>> GetBggItemByBggId(int bggId)
        {
            _logger.LogInformation("{Class}/{Method}: Get bgg item {BggId}", nameof(CatalogController), nameof(GetBggItemByBggId), bggId);
            CatalogItemInfo catalogItemInfo = await _getBggItemUseCase.ExecuteAsync(bggId);
            return Ok(catalogItemInfo);
        }

        [HttpGet("{catalogItemId:guid}")]
        public async Task<ActionResult<CatalogItemInfo>> GetCatalogItemById(Guid catalogItemId)
        {
            _logger.LogInformation("{Class}/{Method}: Get catalog item {CatalogItemId}", nameof(CatalogController), nameof(GetCatalogItemById), catalogItemId);
            CatalogItem? catalogItem = await _catalogItemGetterService.GetCatalogItemByInternalIdASync(catalogItemId);

            if (catalogItem == null)
            {
                return NotFound();
            }

            return Ok(catalogItem.ToCatalogItemInfo());
        }

        [HttpGet("custom/{name}")]
        public async Task<ActionResult<CatalogItemInfo>> GetCustomCatalogItem(string name)
        {
            _logger.LogInformation("{Class}/{Method}: Get custom item {Name}", nameof(CatalogController), nameof(GetCustomCatalogItem), name);
            CatalogItemInfo catalogItemInfo = await _getCustomItemUseCase.ExecuteAsync(name);
            return Ok(catalogItemInfo);
        }

        [HttpPost("custom")]
        public async Task<ActionResult<CatalogItemInfo[]>> GetCustomCatalogItems([FromBody] CustomCatalogItemsRequest request)
        {
            _logger.LogInformation("{Class}/{Method}: Get custom items {Names}", nameof(CatalogController), nameof(GetCustomCatalogItems), string.Join(',', request.Names));
            IReadOnlyCollection<CatalogItemInfo> items = await _getCustomItemsUseCase.Execute(request.Names);
            return Ok(items);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CatalogItemInfo>>> Get()
        {
            _logger.LogInformation("{Class}/{Method}", nameof(CatalogController), nameof(Get));
            IEnumerable<CatalogItemInfo> items = (await _catalogItemGetterService.GetCatalogItemsAsync()).Select(i => i.ToCatalogItemInfo());
            return Ok(items);
        }
    }
}
