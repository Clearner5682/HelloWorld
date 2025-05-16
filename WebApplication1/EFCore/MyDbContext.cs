using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication1.EFCore.Models.AIO_Framework;
using WebApplication1.EFCore.Models.TestDb20240721;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.EFCore
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Configure_TestDb20240721(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private void Configure_AIO_Framework(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExtraCondition>(entityBuilder =>
            {
                //create procedure Elsa.SP_GetApproverByExtraConditions(@BusinessType nvarchar(100), @Position nvarchar(100), @ExtraConditions nvarchar(1000))
                //as
                //begin

                //declare @Table_ExtraConditions table
                //(
                //ExtraConditionName nvarchar(100) not null,
                //ExtraConditionValue nvarchar(100) null
                //)

                //insert into @Table_ExtraConditions
                //select ExtraConditionName,ExtraConditionValue from OPENJSON(@ExtraConditions)
                //WITH
                //(
                //ExtraConditionName nvarchar(100) '$.ExtraConditionName',
                //ExtraConditionValue nvarchar(100) '$.ExtraConditionValue'
                //)

                //select* from @Table_ExtraConditions

                //end

                entityBuilder.HasNoKey();
                entityBuilder.ToTable("SP_GetApproverByExtraConditions", "Elsa");
            });
        }

        private void Configure_TestDb20240721(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TorrentInfo>(entityBuilder =>
            {
                entityBuilder.ToTable("TorrentInfos");
                entityBuilder.HasKey(t => t.Id);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
