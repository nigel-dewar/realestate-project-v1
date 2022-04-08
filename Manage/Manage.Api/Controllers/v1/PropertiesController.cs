using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Manage.API.Interfaces;
using Manage.API.Models.v1.Responses.Filters;
using Manage.API.Models.v1.Responses.Properties;
using Manage.Repository.Data;
using Manage.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manage.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        
        private readonly ManageDataContext _context;
        private readonly IMapper _mapper;
        private readonly IPropertiesService _propertiesService;

        public PropertiesController(ManageDataContext context, IMapper mapper, IPropertiesService propertiesService)
        {
            _context = context;
            _mapper = mapper;
            _propertiesService = propertiesService;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<PropertyResponse>>> List() {
            var properties = await _context.Properties
                .ToListAsync();

            return _mapper.Map<List<Property>, List<PropertyResponse>>(properties);
        }

        [AllowAnonymous]
        [HttpGet("find")]
        public async Task<ActionResult<PropertiesFindResponse>> Find(string mode, int page, string suburbs, string propertyTypes, string features, int? minPrice, int? maxPrice, string bedRooms, string bathRooms, string parking)
        {
            return await _propertiesService.FindProperties(
                mode,
                page,
                suburbs,
                propertyTypes,
                features,
                minPrice,
                maxPrice,
                bedRooms,
                bathRooms,
                parking);
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<PropertyResponse>> Slug(string slug)
        {
            return await _propertiesService.GetPropertyBySlug(slug);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyResponse>> Details(string id)
        {
            var property = await _context.Properties
                .SingleOrDefaultAsync(x=>x.Id == id);

            if (property == null)
            {
                throw new HttpRequestException("Property not found");
            }

            var propertyToReturn = _mapper.Map<Property, PropertyResponse>(property);

            return propertyToReturn;
        }

        
        
        [AllowAnonymous]
        [HttpGet("filters")]
        public async Task<ActionResult<FiltersResponse>> Filters()
        {
            return await _propertiesService.GetFilters();
        }

        // [HttpPut("{id}")]
        // public async Task<ActionResult> Edit(string id, Edit.Command command)
        // {
        //     try
        //     {
        //         command.Id = id;
        //         var result = await Mediator.Send(command);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {
        //
        //         return BadRequest(ex);
        //     }
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Unit>> Delete(string id)
        // {
        //     return await Mediator.Send(new Delete.Command { Id = id});
        // }

        // [HttpPost("{id}/attend")]
        // public async Task<ActionResult<Unit>> Attend(string id)
        // {
        //     return await Mediator.Send(new Attend.Command {Id = id});
        // }
        //
        // [HttpDelete("{id}/attend")]
        // public async Task<ActionResult<Unit>> UnAttend(string id)
        // {
        //     return await Mediator.Send(new UnAttend.Command {Id = id});
        // }
        //
        
        //
        // [HttpPost("{id}/addActivity")]
        // public async Task<ActionResult<Unit>> AddActivity(string id, AddActivity.Command command)
        // {
        //     try
        //     {
        //         command.Id = id;
        //         var result = await Mediator.Send(command);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {
        //
        //         return BadRequest(ex);
        //     }
        // }
    }
}