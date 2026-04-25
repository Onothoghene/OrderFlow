using Application.DTOs.Pagination;
using Application.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Wrappers
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<TDestination>> CreatePagedReponse<TSource, TDestination>(IQueryable<TSource> pagedData, PaginationFilter validFilter, IUriService uriService, string route, IMapper mapper)
        {
            List<TSource> items = new List<TSource>();
            if (validFilter.PageSize > 0)
            {
                items = pagedData.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize).ToList();
            }
            else
            {
                items = pagedData.ToList();
            }

            var respose = new PagedResponse<List<TDestination>>(mapper.Map<List<TSource>, List<TDestination>>(items), validFilter.PageNumber, validFilter.PageSize);
            var totalPages = validFilter.PageSize < 1 ? 1 : Convert.ToInt32(Math.Ceiling((double)pagedData.Count() / (double)validFilter.PageSize));
            
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < totalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= totalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilter(totalPages, validFilter.PageSize), route);
            respose.TotalPages = totalPages;
            respose.TotalRecords = pagedData.Count();
            return respose;
        }
    }
}
