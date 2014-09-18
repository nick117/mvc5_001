using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using appraisal.Models;

namespace appraisal.Controllers
{
    public class ImportApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/ImportApi
        public IEnumerable<string> Get()
        {
            return new string[] { "Records", (from data in db.importtss where data.CardNo1 != null select data).Count().ToString() };
        }
    }
}
