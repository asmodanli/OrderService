using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderService.Composers.Interfaces;
using OrderService.Models;
using OrderService.Validators;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ISearchOrderComposer _searchOrderComposer;
        private readonly IInsertOrderComposer _insertOrderComposer;
        private readonly SearchOrderValidator _searchValidator;
        private readonly OrderCollectionValidator _insertOrderCollectionValidator;

        public OrderController(ILogger<OrderController> logger,
            ISearchOrderComposer searchOrderComposer,
           IInsertOrderComposer insertOrderComposer,
           SearchOrderValidator searchValidator,
           OrderCollectionValidator insertValidator)
        {
            _insertOrderComposer = insertOrderComposer ?? throw new ArgumentNullException(nameof(insertOrderComposer));
            _searchOrderComposer = searchOrderComposer ?? throw new ArgumentNullException(nameof(searchOrderComposer));
            _searchValidator = searchValidator ?? throw new ArgumentNullException(nameof(searchValidator));
            _insertOrderCollectionValidator = insertValidator ?? throw new ArgumentNullException(nameof(insertValidator));
            _logger = logger;
        }

        [Route("InsertOrders")]
        [HttpPost]
        public async Task<IActionResult> InsertOrders([FromBody] List<Order> orders)
        {
            var validationResult = await _insertOrderCollectionValidator.ValidateAsync(orders);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation("[{now}] OrderService has recieved non-valid input for InsertOrders endpoint.", DateTimeOffset.Now);

                return BadRequest(new Error("BadRequestParams",
                    String.Join(" | ",
                        validationResult.Errors.Select(x => x.ErrorMessage))
                ));
            }
            var addedOrders = await _insertOrderComposer.InsertOrders(orders);
            if (addedOrders == null || addedOrders.Count == 0)
            {
                return BadRequest();
            }
            return Ok(addedOrders);

        }

        [Route("SearchOrder")]
        [HttpPost]
        public async Task<IActionResult> SearchOrder([FromBody] OrderFilter orderFilter)
        {
            var validationResult = await _searchValidator.ValidateAsync(orderFilter);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation("[{now}] OrderService has recieved non-valid input for SearchOrder endpoint.", DateTimeOffset.Now);
                return BadRequest(new Error("BadRequestParams",
                    String.Join(" | ",
                        validationResult.Errors.Select(x => x.ErrorMessage))
                ));
            }
            var orders = await _searchOrderComposer.SearchOrder(orderFilter);
            if (orders == null || orders.Count == 0)
            {
                _logger.LogInformation("[{now}] No result has returned for SearchOrder endpoint.", DateTimeOffset.Now);
                return NoContent();
            }
            return Ok(orders);
        }


    }
}