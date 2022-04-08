using AutoMapper;
using Manage.API.Models.v1.Responses.Activities;
using Manage.API.Models.v1.Responses.Agencies;
using Manage.API.Models.v1.Responses.Agents;
using Manage.API.Models.v1.Responses.Images;
using Manage.API.Models.v1.Responses.Listings;
using Manage.API.Models.v1.Responses.PostCodes;
using Manage.API.Models.v1.Responses.Properties;
using Manage.Repository.Entities;
using MessageBus.Events;

namespace Manage.API.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            CreateMap<Property, PropertyResponse>();

            CreateMap<Listing, ListingResponse>();

            CreateMap<Agent, AgentResponse>();

            CreateMap<Agency, AgencyResponse>();

            CreateMap<Image, CreateImageResponse>();

            CreateMap<PostCode, PostCodeResponse>();


            CreateMap<Activity, ActivityResponse>();
            
            // CreateMap<UserProperty, AttendeeResponse>()
            //     .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
            //     .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            //     .ForMember(d => d.Image, o => o.MapFrom(s => s.User.Images.FirstOrDefault(x => x.IsMain).ImageUrl));
            //
            // CreateMap<UserActivity, AttendeeResponse>()
            //     .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
            //     .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            //     .ForMember(d => d.Image, o => o.MapFrom(s => s.User.Images.FirstOrDefault(x => x.IsMain).ImageUrl));
            //
            // CreateMap<Comment, CommentResponse>()
            //     .ForMember(d => d.UserName, o => o.MapFrom(s => s.Author.UserName))
            //     .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
            //     .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Images.FirstOrDefault(x => x.IsMain).ImageUrl));
        }
    }
}