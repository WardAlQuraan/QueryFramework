using CONNECTION_FACTORY.DAL_SESSION;
using Dapper;
using ENTITIES;
using Microsoft.AspNetCore.Mvc;
using COMMON;
using System.Data;
using BASES.BASE_REPO;
using FAMILY_TREE.BASE_CONTROLLER;
using BASES.BASE_SERVICE;
using SERVICES.TEST_ENTITY;

namespace FAMILY_TREE.Controllers
{
    public class TestEntityController : BaseController<TestEntity>
    {
        private readonly ITestEntityService _service;
        public TestEntityController(ITestEntityService service):base (service)
        {
        }


    }
}