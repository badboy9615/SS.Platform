﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SS.Platform.OA.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataModelContainer : DbContext
    {
        public DataModelContainer()
            : base("name=DataModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<BaseEntity> BaseEntity { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Family> Family { get; set; }
        public DbSet<Train> Train { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Control> Control { get; set; }
        public DbSet<ActionInfo> ActionInfo { get; set; }
        public DbSet<MenuInfo> MenuInfo { get; set; }
    }
}
