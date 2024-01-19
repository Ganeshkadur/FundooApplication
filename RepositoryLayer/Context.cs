using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer
{
    public class Context:DbContext
    {
        public Context(DbContextOptions options) : base(options) { }

        public DbSet<UserRegistration> usersRegistation { get; set; }

        public DbSet<Notes> notes { get; set;}

        public DbSet<Collaborator> collaborators { get; set;}

        public DbSet<Review> reviews { get; set;}

        public DbSet<Label> labels { get; set;}

    }
}
