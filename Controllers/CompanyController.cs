﻿using ApiCos.DTOs.CompanyDTO;
using ApiCos.DTOs.UserDTO;
using ApiCos.Models.Entities;
using ApiCos.Response;
using ApiCos.Services.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ApiCos.Controllers
{
    public class CompanyController : GenericController<CompanyController>
    {
        public CompanyController(ILogger<CompanyController> logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger, unitOfWork, mapper)
        {
        }

        [HttpPost]
        [Route("api/[controller]/AddCompany")]
        public async Task<ActionResult<UserRequest>> AddCompany([FromBody] CompanyRequest company)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Company companyTable = _mapper.Map<Company>(company);
                    await _unitOfWork.Company.Add(companyTable);
                    await _unitOfWork.CompleteAsync();
                    return Ok(ResponseHandler.GetApiResponse(ResponseType.Success, company));
                }
                return Ok(ResponseHandler.GetApiResponse(ResponseType.Failure, "Error"));
            } catch(Exception e)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(e));
            }

        }

        [HttpGet]
        [Route("api/[controller]/GetUsersByBusinessName")]
        public async Task<ActionResult<ICollection<User>?>> GetUsersByBusinessName([FromQuery] string businessName)
        {
            try
            {
                List<User>? usersDb = _unitOfWork.Company.GetUsersByBusinessName(businessName).Result;
                List<UserSending>? users = _mapper.Map<List<User>, List<UserSending>>(usersDb);

                return Ok(ResponseHandler.GetApiResponse(ResponseType.Success, users));
            } catch(Exception e)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(e));
            }
        }
    }
}