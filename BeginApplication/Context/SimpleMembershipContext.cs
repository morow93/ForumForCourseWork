using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BeginApplication.Context
{
    public class SimpleMembershipContext : DbContext
    {
        public SimpleMembershipContext()
            : base("SimpleMembershipConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Membership> webpages_Memberships { get; set; }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserProperty> UserProperties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Configurations.Add(null);
        } 
    }
}