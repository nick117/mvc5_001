using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using appraisal.Models;

namespace appraisal.Controllers
{
    public class tsAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/tsAPI
        public IEnumerable<string> Get()
        {
            return new string[] { "Records", (from p in db.ts select p.emp).Distinct().Count().ToString() };
        }
    }
}
