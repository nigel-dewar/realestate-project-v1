using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage.API.Models.v1.Responses.PostCodes;
using Manage.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Manage.API.Services.v1
{
    public class PostCodesRepository: Interfaces.IPostCodesRepository
    {
        private readonly ManageDataContext _context;

        public PostCodesRepository(ManageDataContext context)
        {
            _context = context;
        }

        public async Task<List<PostCodeResponse>> FindPostCodesByAny(string q)
        {
            try
            {
                var Query = $"%{q?.ToLower()}%";

                string[] words = q.Split(' ');

                int matchedPostCode = 0;

                foreach (string word in words)
                {
                    if (word.Length == 4)
                    {
                        int n;
                        bool isNumeric = int.TryParse(word, out n);
                        if (isNumeric)
                        {
                            matchedPostCode = int.Parse(word);
                        }
                    }
                }

                using (_context)
                {
                    return await _context.PostCodes.Where(x =>
                          string.IsNullOrEmpty(q) ||
                          (
                            EF.Functions.Like(x.Code.ToString(), Query) ||
                            EF.Functions.Like(x.Locality.ToLower(), Query) ||
                            EF.Functions.Like(x.State.ToLower(), Query)
                        )

                    ).Select(x => new PostCodeResponse
                    {
                        PostCode = x.Code,
                        State = x.State,
                        Locality = x.Locality,
                        Long = x.Long,
                        Lat = x.Lat,
                        Label = x.Locality + ", " + x.State + ", " + x.Code,
                        Suburb = x.Suburb
                    }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR at GetPostCodes: " + ex.Message);
            }
        }

        public async Task<List<PostCodeResponse>> FindPostCodesBySlug(string q)
        {
            try
            {
                using (_context)
                {
                    var Suburbs = string.IsNullOrEmpty(q) ? new List<string>() : q.Split('|').ToList();

                    var postCodes = await _context.PostCodes
                        .Where(x => Suburbs.Any() == false || Suburbs.Contains(x.Suburb))
                        .Select(x => new PostCodeResponse
                        {
                            PostCode = x.Code,
                            State = x.State,
                            Locality = x.Locality,
                            Label = x.Locality + ", " + x.State + ", " + x.Code,
                            Suburb = x.Suburb
                        }).ToListAsync();

                    return postCodes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR at GetPostCodesBySlug: " + ex.Message);
            }
        }
    }
}
