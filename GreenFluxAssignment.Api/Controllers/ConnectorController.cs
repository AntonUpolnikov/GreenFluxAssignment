using AutoMapper;
using GreenFluxAssignment.Api.Contracts.Requests;
using GreenFluxAssignment.Api.Contracts.Responses;
using GreenFluxAssignment.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Controllers
{
    [ApiController]
    [Route(Routes.ConnectorsPath)]
    public class ConnectorController : ControllerBase
    {
        private readonly IConnectorService _connectorService;
        private readonly IMapper _mapper;

        public ConnectorController(IConnectorService connectorService, IMapper mapper)
        {
            _connectorService = connectorService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Connector))]
        public async Task<IActionResult> Create(
            [FromRoute] Guid groupId,
            [FromRoute] Guid stationId,
            CreateConnector request)
        {
            var connector = await _connectorService.Create(groupId, stationId, request.ConnectorMaxCurrent);
            return Ok(_mapper.Map<Connector>(connector));
        }

        [HttpDelete(Routes.ConnectorIdSegment)]
        [ProducesResponseType(200, Type = typeof(Connector))]
        public async Task<IActionResult> Remove(
            [FromRoute] Guid groupId,
            [FromRoute] Guid stationId,
            [FromRoute]int connectorId)
        {
            var connector = await _connectorService.Remove(groupId, stationId, connectorId);
            return Ok(_mapper.Map<Connector>(connector));
        }

        [HttpPatch(Routes.ConnectorIdSegment)]
        [ProducesResponseType(200, Type = typeof(Connector))]
        public async Task<IActionResult> Update(
            [FromRoute] Guid groupId,
            [FromRoute] Guid stationId,
            [FromRoute]int connectorId,
            [FromBody]UpdateConnector request)
        {
            var connector = await _connectorService.ChangeCurrent(groupId, stationId, connectorId, request.ConnectorMaxCurrent);
            return Ok(_mapper.Map<Connector>(connector));
        }

        [HttpGet(Routes.ConnectorIdSegment)]
        [ProducesResponseType(200, Type = typeof(Connector))]
        public async Task<IActionResult> Get(
            [FromRoute] Guid groupId,
            [FromRoute] Guid stationId,
            [FromRoute]int connectorId)
        {
            var connector = await _connectorService.Get(groupId, stationId, connectorId);
            return Ok(_mapper.Map<Connector>(connector));
        }
    }
}
