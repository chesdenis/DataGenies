using System.Linq;
using DataGenies.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataGenies.InMemoryHost.Models
{
    public class FlowSchemaDbContext : DbContext, IFlowSchemaContext
    {
        public FlowSchemaDbContext(DbContextOptions<FlowSchemaDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            {
                var entity = modelBuilder.Entity<ApplicationTemplateEntity>();
                entity.Property(x => x.Id).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Version).IsRequired();
                entity.Property(x => x.AssemblyPath).IsRequired();
                entity.Property(x => x.ConfigTemplateJson).IsRequired();
                
            }
            {
                var entity = modelBuilder.Entity<ApplicationInstanceEntity>();
            }
            {
                var entity = modelBuilder.Entity<BehaviourTemplateEntity>();
            }
            {
                var entity = modelBuilder.Entity<BehaviourInstanceEntity>();
            }
            {
                var entity =  modelBuilder.Entity<BindingEntity>();
            }
        }

        public DbSet<ApplicationTemplateEntity> ApplicationTemplates { get; set; }
        public DbSet<ApplicationInstanceEntity> ApplicationInstances { get; set; }
        public DbSet<BehaviourTemplateEntity> BehaviourTemplates { get; set; }
        public DbSet<BehaviourInstanceEntity> BehaviourInstances { get; set; }
        public DbSet<BindingEntity> Bindings { get; set; }

        IQueryable<ApplicationInstanceEntity> IFlowSchemaContext.ApplicationInstances => this.ApplicationInstances;
        IQueryable<ApplicationTemplateEntity> IFlowSchemaContext.ApplicationTemplates => this.ApplicationTemplates;
        IQueryable<BehaviourInstanceEntity> IFlowSchemaContext.BehaviourInstances => this.BehaviourInstances;
        IQueryable<BehaviourTemplateEntity> IFlowSchemaContext.BehaviourTemplates => this.BehaviourTemplates;
        IQueryable<BindingEntity> IFlowSchemaContext.Bindings => this.Bindings;
    }
}