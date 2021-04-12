using System;
using System.Threading.Tasks;
using AutoMapper;
using GreenFluxAssignment.Api.Contracts.Requests;
using GreenFluxAssignment.Api.Contracts.Responses;
using GreenFluxAssignment.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreenFluxAssignment.Api.Controllers
{
    [ApiController]
    [Route(Routes.ChargeStationsPath)]
    public class ChargeStationsController : ControllerBase
    {
        private readonly IChargeStationService _chargeStationService;
        private readonly IMapper _mapper;

        public ChargeStationsController(IChargeStationService chargeStationService, IMapper mapper)
        {
            _chargeStationService = chargeStationService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ChargeStation))]
        public async Task<IActionResult> Create(
            [FromRoute] Guid groupId,
            [FromBody] CreateChargeStation request)
        {
            var station = await _chargeStationService.Create(groupId, request.Name, request.ConnectorMaxCurrent);
            return Ok(_mapper.Map<ChargeStation>(station));
        }

        [HttpPatch(Routes.StationIdSegment)]
        [ProducesResponseType(200, Type = typeof(ChargeStation))]
        public async Task<IActionResult> Update(
            [FromRoute] Guid groupId,
            [FromRoute] Guid stationId,
            [FromBody] UpdateChargeStation request)
        {
            var station = await _chargeStationService.ChangeName(groupId, stationId, request.Name);
            return Ok(_mapper.Map<ChargeStation>(station));
        }

        [HttpDelete(Routes.StationIdSegment)]
        [ProducesResponseType(200, Type = typeof(ChargeStation))]
        public async Task<IActionResult> Remove([FromRoute] Guid groupId, [FromRoute] Guid stationId)
        {
            var station = await _chargeStationService.Remove(groupId, stationId);
            return Ok(_mapper.Map<ChargeStation>(station));
        }

        [HttpGet(Routes.StationIdSegment)]
        [ProducesResponseType(200, Type = typeof(ChargeStation))]
        public async Task<IActionResult> Get([FromRoute] Guid groupId, [FromRoute] Guid stationId)
        {
            var station = await _chargeStationService.Get(groupId, stationId);
            return Ok(_mapper.Map<ChargeStation>(station));
        }
    }
}