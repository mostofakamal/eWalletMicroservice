﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wallet.sagas.core.SagaDbConfigurations;

namespace wallet.sagas.core.Migrations
{
    [DbContext(typeof(WalletSagaDbContext))]
    partial class WalletSagaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("wallet.sagas.core.StateMachines.RewardTransactionStateData", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CurrentState")
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<DateTime?>("RequestCancelledOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RequestCompletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RequestStartedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RewardReceiverUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RewardSenderWalletUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CorrelationId");

                    b.ToTable("RewardTransactionStateData");
                });
#pragma warning restore 612, 618
        }
    }
}
