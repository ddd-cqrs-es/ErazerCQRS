﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.ViewModels;
using Erazer.Web.ReadAPI.Queries.Requests;

namespace Erazer.Web.ReadAPI.Queries.Handler
{
    public class StatusAllQueryHandler : IAsyncRequestHandler<StatusAllQuery, List<StatusViewModel>>
    {
        private readonly IStatusQueryRepository _repository;
        private readonly IMapper _mapper;

        public StatusAllQueryHandler(IStatusQueryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<StatusViewModel>> Handle(StatusAllQuery message)
        {
            var statuses =  await _repository.All();
            return _mapper.Map<List<StatusViewModel>>(statuses);
        }
    }
}
