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
    [Route(Routes.GroupPath)]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Group))]
        public async Task<IActionResult> Create([FromBody] CreateGroup request)
        {
            var group = await _groupService.Create(request.Name, request.Capacity);
            return Ok(_mapper.Map<Group>(group));
        }

        [HttpPatch(Routes.GroupIdSegment)]
        [ProducesResponseType(200, Type = typeof(Group))]
        public async Task<IActionResult> Update([FromRoute] Guid groupId, [FromBody] UpdateGroup request)
        {
            var group = await _groupService.Update(groupId, request.Name, request.Capacity);
            return Ok(_mapper.Map<Group>(group));
        }

        [HttpDelete(Routes.GroupIdSegment)]
        [ProducesResponseType(200, Type = typeof(Group))]
        public async Task<IActionResult> Remove([FromRoute] Guid groupId)
        {
            var group = await _groupService.Remove(groupId);
            return Ok(_mapper.Map<Group>(group));
        }

        [HttpGet(Routes.GroupIdSegment)]
        [ProducesResponseType(200, Type = typeof(Group))]
        public async Task<IActionResult> Get([FromRoute] Guid groupId)
        {
            var group = await _groupService.Get(groupId);
            return Ok(_mapper.Map<Group>(group));
        }
    }
}