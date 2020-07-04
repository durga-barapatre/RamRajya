using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Test_Owin_Identity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    //    public int Id { get; set; }
    public virtual ICollection<IncidentDetail> IncidentDetails { get; set; }
        public ApplicationUser()
        {
            
        }

      

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<IncidentDetail> IncidentDetails { get; set; }

        public virtual DbSet<IdentityUserRole> UserRoles { get; set; }
       // public virtual DbSet<Microsoft.VisualBasic.ApplicationServices.User> UserS { get; set; }
        // Add the method used to configure EF mapping
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Invoke the Identity version of this method to configure relationships 
            // in the AspNetIdentity models/tables
            base.OnModelCreating(modelBuilder);

            // Add a configuration for our new table.  Choose one end of the relationship
            // and tell it how it's supposed to work
            // modelBuilder.Entity<FormModel>().HasOptional(x => x.Place); // ApplicationUser has many MyUser
            modelBuilder.Entity<IncidentDetail>().ToTable("IncidentDetails","master");
            modelBuilder.Entity<IncidentDetail>().HasRequired(x => x.Users).WithMany(u => u.IncidentDetails).HasForeignKey(x => x.UserId);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
             
        }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}