using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace appraisal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<appraisal.Models.dep> deps { get; set; }

        public System.Data.Entity.DbSet<appraisal.Models.emp> emps { get; set; }

        public System.Data.Entity.DbSet<appraisal.Models.ots> ots { get; set; }

        public System.Data.Entity.DbSet<appraisal.Models.ts> ts { get; set; }

        public System.Data.Entity.DbSet<appraisal.Models.exm> exms { get; set; }

        public System.Data.Entity.DbSet<appraisal.Models.actlog> actlogs { get; set; }
    }

    public static class DefaultValues
{
  public static SelectList ItemsPerPageList 
    { 
      get 
      { return (new SelectList(new [] { 5, 10, 25, 50, 100 },selectedValue: 10)); 
      } 
    }

}
}