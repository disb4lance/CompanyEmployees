﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repository, ILoggerManager logger,
IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            CheckIfCompanyExists(companyId, trackChanges);
            ;
            var employeesFromDb = _repository.Employee.GetEmployees(companyId,
            trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return employeesDto;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
             CheckIfCompanyExists(companyId, trackChanges);

            var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto
            employeeForCreation, bool trackChanges)
        {
            CheckIfCompanyExists(companyId, trackChanges);
            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }
        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeForCompany = _repository.Employee.GetEmployee(companyId, id,
            trackChanges);
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);
            _repository.Employee.DeleteEmployee(employeeForCompany);
            _repository.SaveAsync();
        }

        public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto
employeeForUpdate,
bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _repository.Employee.GetEmployee(companyId, id,
            empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeEntity);
            _repository.SaveAsync();
        }
        public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch
(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _repository.Employee.GetEmployee(companyId, id,
            empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(companyId);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
        }
        public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee
        employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            _repository.SaveAsync();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId,
            trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }
        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
        (Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb =  _repository.Employee.GetEmployee(companyId, id,
            trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }


    }
}
