﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reward.Infrastructure;

namespace Reward.API.Infrastructure.Migrations
{
    [DbContext(typeof(RewardContext))]
    [Migration("20200904054247_AddedStatusOnUserReward")]
    partial class AddedStatusOnUserReward
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Reward.Domain.AggregateModel.RewardOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("RewardOperation");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "SubmitKyc"
                        },
                        new
                        {
                            Id = 2,
                            Name = "TransferMoney"
                        },
                        new
                        {
                            Id = 3,
                            Name = "BillPayment"
                        });
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.RewardRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.Property<int>("RequiredMinOccurance")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OperationId");

                    b.ToTable("RewardRules");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 2m,
                            IsEnabled = true,
                            OperationId = 1,
                            RequiredMinOccurance = 1,
                            ValidFrom = new DateTime(2020, 9, 4, 5, 42, 46, 748, DateTimeKind.Utc).AddTicks(8153),
                            ValidTo = new DateTime(2020, 10, 4, 5, 42, 46, 748, DateTimeKind.Utc).AddTicks(9090)
                        },
                        new
                        {
                            Id = 2,
                            Amount = 1m,
                            IsEnabled = true,
                            OperationId = 2,
                            RequiredMinOccurance = 2,
                            ValidFrom = new DateTime(2020, 9, 4, 5, 42, 46, 748, DateTimeKind.Utc).AddTicks(9996),
                            ValidTo = new DateTime(2020, 11, 4, 5, 42, 46, 749, DateTimeKind.Utc).AddTicks(7)
                        },
                        new
                        {
                            Id = 3,
                            Amount = 2m,
                            IsEnabled = true,
                            OperationId = 3,
                            RequiredMinOccurance = 1,
                            ValidFrom = new DateTime(2020, 9, 4, 5, 42, 46, 749, DateTimeKind.Utc).AddTicks(27),
                            ValidTo = new DateTime(2020, 11, 4, 5, 42, 46, 749, DateTimeKind.Utc).AddTicks(28)
                        });
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.TransactionType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("TransactionType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Transfer"
                        },
                        new
                        {
                            Id = 2,
                            Name = "BillPayment"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Reward"
                        });
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCountryAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTransactionEligible")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserIdentityGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.UserReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ReceivedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RewardGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RewardRuleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WalletUserId")
                        .HasColumnType("int");

                    b.Property<int>("_statusId")
                        .HasColumnName("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RewardRuleId");

                    b.HasIndex("UserId");

                    b.HasIndex("WalletUserId");

                    b.HasIndex("_statusId");

                    b.ToTable("UserRewards");
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.UserRewardStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("UserRewardStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Pending"
                        },
                        new
                        {
                            Id = 2,
                            Name = "PaidOut"
                        },
                        new
                        {
                            Id = 3,
                            Name = "TransactionFailed"
                        });
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.RewardRule", b =>
                {
                    b.HasOne("Reward.Domain.AggregateModel.RewardOperation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Reward.Domain.AggregateModel.UserReward", b =>
                {
                    b.HasOne("Reward.Domain.AggregateModel.RewardRule", "RewardRule")
                        .WithMany()
                        .HasForeignKey("RewardRuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Reward.Domain.AggregateModel.User", "User")
                        .WithMany("UserRewards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Reward.Domain.AggregateModel.User", "WalletUser")
                        .WithMany()
                        .HasForeignKey("WalletUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Reward.Domain.AggregateModel.UserRewardStatus", "Status")
                        .WithMany()
                        .HasForeignKey("_statusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
